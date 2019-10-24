namespace DungeonGenerator
{
    public enum ConnectionType
    {
        None,
        Border,
        Wall,
        Medium,
        Small,
        CorridorWall,
        SecretRoomDoor
    }

    [System.Serializable]
    public class Connection
    {
        public static Connection Start = new Connection()
        {
            Top = ConnectionType.Small,
            Bottom = ConnectionType.Wall,
            Left = ConnectionType.Wall,
            Right = ConnectionType.Wall
        };

        public static Connection Border = new Connection()
        {
            Top = ConnectionType.Border,
            Bottom = ConnectionType.Border,
            Left = ConnectionType.Border,
            Right = ConnectionType.Border
        };

        public static Connection None = new Connection()
        {
            Top = ConnectionType.None,
            Bottom = ConnectionType.None,
            Left = ConnectionType.None,
            Right = ConnectionType.None
        };

        public ConnectionType Top;
        public ConnectionType Bottom;
        public ConnectionType Left;
        public ConnectionType Right;
    }
}