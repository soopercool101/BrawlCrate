namespace BrawlLib.BrawlCrate
{
    public static class PerSessionSettings
    {
        public static readonly bool ProgramBirthday = System.DateTime.Now.Day == 8 && System.DateTime.Now.Month == 4;

        public static readonly bool BrawlCrateBirthday =
            System.DateTime.Now.Day == 30 && System.DateTime.Now.Month == 8;
    }
}