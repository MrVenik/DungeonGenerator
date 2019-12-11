namespace DungeonGenerator
{
    public enum Side
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public static class SideExtansions
    {
        public static int X(this Side side)
        {
            switch (side)
            {
                case Side.Top:
                    return 0;
                case Side.Bottom:
                    return 0;
                case Side.Left:
                    return -1;
                case Side.Right:
                    return 1;
                default:
                    break;
            }
            throw new System.Exception("Invalid side type");
        }

        public static int Y(this Side side)
        {
            switch (side)
            {
                case Side.Top:
                    return 1;
                case Side.Bottom:
                    return -1;
                case Side.Left:
                    return 0;
                case Side.Right:
                    return 0;
                default:
                    break;
            }
            throw new System.Exception("Invalid side type");
        }

        public static Side Oposite(this Side side)
        {
            switch (side)
            {
                case Side.Top:
                    return Side.Bottom;
                case Side.Bottom:
                    return Side.Top;
                case Side.Left:
                    return Side.Right;
                case Side.Right:
                    return Side.Left;
                default:
                    break;
            }
            throw new System.Exception("Invalid side type");
        }

        public static Side Rotate(this Side side, bool clockwise = false)
        {
            switch (side)
            {
                case Side.Top:
                    return clockwise ? Side.Right : Side.Left;
                case Side.Bottom:
                    return clockwise ? Side.Left : Side.Right;
                case Side.Left:
                    return clockwise ? Side.Top : Side.Bottom;
                case Side.Right:
                    return clockwise ? Side.Bottom : Side.Top;
                default:
                    break;
            }
            throw new System.Exception("Invalid side type");
        }
    }
}