using System;
using BrawlLib.SSBBTypes;

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
            var strmDataInfo = *rstm->HEADData->Part1;
            int channels = strmDataInfo._format._channels;

            if (strmDataInfo._format._encoding != (byte) WaveEncoding.ADPCM)
                throw new NotImplementedException("FSTM export only supports ADPCM encoding.");

            // Get section sizes from the BRSTM - BFSTM is such a similar format that we can assume the sizes will match.
            var rstmSize = 0x40;
            int infoSize = rstm->_headLength;
            int seekSize = rstm->_adpcLength;
            int dataSize = rstm->_dataLength;

            //Create byte array
            var array = new byte[rstmSize + infoSize + seekSize + dataSize];

            fixed (byte* address = array)
            {
                //Get section pointers
                var fstm = (FSTMHeader*) address;
                var info = (FSTMINFOHeader*) ((byte*) fstm + rstmSize);
                var seek = (FSTMSEEKHeader*) ((byte*) info + infoSize);
                var data = (FSTMDATAHeader*) ((byte*) seek + seekSize);

                //Initialize sections
                fstm->Set(infoSize, seekSize, dataSize);
                info->Set(infoSize, channels);
                seek->Set(seekSize);
                data->Set(dataSize);

                //Set HEAD data
                info->_dataInfo = new FSTMDataInfo(strmDataInfo);

                //Create one ADPCMInfo for each channel
                var adpcData = stackalloc IntPtr[channels];
                var pAdpcm = (FSTMADPCMInfo**) adpcData;
                for (var i = 0; i < channels; i++)
                    *(pAdpcm[i] = info->GetChannelInfo(i)) = new FSTMADPCMInfo(*rstm->HEADData->GetChannelInfo(i));

                var seekFrom = (bshort*) rstm->ADPCData->Data;
                var seekTo = (bshort*) seek->Data;
                for (var i = 0; i < seek->_length / 2 - 8; i++) *seekTo++ = *seekFrom++;

                var dataFrom = rstm->DATAData->Data;
                var dataTo = data->Data;
                Memory.Move(dataTo, dataFrom, (uint) data->_length);
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
            var fstmDataInfo = fstm->INFOData->_dataInfo;
            int channels = fstmDataInfo._format._channels;

            // Get section sizes from the BRSTM - BFSTM is such a similar format that we can assume the sizes will match.
            var rstmSize = 0x40;
            int infoSize = fstm->_infoBlockSize;
            int seekSize = fstm->_seekBlockSize;
            int dataSize = fstm->_dataBlockSize;

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
                info->Set(infoSize, channels, (WaveEncoding) fstm->INFOData->_dataInfo._format._encoding);
                seek->Set(seekSize);
                data->Set(dataSize);

                //Set HEAD data
                *info->Part1 = new StrmDataInfo(fstmDataInfo, rstmSize + infoSize + seekSize + 0x20);

                //Create one ADPCMInfo for each channel
                var adpcData = stackalloc IntPtr[channels];
                var pAdpcm = (ADPCMInfo**) adpcData;
                for (var i = 0; i < channels; i++)
                    *(pAdpcm[i] = info->GetChannelInfo(i)) = new ADPCMInfo(*fstm->INFOData->GetChannelInfo(i));

                var seekFrom = (bshort*) fstm->SEEKData->Data;
                var seekTo = (bshort*) seek->Data;
                for (var i = 0; i < seek->_length / 2 - 8; i++) *seekTo++ = *seekFrom++;

                var dataFrom = fstm->DATAData->Data;
                var dataTo = data->Data;
                Memory.Move(dataTo, dataFrom, (uint) data->_length - 8);
            }

            return array;
        }
    }
}