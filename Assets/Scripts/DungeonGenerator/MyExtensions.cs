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
            switch (side)
            {
                case Side.Top:
                    room.Rotate(180f);
                    break;
                case Side.Bottom:
                    //room.Rotate(0);
                    break;
                case Side.Left:
                    room.Rotate(270f);
                    break;
                case Side.Right:
                    room.Rotate(90f);
                    break;
                default:
                    break;
            }
        }

        public static void Rotate(this Room room, float degrees)
        {
            room.Rotate((int)degrees / 90);
        }

        public static void Rotate(this Room room, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                room.Rotate();
            }
        }

        public static void Rotate(this Room room)
        {
            Connection newConnection = new Connection()
            {
                Left = room.Connection.Top,
                Bottom = room.Connection.Left,
                Right = room.Connection.Bottom,
                Top = room.Connection.Right
            };
            room.Connection = newConnection;
        }
    }
}
