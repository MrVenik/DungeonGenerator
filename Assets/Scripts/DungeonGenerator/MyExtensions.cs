using System.Collections.Generic;

namespace DungeonGenerator
{
    public static class MyExtensions
    {
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

        public static void Rotate(this RoomBehaviour room, Side side)
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

        public static void Rotate(this RoomBehaviour room, float degrees)
        {
            room.Rotate((int)degrees / 90);
        }

        public static void Rotate(this RoomBehaviour room, int steps, bool clockwise = false)
        {
            for (int i = 0; i < steps; i++)
            {
                room.Rotate(clockwise);
            }
        }

        public static void Rotate(this RoomBehaviour room, bool clockwise = false)
        {
            room.Connection = room.Connection.Rotate(clockwise);
            room.Entrance = room.Entrance.Rotate(clockwise);
        }

        public static T[] Rotate<T>(this T[] array, bool clockwise = false)
        {
            int size = (int)System.Math.Sqrt(array.Length);

            T[] newArray = new T[size * size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int i = x + y * size;
                    int nx = clockwise ? y : size - 1 - y;
                    int ny = clockwise ? size - 1 - x : x;
                    int ni = nx + ny * size;
                    newArray[ni] = array[i];
                }
            }
            return newArray;
        }
    }
}
