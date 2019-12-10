using System.Collections.Generic;

namespace DungeonGenerator
{
    public static class ArrayExtensions
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

        public static T[,] Rotate<T>(this T[,] array, bool clockwise = false)
        {
            int size = (int)System.Math.Sqrt(array.Length);

            T[,] newArray = new T[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int nx = clockwise ? y : size - 1 - y;
                    int ny = clockwise ? size - 1 - x : x;
                    newArray[nx, ny] = array[x, y];
                }
            }
            return newArray;
        }
    }
}
