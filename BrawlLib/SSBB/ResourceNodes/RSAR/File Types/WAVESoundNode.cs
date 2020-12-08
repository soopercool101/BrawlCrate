using BrawlLib.Internal;
using BrawlLib.Internal.Audio;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.SSBB.Types.Audio;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class RSARFileEntryNode : ResourceNode
    {
        internal RSARNode RSARNode
        {
            get
            {
                ResourceNode n = this;
                while ((n = n.Parent) != null && !(n is RSARNode))
                {
                    ;
                }

                return n as RSARNode;
            }
        }

        internal VoidPtr _offset;
    }

    public unsafe class WAVESoundNode : RSARFileAudioNode
    {
        public override bool OnInitialize()
        {
            if (_name == null)
            {
                _name = $"[{Index}]Audio";
            }

            Info = *(WaveInfo*) WorkingUncompressed.Address;

            if (!_replaced)
            {
                SetSizeInternal(WaveInfo.Size + Info._format._channels *
                    (4 + ChannelInfo.Size + (Info._format._encoding == 2 ? ADPCMInfo.Size : 0)));
            }

            return false;
        }

        public void GetAudio()
        {
            uint _audioLen;
            VoidPtr _dataAddr = ((RSARFileNode) Parent._parent)._audioSource.Address + Info._dataLocation;

            uint nextBiggest = (uint) ((RSARFileNode) Parent._parent)._audioSource.Length;
            foreach (WAVESoundNode s in Parent.Children)
            {
                if (s != this && s.Info._dataLocation > Info._dataLocation && s.Info._dataLocation < nextBiggest)
                {
                    nextBiggest = s.Info._dataLocation;
                }
            }

            _audioLen = nextBiggest - Info._dataLocation;

            Init(_dataAddr, (int) _audioLen, (WaveInfo*) WorkingUncompressed.Address);
        }

        public override void Replace(string fileName)
        {
            if (fileName.EndsWith(".wav"))
            {
                using (BrstmConverterDialog dlg = new BrstmConverterDialog())
                {
                    dlg.Type = 1;
                    dlg.AudioSource = fileName;
                    if (dlg.ShowDialog(null) == DialogResult.OK)
                    {
                        ReplaceRaw(dlg.AudioData);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                base.Replace(fileName);
            }

            Init(WorkingUncompressed.Address + Info._dataLocation,
                (int) (WorkingUncompressed.Length - Info._dataLocation), (WaveInfo*) WorkingUncompressed.Address);

            //Cut out the audio samples from the imported data
            SetSizeInternal((int) Info._dataLocation);

            UpdateCurrentControl();
            SignalPropertyChange();
            Parent.Parent.SignalPropertyChange();
            RSARNode?.SignalPropertyChange();
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