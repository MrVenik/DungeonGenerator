namespace DungeonGenerator
{
    public enum RoomSize
    {
        Small = 5,
        Medium = 7,
        Big = 9,
        Huge = 13
    }

    public static class RoomSizeExtensions
    {
        public static bool IsEqualsToConnectionType(this RoomSize roomSize, ConnectionType connectionType)
        {
            if (roomSize == RoomSize.Big && connectionType == ConnectionType.Big) return true;
            if (roomSize == RoomSize.Medium && connectionType == ConnectionType.Medium) return true;
            if (roomSize == RoomSize.Small && connectionType == ConnectionType.Small) return true;
            return false;
        }
    }
}
