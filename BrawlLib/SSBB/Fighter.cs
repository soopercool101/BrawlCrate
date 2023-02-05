namespace BrawlLib.SSBB
{
    public class Fighter
    {
        /// <summary>
        /// The character slot index, as used by common2.pac event match and all-star data.
        /// See: http://opensa.dantarion.com/wiki/Character_Slots
        /// </summary>
        public uint ID { get; private set; }

        /// <summary>
        /// The fighter name (e.g. "Yoshi").
        /// </summary>
        public string Name { get; private set; }

        public Fighter(uint id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static readonly Fighter[] Fighters = new Fighter[]
        {
            //          ID     Display Name     
            new Fighter(0x00, "Mario"),
            new Fighter(0x01, "Donkey Kong"),
            new Fighter(0x02, "Link"),
            new Fighter(0x03, "Samus"),
            new Fighter(0x04, "Zero Suit Samus"),
            new Fighter(0x05, "Yoshi"),
            new Fighter(0x06, "Kirby"),
            new Fighter(0x07, "Fox"),
            new Fighter(0x08, "Pikachu"),
            new Fighter(0x09, "Luigi"),
            new Fighter(0x0a, "Captain Falcon"),
            new Fighter(0x0b, "Ness"),
            new Fighter(0x0c, "Bowser"),
            new Fighter(0x0d, "Peach"),
            new Fighter(0x0e, "Zelda"),
            new Fighter(0x0f, "Sheik"),
            new Fighter(0x10, "Ice Climbers"),
            new Fighter(0x11, "Popo"),
            new Fighter(0x12, "Nana"),
            new Fighter(0x13, "Marth"),
            new Fighter(0x14, "Mr. Game & Watch"),
            new Fighter(0x15, "Falco"),
            new Fighter(0x16, "Ganondorf"),
            new Fighter(0x17, "Wario"),
            new Fighter(0x18, "Meta Knight"),
            new Fighter(0x19, "Pit"),
            new Fighter(0x1a, "Pikmin & Olimar"),
            new Fighter(0x1b, "Lucas"),
            new Fighter(0x1c, "Diddy Kong"),
            new Fighter(0x1d, "Charizard"),
            new Fighter(0x1e, "Charizard (independent)"),
            new Fighter(0x1f, "Squirtle"),
            new Fighter(0x20, "Squirtle (independent)"),
            new Fighter(0x21, "Ivysaur"),
            new Fighter(0x22, "Ivysaur (independent)"),
            new Fighter(0x23, "King Dedede"),
            new Fighter(0x24, "Lucario"),
            new Fighter(0x25, "Ike"),
            new Fighter(0x26, "R.O.B."),
            new Fighter(0x27, "Jigglypuff"),
            new Fighter(0x28, "Toon Link"),
            new Fighter(0x29, "Wolf"),
            new Fighter(0x2a, "Snake"),
            new Fighter(0x2b, "Sonic"),
            new Fighter(0x2c, "Giga Bowser"),
            new Fighter(0x2d, "Warioman"),
            new Fighter(0x2e, "Red Alloy (don't use in event matches)"),
            new Fighter(0x2f, "Blue Alloy (don't use in event matches)"),
            new Fighter(0x30, "Yellow Alloy (don't use in event matches)"),
            new Fighter(0x31, "Green Alloy (don't use in event matches)"),
            new Fighter(0x32, "Roy (PM)"),
            new Fighter(0x33, "Mewtwo (PM)"),
            new Fighter(0x33, "Knuckles (P+)"),
            new Fighter(0x3F, "ExFighter3F"),
            new Fighter(0x40, "ExFighter40"),
            new Fighter(0x41, "ExFighter41"),
            new Fighter(0x42, "ExFighter42"),
            new Fighter(0x43, "ExFighter43"),
            new Fighter(0x44, "ExFighter44"),
            new Fighter(0x45, "ExFighter45"),
            new Fighter(0x46, "ExFighter46"),
            new Fighter(0x47, "ExFighter47"),
            new Fighter(0x4B, "ExFighter4B"),
            new Fighter(0x4C, "ExFighter4C"),
            new Fighter(0x4D, "ExFighter4D"),
            new Fighter(0x4E, "ExFighter4E"),
            new Fighter(0x4F, "ExFighter4F"),
            new Fighter(0x50, "ExFighter50"),
            new Fighter(0x51, "ExFighter51"),
            new Fighter(0x52, "ExFighter52"),
            new Fighter(0x53, "ExFighter53"),
            new Fighter(0x54, "ExFighter54"),
            new Fighter(0x55, "ExFighter55"),
            new Fighter(0x56, "ExFighter56"),
            new Fighter(0x57, "ExFighter57"),
            new Fighter(0x58, "ExFighter58"),
            new Fighter(0x59, "ExFighter59"),
            new Fighter(0x5A, "ExFighter5A"),
            new Fighter(0x5B, "ExFighter5B"),
            new Fighter(0x5C, "ExFighter5C"),
            new Fighter(0x5D, "ExFighter5D"),
            new Fighter(0x5E, "ExFighter5E"),
            new Fighter(0x5F, "ExFighter5F"),
            new Fighter(0x60, "ExFighter60"),
            new Fighter(0x61, "ExFighter61"),
            new Fighter(0x62, "ExFighter62"),
            new Fighter(0x63, "ExFighter63"),
            new Fighter(0x64, "ExFighter64"),
            new Fighter(0x65, "ExFighter65"),
            new Fighter(0x66, "ExFighter66"),
            new Fighter(0x67, "ExFighter67"),
            new Fighter(0x68, "ExFighter68"),
            new Fighter(0x69, "ExFighter69"),
            new Fighter(0x6A, "ExFighter6A"),
            new Fighter(0x6B, "ExFighter6B"),
            new Fighter(0x6C, "ExFighter6C"),
            new Fighter(0x6D, "ExFighter6D"),
            new Fighter(0x6E, "ExFighter6E"),
            new Fighter(0x6F, "ExFighter6F"),
            new Fighter(0x70, "ExFighter70"),
            new Fighter(0x71, "ExFighter71"),
            new Fighter(0x72, "ExFighter72"),
            new Fighter(0x73, "ExFighter73"),
            new Fighter(0x74, "ExFighter74"),
            new Fighter(0x75, "ExFighter75"),
            new Fighter(0x76, "ExFighter76"),
            new Fighter(0x77, "ExFighter77"),
            new Fighter(0x78, "ExFighter78"),
            new Fighter(0x79, "ExFighter79"),
            new Fighter(0x7A, "ExFighter7A"),
            new Fighter(0x7B, "ExFighter7B"),
            new Fighter(0x7C, "ExFighter7C"),
            new Fighter(0x7D, "ExFighter7D"),
            new Fighter(0x7E, "ExFighter7E"),
            new Fighter(0x7F, "ExFighter7F"),

            // Event Matches
            new Fighter(0x3E, "None / Select Character"),
            new Fighter(0x48, "Pokémon Trainer"),
            new Fighter(0x49, "Samus/ZSS"),
            new Fighter(0x4A, "Zelda/Sheik"),

            new Fighter(0xFF, "None")
        };
    }
}