using BrawlLib.Internal;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct THPHeader
    {
        public const uint Size = 0x30;
        public const uint Tag = 0x00504854;

        public BinTag _tag;            // "THP\0"
        public buint _version;         // version number
        public buint _bufSize;         // max frame size for buffer computation
        public buint _audioMaxSamples; // max samples of audio data

        public bfloat _frameRate;     // frame per seconds
        public buint _numFrames;      // frame count
        public buint _firstFrameSize; // how much to load
        public buint _movieDataSize;  // file size

        public buint _compInfoDataOffsets;   // offset to component infomation data
        public buint _offsetDataOffsets;     // offset to array of frame offsets
        public buint _movieDataOffsets;      // offset to first frame (start of movie data) 
        public buint _finalFrameDataOffsets; // offset to final frame

        private VoidPtr Address
        {
            get
            {
                fixed (void* p = &this)
                {
                    return p;
                }
            }
        }

        public VoidPtr FirstFrame => Address + _movieDataOffsets;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct THPAudioInfo
    {
        public buint _sndChannels;
        public buint _sndFrequency;
        public buint _sndNumSamples;
        public buint _sndNumTracks; // number of Tracks
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct THPVideoInfo
    {
        public buint _xSize; // width  of video
        public buint _ySize; // height of video
        public buint _videoType;

        public enum VideoType
        {
            NonInterlaced = 0,
            OddInterlaced = 1,
            EvenInterlaced = 2
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct THPFrameCompInfo
    {
        public buint _numComponents;      // a number of Components in a frame
        public fixed byte _frameComp[16]; // kind of Components
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct THPFrameHeader
    {
        public buint _frameSizeNext;
        public buint _frameSizePrevious;
        public buint _firstComp; //up to 16

        public buint* CompAddr => (buint*) _firstComp.Address;

        public VoidPtr GetComp(int numComp, int index)
        {
            uint offset = 8 + 4 * (uint) numComp.Clamp(0, 15);
            for (int i = 0; i < index; i++)
            {
                offset += CompAddr[i];
            }

            return Address + offset;
        }

        internal VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct THPFile
    {
        public THPHeader _header;
        public THPFrameCompInfo _frameCompInfo;
        public THPVideoInfo _videoInfo;
        public THPAudioInfo _audioInfo;
    }

    public unsafe struct ThpAudioFrameHeader
    {
        //50 bytes

        public buint _blockSize; //For both channels
        public buint _numSamples;

        public fixed short _chan1Coefs[16];
        public fixed short _chan2Coefs[16];

        public bshort _c1yn1;
        public bshort _c1yn2;
        public bshort _c2yn1;
        public bshort _c2yn2;

        internal VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }

        internal VoidPtr Audio => Address + 0x50;

        public byte* GetAudioChannel(int channel)
        {
            return (byte*) Audio + (uint) (channel * _blockSize);
        }

        public short[] Coefs1
        {
            get
            {
                short[] arr = new short[16];
                fixed (short* ptr = _chan1Coefs)
                {
                    bshort* sPtr = (bshort*) ptr;
                    for (int i = 0; i < 16; i++)
                    {
                        arr[i] = sPtr[i];
                    }
                }

                return arr;
            }
        }

        public short[] Coefs2
        {
            get
            {
                short[] arr = new short[16];
                fixed (short* ptr = _chan2Coefs)
                {
                    bshort* sPtr = (bshort*) ptr;
                    for (int i = 0; i < 16; i++)
                    {
                        arr[i] = sPtr[i];
                    }
                }

                return arr;
            }
        }

        /*
Directly after the ThpAudioFrameHeader ThpAudioFrameHeader.channelSize bytes follow for the first channel, and if the video is stereo (ThpAudioInfo.numChannels = 2), that many bytes follow for the second channel.

The audio data is made up of small packets of 8 byte, each packet contains 14 samples. Some kind of adpcm coding is used. A sample is calculated like this:

newSample = previousSample*factor1 + sampleBeforePreviousSample*factor2 + (sampleData * 2^exponent);

For each packet, the first byte stores factor1, factor2 and exponent:
u8 index = (firstByte >> 4) & 0x7; //highest bit of byte is ignored
u8 exponent = firstByte & 0xf;
float factor1 = ThpAudioFrameHeader.table[2*index]/pow(2.f, 11);
float factor2 = ThpAudioFrameHeader.table[2*index + 1]/pow(2.f, 11);

The following 7 bytes store 14 sampleData (each 4 bit, interpreted as a signed two's complement number).
         */
    }

    /*

.thp files
==========

Version 1.0 (20050515)

.thp is a video format on the gamecube. The video frames are independent "quasi-jpegs", and if audio frames are present, they are in an adpcm format (described below).


Header data
-----------

The file starts with a thp header:


struct ThpHeader
{
  char tag[4]; //'THP\0'

  u32 version; //0x00011000 = 1.1, 0x00010000 = 1.0
  u32 maxBufferSize; //maximal buffer size needed for one complete frame (header + video + audio)
  u32 maxAudioSamples; //!= 0 if sound is stored in file, maximal number of samples in one frame.
                       //you can use this field to check if file contains audio.


  float fps; //usually 29.something (=0x41efc28f) for ntsc
  u32 numFrames; //number of frames in the thp file
  u32 firstFrameSize; //size of first frame (header + video + audio)

  u32 dataSize; //size of all frames (not counting the thp header structures)

  u32 componentDataOffset; //ThpComponents stored here (see below)
  u32 offsetsDataOffset; //?? if != 0, offset to table with offsets of all frames?

  u32 firstFrameOffset; //offset to first frame's data
  u32 lastFrameOffset; //offset to last frame's data
};

At ThpHeader.componentDataOffset, a ThpComponents structure is stored:

struct ThpComponents
{
  u32 numComponents; //usually 1 or 2 (video or video + audio)

  //component type 0 is video, type 1 is audio,
  //type 0xff is "no component" (numComponent many entries
  //are != 0xff)
  u8 componentTypes[16];
};

The first ThpComponents.numComponents entries of ThpComponents.componentTypes are valid. For each component, an information structure is stored after the ThpComponents struct.

Component type 0 is video, a ThpVideoInfo struct looks like this:

struct ThpVideoInfo
{
  u32 width;
  u32 height;
  u32 unknown; //only for version 1.1 thp files
};

Component type 1 is audio (not always included), a ThpAudioInfo struct looks like this:

ThpAudioInfo
{
  u32 numChannels;
  u32 frequency;
  u32 numSamples;
  u32 numData; //only for version 1.1 - that many
               //audio blocks are stored after each video block
               //(for surround sound?)
};


Frame data
----------

A frame is made up of a frame header followed by a video frame followed by ThpAudioInfo.numData audio frames (only if the video contains sound).

The frame header consists of 3 (or 4, if the video contains sound) u32's:
struct FrameHeader
{
  u32 nextTotalSize; //total size of NEXT frame (frame header, video and audio)
  u32 prevTotalSize; //total size of PREVIOUS frame
  u32 imageSize; //size of image frame of THIS frame
  (u32 audioSize; //size of one audio frame of THIS frame) <- only if the file contains audio
};


Directly after the frame header FrameHeader.imageSize bytes video information follow. Directly after the video information, ThpAudioInfo.numData audio frames follow, each Frameheader.audioSize bytes large (only if the file contains audio).

Video Frames
------------

A video frame is more or less a jpeg image. A jpeg file is structured by several markers. A marker is a two-byte code, the first of the two bytes is 0xff. The jpeg standard states that if you want to store the value 0xff, you have to store it as 0xff 0x00 (else it would be confused with a marker). This is NOT the case in .thp files, the value 0xff is stored simply as 0xff in the image data. So if you want to use jpeglib to read the frame, you have to convert the thp "quasi-jpeg" to a real jpeg by converting 0xff values to 0xff 0x00 in the image data. You have to be careful that you don't convert the terminating End-Of-Image marker, though.

- search for Start-Of-Image marker (0xff 0xda)
- search for End-Of-Image marker (0xff 0xd9) (start search at end of buffer and search backwards!)
- convert each 0xff between image data start and image data end to 0xff 0x00
- the resulting buffer can be passed to jpeglib to let it decode the image for you

Audio Frames
------------

An audio frame starts with a ThpAudioFrameHeader:

struct ThpAudioFrameHeader
{
  //size 80 byte
  u32 channelSize; //size of one channel in bytes
                   //(audio frame size = sizeof(ThpAudioFrameHeader)
                   // + ThpAudioInfo.numChannels*ThpAudioFrameHeader.channelSize)
  u32 numSamples; //number of samples/channel
  
  //the following tables store 5.11 fixed point numbers
  s16 table1[16]; //table for first channel
  s16 table2[16]; //table for second channel (stored for one channel videos as well)
  
  //these are explained below
  s16 channel1Prev1;
  s16 channel1Prev2;
  s16 channel2Prev1;
  s16 channel2Prev2;
};

Directly after the ThpAudioFrameHeader ThpAudioFrameHeader.channelSize bytes follow for the first channel, and if the video is stereo (ThpAudioInfo.numChannels = 2), that many bytes follow for the second channel.

The audio data is made up of small packets of 8 byte, each packet contains 14 samples. Some kind of adpcm coding is used. A sample is calculated like this:

newSample = previousSample*factor1 + sampleBeforePreviousSample*factor2 + (sampleData * 2^exponent);

For each packet, the first byte stores factor1, factor2 and exponent:
u8 index = (firstByte >> 4) & 0x7; //highest bit of byte is ignored
u8 exponent = firstByte & 0xf;
float factor1 = ThpAudioFrameHeader.table[2*index]/pow(2.f, 11);
float factor2 = ThpAudioFrameHeader.table[2*index + 1]/pow(2.f, 11);

The following 7 bytes store 14 sampleData (each 4 bit, interpreted as a signed two's complement number).

You might want to do this with fixed point math for efficiency.


You can find sample code at my website (thpplay, files gcvid.h/cpp).
thakis (with some of the thp file header information taken from monks thp player).
www.amnoid.de/gc/

     */
}