namespace DungeonGenerator
{
    public enum ConnectionType
    {
        None,
        Border,
        Wall,
        Small,
        Medium,
        Big,
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
            Top = ConnectionType.Wall,
            Bottom = ConnectionType.Wall,
            Left = ConnectionType.Wall,
            Right = ConnectionType.Wall
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