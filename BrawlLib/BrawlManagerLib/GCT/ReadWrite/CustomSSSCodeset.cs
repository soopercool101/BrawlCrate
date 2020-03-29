using BrawlLib.BrawlManagerLib.GCT.ReadOnly;
using BrawlLib.BrawlManagerLib.Songs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrawlLib.BrawlManagerLib.GCT.ReadWrite
{
    /// <summary>
    /// Allows read-write access to the Custom SSS code, and read-only access to some other codes.
    /// This class will store the entire codeset, split between itself (the SSS code) and the portions before/after (in raw byte[]).
    /// </summary>
    public class CustomSSSCodeset
    {
        public byte[] sss1 { get; private set; }
        public byte[] sss2 { get; private set; }
        public byte[] sss3 { get; private set; }

        public byte[] DataBefore { get; private set; }
        public byte[] DataAfter { get; private set; }

        /// <summary>
        /// If there is more than one Custom SSS code in the file, this variable will be incremented.
        /// </summary>
        public int OtherCodesIgnoredInSameFile { get; private set; }

        /// <summary>
        /// BrawlBox puts metadata after the end of the GCT so it can remember which codes are in the file, and where.
        /// SSS Editor doesn't retain this but will warn the user that it will be lost if it exists.
        /// </summary>
        public bool IgnoredMetadata { get; private set; }

        public Tuple<byte, byte> this[int index] => new Tuple<byte, byte>(sss3[2 * index], sss3[2 * index + 1]);

        private Tuple<byte[], byte[]> _iconsInGroups;

        public Tuple<byte[], byte[]> IconsInGroups
        {
            get
            {
                if (_iconsInGroups == null)
                {
                    byte[] b1 = new byte[sss1.Length];
                    for (int i = 0; i < b1.Length; i++)
                    {
                        b1[i] = sss3[sss1[i] * 2 + 1];
                    }

                    byte[] b2 = new byte[sss2.Length];
                    for (int i = 0; i < b2.Length; i++)
                    {
                        b2[i] = sss3[sss2[i] * 2 + 1];
                    }

                    _iconsInGroups = new Tuple<byte[], byte[]>(b1, b2);
                }

                return _iconsInGroups;
            }
        }

        #region Other codes

        private StageDependentSongLoader _songLoaders;

        /// <summary>
        /// Gets read-only access to instances of the Stage-Dependent Song Loader in this codeset.
        /// </summary>
        public StageDependentSongLoader SongLoaders
        {
            get
            {
                if (_songLoaders == null)
                {
                    _songLoaders = new StageDependentSongLoader(DataBefore.Concat(DataAfter).ToArray());
                }

                return _songLoaders;
            }
        }

        private TracklistModifier _tracklistModifier;

        /// <summary>
        /// Gets read-only access to the instance of the TracklistModifier in this codeset.
        /// </summary>
        public TracklistModifier TracklistModifier
        {
            get
            {
                if (_tracklistModifier == null)
                {
                    _tracklistModifier = new TracklistModifier(DataBefore.Concat(DataAfter).ToArray());
                }

                return _tracklistModifier;
            }
        }

        private AltStageSongForcer _altStageSongForcer;

        /// <summary>
        /// Gets read-only access to the instance of the TracklistModifier in this codeset.
        /// </summary>
        public AltStageSongForcer AltStageSongForcer
        {
            get
            {
                if (_altStageSongForcer == null)
                {
                    _altStageSongForcer = new AltStageSongForcer(DataBefore.Concat(DataAfter).ToArray());
                }

                return _altStageSongForcer;
            }
        }

        private AlternateStageLoaderData _alternateStageLoaderData;

        /// <summary>
        /// Gets read-only access to instances of the Stage-Dependent Song Loader in this codeset.
        /// </summary>
        public AlternateStageLoaderData AlternateStageLoaderData
        {
            get
            {
                if (_alternateStageLoaderData == null)
                {
                    _alternateStageLoaderData = new AlternateStageLoaderData(DataBefore.Concat(DataAfter).ToArray());
                }

                return _alternateStageLoaderData;
            }
        }

        private CMM _cmm;

        /// <summary>
        /// Gets read-only access to instances of the CMM Setting Code [JOJI] in this codeset.
        /// </summary>
        public CMM CMM
        {
            get
            {
                if (_cmm == null)
                {
                    _cmm = new CMM(DataBefore.Concat(DataAfter).ToArray());
                }

                return _cmm;
            }
        }

        private CNMT _cnmt;

        /// <summary>
        /// Gets read-only access to instances of the CMM Setting Code [JOJI] in this codeset.
        /// </summary>
        public CNMT CNMT
        {
            get
            {
                if (_cnmt == null)
                {
                    _cnmt = new CNMT(DataBefore.Concat(DataAfter).ToArray());
                }

                return _cnmt;
            }
        }

        #endregion

        private byte[] _stageIDsInOrder;

        public byte[] StageIDsInOrder
        {
            get
            {
                if (_stageIDsInOrder == null)
                {
                    _stageIDsInOrder = new byte[sss1.Length + sss2.Length];
                    for (int i = 0; i < sss1.Length; i++)
                    {
                        _stageIDsInOrder[i] = sss3[sss1[i] * 2];
                    }

                    for (int i = 0; i < sss2.Length; i++)
                    {
                        _stageIDsInOrder[sss1.Length + i] = sss3[sss2[i] * 2];
                    }
                }

                return _stageIDsInOrder;
            }
        }

        public byte IconForStage(int stage_id)
        {
            for (int i = 0; i < sss3.Length; i += 2)
            {
                if (sss3[i] == stage_id)
                {
                    return sss3[i + 1];
                }
            }

            return 0xFF;
        }

        public byte StageForIcon(int icon_id)
        {
            for (int i = 0; i < sss3.Length; i += 2)
            {
                if (sss3[i + 1] == icon_id)
                {
                    return sss3[i];
                }
            }

            return 0xFF;
        }

        public Song GetSong(string filename, Song originalSong)
        {
            int stageID = StageIDMap.StageIDForPac(filename);
            if (AlternateStageLoaderData.TryGetDefinition(filename, out AlternateStageEntry entry))
            {
                foreach (AlternateStageEntry.Alternate alt in entry.ButtonActivated.Where(b =>
                    filename.EndsWith($"_{b.Letter}.pac",
                        StringComparison
                            .InvariantCultureIgnoreCase)))
                {
                    Song newSong = AltStageSongForcer.GetSong(stageID, alt.ButtonMask, originalSong);
                    if (newSong.ID != originalSong.ID)
                    {
                        return newSong;
                    }
                }
            }

            return SongLoaders.GetSong(stageID, originalSong);
        }

        public CustomSSSCodeset()
        {
            init(new byte[0]);
        }

        public CustomSSSCodeset(string[] s)
        {
            init(s);
        }

        public CustomSSSCodeset(byte[] data)
        {
            init(data);
        }

        public CustomSSSCodeset(string filename)
        {
            if (filename.EndsWith("gct", StringComparison.InvariantCultureIgnoreCase))
            {
                init(File.ReadAllBytes(filename));
            }
            else
            {
                init(File.ReadAllLines(filename));
            }
        }

        private CustomSSSCodeset(CustomSSSCodeset copyfrom, byte[] sss1, byte[] sss2, byte[] sss3)
        {
            this.sss1 = sss1;
            this.sss2 = sss2;
            this.sss3 = sss3;
            DataBefore = copyfrom.DataBefore;
            DataAfter = copyfrom.DataAfter;
        }

        private static byte[] gctheader = {0x00, 0xd0, 0xc0, 0xde, 0x00, 0xd0, 0xc0, 0xde};
        private static byte[] gctfooter = {0xf0, 0, 0, 0, 0, 0, 0, 0};

        private void init(string[] s)
        {
            Regex r = new Regex(@"(\* )?[A-Fa-f0-9]{8} [A-Fa-f0-9]{8}");
            IEnumerable<string> matching_lines =
                from line in s
                where r.IsMatch(line)
                select line;

            byte[] core = ByteUtilities.StringToByteArray(string.Join("\n", matching_lines));
            byte[] data = new byte[core.Length + 16];
            Array.ConstrainedCopy(gctheader, 0, data, 0, 8);
            Array.ConstrainedCopy(core, 0, data, 8, core.Length);
            Array.ConstrainedCopy(gctfooter, 0, data, data.Length - 8, 8);
            init(data);
        }

        private static byte[] SSS_HEADER = ByteUtilities.StringToByteArray("046b8f5c 7c802378");

        private void init(byte[] data)
        {
            OtherCodesIgnoredInSameFile = 0;
            IgnoredMetadata = false;
            int index = -1;
            for (int line = 0; line < data.Length; line += 8)
            {
                if (ByteUtilities.ByteArrayEquals(data, line, SSS_HEADER, 0, SSS_HEADER.Length))
                {
                    if (index != -1)
                    {
                        OtherCodesIgnoredInSameFile++;
                    }

                    index = line;
                }
            }

            if (index < 0)
            {
                if (data.Length > 0)
                {
                    MessageBox.Show("No custom SSS code found. A default code will be used.");
                }

                DataBefore = gctheader.ToArray();
                sss1 = ByteUtilities.StringToByteArray(
                    "00010203 04050709 080A0B0C 0D0E0F10 11141516 1A191217 0618131D 1E1B1C");
                sss2 = ByteUtilities.StringToByteArray("1F202122 23242526 2728");
                sss3 = ByteUtilities.StringToByteArray(
                    "01010202 03030404 05050606 07070808 0909330A 0B0B0C0C 0D0D0E0E 130F1410 " +
                    "15111612 17131814 19151C16 1D171E18 1F19201A 211B221C 231D241E 251F2932 " +
                    "2A332B34 2C352D36 2F373038 3139323A 2E3BFFFF");
                DataAfter = data.Skip(gctheader.Length).ToArray();
            }
            else
            {
                int start = index;
                DataBefore = new byte[start];
                Array.ConstrainedCopy(data, 0, DataBefore, 0, start);

                index += 14 * 8;
                byte sss1_count = data[index - 1];
                sss1 = new byte[sss1_count];
                Array.ConstrainedCopy(data, index, sss1, 0, sss1_count);

                index += sss1_count;
                while (index % 8 != 0)
                {
                    index++;
                }

                index += 2 * 8;
                byte sss2_count = data[index - 1];
                sss2 = new byte[sss2_count];
                Array.ConstrainedCopy(data, index, sss2, 0, sss2_count);

                index += sss2_count;
                while (index % 8 != 0)
                {
                    index++;
                }

                index += 1 * 8;
                byte sss3_count = data[index - 1];
                sss3 = new byte[sss3_count];
                Array.ConstrainedCopy(data, index, sss3, 0, sss3_count);

                index += sss3_count;
                while (index % 8 != 0)
                {
                    index++;
                }

                DataAfter = new byte[data.Length - index];
                Array.ConstrainedCopy(data, index, DataAfter, 0, data.Length - index);
            }

            bool footer_found = false;
            for (int i = 0; i < DataAfter.Length; i += 8)
            {
                if (footer_found)
                {
                    IgnoredMetadata = true;
                    DataAfter = DataAfter.Take(i).ToArray();
                    break;
                }

                if (ByteUtilities.ByteArrayEquals(DataAfter, i, gctfooter, 0, 8))
                {
                    footer_found = true;
                }
            }
        }

        public override string ToString()
        {
            return $"Custom SSS: {sss1.Length}/{sss2.Length} stages, from pool of {sss3.Length / 2} pairs";
        }
    }
}