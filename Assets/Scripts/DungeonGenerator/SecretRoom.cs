using UnityEngine;

namespace DungeonGenerator
{
    public class SecretRoom : ProceduralRoom
    {
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

        protected override void CreateNextRoom(int x, int y)
        {
            IRoom nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom is EmptyRoom)
            {
                Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                nextRoom = Instantiate(DungeonManager.Dungeon.SecretRoomPrefab, nextRoomPosition, Transform.rotation, Transform.parent).GetComponent<SecretRoom>();
                DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
            }
        }
    }
}