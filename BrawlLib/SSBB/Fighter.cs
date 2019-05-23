namespace BrawlLib.SSBB
{
    public class Fighter
    {
        /// <summary>
        /// The character slot index, as used by common2.pac event match and all-star data.
        /// See: http://opensa.dantarion.com/wiki/Character_Slots
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// The fighter name (e.g. "Yoshi").
        /// </summary>
        public string Name { get; private set; }

        public Fighter(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override string ToString() { return Name; }

        public readonly static Fighter[] Fighters = new Fighter[] {
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

            // Event Matches
            new Fighter(0x3e, "None / Select Character"),
            new Fighter(0x48, "Pokémon Trainer"),
            new Fighter(0x49, "Samus/ZSS"),
            new Fighter(0x4a, "Zelda/Sheik")
        };
    }
}
