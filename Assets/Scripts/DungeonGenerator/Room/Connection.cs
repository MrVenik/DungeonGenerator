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

    public static class ConnectionTypeExtensions
    {
        public static bool CanCreateNextRoom(this ConnectionType type)
        {
            return type != ConnectionType.Wall
                && type != ConnectionType.None
                && type != ConnectionType.Border;
        }
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

        public ConnectionType GetConnectionTypeBySide(Side side)
        {
            switch (side)
            {
                case Side.Top:
                    return Top;
                case Side.Bottom:
                    return Bottom;
                case Side.Left:
                    return Left;
                case Side.Right:
                    return Right;
                default:
                    break;
            }
            throw new System.Exception("Invalid Side type " + side);
        }

        public void SetConnectionTypeBySide(ConnectionType type, Side side)
        {
            switch (side)
            {
                case Side.Top:
                    Top = type;
                    break;
                case Side.Bottom:
                    Bottom = type;
                    break;
                case Side.Left:
                    Left = type;
                    break;
                case Side.Right:
                    Right = type;
                    break;
                default:
                    throw new System.Exception("Invalid Side type " + side);
            }
        }

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