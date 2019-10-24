using UnityEngine;

namespace DungeonGenerator
{
    public class SecretRoom : ProceduralRoom
    {
        public override bool CanCreate(int x, int y)
        {
            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (topConnection.Bottom == ConnectionType.Door || topConnection.Bottom == ConnectionType.Open)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x, y + 1);
                if (room != null)
                {
                    if (room is SecretRoom)
                    {
                        return true;
                    }
                }
                return false;
            }
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (bottomConnection.Top == ConnectionType.Door || bottomConnection.Top == ConnectionType.Open)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x, y - 1);
                if (room != null)
                {
                    if (room is SecretRoom)
                    {
                        return true;
                    }
                }
                return false;
            }
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (leftConnection.Right == ConnectionType.Door || leftConnection.Right == ConnectionType.Open)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x - 1, y);
                if (room != null)
                {
                    if (room is SecretRoom)
                    {
                        return true;
                    }
                }
                return false;
            }
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (rightConnection.Left == ConnectionType.Door || rightConnection.Left == ConnectionType.Open)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x + 1, y);
                if (room != null)
                {
                    if (room is SecretRoom)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        protected override ConnectionType CreateNewConnection()
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance > 0.1f && chance <= 0.2f)
            {
                return ConnectionType.Open;
            }
            else if (chance <= 0.1f)
            {
                return ConnectionType.Door;
            }
            else return ConnectionType.Wall;

            //int index = UnityEngine.Random.Range(0, PossibleConnectionTypes.Length);
            //return PossibleConnectionTypes[index];
        }
    }
}