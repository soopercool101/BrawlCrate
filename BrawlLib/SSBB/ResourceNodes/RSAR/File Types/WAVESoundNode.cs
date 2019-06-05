using System;
using System.Audio;
using System.IO;
using System.Windows.Forms;
using BrawlLib.IO;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class RSARFileEntryNode : ResourceNode
    {
        internal VoidPtr _offset;

        internal RSARNode RSARNode
        {
            get
            {
                ResourceNode n = this;
                while ((n = n.Parent) != null && !(n is RSARNode)) ;

                return n as RSARNode;
            }
        }
    }

    public unsafe class WAVESoundNode : RSARFileAudioNode
    {
        public override bool OnInitialize()
        {
            if (_name == null) _name = string.Format("[{0}]Audio", Index);

            Info = *(WaveInfo*) WorkingUncompressed.Address;

            if (!_replaced)
                SetSizeInternal(WaveInfo.Size + Info._format._channels *
                                (4 + ChannelInfo.Size + (Info._format._encoding == 2 ? ADPCMInfo.Size : 0)));

            return false;
        }

        public void GetAudio()
        {
            uint _audioLen;
            var _dataAddr = ((RSARFileNode) Parent._parent)._audioSource.Address + Info._dataLocation;

            var nextBiggest = (uint) ((RSARFileNode) Parent._parent)._audioSource.Length;
            foreach (WAVESoundNode s in Parent.Children)
                if (s != this && s.Info._dataLocation > Info._dataLocation && s.Info._dataLocation < nextBiggest)
                    nextBiggest = s.Info._dataLocation;

            _audioLen = nextBiggest - Info._dataLocation;

            Init(_dataAddr, (int) _audioLen, (WaveInfo*) WorkingUncompressed.Address);
        }

        public override void Replace(string fileName)
        {
            if (fileName.EndsWith(".wav"))
                using (var dlg = new BrstmConverterDialog())
                {
                    dlg.Type = 1;
                    dlg.AudioSource = fileName;
                    if (dlg.ShowDialog(null) == DialogResult.OK) ReplaceRaw(dlg.AudioData);
                }
            else
                base.Replace(fileName);

            Init(WorkingUncompressed.Address + Info._dataLocation,
                (int) (WorkingUncompressed.Length - Info._dataLocation), (WaveInfo*) WorkingUncompressed.Address);

            //Cut out the audio samples from the imported data
            SetSizeInternal((int) Info._dataLocation);

            UpdateCurrentControl();
            SignalPropertyChange();
            Parent.Parent.SignalPropertyChange();
            if (RSARNode != null) RSARNode.SignalPropertyChange();
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
                            *(bint*) (addr + 0x14) = WorkingUncompressed.Length;

                            addr += WorkingUncompressed.Length;

                            //Append audio samples to the end
                            Memory.Move(addr, _audioSource.Address, (uint) _audioSource.Length);
                        }
                    }
                }
                else
                {
                    base.Export(outPath);
                }
            }
        }
    }
}