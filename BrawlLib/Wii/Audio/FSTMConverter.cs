using BrawlLib.Internal;
using BrawlLib.SSBB.Types.Audio;
using System;

namespace BrawlLib.Wii.Audio
{
    public static class FSTMConverter
    {
        public static unsafe byte[] FromRSTM(byte[] rstm)
        {
            fixed (byte* ptr = rstm)
            {
                return FromRSTM((RSTMHeader*) ptr);
            }
        }

        internal static unsafe byte[] FromRSTM(RSTMHeader* rstm)
        {
            StrmDataInfo strmDataInfo = *rstm->HEADData->Part1;
            int channels = strmDataInfo._format._channels;

            if (strmDataInfo._format._encoding != (byte) WaveEncoding.ADPCM)
            {
                throw new NotImplementedException("FSTM export only supports ADPCM encoding.");
            }

            // Get section sizes from the BRSTM - BFSTM is such a similar format that we can assume the sizes will match.
            int rstmSize = 0x40;
            int infoSize = rstm->_headLength;
            int seekSize = rstm->_adpcLength;
            int dataSize = rstm->_dataLength;

            //Create byte array
            byte[] array = new byte[rstmSize + infoSize + seekSize + dataSize];

            fixed (byte* address = array)
            {
                //Get section pointers
                FSTMHeader* fstm = (FSTMHeader*) address;
                FSTMINFOHeader* info = (FSTMINFOHeader*) ((byte*) fstm + rstmSize);
                FSTMSEEKHeader* seek = (FSTMSEEKHeader*) ((byte*) info + infoSize);
                FSTMDATAHeader* data = (FSTMDATAHeader*) ((byte*) seek + seekSize);

                //Initialize sections
                fstm->Set(infoSize, seekSize, dataSize);
                info->Set(infoSize, channels);
                seek->Set(seekSize);
                data->Set(dataSize);

                //Set HEAD data
                info->_dataInfo = new FSTMDataInfo(strmDataInfo);

                //Create one ADPCMInfo for each channel
                IntPtr* adpcData = stackalloc IntPtr[channels];
                FSTMADPCMInfo** pAdpcm = (FSTMADPCMInfo**) adpcData;
                for (int i = 0; i < channels; i++)
                {
                    *(pAdpcm[i] = info->GetChannelInfo(i)) = new FSTMADPCMInfo(*rstm->HEADData->GetChannelInfo(i));
                }

                bshort* seekFrom = (bshort*) rstm->ADPCData->Data;
                bshort* seekTo = (bshort*) seek->Data;
                for (int i = 0; i < seek->_length / 2 - 8; i++)
                {
                    *seekTo++ = *seekFrom++;
                }

                VoidPtr dataFrom = rstm->DATAData->Data;
                VoidPtr dataTo = data->Data;
                VoidPtr dataEnd = address + array.LongLength;
                Memory.Move(dataTo, dataFrom, (uint)(dataEnd - dataTo));
            }

            return array;
        }

        public static unsafe byte[] ToRSTM(byte[] fstm)
        {
            fixed (byte* ptr = fstm)
            {
                return ToRSTM((FSTMHeader*) ptr);
            }
        }

        internal static unsafe byte[] ToRSTM(FSTMHeader* fstm)
        {
            FSTMDataInfo fstmDataInfo = fstm->INFOData->_dataInfo;
            int channels = fstmDataInfo._format._channels;

            // Get section sizes from the BRSTM - BFSTM is such a similar format that we can assume the sizes will match.
            int rstmSize = 0x40;
            int infoSize = fstm->_infoBlockSize;
            int seekSize = fstm->_seekBlockSize;
            int dataSize = fstm->_dataBlockSize;

            //Create byte array
            byte[] array = new byte[rstmSize + infoSize + seekSize + dataSize];

            fixed (byte* address = array)
            {
                //Get section pointers
                RSTMHeader* rstm = (RSTMHeader*) address;
                HEADHeader* info = (HEADHeader*) ((byte*) rstm + rstmSize);
                ADPCHeader* seek = (ADPCHeader*) ((byte*) info + infoSize);
                RSTMDATAHeader* data = (RSTMDATAHeader*) ((byte*) seek + seekSize);

                //Initialize sections
                rstm->Set(infoSize, seekSize, dataSize);
                info->Set(infoSize, channels, (WaveEncoding) fstm->INFOData->_dataInfo._format._encoding);
                seek->Set(seekSize);
                data->Set(dataSize);

                //Set HEAD data
                *info->Part1 = new StrmDataInfo(fstmDataInfo, rstmSize + infoSize + seekSize + 0x20);

                //Create one ADPCMInfo for each channel
                IntPtr* adpcData = stackalloc IntPtr[channels];
                ADPCMInfo** pAdpcm = (ADPCMInfo**) adpcData;
                for (int i = 0; i < channels; i++)
                {
                    *(pAdpcm[i] = info->GetChannelInfo(i)) = new ADPCMInfo(*fstm->INFOData->GetChannelInfo(i));
                }

                bshort* seekFrom = (bshort*) fstm->SEEKData->Data;
                bshort* seekTo = (bshort*) seek->Data;
                for (int i = 0; i < seek->_length / 2 - 8; i++)
                {
                    *seekTo++ = *seekFrom++;
                }

                VoidPtr dataFrom = fstm->DATAData->Data;
                VoidPtr dataTo = data->Data;
                Memory.Move(dataTo, dataFrom, (uint) data->_length - 8);
            }

            return array;
        }
    }
}