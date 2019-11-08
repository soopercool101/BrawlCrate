namespace BrawlLib.Internal.Windows.Controls.Hex_Editor
{
    /// <summary>
    /// Represents a position in the HexBox control
    /// </summary>
    public struct BytePositionInfo
    {
        public BytePositionInfo(long index, int characterPosition)
        {
            _index = index;
            _characterPosition = characterPosition;
        }

        public int CharacterPosition => _characterPosition;

        private readonly int _characterPosition;

        public long Index => _index;

        private readonly long _index;
    }
}