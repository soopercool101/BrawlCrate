using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.Internal.IO;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;
using BrawlLib.Wii.Audio;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSTMNode : RSARFileNode, IAudioSource
    {
        internal RSTMHeader* Header => (RSTMHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RSTM;

        private int _encoding;
        private int _channels;
        private bool _looped;
        private int _sampleRate;
        private int _loopStart;
        private int _numSamples;
        private int _dataOffset;
        private int _numBlocks;
        private int _blockSize;
        private int _bps;

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
        [Category("Audio Stream")] public int Channels => _channels;
        [Category("Audio Stream")] public bool IsLooped => _looped;
        [Category("Audio Stream")] public int SampleRate => _sampleRate;
        [Category("Audio Stream")] public int LoopStartSample => _loopStart;

        [Category("Audio Stream")] public int NumSamples => _numSamples;

        //[Category("Audio Stream")]
        //public int DataOffset { get { return _dataOffset; } }
        [Category("Audio Stream")] public int NumBlocks => _numBlocks;
        [Category("Audio Stream")] public int BlockSize => _blockSize;
        [Category("Audio Stream")] public int BitsPerSample => _bps;

        public IAudioStream[] CreateStreams()
        {
            if (Header == null)
            {
                return null;
            }

            StrmDataInfo* info = Header->HEADData->Part1;
            if (Header != null)
            {
                switch ((WaveEncoding) info->_format._encoding)
                {
                    case WaveEncoding.ADPCM:
                        return ADPCMStream.GetStreams(Header, _audioSource.Address);
                    case WaveEncoding.PCM16:
                        return PCMStream.GetStreams(Header, _audioSource.Address);
                }
            }

            return new IAudioStream[] {null};
        }

        public override bool OnInitialize()
        {
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            base.OnInitialize();

            if (Header->_header._tag == CSTMHeader.Tag)
            {
                ShowADPCMConversionWarning();
                byte[] brstm_temp = CSTMConverter.ToRSTM((CSTMHeader*) Header);
                fixed (byte* ptr = brstm_temp)
                {
                    ReplaceRaw(ptr, brstm_temp.Length);
                    return false;
                }
            }

            if (Header->_header._tag == FSTMHeader.Tag)
            {
                ShowADPCMConversionWarning();
                byte[] brstm_temp = FSTMConverter.ToRSTM((FSTMHeader*) Header);
                fixed (byte* ptr = brstm_temp)
                {
                    ReplaceRaw(ptr, brstm_temp.Length);
                    return false;
                }
            }

            StrmDataInfo* part1 = Header->HEADData->Part1;

            _encoding = part1->_format._encoding;
            _channels = part1->_format._channels;
            _looped = part1->_format._looped != 0;
            _sampleRate = part1->_sampleRate;
            _loopStart = part1->_loopStartSample;
            _numSamples = part1->_numSamples;
            _dataOffset = part1->_dataOffset;
            _numBlocks = part1->_numBlocks;
            _blockSize = part1->_blockSize;
            _bps = part1->_bitsPerSample;

            int offset = Header->DATAData->Data - Header;
            if (offset < WorkingUncompressed.Length)
            {
                _audioSource = new DataSource(Header->DATAData->Data, WorkingUncompressed.Length - offset);
                SetSizeInternal(offset);
            }

            return false;
        }

        public static bool ADPCMConversionWarningShown;

        public static void ShowADPCMConversionWarning()
        {
            if (!ADPCMConversionWarningShown)
            {
                ADPCMConversionWarningShown = true;
                System.Windows.Forms.MessageBox.Show(
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
                    int size = WorkingUncompressed.Length + _audioSource.Length;
                    using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                        FileShare.None))
                    {
                        stream.SetLength(size);
                        using (FileMap map = FileMap.FromStreamInternal(stream, FileMapProtect.ReadWrite, 0, size))
                        {
                            VoidPtr addr = map.Address;

                            //Write header
                            Memory.Move(addr, WorkingUncompressed.Address, (uint) WorkingUncompressed.Length);

                            //Set the offset to the audio samples (_dataLocation)
                            RSTMHeader* hdr = (RSTMHeader*) addr;
                            hdr->_header._length = WorkingUncompressed.Length + _audioSource.Length;
                            hdr->DATAData->Set(_audioSource.Length + 0x20);

                            addr += WorkingUncompressed.Length;

                            //Append audio samples to the end
                            Memory.Move(addr, _audioSource.Address, (uint) _audioSource.Length);

                            if (outPath.EndsWith(".bcstm"))
                            {
                                ShowADPCMConversionWarning();
                                byte[] bcstm_temp = CSTMConverter.FromRSTM(hdr);
                                fixed (byte* ptr = bcstm_temp)
                                {
                                    Memory.Move(map.Address, ptr, (uint) bcstm_temp.Length);
                                }
                            }
                            else if (outPath.EndsWith(".bfstm"))
                            {
                                ShowADPCMConversionWarning();
                                byte[] bfstm_temp = FSTMConverter.FromRSTM(hdr);
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
            {
                stream = WAV.FromFile(fileName);
            }
            else
            {
                base.Replace(fileName);
            }

            if (stream != null)
            {
                try
                {
                    ReplaceRaw(RSTMConverter.Encode(stream, null));
                }
                finally
                {
                    stream.Dispose();
                }
            }

            if (Header->_header._tag == CSTMHeader.Tag || Header->_header._tag == FSTMHeader.Tag)
            {
                throw new NotImplementedException();
            }

            int offset = (int) Header->DATAData->Data - (int) Header;
            if (offset < WorkingUncompressed.Length)
            {
                _audioSource = new DataSource(Header->DATAData->Data, WorkingUncompressed.Length - offset);
                SetSizeInternal(offset);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            BinTag tag = ((RSTMHeader*) source.Address)->_header._tag;
            return tag == RSTMHeader.Tag || tag == CSTMHeader.Tag || tag == FSTMHeader.Tag ? new RSTMNode() : null;
        }
    }
}