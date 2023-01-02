using BrawlLib.CustomLists;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace BrawlLib.Internal
{
    public class FranchiseIcon
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public FranchiseIcon(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static readonly FranchiseIcon[] Icons = new FranchiseIcon[]
        {
            //                ID     Display Name
            new FranchiseIcon(0x00, "Super Mario"),
            new FranchiseIcon(0x01, "Donkey Kong"),
            new FranchiseIcon(0x02, "Zelda"),
            new FranchiseIcon(0x03, "Metroid"),
            new FranchiseIcon(0x04, "Yoshi"),
            new FranchiseIcon(0x05, "Kirby"),
            new FranchiseIcon(0x06, "Star Fox"),
            new FranchiseIcon(0x07, "Pokémon"),
            new FranchiseIcon(0x08, "F-Zero"),
            new FranchiseIcon(0x09, "Earthbound"),
            new FranchiseIcon(0x0A, "Ice Climbers"),
            new FranchiseIcon(0x0B, "Wario"),
            new FranchiseIcon(0x0C, "Kid Icarus"),
            new FranchiseIcon(0x0D, "Pikmin"),
            new FranchiseIcon(0x0E, "Fire Emblem"),
            new FranchiseIcon(0x0F, "Gyromite"),
            new FranchiseIcon(0x10, "Metal Gear Solid"),
            new FranchiseIcon(0x11, "Game & Watch"),
            new FranchiseIcon(0x12, "Sonic the Hedgehog"),
            new FranchiseIcon(0x13, "Super Smash Bros."),
            new FranchiseIcon(0x14, "Bowser (PM)"),
            new FranchiseIcon(0xFF, "None")
        };
    }

    public class BrawlExColorID
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public BrawlExColorID(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static readonly BrawlExColorID[] Colors = new BrawlExColorID[]
        {
            //                 ID     Display Name
            new BrawlExColorID(0x00, "Red (Team Color)"),
            new BrawlExColorID(0x01, "Blue (Team Color)"),
            new BrawlExColorID(0x02, "Yellow"),
            new BrawlExColorID(0x03, "Green (Team Color)"),
            new BrawlExColorID(0x04, "Purple"),
            new BrawlExColorID(0x05, "Light Blue"),
            new BrawlExColorID(0x06, "Pink"),
            new BrawlExColorID(0x07, "Brown"),
            new BrawlExColorID(0x08, "Black"),
            new BrawlExColorID(0x09, "White"),
            new BrawlExColorID(0x0A, "Orange"),
            new BrawlExColorID(0x0B, "Grey")
        };
    }

    public class RecordBank
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public RecordBank(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static readonly RecordBank[] Records = new RecordBank[]
        {
            //             ID     Display Name
            new RecordBank(0x00, "Mario"),
            new RecordBank(0x01, "Donkey Kong"),
            new RecordBank(0x02, "Link"),
            new RecordBank(0x03, "Samus/Zero Suit Samus"),
            new RecordBank(0x04, "Yoshi"),
            new RecordBank(0x05, "Kirby"),
            new RecordBank(0x06, "Fox"),
            new RecordBank(0x07, "Pikachu"),
            new RecordBank(0x08, "Luigi"),
            new RecordBank(0x09, "Captain Falcon"),
            new RecordBank(0x0a, "Ness"),
            new RecordBank(0x0b, "Bowser"),
            new RecordBank(0x0c, "Peach"),
            new RecordBank(0x0d, "Zelda/Sheik"),
            new RecordBank(0x0e, "Ice Climbers"),
            new RecordBank(0x0f, "Falco"),
            new RecordBank(0x10, "Ganondorf"),
            new RecordBank(0x11, "Wario"),
            new RecordBank(0x12, "Meta Knight"),
            new RecordBank(0x13, "Pit"),
            new RecordBank(0x14, "Olimar"),
            new RecordBank(0x15, "Lucas"),
            new RecordBank(0x16, "Diddy Kong"),
            new RecordBank(0x17, "King Dedede"),
            new RecordBank(0x18, "Ike"),
            new RecordBank(0x19, "R.O.B."),
            new RecordBank(0x1a, "Snake"),
            new RecordBank(0x1b, "Pokémon Trainer"),
            new RecordBank(0x1c, "Lucario"),
            new RecordBank(0x1d, "Marth"),
            new RecordBank(0x1e, "Mr. Game & Watch"),
            new RecordBank(0x1f, "Jigglypuff"),
            new RecordBank(0x20, "Toon Link"),
            new RecordBank(0x21, "Wolf"),
            new RecordBank(0x22, "Sonic")
        };
    }

    public class AIController
    {
        public uint ID { get; private set; }
        public string Name { get; private set; }

        public AIController(uint id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static readonly AIController[] aIControllers = new AIController[]
        {
            //            ID     Display Name     
            new AIController(0x00, "Mario"),
            new AIController(0x01, "Donkey Kong"),
            new AIController(0x02, "Link"),
            new AIController(0x03, "Samus"),
            new AIController(0x04, "Yoshi"),
            new AIController(0x05, "Kirby"),
            new AIController(0x06, "Fox"),
            new AIController(0x07, "Pikachu"),
            new AIController(0x08, "Luigi"),
            new AIController(0x09, "Captain Falcon"),
            new AIController(0x0A, "Ness"),
            new AIController(0x0B, "Bowser"),
            new AIController(0x0C, "Peach"),
            new AIController(0x0D, "Zelda"),
            new AIController(0x0E, "Sheik"),
            new AIController(0x0F, "Popo"),
            new AIController(0x10, "Nana"),
            new AIController(0x11, "Marth"),
            new AIController(0x12, "Mr. Game & Watch"),
            new AIController(0x13, "Falco"),
            new AIController(0x14, "Ganondorf"),
            new AIController(0x15, "Wario"),
            new AIController(0x16, "Meta Knight"),
            new AIController(0x17, "Pit"),
            new AIController(0x18, "Zero Suit Samus"),
            new AIController(0x19, "Olimar"),
            new AIController(0x1A, "Lucas"),
            new AIController(0x1B, "Diddy Kong"),
            new AIController(0x1C, "Pokémon Trainer"),
            new AIController(0x1D, "Charizard"),
            new AIController(0x1E, "Squirtle"),
            new AIController(0x1F, "Ivysaur"),
            new AIController(0x20, "King Dedede"),
            new AIController(0x21, "Lucario"),
            new AIController(0x22, "Ike"),
            new AIController(0x23, "R.O.B."),
            new AIController(0x25, "Jigglypuff"),
            new AIController(0x29, "Toon Link"),
            new AIController(0x2C, "Wolf"),
            new AIController(0x2E, "Snake"),
            new AIController(0x2F, "Sonic"),
            new AIController(0x30, "Giga Bowser"),
            new AIController(0x31, "Warioman"),
            new AIController(0x32, "Alloys"),
            new AIController(0xFFFFFFFF, "None")
        };
    }

    public class DropDownListBrawlExIconIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FranchiseIcon.Icons
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                FranchiseIcon icon = FranchiseIcon.Icons.Where(s => s.ID == (byte) value).FirstOrDefault();
                return "0x" + ((byte) value).ToString("X2") + (icon == null ? "" : " - " + icon.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExColorIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BrawlExColorID.Colors
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                BrawlExColorID color = BrawlExColorID.Colors.Where(s => s.ID == (byte) value).FirstOrDefault();
                return "0x" + ((byte) value).ToString("X2") + (color == null ? "" : " - " + color.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExRecordIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(RecordBank.Records
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                RecordBank record = RecordBank.Records.Where(s => s.ID == (byte) value).FirstOrDefault();
                return "0x" + ((byte) value).ToString("X2") + (record == null ? "" : " - " + record.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    // Different from fighterIDs as it includes EX48-4A
    public class DropDownListBrawlExSlotIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FighterNameGenerators.slotIDList
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                SSBB.Fighter fighter = FighterNameGenerators.slotIDList
                    .Where(s => s.ID == (byte) value).FirstOrDefault();
                return "0x" + ((byte) value).ToString("X2") + (fighter == null ? "" : " - " + fighter.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    // Used for single-player modes, includes +s flag list items
    public class DropDownListBrawlExSlotIDsSinglePlayer : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FighterNameGenerators.singlePlayerSlotIDList
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                SSBB.Fighter fighter = FighterNameGenerators.singlePlayerSlotIDList
                    .Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (fighter == null ? "" : " - " + fighter.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    // Used by SLTC
    public class DropDownListBrawlExFighterIDsLong : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FighterNameGenerators.fighterIDLongList
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToUInt32(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(uint))
            {
                SSBB.Fighter fighter = FighterNameGenerators.fighterIDLongList
                    .Where(s => s.ID == (uint) value).FirstOrDefault();
                return "0x" + ((uint) value).ToString("X8") + (fighter == null ? "" : " - " + fighter.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(uint)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExAIControllers : UInt32Converter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(AIController.aIControllers
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToUInt32(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(uint))
            {
                AIController fighter = AIController.aIControllers.Where(s => s.ID == (uint) value).FirstOrDefault();
                return "0x" + ((uint) value).ToString("X8") + (fighter == null ? "" : " - " + fighter.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(uint)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExCSSIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FighterNameGenerators.cssSlotIDList
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                SSBB.Fighter fighter = FighterNameGenerators.cssSlotIDList
                    .Where(s => s.ID == (byte) value).FirstOrDefault();
                return "0x" + ((byte) value).ToString("X2") + (fighter == null ? "" : " - " + fighter.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExCosmeticIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FighterNameGenerators.cosmeticIDList
                .Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name)
                .ToList());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                SSBB.Fighter fighter = FighterNameGenerators.cosmeticIDList
                    .Where(s => s.ID == (byte) value).FirstOrDefault();
                return "0x" + ((byte) value).ToString("X2") + (fighter == null ? "" : " - " + fighter.Name);
            }

            if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null &&
                value.GetType() == typeof(string))
            {
                return 0;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}