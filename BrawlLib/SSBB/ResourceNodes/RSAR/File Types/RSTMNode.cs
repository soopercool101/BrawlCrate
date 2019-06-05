using System;
using System.Audio;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSTMNode : RSARFileNode, IAudioSource
    {
        public static bool ADPCMConversionWarningShown;
        private int _dataOffset;

        private int _encoding;
        internal RSTMHeader* Header => (RSTMHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RSTM;

        [Category("File Node")]
        [Browsable(false)]
        public override int FileNodeIndex => _fileIndex;

        [Category("Data")] [Browsable(false)] public override string AudioLength => _audioSource.Length.ToString("X");

        [Category("Data")]
        [Browsable(false)]
        public override string DataLength => WorkingUncompressed.Length.ToString("X");

        [Browsable(false)] public override string[] GroupRefs => _groupRefs.Select(x => x.TreePath).ToArray();

        [Browsable(false)] public override string[] EntryRefs => _references.ToArray();

        [Category("Audio Stream")] public WaveEncoding Encoding => (WaveEncoding) _encoding;

        [Category("Audio Stream")] public int Channels { get; private set; }

        [Category("Audio Stream")] public bool IsLooped { get; private set; }

        [Category("Audio Stream")] public int SampleRate { get; private set; }

        [Category("Audio Stream")] public int LoopStartSample { get; private set; }

        [Category("Audio Stream")] public int NumSamples { get; private set; }

        //[Category("Audio Stream")]
        //public int DataOffset { get { return _dataOffset; } }
        [Category("Audio Stream")] public int NumBlocks { get; private set; }

        [Category("Audio Stream")] public int BlockSize { get; private set; }

        [Category("Audio Stream")] public int BitsPerSample { get; private set; }

        public IAudioStream[] CreateStreams()
        {
            if (Header == null) return null;

            var info = Header->HEADData->Part1;
            if (Header != null)
                switch ((WaveEncoding) info->_format._encoding)
                {
                    case WaveEncoding.ADPCM:
                        return ADPCMStream.GetStreams(Header, _audioSource.Address);
                    case WaveEncoding.PCM16:
                        return PCMStream.GetStreams(Header, _audioSource.Address);
                }

            return new IAudioStream[] {null};
        }

        public override bool OnInitialize()
        {
            if (_name == null && _origPath != null) _name = Path.GetFileNameWithoutExtension(_origPath);

            base.OnInitialize();

            if (Header->_header._tag == CSTMHeader.Tag)
            {
                ShowADPCMConversionWarning();
                var brstm_temp = CSTMConverter.ToRSTM((CSTMHeader*) Header);
                fixed (byte* ptr = brstm_temp)
                {
                    ReplaceRaw(ptr, brstm_temp.Length);
                    return false;
                }
            }

            if (Header->_header._tag == FSTMHeader.Tag)
            {
                ShowADPCMConversionWarning();
                var brstm_temp = FSTMConverter.ToRSTM((FSTMHeader*) Header);
                fixed (byte* ptr = brstm_temp)
                {
                    ReplaceRaw(ptr, brstm_temp.Length);
                    return false;
                }
            }

            var part1 = Header->HEADData->Part1;

            _encoding = part1->_format._encoding;
            Channels = part1->_format._channels;
            IsLooped = part1->_format._looped != 0;
            SampleRate = part1->_sampleRate;
            LoopStartSample = part1->_loopStartSample;
            NumSamples = part1->_numSamples;
            _dataOffset = part1->_dataOffset;
            NumBlocks = part1->_numBlocks;
            BlockSize = part1->_blockSize;
            BitsPerSample = part1->_bitsPerSample;

            var offset = Header->DATAData->Data - Header;
            if (offset < WorkingUncompressed.Length)
            {
                _audioSource = new DataSource(Header->DATAData->Data, WorkingUncompressed.Length - offset);
                SetSizeInternal(offset);
            }

            return false;
        }

        public static void ShowADPCMConversionWarning()
        {
            if (!ADPCMConversionWarningShown)
            {
                ADPCMConversionWarningShown = true;
                MessageBox.Show(
                    "Note: FSTM and CSTM support is still in beta. Playback and output files might not be completely accurate.");
            }
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".wav"))
            {
                WAV.ToFile(CreateStreams()[0], outPath);
            }
            else
            {
                if (_audioSource != DataSource.Empty)
                {
                    var size = WorkingUncompressed.Length + _audioSource.Length;
                    using (var stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                        FileShare.None))
                    {
                        stream.SetLength(size);
                        using (var map = FileMap.FromStreamInternal(stream, FileMapProtect.ReadWrite, 0, size))
                        {
                            var addr = map.Address;

                            //Write header
                            Memory.Move(addr, WorkingUncompressed.Address, (uint) WorkingUncompressed.Length);

                            //Set the offset to the audio samples (_dataLocation)
                            var hdr = (RSTMHeader*) addr;
                            hdr->_header._length = WorkingUncompressed.Length + _audioSource.Length;
                            hdr->DATAData->Set(_audioSource.Length + 0x20);

                            addr += WorkingUncompressed.Length;

                            //Append audio samples to the end
                            Memory.Move(addr, _audioSource.Address, (uint) _audioSource.Length);

                            if (outPath.EndsWith(".bcstm"))
                            {
                                ShowADPCMConversionWarning();
                                var bcstm_temp = CSTMConverter.FromRSTM(hdr);
                                fixed (byte* ptr = bcstm_temp)
                                {
                                    Memory.Move(map.Address, ptr, (uint) bcstm_temp.Length);
                                }
                            }
                            else if (outPath.EndsWith(".bfstm"))
                            {
                                ShowADPCMConversionWarning();
                                var bfstm_temp = FSTMConverter.FromRSTM(hdr);
                                fixed (byte* ptr = bfstm_temp)
                                {
                                    Memory.Move(map.Address, ptr, (uint) bfstm_temp.Length);
                                }
                            }
                        }
                    }
                }
                else
                {
                    base.Export(outPath);
                }
            }
        }

        public override void Replace(string fileName)
        {
            IAudioStream stream = null;

            if (fileName.EndsWith(".wav"))
                stream = WAV.FromFile(fileName);
            else
                base.Replace(fileName);

            if (stream != null)
                try
                {
                    ReplaceRaw(RSTMConverter.Encode(stream, null));
                }
                finally
                {
                    stream.Dispose();
                }

            if (Header->_header._tag == CSTMHeader.Tag || Header->_header._tag == FSTMHeader.Tag)
                throw new NotImplementedException();

            var offset = (int) Header->DATAData->Data - (int) Header;
            if (offset < WorkingUncompressed.Length)
            {
                _audioSource = new DataSource(Header->DATAData->Data, WorkingUncompressed.Length - offset);
                SetSizeInternal(offset);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            var tag = ((RSTMHeader*) source.Address)->_header._tag;
            return tag == RSTMHeader.Tag || tag == CSTMHeader.Tag || tag == FSTMHeader.Tag ? new RSTMNode() : null;
        }
    }
}