using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.SSBB.Types;
using BrawlLib.Wii.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class THPNode : ResourceNode, IVideo
    {
        internal THPFile* Header => (THPFile*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        private THPHeader hdr;
        private THPFrameCompInfo cmp;
        private THPAudioInfo audio;
        private THPVideoInfo video;

        [Category("THP Video Data")] public uint Width => video._xSize;
        [Category("THP Video Data")] public uint Height => video._ySize;
        [Category("THP Video Data")] public uint Type => video._videoType;

        [Category("THP Audio Data")] public uint Channels => audio._sndChannels;
        [Category("THP Audio Data")] public uint Frequency => audio._sndFrequency;
        [Category("THP Audio Data")] public uint NumSamples => audio._sndNumSamples;
        [Category("THP Audio Data")] public uint NumTracks => audio._sndNumTracks;

        [Category("THP Header Data")] public float FrameRate => hdr._frameRate;
        [Category("THP Header Data")] public uint NumFrames => hdr._numFrames;

        [Browsable(false)] public bool Loop => false;

        public THPFrame[] _frames;
        public List<byte> _componentTypes;

        public IAudioStream Audio => _audio;
        private THPStream _audio;

        public override bool OnInitialize()
        {
            if (_name == null && _origPath != null)
            {
                _name = Path.GetFileNameWithoutExtension(_origPath);
            }

            base.OnInitialize();

            hdr = Header->_header;
            cmp = Header->_frameCompInfo;
            audio = Header->_audioInfo;
            video = Header->_videoInfo;

            _componentTypes = new List<byte>();

            for (int i = 0; i < Header->_frameCompInfo._numComponents; i++)
            {
                _componentTypes.Add(Header->_frameCompInfo._frameComp[i]);
            }

            uint size = Header->_header._firstFrameSize;
            VoidPtr addr = Header->_header.FirstFrame;
            _frames = new THPFrame[NumFrames];
            for (int i = 0; i < NumFrames; i++)
            {
                _frames[i] = new THPFrame(addr, size, this);
                addr += size;
                size = _frames[i].Header->_frameSizeNext;
            }

            if (_componentTypes.Count > 1)
            {
                _audio = new THPStream(this);
            }
            else
            {
                _audio = null;
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return base.OnCalculateSize(force);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((THPHeader*) source.Address)->_tag == THPHeader.Tag ? new THPNode() : null;
        }

        [Browsable(false)] public int ImageCount => _frames.Length;

        public Bitmap GetImage(int index)
        {
            return _frames[index.Clamp(0, ImageCount - 1)].GetImage();
        }

        public int GetImageIndexAtFrame(int frame)
        {
            return frame;
        }
    }

    public enum JpegMarkers : ushort
    {
        // Start of Frame markers, non-differential, Huffman coding
        HuffBaselineDCT = 0xFFC0,
        HuffExtSequentialDCT = 0xFFC1,
        HuffProgressiveDCT = 0xFFC2,
        HuffLosslessSeq = 0xFFC3,

        // Start of Frame markers, differential, Huffman coding
        HuffDiffSequentialDCT = 0xFFC5,
        HuffDiffProgressiveDCT = 0xFFC6,
        HuffDiffLosslessSeq = 0xFFC7,

        // Start of Frame markers, non-differential, arithmetic coding
        ArthBaselineDCT = 0xFFC8,
        ArthExtSequentialDCT = 0xFFC9,
        ArthProgressiveDCT = 0xFFCA,
        ArthLosslessSeq = 0xFFCB,

        // Start of Frame markers, differential, arithmetic coding
        ArthDiffSequentialDCT = 0xFFCD,
        ArthDiffProgressiveDCT = 0xFFCE,
        ArthDiffLosslessSeq = 0xFFCF,

        // Huffman table spec
        HuffmanTableDef = 0xFFC4,

        // Arithmetic table spec
        ArithmeticTableDef = 0xFFCC,

        // Restart Interval termination
        RestartIntervalStart = 0xFFD0,
        RestartIntervalEnd = 0xFFD7,

        // Other markers
        StartOfImage = 0xFFD8,
        EndOfImage = 0xFFD9,
        StartOfScan = 0xFFDA,
        QuantTableDef = 0xFFDB,
        NumberOfLinesDef = 0xFFDC,
        RestartIntervalDef = 0xFFDD,
        HierarchProgressionDef = 0xFFDE,
        ExpandRefComponents = 0xFFDF,

        // App segments
        App0 = 0xFFE0,
        App1 = 0xFFE1,
        App2 = 0xFFE2,
        App3 = 0xFFE3,
        App4 = 0xFFE4,
        App5 = 0xFFE5,
        App6 = 0xFFE6,
        App7 = 0xFFE7,
        App8 = 0xFFE8,
        App9 = 0xFFE9,
        App10 = 0xFFEA,
        App11 = 0xFFEB,
        App12 = 0xFFEC,
        App13 = 0xFFED,
        App14 = 0xFFEE,
        App15 = 0xFFEF,

        // Jpeg Extensions
        JpegExt0 = 0xFFF0,
        JpegExt1 = 0xFFF1,
        JpegExt2 = 0xFFF2,
        JpegExt3 = 0xFFF3,
        JpegExt4 = 0xFFF4,
        JpegExt5 = 0xFFF5,
        JpegExt6 = 0xFFF6,
        JpegExt7 = 0xFFF7,
        JpegExt8 = 0xFFF8,
        JpegExt9 = 0xFFF9,
        JpegExtA = 0xFFFA,
        JpegExtB = 0xFFFB,
        JpegExtC = 0xFFFC,
        JpegExtD = 0xFFFD,

        // Comments
        Comment = 0xFFFE,

        // Reserved
        ArithTemp = 0xFF01,
        ReservedStart = 0xFF02,
        ReservedEnd = 0xFFBF
    }

    public unsafe class THPFrame
    {
        private readonly THPNode _node;

        public Bitmap GetImage()
        {
            //We have to convert the raw buffer to a usable image every time the image is called.
            //Dispose of image when done displaying. This way we won't run out of memory.
            //Doesn't seem to slow down frame rate or anything.

            byte[] buffer = new byte[Header->CompAddr[0]];
            Marshal.Copy(Header->GetComp(_node._componentTypes.Count, 0), buffer, 0, buffer.Length);

            bool begun = false;
            List<byte> temp = buffer.ToList();

            int end = 0;
            for (int i = temp.Count - 2; i >= 0; i--)
            {
                byte b1 = temp[i];
                byte b2 = temp[i + 1];
                ushort code = (ushort) ((b1 << 8) | b2);

                if (Enum.IsDefined(typeof(JpegMarkers), code))
                {
                    JpegMarkers m = (JpegMarkers) code;
                    if (m == JpegMarkers.EndOfImage)
                    {
                        end = i;
                        break;
                    }
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                byte b1 = temp[i];
                if (b1 == 0xFF)
                {
                    byte b2 = temp[i + 1];
                    ushort code = (ushort) ((b1 << 8) | b2);

                    if (Enum.IsDefined(typeof(JpegMarkers), code))
                    {
                        JpegMarkers m = (JpegMarkers) code;
                        if (m == JpegMarkers.EndOfImage && i == end)
                        {
                            break;
                        }

                        if (begun)
                        {
                            temp.Insert(i + 1, 0);
                            i++;
                        }

                        if (m == JpegMarkers.StartOfScan)
                        {
                            begun = true;
                        }
                    }
                    else
                    {
                        if (begun)
                        {
                            temp.Insert(i + 1, 0);
                            i++;
                        }
                    }
                }
            }

            buffer = temp.ToArray();

            return (Bitmap) new ImageConverter().ConvertFrom(buffer);
        }

        private DataSource _source;
        public THPFrameHeader* Header => (THPFrameHeader*) _source.Address;
        public ThpAudioFrameHeader* Audio => (ThpAudioFrameHeader*) Header->GetComp(2, 1);

        public THPFrame(VoidPtr addr, uint size, THPNode node)
        {
            _source = new DataSource(addr, (int) size);
            _node = node;
        }
    }

    internal class THPAudioBlock
    {
        public uint _srcLen;
        public uint _numSamples;

        public THPAudioBlock(uint blockSize, uint numSamples)
        {
            _srcLen = blockSize;
            _numSamples = numSamples;
        }
    }

    internal unsafe class THPStream : IAudioStream
    {
        private readonly int _sampleRate;
        private readonly int _numSamples;
        private readonly int _numChannels;
        private readonly int _numBlocks;

        private int _samplePos;
        public int _blockId;

        private readonly ADPCMState[,] _blockStates;
        internal ADPCMState[] _currentStates;
        private readonly THPAudioBlock[] _audioBlocks;

        public THPStream(THPNode node)
        {
            byte* sPtr;
            short yn1 = 0, yn2 = 0;

            _numChannels = (int) node.Channels;
            _sampleRate = (int) node.Frequency;
            _numSamples = (int) node.NumSamples;
            _numBlocks = (int) node.NumFrames;

            _blockStates = new ADPCMState[_numChannels, _numBlocks];
            _currentStates = new ADPCMState[_numChannels];
            _audioBlocks = new THPAudioBlock[_numBlocks];

            //Fill block states in a linear fashion
            for (int frame = 0; frame < node.NumFrames; frame++)
            {
                THPFrame f = node._frames[frame];
                ThpAudioFrameHeader* header = f.Audio;
                for (int channel = 0; channel < _numChannels; channel++)
                {
                    sPtr = header->GetAudioChannel(channel);

                    short[] coefs;
                    if (channel == 0)
                    {
                        yn1 = header->_c1yn1;
                        yn2 = header->_c1yn2;
                        coefs = header->Coefs1;
                    }
                    else
                    {
                        yn1 = header->_c2yn1;
                        yn2 = header->_c2yn2;
                        coefs = header->Coefs2;
                    }

                    //Get block state
                    _blockStates[channel, frame] =
                        new ADPCMState(sPtr, *sPtr, yn1, yn2, coefs); //Use ps from data stream
                }

                _audioBlocks[frame] = new THPAudioBlock(header->_blockSize, header->_numSamples);
            }
        }

        private void RefreshStates()
        {
            //Clamp sample position to start of block
            _blockId = 0;
            int temp = 0;
            for (int i = 0; i < _numBlocks; i++)
            {
                if (_samplePos < _audioBlocks[_blockId]._numSamples + temp)
                {
                    break;
                }

                temp += (int) _audioBlocks[_blockId]._numSamples;
                _blockId++;
            }
            //_samplePos = temp;

            for (int i = 0; i < _numChannels; i++)
            {
                (_currentStates[i] = _blockStates[i, _blockId]).InitBlock();
                for (int x = temp; x < _samplePos; x++)
                {
                    _currentStates[i].ReadSample();
                }
            }
        }

        public RIFFHeader GetPCMHeader()
        {
            return new RIFFHeader(1, _numChannels, 16, _sampleRate, _numSamples);
        }

        public void WriteStream(Stream outStream)
        {
            int oldPos = _samplePos;
            short sample;

            for (_samplePos = 0; _samplePos < _numSamples; _samplePos++)
            {
                if (AtStartOfABlock(_samplePos))
                {
                    RefreshStates();
                }

                foreach (ADPCMState state in _currentStates)
                {
                    sample = state.ReadSample();
                    outStream.WriteByte((byte) (sample & 0xFF));
                    outStream.WriteByte((byte) ((sample >> 8) & 0xFF));
                }
            }

            SamplePosition = oldPos;
        }

        public int GetSampleAtFrame(int frame)
        {
            int x = 0;
            for (int i = 0; i < frame; i++)
            {
                x += (int) _audioBlocks[i]._numSamples;
            }

            return x;
        }

        public void GetBlock()
        {
            int temp = 0;
            for (int i = 0; i < _numBlocks; i++)
            {
                if (_samplePos >= temp && _samplePos < _audioBlocks[i]._numSamples)
                {
                    _blockId = i;
                    break;
                }

                temp += (int) _audioBlocks[i]._numSamples;
            }
        }

        public bool AtStartOfABlock(int sample)
        {
            int block = 0;
            int temp = 0;
            for (int i = 0; i < _numBlocks; i++)
            {
                if (sample == temp)
                {
                    return true;
                }

                if (temp > sample)
                {
                    return false;
                }

                temp += (int) _audioBlocks[block++]._numSamples;
            }

            return false;
        }

        #region IAudioStream Members

        public WaveFormatTag Format => WaveFormatTag.WAVE_FORMAT_PCM;
        public int BitsPerSample => 16;
        public int Samples => _numSamples;
        public int Channels => _numChannels;
        public int Frequency => _sampleRate;

        public bool IsLooping
        {
            get => false;
            set { }
        }

        public int LoopStartSample
        {
            get => 0;
            set { }
        }

        public int LoopEndSample
        {
            get => _numSamples;
            set { }
        }

        public int SamplePosition
        {
            get => _samplePos;
            set
            {
                value = Math.Min(Math.Max(value, 0), _numSamples);
                if (_samplePos == value)
                {
                    return;
                }

                _samplePos = value;

                //Refresh states up to sample pos. If first in block, will be updated on next read.
                if (!AtStartOfABlock(_samplePos))
                {
                    RefreshStates();
                }
            }
        }

        public int ReadSamples(VoidPtr destAddr, int numSamples)
        {
            short* dPtr = (short*) destAddr;
            int samples = Math.Min(numSamples, _numSamples - _samplePos);

            for (int i = 0; i < samples; i++, _samplePos++)
            {
                if (AtStartOfABlock(_samplePos))
                {
                    RefreshStates();
                }

                for (int x = 0; x < _numChannels; x++)
                {
                    *dPtr++ = _currentStates[x].ReadSample();
                }
            }

            GetBlock();

            return samples;
        }

        public void Wrap()
        {
            if (SamplePosition == 0)
            {
                return;
            }

            SamplePosition = 0;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}