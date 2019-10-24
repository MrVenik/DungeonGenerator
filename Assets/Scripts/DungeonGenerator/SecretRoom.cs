using UnityEngine;

namespace DungeonGenerator
{
    public class SecretRoom : ProceduralRoom
    {
        public override bool CanCreate(int x, int y)
        {
            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (topConnection.Bottom == ConnectionType.Small || topConnection.Bottom == ConnectionType.Medium)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x, y + 1);
                return CheckSecretRoom(room);
            }
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (bottomConnection.Top == ConnectionType.Small || bottomConnection.Top == ConnectionType.Medium)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x, y - 1);
                return CheckSecretRoom(room);
            }
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (leftConnection.Right == ConnectionType.Small || leftConnection.Right == ConnectionType.Medium)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x - 1, y);
                return CheckSecretRoom(room);
            }
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (rightConnection.Left == ConnectionType.Small || rightConnection.Left == ConnectionType.Medium)
            {
                Room room = DungeonManager.Dungeon.GetRoom(x + 1, y);
                return CheckSecretRoom(room);
            }
            return true;
        }

        private bool CheckSecretRoom(Room room)
        {
            if (room != null)
            {
                if (room is SecretRoom)
                {
                    return true;
                }
            }
            return false;
        }

        protected override ConnectionType CreateNewConnection()
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance > 0.1f && chance <= 0.2f)
            {
                return ConnectionType.Medium;
            }
            else if (chance <= 0.1f)
            {
                return ConnectionType.Small;
            }
            else return ConnectionType.Wall;
        }
    }
}