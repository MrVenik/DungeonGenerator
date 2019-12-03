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

        public Connection Rotate(bool clockwise = false)
        {
            Connection newConnection = new Connection()
            {
                Top = clockwise ? Left : Right,
                Bottom = clockwise ? Right : Left,
                Left = clockwise ? Bottom : Top,
                Right = clockwise ? Top : Bottom,
            };
            return newConnection;
        }
    }
}