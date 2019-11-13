using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public static class RoomCreator
    {
        public static void Create(int x, int y, Side side)
        {
            Create(x, y, side, DungeonManager.Dungeon.AllRooms);
        }

        public static void Create(int x, int y, Side side, RoomPrefabData roomPrefabData)
        {
            Room nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom == null)
            {
                if (roomPrefabData != null)
                {
                    Room possibleNextRoom = roomPrefabData.Prefab.GetComponent<Room>();

                    if (CheckRoom(x, y, side, possibleNextRoom))
                    {
                        CreateRoom(x, y, roomPrefabData);
                    }
                    else throw new Exception($"Cant create room: {roomPrefabData.Name} on X={x}, Y={y}");
                }
            }
        }

        public static void Create(int x, int y, Side side, List<RoomPrefabData> possibleRooms)
        {
            Room nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom == null)
            {
                if (possibleRooms != null)
                {
                    if (possibleRooms.Count == 0)
                    {
                        possibleRooms = DungeonManager.Dungeon.AllRooms;
                    }

                    RoomPrefabData nextRoomPrefabData = GetRandomRoom(x, y, side, possibleRooms);
                    if (nextRoomPrefabData != null)
                    {
                        CreateRoom(x, y, nextRoomPrefabData);
                    }
                }
                else throw new Exception("Possible Rooms array is null");
            }
        }

        private static Room CreateRoom(int x, int y, RoomPrefabData roomPrefabData)
        {
            Room nextRoom;
            GameObject nextRoomPrefab = roomPrefabData.Prefab;
            Vector3 nextRoomPosition = new Vector3(x * (int)DungeonManager.Dungeon.MaximumRoomSize, y * (int)DungeonManager.Dungeon.MaximumRoomSize);
            nextRoom = RoomSpawner.Spawn(nextRoomPosition, nextRoomPrefab);
            DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
            return nextRoom;
        }

        private static bool CheckRoom(int x, int y, Side side, Room possibleNextRoom)
        {
            if (possibleNextRoom.Size <= DungeonManager.Dungeon.MaximumRoomSize)
            {
                if (possibleNextRoom is TemplateRoom)
                {
                    possibleNextRoom.Rotate(side.Oposite());
                }

                possibleNextRoom.Entrance = side.Oposite();

                return possibleNextRoom.CanCreate(x, y);
            }
            else return false;
        }

        private static RoomPrefabData GetRandomRoom(int x, int y, Side side, List<RoomPrefabData> possibleRooms)
        {
            List<RoomPrefabData> nextRooms = new List<RoomPrefabData>();

            float chance = UnityEngine.Random.Range(0f, 1f);

            foreach (var room in possibleRooms)
            {
                float chanceMultiplier = room.IsPlug ? DungeonManager.Dungeon.PlugChance : DungeonManager.Dungeon.FillingChance;

                if (chance < (room.Chance * chanceMultiplier))
                {
                    Room possibleNextRoom = room.Prefab.GetComponent<Room>();
                    if (CheckRoom(x, y, side, possibleNextRoom)) nextRooms.Add(room);
                }
            }

            if (nextRooms.Count > 0)
            {
                int rndIndex = UnityEngine.Random.Range(0, nextRooms.Count);
                return nextRooms[rndIndex];
            }
            return null;
        }
    }
}
