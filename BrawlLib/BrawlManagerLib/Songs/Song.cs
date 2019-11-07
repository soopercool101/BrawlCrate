namespace BrawlLib.BrawlManagerLib.Songs
{
    public class Song
    {
        public string DefaultName { get; private set; }
        public string Filename { get; private set; }
        public ushort ID { get; private set; }
        public byte? DefaultVolume { get; private set; }
        public int? InfoPacIndex { get; private set; }

        public Song(string name, string filename, int id, byte? volume, int? infoPacIndex)
        {
            DefaultName = name;
            Filename = filename;
            ID = (ushort) id;
            DefaultVolume = volume;
            InfoPacIndex = infoPacIndex;
        }

        public override string ToString()
        {
            return $"{ID.ToString("X4")} {Filename} {DefaultName}";
        }
    }
}