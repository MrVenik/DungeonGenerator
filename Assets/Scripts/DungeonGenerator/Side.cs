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