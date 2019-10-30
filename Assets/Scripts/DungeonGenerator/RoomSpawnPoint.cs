using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public static class RoomSpawnPoint
    {
        public static void Spawn(int x, int y, Side side)
        {
            Room nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom == null)
            {
                Spawn(x, y, DungeonManager.Dungeon.AllRooms, side);
            }
        }

        public static void Spawn(int x, int y, List<RoomPrefabData> possibleRooms, Side side)
        {
            if (possibleRooms.Count == 0)
            {
                Spawn(x, y, side);
                return;
            }

            if (possibleRooms.Count == 1)
            {
                Spawn(x, y, possibleRooms[0], side);
                return;
            }

            Room nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom == null)
            {
                List<RoomPrefabData> nextRooms = new List<RoomPrefabData>();

                float chance = UnityEngine.Random.Range(0f, 1f);

                foreach (var room in possibleRooms)
                {
                    if (chance < room.Chance)
                    {
                        Room possibleNextRoom = room.Prefab.GetComponent<Room>();
                        if (possibleNextRoom.Size <= DungeonManager.Dungeon.MaximumRoomSize)
                        {
                            if (possibleNextRoom is TemplateRoom)
                            {
                                possibleNextRoom.Rotate(side);
                            }
                            if (possibleNextRoom.CanCreate(x, y))
                            {
                                nextRooms.Add(room);
                            }
                        }
                    }
                }

                if (nextRooms.Count > 0)
                {
                    int rndIndex = UnityEngine.Random.Range(0, nextRooms.Count);
                    Spawn(x, y, nextRooms[rndIndex], side);
                }
                else throw new Exception("There no room to create");
            }
        }

        public static void Spawn(int x, int y, RoomPrefabData room, Side side)
        {
            Room nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom == null)
            {
                GameObject nextRoomPrefab = room.Prefab;
                Vector3 nextRoomPosition = new Vector3(x * (int)DungeonManager.Dungeon.MaximumRoomSize, y * (int)DungeonManager.Dungeon.MaximumRoomSize);
                nextRoom = UnityEngine.Object.Instantiate(nextRoomPrefab, nextRoomPosition, Quaternion.identity, DungeonManager.Dungeon.Transform).GetComponent<Room>();
                nextRoom.name = nextRoomPrefab.name;
                nextRoom.Entrance = side.Oposite();
                nextRoom.Rotate(side);
                DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
            }
        }
    }
}
