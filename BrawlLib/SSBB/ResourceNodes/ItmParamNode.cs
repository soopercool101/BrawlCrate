using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ItmParamNode : ARCEntryNode
    {
        public override Type[] AllowedChildTypes => new[] { typeof(ItmParamEntryNode) };

        public static readonly Dictionary<string, uint> ItemOffsets = new Dictionary<string, uint>
        {
            { "Assist Trophy" , 0xD1C },
            { "Franklin Badge" , 0x491C },
            { "Banana Peel" , 0x392C },
            { "Barrel" , 0xE0C },
            { "Beam Sword" , 0x2EDC },
            { "Bill" , 0x194C },
            { "Bob-Omb" , 0x3A1C },
            { "Crate" , 0xEFC },
            { "Bumper" , 0x3B0C },
            { "Capsule" , 0xFEC },
            { "Rolling Crate" , 0x10DC },
            { "CD" , 0x1A3C },
            { "Gooey Bomb" , 0x3BFC },
            { "Cracker Launcher" , 0x275C },
            { "Cracker Launcher Shot" , 0x284C },
            { "Coin" , 0x1B2C },
            { "Superspicy Curry" , 0x1C1C },
            { "Superspicy Curry Shot" , 0x1D0C },
            { "Deku Nut" , 0x3CEC },
            { "Mr. Saturn" , 0x3DDC },
            { "Dragoon Part" , 0x4DCC },
            { "Dragoon Set" , 0x4EBC },
            { "Dragoon Sight" , 0x4FAC },
            { "Trophy" , 0x185C },
            { "Fire Flower" , 0x293C },
            { "Fire Flower Shot" , 0x2A2C },
            { "Freezie" , 0x3ECC },
            { "Golden Hammer" , 0x2FCC },
            { "Green Shell" , 0x3FBC },
            { "Hammer" , 0x30BC },
            { "Hammer Head" , 0x31AC },
            { "Fan" , 0x329C },
            { "Heart Container" , 0x158C },
            { "Homerun Bat" , 0x338C },
            { "Party Ball" , 0x11CC },
            { "Manaphy Heart" , 0x9F5C },
            { "Maxim Tomato" , 0x167C },
            { "Poison Mushroom" , 0x1FDC },
            { "Super Mushroom" , 0x20CC },
            { "Metal Box" , 0x1EEC },
            { "Hothead" , 0x419C },
            { "Pitfall" , 0x428C },
            { "Pokeball" , 0x12BC },
            { "Blast Box" , 0x509C },
            { "Ray Gun" , 0x2B1C },
            { "Ray Gun Shot" , 0x2C0C },
            { "Lipstick" , 0x347C },
            { "Lipstick Flower" , 0x365C },
            { "Lipstick Shot" , 0x356C },
            { "Sandbag" , 0x149C },
            { "Screw Attack" , 0x4A0C },
            { "Sticker" , 0x21BC },
            { "Motion-Sensor Bomb" , 0x437C },
            { "Timer" , 0x22AC },
            { "Smart Bomb" , 0x446C },
            { "Smash Ball" , 0x518C },
            { "Smoke Screen" , 0x464C },
            { "Spring" , 0x473C },
            { "Star Rod" , 0x374C },
            { "Star Rod Shot" , 0x383C },
            { "Soccer Ball" , 0x4BEC },
            { "Super Scope" , 0x2CFC },
            { "Super Scope shot" , 0x2DEC },
            { "Star" , 0x248C },
            { "Food" , 0x176C },
            { "Team Healer" , 0x482C },
            { "Lightning" , 0x257C },
            { "Unira" , 0x4CDC },
            { "Bunny Hood" , 0x4AFC },
            { "Warpstar" , 0x266C },
            { "Key" , 0x40AC },
            { "Trophy Stand" , 0x455C },
            { "Stock Ball" , 0x239C },
            { "Apple" , 0xAC7C },
            { "Sidestepper" , 0xAD6C },
            { "Shellcreeper" , 0xAE5C },
            { "Pellet" , 0xAF4C },
            { "Vegetable" , 0xB03C },
            { "Auroros" , 0xB12C },
            { "Koopa" , 0xB30C },
            { "Snake's Box" , 0x527C },
            { "Diddy's Peanut" , 0x536C },
            { "Link's Bomb" , 0x545C },
            { "Peach's Turnup" , 0x554C },
            { "R.O.B.'s Gyro" , 0x563C },
            { "Seed" , 0x572C },
            { "Snake's Grenade" , 0x581C },
            { "Samus's Armor piece" , 0x5F9C },
            { "Toon Link's Bomb" , 0x590C },
            { "Wario's Bike" , 0x59FC },
            { "Wario's Bike A" , 0x5AEC },
            { "Wario's Bike B" , 0x5BDC },
            { "Wario's Bike C" , 0x5CCC },
            { "Wario's Bike D" , 0x5DBC },
            { "Wario's Bike E" , 0x5EAC },
            { "Torchic" , 0x89CC },
            { "Celebi" , 0x8ABC },
            { "Chikorita" , 0x8BAC },
            { "Chikorita's Shot" , 0x8C9C },
            { "Entei" , 0x8E7C },
            { "Moltres" , 0x8F6C },
            { "Munchlax" , 0x914C },
            { "Deoxys" , 0x8D8C },
            { "Groudon" , 0x923C },
            { "Gulpin" , 0x905C },
            { "Staryu" , 0x932C },
            { "Staryu's Shot" , 0x941C },
            { "Ho-oh" , 0x950C },
            { "Ho-oh's Shot" , 0x95FC },
            { "Jirachi" , 0x96EC },
            { "Snorlax" , 0x97DC },
            { "Bellossom" , 0x98CC },
            { "Kyogre" , 0x99BC },
            { "Kyogre's Shot" , 0x9AAC },
            { "Latias/Latios" , 0x9B9C },
            { "Lugia" , 0x9C8C },
            { "Lugia's Shot" , 0x9D7C },
            { "Manaphy" , 0x9E6C },
            { "Weavile" , 0xA04C },
            { "Electrode" , 0xA13C },
            { "Metagross" , 0xA22C },
            { "Mew" , 0xA31C },
            { "Meowth" , 0xA40C },
            { "Meowth's Shot" , 0xA4FC },
            { "Piplup" , 0xA5EC },
            { "Togepi" , 0xA9AC },
            { "Goldeen" , 0xAA9C },
            { "Gardevoir" , 0xA6DC },
            { "Wobuffet" , 0xA7CC },
            { "Suicune" , 0xA8BC },
            { "Bonsly" , 0xAB8C },
            { "Andross" , 0x608C },
            { "Andross Shot" , 0x617C },
            { "Barbara" , 0x626C },
            { "Gray Fox" , 0x662C },
            { "Ray MKII" , 0x635C },
            { "Ray MKII Bomb" , 0x653C },
            { "Ray MKII Gun Shot" , 0x644C },
            { "Samurai Goroh" , 0x68FC },
            { "Devil" , 0x671C },
            { "Excitebike" , 0x680C },
            { "Jeff Andonuts" , 0x6CBC },
            { "Jeff Pencil Bullet" , 0x6E9C },
            { "Jeff Pencil Rocket" , 0x6DAC },
            { "Lakitu" , 0x716C },
            { "Knuckle Joe" , 0x6F8C },
            { "Knuckle Joe Shot" , 0x707C },
            { "Hammer Bro." , 0x69EC },
            { "Hammer Bro. Hammer" , 0x6ADC },
            { "Helirin" , 0x6BCC },
            { "Kat & Ana" , 0x725C },
            { "Ana" , 0x734C },
            { "Jill & Drill Dozer" , 0x743C },
            { "Lyn" , 0x752C },
            { "Little Mac" , 0x761C },
            { "Metroid" , 0x770C },
            { "Nintendog" , 0x77FC },
            { "NintendogFull" , 0x78EC },
            { "Mr. Resetti" , 0x79DC },
            { "Isaac" , 0x7ACC },
            { "Isaac Shot" , 0x7BBC },
            { "Saki Amamiya" , 0x7CAC },
            { "Saki Shot 1" , 0x7D9C },
            { "Saki Shot 2" , 0x7E8C },
            { "Shadow the Hedgehog" , 0x7F7C },
            { "Infantry" , 0x806C },
            { "Infantry Shot" , 0x815C },
            { "Starfy" , 0x824C },
            { "Tank" , 0x833C },
            { "Tank Shot" , 0x842C },
            { "Tingle" , 0x851C },
            { "togezo" , 0x860C },
            { "Waluigi" , 0x86FC },
            { "Dr. Wright" , 0x87EC },
            { "Wright Buildings" , 0x88DC }
        };

        public override bool OnInitialize()
        {
            return true;
        }

        public override void OnPopulate()
        {
            foreach (string item in ItemOffsets.Keys)
            {
                ItmParamEntryNode child = new ItmParamEntryNode();
                child._name = item;
                child.Initialize(this, WorkingUncompressed.Address + ItemOffsets[item], ItmParamEntry.Size);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            VoidPtr addr = source.Address;
            TyDataHeader* header = (TyDataHeader*)addr;

            if (header->_size != source.Length || header->_size != 48033 || header->_pad1 != 0 || header->_pad2 != 0 || header->_pad3 != 0 ||
                header->_pad4 != 0 || header->_dataOffset > source.Length || header->_entries != 1 ||
                header->_dataOffset + header->_dataEntries * 4 > source.Length)
            {
                return null;
            }

            return header->GetEntryName(0).Equals("allParam") ? new ItmParamNode() : null;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
            foreach (ResourceNode child in Children.Where(child => ItemOffsets.ContainsKey(child.Name)))
            {
                child.OnRebuild(address + ItemOffsets[child.Name], ItmParamEntry.Size, true);
            }
        }
    }

    public unsafe class ItmParamEntryNode : ResourceNode
    {
        internal ItmParamEntry Data;
        public override ResourceType ResourceFileType => ResourceType.NoEditItem;

        [Category("Item Parameters")]
        public float HurtboxSize
        {
            get => Data._hurtboxSize;
            set
            {
                Data._hurtboxSize = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public float ThrowSpeedMultiplier
        {
            get => Data._throwSpeedMultiplier;
            set
            {
                Data._throwSpeedMultiplier = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x08
        {
            get => Data._unknown0x08;
            set
            {
                Data._unknown0x08 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x0C
        {
            get => Data._unknown0x0C;
            set
            {
                Data._unknown0x0C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x10
        {
            get => Data._unknown0x10;
            set
            {
                Data._unknown0x10 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x14
        {
            get => Data._unknown0x14;
            set
            {
                Data._unknown0x14 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x18
        {
            get => Data._unknown0x18;
            set
            {
                Data._unknown0x18 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public float Gravity
        {
            get => Data._gravity;
            set
            {
                Data._gravity = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public float TerminalVelocity
        {
            get => Data._terminalVelocity;
            set
            {
                Data._terminalVelocity = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x24
        {
            get => Data._unknown0x24;
            set
            {
                Data._unknown0x24 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x28
        {
            get => Data._unknown0x28;
            set
            {
                Data._unknown0x28 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x2C
        {
            get => Data._unknown0x2C;
            set
            {
                Data._unknown0x2C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x30
        {
            get => Data._unknown0x30;
            set
            {
                Data._unknown0x30 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x34
        {
            get => Data._unknown0x34;
            set
            {
                Data._unknown0x34 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x38
        {
            get => Data._unknown0x38;
            set
            {
                Data._unknown0x38 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x3C
        {
            get => Data._unknown0x3C;
            set
            {
                Data._unknown0x3C = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 GrabRangeDistance
        {
            get => new Vector2(Data._grabRangeDistanceX, Data._grabRangeDistanceY);
            set
            {
                Data._grabRangeDistanceX = value.X;
                Data._grabRangeDistanceY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [DisplayName("ECB Height")]
        public float EcbHeight
        {
            get => Data._ecbHeight;
            set
            {
                Data._ecbHeight = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [DisplayName("ECB Y-Offset")]
        public float EcbOffsetY
        {
            get => Data._ecbOffsetY;
            set
            {
                Data._ecbOffsetY = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [DisplayName("ECB Width")]
        public float EcbWidth
        {
            get => Data._ecbWidth;
            set
            {
                Data._ecbWidth = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x54
        {
            get => Data._unknown0x54;
            set
            {
                Data._unknown0x54 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x58
        {
            get => Data._unknown0x58;
            set
            {
                Data._unknown0x58 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("The angle at which an item will begin to slide down a slope")]
        public float SlideAngle
        {
            get => Data._slideAngle;
            set
            {
                Data._slideAngle = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("Gravity when an item is in a sliding state")]
        public float SlideGravity
        {
            get => Data._slideGravity;
            set
            {
                Data._slideGravity = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x64
        {
            get => Data._unknown0x64;
            set
            {
                Data._unknown0x64 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x68
        {
            get => Data._unknown0x68;
            set
            {
                Data._unknown0x68 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x6C
        {
            get => Data._unknown0x6C;
            set
            {
                Data._unknown0x6C = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 WallBounceSpeedMultipliers
        {
            get => new Vector2(Data._wallBounceSpeedMultiplierX, Data._wallBounceSpeedMultiplierY);
            set
            {
                Data._wallBounceSpeedMultiplierX = value.X;
                Data._wallBounceSpeedMultiplierY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 CeilingBounceSpeedMultipliers
        {
            get => new Vector2(Data._ceilingBounceSpeedMultiplierX, Data._ceilingBounceSpeedMultiplierY);
            set
            {
                Data._ceilingBounceSpeedMultiplierX = value.X;
                Data._ceilingBounceSpeedMultiplierY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 FighterBounceSpeedMultipliers
        {
            get => new Vector2(Data._fighterBounceSpeedMultiplierX, Data._fighterBounceSpeedMultiplierY);
            set
            {
                Data._fighterBounceSpeedMultiplierX = value.X;
                Data._fighterBounceSpeedMultiplierY = value.Y;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x88
        {
            get => Data._unknown0x88;
            set
            {
                Data._unknown0x88 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x8C
        {
            get => Data._unknown0x8C;
            set
            {
                Data._unknown0x8C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x90
        {
            get => Data._unknown0x90;
            set
            {
                Data._unknown0x90 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x94
        {
            get => Data._unknown0x94;
            set
            {
                Data._unknown0x94 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x98
        {
            get => Data._unknown0x98;
            set
            {
                Data._unknown0x98 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0x9C
        {
            get => Data._unknown0x9C;
            set
            {
                Data._unknown0x9C = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xA0
        {
            get => Data._unknown0xA0;
            set
            {
                Data._unknown0xA0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xA4
        {
            get => Data._unknown0xA4;
            set
            {
                Data._unknown0xA4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xA8
        {
            get => Data._unknown0xA8;
            set
            {
                Data._unknown0xA8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("Set to true for the Barrel, Crate, Rolling Crate, and Party Ball. Causes the heavy carrying animation")]
        public bool IsHeavy
        {
            get => Data._isHeavy;
            set
            {
                Data._isHeavy = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB0
        {
            get => Data._unknown0xB0;
            set
            {
                Data._unknown0xB0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexIntConverter))]
        public int Unknown0xB4
        {
            get => Data._unknown0xB4;
            set
            {
                Data._unknown0xB4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexIntConverter))]
        public int Unknown0xB8
        {
            get => Data._unknown0xB8;
            set
            {
                Data._unknown0xB8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xBC
        {
            get => Data._unknown0xBC;
            set
            {
                Data._unknown0xBC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xC0
        {
            get => Data._unknown0xC0;
            set
            {
                Data._unknown0xC0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xC4
        {
            get => Data._unknown0xC4;
            set
            {
                Data._unknown0xC4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xC8
        {
            get => Data._unknown0xC8;
            set
            {
                Data._unknown0xC8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xCC
        {
            get => Data._unknown0xCC;
            set
            {
                Data._unknown0xCC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xD0
        {
            get => Data._unknown0xD0;
            set
            {
                Data._unknown0xD0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xD4
        {
            get => Data._unknown0xD4;
            set
            {
                Data._unknown0xD4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xD8
        {
            get => Data._unknown0xD8;
            set
            {
                Data._unknown0xD8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xDC
        {
            get => Data._unknown0xDC;
            set
            {
                Data._unknown0xDC = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xE0
        {
            get => Data._unknown0xE0;
            set
            {
                Data._unknown0xE0 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xE4
        {
            get => Data._unknown0xE4;
            set
            {
                Data._unknown0xE4 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexIntConverter))]
        public int Unknown0xE8
        {
            get => Data._unknown0xE8;
            set
            {
                Data._unknown0xE8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexIntConverter))]
        public int Unknown0xEC
        {
            get => Data._unknown0xEC;
            set
            {
                Data._unknown0xEC = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            Data = *(ItmParamEntry*)WorkingUncompressed.Address;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            *(ItmParamEntry*)address = Data;
        }
    }
}