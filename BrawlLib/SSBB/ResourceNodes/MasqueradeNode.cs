using BrawlLib.Internal;
using BrawlLib.SSBB.Types.BrawlEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class MasqueradeNode : ResourceNode
    {
        internal VoidPtr Header => WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MASQ;

        public byte _cosmeticSlot; // Recieved from filename since it isn't referenced internally

        public int Size { get; private set; }

        public static readonly string[] MasqueradeIDs =
        {
            "00 - Mario",
            "01 - Donkey Kong",
            "02 - Link",
            "03 - Samus",
            "04 - Zero Suit Samus",
            "05 - Yoshi",
            "06 - Kirby",
            "07 - Fox",
            "08 - Pikachu",
            "09 - Luigi",
            "10 - Captain Falcon",
            "11 - Ness",
            "12 - Bowser / Giga Bowser",
            "13 - Peach",
            "14 - Zelda",
            "15 - Sheik",
            "16 - Ice Climbers",
            "17 - Marth",
            "18 - Mr. Game & Watch",
            "19 - Falco",
            "20 - Ganondorf",
            "21 - Wario / Wario-Man",
            "22 - Meta Knight",
            "23 - Pit",
            "24 - Olimar",
            "25 - Lucas",
            "26 - Diddy Kong",
            "27 - Pokemon Trainer",
            "28 - Charizard",
            "29 - Squirtle",
            "30 - Ivysaur",
            "31 - King Dedede",
            "32 - Lucario",
            "33 - Ike",
            "34 - R.O.B.",
            "35 - Jigglypuff",
            "36 - Toon Link",
            "37 - Wolf",
            "38 - Snake",
            "39 - Sonic",
            "40 - Mewtwo",
            "41 - Roy",
            "42 - Knuckles"
        };

        public static readonly string[] MasqueradeInternalNames =
        {
            "Mario",
            "Donkey",
            "Link",
            "Samus",
            "SZerosuit",
            "Yoshi",
            "Kirby",
            "Fox",
            "Pikachu",
            "Luigi",
            "Captain",
            "Ness",
            "Koopa/GKoopa",
            "Peach",
            "Zelda",
            "Sheik",
            "Popo",
            "Marth",
            "GameWatch",
            "Falco",
            "Ganon",
            "Wario/WarioMan",
            "Metaknight",
            "Pit",
            "Pikmin",
            "Lucas",
            "Diddy",
            "PokeTrainer",
            "PokeLizardon",
            "PokeZenigame",
            "PokeFushigisou",
            "Dedede",
            "Lucario",
            "Ike",
            "Robot",
            "Purin",
            "ToonLink",
            "Wolf",
            "Snake",
            "Sonic",
            "Mewtwo",
            "Roy",
            "Knuckles"
        };

        public override void OnPopulate()
        {
            MasqueradeEntryNode end = new MasqueradeEntryNode(true);
            for (int i = 0; i < Size / 2; i++)
            {
                new MasqueradeEntryNode().Initialize(this, new DataSource(Header[i, 2], 2));
                MasqueradeEntryNode m = (MasqueradeEntryNode) Children[Children.Count - 1];
                if (m.Color == end.Color && m.CostumeID == end.CostumeID)
                {
                    RemoveChild(m);
                    _changed = false;
                    break;
                }
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            uint offset = 0x00;
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                r.Rebuild(address + offset, 2, true);
                offset += 2;
            }

            MasqueradeEntryNode end = new MasqueradeEntryNode(true);
            end.Rebuild(address + offset, 2, true);
            offset += 2;
            while (offset < Size)
            {
                MasqueradeEntryNode blank = new MasqueradeEntryNode(false);
                blank.Rebuild(address + offset, 2, true);
                offset += 2;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return Size;
        }

        public override bool OnInitialize()
        {
            byte.TryParse(Path.GetFileNameWithoutExtension(_origPath), out _cosmeticSlot);
            if (_name == null && _origPath != null)
            {
                _name = _cosmeticSlot >= MasqueradeIDs.Length ? $"{_cosmeticSlot}" : MasqueradeIDs[_cosmeticSlot];
            }

            Size = OriginalSource.Length;

            return true;
        }
    }

    public unsafe class MasqueradeEntryNode : ResourceNode
    {
        internal CSSCEntry* Header => (CSSCEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MASQEntry;

        public byte _colorID;
        public byte _costumeID;

        public MasqueradeEntryNode()
        {
            _colorID = 0x00;
            _costumeID = 0x00;
        }

        // Defaults to the costume end marker
        public MasqueradeEntryNode(bool end)
        {
            _colorID = (byte) (end ? 0x0C : 0x00);
            _costumeID = 0x00;
        }

        [Category("Costume")]
        [DisplayName("Costume ID")]
        public byte CostumeID
        {
            get => _costumeID;
            set
            {
                if (((MasqueradeNode) Parent)._cosmeticSlot == 21 && (
                    value == 15 ||
                    value == 31 ||
                    value == 47 ||
                    value == 63))
                {
                    if (System.Windows.Forms.MessageBox.Show(
                            "Costume slot " + value +
                            " is known to be bugged for WarioMan. Are you sure you'd like to proceed?",
                            "Warning",
                            System.Windows.Forms.MessageBoxButtons.YesNo) !=
                        System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }

                _costumeID = value;
                regenName();
                SignalPropertyChange();
            }
        }

        [Category("Costume")]
        [TypeConverter(typeof(DropDownListBrawlExColorIDs))]
        [DisplayName("Color")]
        public byte Color
        {
            get => _colorID;
            set
            {
                _colorID = value;
                regenName();
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return CSSCEntry.Size;
        }

        public override bool OnInitialize()
        {
            _colorID = Header->_colorID;
            _costumeID = Header->_costumeID;
            if (_name == null)
            {
                regenName();
                IsDirty = false;
            }

            return false;
        }

        public void regenName()
        {
            Name = "Fit" +
                   (((MasqueradeNode) Parent)._cosmeticSlot >= MasqueradeNode.MasqueradeInternalNames.Length
                       ? $"Fighter{((MasqueradeNode) Parent)._cosmeticSlot}_"
                       : MasqueradeNode.MasqueradeInternalNames[((MasqueradeNode) Parent)._cosmeticSlot]) +
                   _costumeID.ToString("00") + (BrawlExColorID.Colors.Length > _colorID
                       ? " - " + BrawlExColorID.Colors[_colorID].Name
                       : "");
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CSSCEntry* hdr = (CSSCEntry*) address;
            hdr->_colorID = _colorID;
            hdr->_costumeID = _costumeID;
        }

        public List<string> GetCostumeFilePath(string currentPath)
        {
            List<string> files = new List<string>();
            if (((MasqueradeNode) Parent)._cosmeticSlot >= MasqueradeNode.MasqueradeInternalNames.Length)
            {
                return files;
            }

            if ((currentPath = currentPath.Substring(0, currentPath.LastIndexOf('\\'))).EndsWith(
                "pf\\info\\costumeslots", StringComparison.OrdinalIgnoreCase))
            {
                currentPath = currentPath.Substring(0,
                    currentPath.LastIndexOf("info", StringComparison.OrdinalIgnoreCase));
                List<string> internalNames = MasqueradeNode
                    .MasqueradeInternalNames[((MasqueradeNode) Parent)._cosmeticSlot]
                    .Split('/').ToList();
                foreach (string s in internalNames)
                {
                    if (File.Exists($"{currentPath}\\fighter\\{s}\\Fit{s}{_costumeID:00}.pac"))
                    {
                        files.Add($"{currentPath}\\fighter\\{s}\\Fit{s}{_costumeID:00}.pac");
                    }
                }
            }

            return files;
        }
    }
}
