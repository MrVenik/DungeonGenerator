using System.Collections.Generic;

namespace DungeonGenerator
{
    public static class MyExtensions
    {
        public static Side Oposite(this Side side)
        {
            switch (side)
            {
                case Side.Top:
                    return Side.Bottom;
                case Side.Bottom:
                    return  Side.Top;
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

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Rotate(this Room room, Side side)
        {
            switch (room.Entrance)
            {
                case Side.Top:
                    switch (side)
                    {
                        case Side.Top:
                            break;
                        case Side.Bottom:
                            room.Rotate(2);
                            break;
                        case Side.Left:
                            room.Rotate(1, false);
                            break;
                        case Side.Right:
                            room.Rotate(1, true);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Bottom:
                    switch (side)
                    {
                        case Side.Top:
                            room.Rotate(2);
                            break;
                        case Side.Bottom:
                            break;
                        case Side.Left:
                            room.Rotate(1, true);
                            break;
                        case Side.Right:
                            room.Rotate(1, false);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Left:
                    switch (side)
                    {
                        case Side.Top:
                            room.Rotate(1, true);
                            break;
                        case Side.Bottom:
                            room.Rotate(1, false);
                            break;
                        case Side.Left:
                            break;
                        case Side.Right:
                            room.Rotate(2);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Right:
                    switch (side)
                    {
                        case Side.Top:
                            room.Rotate(1, false);
                            break;
                        case Side.Bottom:
                            room.Rotate(1, true);
                            break;
                        case Side.Left:
                            room.Rotate(2);
                            break;
                        case Side.Right:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public static void Rotate(this Room room, float degrees)
        {
            room.Rotate((int)degrees / 90);
        }

        public static void Rotate(this Room room, int steps, bool clockwise = false)
        {
            for (int i = 0; i < steps; i++)
            {
                room.Rotate(clockwise);
            }
        }

        public static void Rotate(this Room room, bool clockwise = false)
        {
            Connection newConnection = new Connection()
            {
                Top = clockwise ? room.Connection.Left : room.Connection.Right,
                Bottom = clockwise ? room.Connection.Right : room.Connection.Left,
                Left = clockwise ? room.Connection.Bottom : room.Connection.Top,
                Right = clockwise ? room.Connection.Top : room.Connection.Bottom,
            };
            room.Connection = newConnection;
            room.Entrance = room.Entrance.Rotate(clockwise);
        }
    }
}
