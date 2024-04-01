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
            { "Home-Run Bat" , 0x338C },
            { "Party Ball" , 0x11CC },
            { "Manaphy Heart" , 0x9F5C },
            { "Maxim Tomato" , 0x167C },
            { "Poison Mushroom" , 0x1FDC },
            { "Super Mushroom" , 0x20CC },
            { "Metal Box" , 0x1EEC },
            { "Hothead" , 0x419C },
            { "Pitfall" , 0x428C },
            { "Poké Ball" , 0x12BC },
            { "Blast Box" , 0x509C },
            { "Ray Gun" , 0x2B1C },
            { "Ray Gun Shot" , 0x2C0C },
            { "Lip's Stick" , 0x347C },
            { "Lip's Stick Flower" , 0x365C },
            { "Lip's Stick Shot" , 0x356C },
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
            { "Starman / Super Star" , 0x248C },
            { "Food" , 0x176C },
            { "Team Healer" , 0x482C },
            { "Lightning" , 0x257C },
            { "Unira" , 0x4CDC },
            { "Bunny Hood" , 0x4AFC },
            { "Warp Star" , 0x266C },
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
            { "Peach's Turnip" , 0x554C },
            { "R.O.B.'s Gyro" , 0x563C },
            { "Diddy's Edible Peanut" , 0x572C },
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
            { "Ray MK III" , 0x635C },
            { "Ray MK III Bomb" , 0x653C },
            { "Ray MK III Gun Shot" , 0x644C },
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
            { "Spiny" , 0x860C },
            { "Waluigi" , 0x86FC },
            { "Dr. Wright" , 0x87EC },
            { "Wright Buildings" , 0x88DC }
        };

        private float[] _usedFloats = new float[16];
        private int[] _usedInts = new int[9];
        
        [Category("Item Parameters")]
        public float StickerSpawnRate
        {
            get => _usedFloats[0];
            set
            {
                _usedFloats[0] = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Item Parameters")]
        public float CDSpawnRate
        {
            get => _usedFloats[1];
            set
            {
                _usedFloats[1] = value;
                SignalPropertyChange();
            }
        }
        
        [Category("Item Parameters")]
        public float MythicalPokemonSpawnRate
        {
            get => _usedFloats[2];
            set
            {
                _usedFloats[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6D0
        {
            get => _usedFloats[3];
            set
            {
                _usedFloats[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6D4
        {
            get => _usedFloats[4];
            set
            {
                _usedFloats[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6D8
        {
            get => _usedFloats[5];
            set
            {
                _usedFloats[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6DC
        {
            get => _usedFloats[6];
            set
            {
                _usedFloats[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6E0
        {
            get => _usedFloats[7];
            set
            {
                _usedFloats[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6E4
        {
            get => _usedFloats[8];
            set
            {
                _usedFloats[8] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6E8
        {
            get => _usedFloats[9];
            set
            {
                _usedFloats[9] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6EC
        {
            get => _usedFloats[10];
            set
            {
                _usedFloats[10] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6F0
        {
            get => _usedFloats[11];
            set
            {
                _usedFloats[11] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6F4
        {
            get => _usedFloats[12];
            set
            {
                _usedFloats[12] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6F8
        {
            get => _usedFloats[13];
            set
            {
                _usedFloats[13] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB6FC
        {
            get => _usedFloats[14];
            set
            {
                _usedFloats[14] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public float Unknown0xB700
        {
            get => _usedFloats[15];
            set
            {
                _usedFloats[15] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB704
        {
            get => _usedInts[0];
            set
            {
                _usedInts[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB708
        {
            get => _usedInts[1];
            set
            {
                _usedInts[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB70C
        {
            get => _usedInts[2];
            set
            {
                _usedInts[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB710
        {
            get => _usedInts[3];
            set
            {
                _usedInts[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB714
        {
            get => _usedInts[4];
            set
            {
                _usedInts[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB718
        {
            get => _usedInts[5];
            set
            {
                _usedInts[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB71C
        {
            get => _usedInts[6];
            set
            {
                _usedInts[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB720
        {
            get => _usedInts[7];
            set
            {
                _usedInts[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public int Unknown0xB724
        {
            get => _usedInts[8];
            set
            {
                _usedInts[8] = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            for (int i = 0; i < _usedFloats.Length; i++)
            {
                _usedFloats[i] = *(bfloat*)(WorkingUncompressed.Address + 0xB6C4 + (i * 4));
            }
            for (int i = 0; i < _usedInts.Length; i++)
            {
                _usedInts[i] = *(bint*)(WorkingUncompressed.Address + 0xB704 + (i * 4));
            }

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

            for (int i = 0; i < _usedFloats.Length; i++)
            {
                *(bfloat*)(address + 0xB6C4 + (i * 4)) = _usedFloats[i];
            }

            for (int i = 0; i < _usedInts.Length; i++)
            {
                *(bint*)(address + 0xB704 + (i * 4)) = _usedInts[i];
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
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 ThrowSpeed
        {
            get => new Vector3(Data._throwSpeedX, Data._throwSpeedY, Data._throwSpeedZ);
            set
            {
                Data._throwSpeedX = value._x;
                Data._throwSpeedY = value._y;
                Data._throwSpeedZ = value._z;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 SpinRate
        {
            get => new Vector3(Data._spinRateX, Data._spinRateY, Data._spinRateZ);
            set
            {
                Data._spinRateX = value._x;
                Data._spinRateY = value._y;
                Data._spinRateZ = value._z;
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

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        [Description("Affects the box around which characters push away the item")]
        public Vector2 CharacterColliderY
        {
            get => new Vector2(Data._characterColliderY1, Data._characterColliderY2);
            set
            {
                Data._characterColliderY1 = value._x;
                Data._characterColliderY2 = value._y;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        [Description("Affects the box around which characters push away the item")]
        public Vector2 CharacterColliderX
        {
            get => new Vector2(Data._characterColliderX1, Data._characterColliderX2);
            set
            {
                Data._characterColliderX1 = value._x;
                Data._characterColliderX2 = value._y;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 GrabRangeOffset
        {
            get => new Vector2(Data._grabRangeOffsetX, Data._grabRangeOffsetY);
            set
            {
                Data._grabRangeOffsetX = value._x;
                Data._grabRangeOffsetY = value._y;
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
        [DisplayName("ECB Left")]
        public float EcbLeft
        {
            get => Data._ecbLeft;
            set
            {
                Data._ecbLeft = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [DisplayName("ECB Right")]
        public float EcbRight
        {
            get => Data._ecbRight;
            set
            {
                Data._ecbRight = value;
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

        [Category("Item Parameters")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 FloorBounceSpeedMultipliers
        {
            get => new Vector2(Data._floorBounceSpeedMultiplierX, Data._floorBounceSpeedMultiplierY);
            set
            {
                Data._floorBounceSpeedMultiplierX = value._x;
                Data._floorBounceSpeedMultiplierY= value._y;
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

        [Category("Item Parameters")]
        public float AirFriction
        {
            get => Data._airFriction;
            set
            {
                Data._airFriction = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public float GroundFriction
        {
            get => Data._groundFriction;
            set
            {
                Data._groundFriction = value;
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

        [Category("Item Parameters")]
        public float BaseDamageMultiplier
        {
            get => Data._baseDamageMultiplier;
            set
            {
                Data._baseDamageMultiplier = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public float SpeedDamageMultiplier
        {
            get => Data._speedDamageMultiplier;
            set
            {
                Data._speedDamageMultiplier = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("Scales model and hitbox. ECB and grab range must be changed separately")]
        public float ItemScale
        {
            get => Data._itemScale;
            set
            {
                Data._itemScale = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("If an item is breakable with damage, this is the amount of damage it will take")]
        public float ItemHealth
        {
            get => Data._itemHealth;
            set
            {
                Data._itemHealth = value;
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

        public enum GrabType : int
        {
            NotGrabbable = 0,
            AutomaticPickup = 1,
            NoHandAnimationChange = 2,
            AnimItemHandHave = 3,
            AnimItemHandPickup = 4,
            AnimItemHandGrip = 5,
            AnimItemHandSmash = 6
        }

        [Category("Item Parameters")]
        public GrabType PickupType
        {
            get => (GrabType) (int)Data._pickupType;
            set
            {
                Data._pickupType = (int)value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexByteConverter))]
        public byte Flags0xB4 => Data._flags0xB4;

        [Category("Unknown")]
        public bool Unknown0xB4a
        {
            get => Data._flags0xB4[0];
            set
            {
                Data._flags0xB4[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB4b
        {
            get => Data._flags0xB4[1];
            set
            {
                Data._flags0xB4[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("If true, flips upsidedown when facing the opposite direction. Used by some projectiles.")]
        public bool FlipTurn
        {
            get => Data._flags0xB4[2];
            set
            {
                Data._flags0xB4[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB4d
        {
            get => Data._flags0xB4[3];
            set
            {
                Data._flags0xB4[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB4e
        {
            get => Data._flags0xB4[4];
            set
            {
                Data._flags0xB4[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB4f
        {
            get => Data._flags0xB4[5];
            set
            {
                Data._flags0xB4[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB4g
        {
            get => Data._flags0xB4[6];
            set
            {
                Data._flags0xB4[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB4h
        {
            get => Data._flags0xB4[7];
            set
            {
                Data._flags0xB4[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexByteConverter))]
        public byte Flags0xB5 => Data._flags0xB5;

        [Category("Item Parameters")]
        public bool SubspaceEnemy
        {
            get => Data._flags0xB5[0];
            set
            {
                Data._flags0xB5[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool StageItem
        {
            get => Data._flags0xB5[1];
            set
            {
                Data._flags0xB5[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool Equippable1
        {
            get => Data._flags0xB5[2];
            set
            {
                Data._flags0xB5[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool IsInedibleNormalItem
        {
            get => Data._flags0xB5[3];
            set
            {
                Data._flags0xB5[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool IsHammer
        {
            get => Data._flags0xB5[4];
            set
            {
                Data._flags0xB5[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("Diddy's Edible Peanut is not spawned directly by him, and is the only case this is set to false when CharacterItem is set to true")]
        public bool SpawnedByCharacter
        {
            get => Data._flags0xB5[5];
            set
            {
                Data._flags0xB5[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [DisplayName("Disable Z-Drop")]
        public bool DisableZDrop
        {
            get => Data._flags0xB5[6];
            set
            {
                Data._flags0xB5[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB5h
        {
            get => Data._flags0xB5[7];
            set
            {
                Data._flags0xB5[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexByteConverter))]
        public byte Flags0xB6 => Data._flags0xB6;

        [Category("Unknown")]
        public bool Unknown0xB6a
        {
            get => Data._flags0xB6[0];
            set
            {
                Data._flags0xB6[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB6b
        {
            get => Data._flags0xB6[1];
            set
            {
                Data._flags0xB6[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("When true, activates Assist Trophy despawn effects when despawning")]
        public bool IsAssistSummon
        {
            get => Data._flags0xB6[2];
            set
            {
                Data._flags0xB6[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("When true, activates Pokémon despawn effects when despawning")]
        public bool IsPokemon
        {
            get => Data._flags0xB6[3];
            set
            {
                Data._flags0xB6[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool CharacterItem
        {
            get => Data._flags0xB6[4];
            set
            {
                Data._flags0xB6[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("Causes the food eating animation to be played on pickup")]
        public bool Edible
        {
            get => Data._flags0xB6[5];
            set
            {
                Data._flags0xB6[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("When true, vanish gfx are disabled and item disappears instantly when timer ends no matter what")]
        public bool IsProjectile
        {
            get => Data._flags0xB6[6];
            set
            {
                Data._flags0xB6[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("When true, Kirby/Dedede/Wario will take damage when inhaling this item")]
        public bool ExplosiveInhale
        {
            get => Data._flags0xB6[7];
            set
            {
                Data._flags0xB6[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexByteConverter))]
        public byte Flags0xB7 => Data._flags0xB7;

        [Category("Item Parameters")]
        [Description("Set to true for things like Poké Balls and Crates")]
        public bool SpawnsItem
        {
            get => Data._flags0xB7[0];
            set
            {
                Data._flags0xB7[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        [Description("Used for held items that fire projectiles")]
        public bool IsFiringWeapon
        {
            get => Data._flags0xB7[1];
            set
            {
                Data._flags0xB7[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool IsBatteringWeapon
        {
            get => Data._flags0xB7[2];
            set
            {
                Data._flags0xB7[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool IsThrowingWeapon
        {
            get => Data._flags0xB7[3];
            set
            {
                Data._flags0xB7[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xB7e
        {
            get => Data._flags0xB7[4];
            set
            {
                Data._flags0xB7[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [Description("Possibly used on contact? Not set for lightning, However")]
        public bool Unknown0xB7f
        {
            get => Data._flags0xB7[5];
            set
            {
                Data._flags0xB7[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool Equippable2
        {
            get => Data._flags0xB7[6];
            set
            {
                Data._flags0xB7[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool Consumable
        {
            get => Data._flags0xB7[7];
            set
            {
                Data._flags0xB7[7] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0xB8
        {
            get => Data._unknown0xB8;
            set
            {
                Data._unknown0xB8 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0xB9
        {
            get => Data._unknown0xB9;
            set
            {
                Data._unknown0xB9 = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0xBA
        {
            get => Data._unknown0xBA;
            set
            {
                Data._unknown0xBA = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        [TypeConverter(typeof(HexByteConverter))]
        public byte Flags0xBB => Data._flags0xBB;

        [Category("Item Parameters")]
        public bool BounceAgainstFloors
        {
            get => Data._flags0xBB[0];
            set
            {
                Data._flags0xBB[0] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xBBb
        {
            get => Data._flags0xBB[1];
            set
            {
                Data._flags0xBB[1] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool BounceAgainstWalls
        {
            get => Data._flags0xBB[2];
            set
            {
                Data._flags0xBB[2] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool BounceAgainstCeilings
        {
            get => Data._flags0xBB[3];
            set
            {
                Data._flags0xBB[3] = value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool BounceAgainstHurtboxes
        {
            get => Data._flags0xBB[4];
            set
            {
                Data._flags0xBB[4] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xBBf
        {
            get => Data._flags0xBB[5];
            set
            {
                Data._flags0xBB[5] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xBBg
        {
            get => Data._flags0xBB[6];
            set
            {
                Data._flags0xBB[6] = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public bool Unknown0xBBh
        {
            get => Data._flags0xBB[7];
            set
            {
                Data._flags0xBB[7] = value;
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

        [Category("Item Parameters")]
        public bool BlinkBeforeDisappearing
        {
            get => Data._blinkBeforeDisappearing;
            set
            {
                Data._blinkBeforeDisappearing = value;
                SignalPropertyChange();
            }
        }

        public enum CameraFocusTypes : int
        {
            Never = 0,
            Always = 1,
            OnCreation = 2,
            UnknownAlways = 3,
            UnknownOnCreation = 4
        }

        [Category("Item Parameters")]
        [Description("Controls when and if the object takes camera focus")]
        public CameraFocusTypes CameraFocus
        {
            get => (CameraFocusTypes)(int)Data._cameraFocus;
            set
            {
                Data._cameraFocus = (int)value;
                SignalPropertyChange();
            }
        }


        public enum OffensiveCollisionInteractionTypes : int
        {
            NoHitstunNoKnockback = 0,
            HitstunNoKnockback = 1,
            HitstunKnockback = 2,
            HitstunHorizontalKnockback = 3
        }

        [Category("Item Parameters")]
        public OffensiveCollisionInteractionTypes OffensiveCollisionInteraction
        {
            get => (OffensiveCollisionInteractionTypes)(int)Data._offensiveCollisionInteraction;
            set
            {
                Data._offensiveCollisionInteraction = (int)value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool CanReflect
        {
            get => Data._canReflect;
            set
            {
                Data._canReflect = value;
                SignalPropertyChange();
            }
        }

        public enum DefensiveCollisionInteractionTypes : int
        {
            NoBounce = 0,
            SelfDelete = 1,
            Bounce = 2
        }

        [Category("Item Parameters")]
        public DefensiveCollisionInteractionTypes DefensiveCollisionInteraction
        {
            get => (DefensiveCollisionInteractionTypes)(int)Data._defensiveCollisionInteraction;
            set
            {
                Data._defensiveCollisionInteraction = (int)value;
                SignalPropertyChange();
            }
        }


        public enum ItemBlastzoneDespawnOptions : int
        {
            NoDespawn = 0,
            SidesAndBottomBlastzone = 1,
            BottomBlastzone = 2,
            AllBlastzones = 3
        }
        [Category("Item Parameters")]
        public ItemBlastzoneDespawnOptions ItemBlastzoneDespawn
        {
            get => (ItemBlastzoneDespawnOptions)(int)Data._blastzoneDespawn;
            set
            {
                Data._blastzoneDespawn = (int)value;
                SignalPropertyChange();
            }
        }

        [Category("Item Parameters")]
        public bool SuffersHitstun
        {
            get => Data._suffersHitstun;
            set
            {
                Data._suffersHitstun = value;
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

        // Padding
        [Browsable(false)]
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

        // Padding
        [Browsable(false)]
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