using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Audio
{
    public static class CSTMConverter
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
            var strmDataInfo = *rstm->HEADData->Part1;
            int channels = strmDataInfo._format._channels;

            if (strmDataInfo._format._encoding != (byte) WaveEncoding.ADPCM)
                throw new NotImplementedException("CSTM export only supports ADPCM encoding.");

            // Get section sizes from the BRSTM - BCSTM is such a similar format that we can assume the sizes will match.
            var rstmSize = 0x40;
            int infoSize = rstm->_headLength;
            int seekSize = rstm->_adpcLength;
            int dataSize = rstm->_dataLength;

            //Create byte array
            var array = new byte[rstmSize + infoSize + seekSize + dataSize];

            fixed (byte* address = array)
            {
                //Get section pointers
                var cstm = (CSTMHeader*) address;
                var info = (CSTMINFOHeader*) ((byte*) cstm + rstmSize);
                var seek = (CSTMSEEKHeader*) ((byte*) info + infoSize);
                var data = (CSTMDATAHeader*) ((byte*) seek + seekSize);

                //Initialize sections
                cstm->Set(infoSize, seekSize, dataSize);
                info->Set(infoSize, channels);
                seek->Set(seekSize);
                data->Set(dataSize);

                //Set HEAD data
                info->_dataInfo = new CSTMDataInfo(strmDataInfo);

                //Create one ADPCMInfo for each channel
                var adpcData = stackalloc IntPtr[channels];
                var pAdpcm = (CSTMADPCMInfo**) adpcData;
                for (var i = 0; i < channels; i++)
                    *(pAdpcm[i] = info->GetChannelInfo(i)) = new CSTMADPCMInfo(*rstm->HEADData->GetChannelInfo(i));

                var seekFrom = (bshort*) rstm->ADPCData->Data;
                var seekTo = (short*) seek->Data;
                for (var i = 0; i < seek->_length / 2 - 8; i++) *seekTo++ = *seekFrom++;

                var dataFrom = rstm->DATAData->Data;
                var dataTo = data->Data;
                Memory.Move(dataTo, dataFrom, (uint) data->_length - 8);
            }

            return array;
        }

        public static unsafe byte[] ToRSTM(byte[] cstm)
        {
            fixed (byte* ptr = cstm)
            {
                return ToRSTM((CSTMHeader*) ptr);
            }
        }

        internal static unsafe byte[] ToRSTM(CSTMHeader* cstm)
        {
            var cstmDataInfo = cstm->INFOData->_dataInfo;
            int channels = cstmDataInfo._format._channels;

            if (cstm->_seekBlockRef._type != CSTMReference.RefType.SeekBlock)
                throw new Exception(
                    "BrawlLib does not recognize this type of CSTM file (the SEEK block is missing or in an unexpected location.)");

            // Get section sizes from the BRSTM - BCSTM is such a similar format that we can assume the sizes will match.
            var rstmSize = 0x40;
            var infoSize = cstm->_infoBlockSize;
            var seekSize = cstm->_seekBlockSize;
            var dataSize = cstm->_dataBlockSize;

            //Create byte array
            var array = new byte[rstmSize + infoSize + seekSize + dataSize];

            fixed (byte* address = array)
            {
                //Get section pointers
                var rstm = (RSTMHeader*) address;
                var info = (HEADHeader*) ((byte*) rstm + rstmSize);
                var seek = (ADPCHeader*) ((byte*) info + infoSize);
                var data = (RSTMDATAHeader*) ((byte*) seek + seekSize);

                //Initialize sections
                rstm->Set(infoSize, seekSize, dataSize);
                info->Set(infoSize, channels, (WaveEncoding) cstm->INFOData->_dataInfo._format._encoding);
                seek->Set(seekSize);
                data->Set(dataSize);

                //Set HEAD data
                *info->Part1 = new StrmDataInfo(cstmDataInfo, rstmSize + infoSize + seekSize + 0x20);

                //Create one ADPCMInfo for each channel
                var adpcData = stackalloc IntPtr[channels];
                var pAdpcm = (ADPCMInfo**) adpcData;
                for (var i = 0; i < channels; i++)
                    *(pAdpcm[i] = info->GetChannelInfo(i)) = new ADPCMInfo(*cstm->INFOData->GetChannelInfo(i));

                var seekFrom = (bshort*) cstm->SEEKData->Data;
                var seekTo = (short*) seek->Data;
                for (var i = 0; i < seek->_length / 2 - 8; i++) *seekTo++ = *seekFrom++;

                var dataFrom = cstm->DATAData->Data;
                var dataTo = data->Data;
                Memory.Move(dataTo, dataFrom, (uint) data->_length - 8);
            }

            return array;
        }
    }
}