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
                    if (roomPrefabData.IsGroup)
                    {
                        RoomGroup possibleNextRoom = roomPrefabData.Prefab.GetComponent<RoomGroup>();

                        if (CheckRoomGroup(x, y, side, possibleNextRoom))
                        {
                            CreateRoomGroup(x, y, roomPrefabData);
                        }
                        else throw new Exception($"Cant create room: {roomPrefabData.Name} on X={x}, Y={y}");
                    }
                    else
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
        }

        private static void CreateRoomGroup(int x, int y, RoomPrefabData roomPrefabData)
        {
            Debug.Log($"I creating room group on {x},{y}");
            RoomGroup roomGroup = roomPrefabData.Prefab.GetComponent<RoomGroup>();
            for (int ix = x - roomGroup.EntranceX, j = 0; ix < x + roomGroup.ArraySize - roomGroup.EntranceX; ix++, j++)
            {
                for (int iy = y - roomGroup.EntranceY, k = 0; iy < y + roomGroup.ArraySize - roomGroup.EntranceY; iy++, k++)
                {
                    GameObject nextRoomPrefab = roomGroup.Rooms[j + k * roomGroup.ArraySize].Prefab;
                    Vector3 nextRoomPosition = new Vector3(ix * (int)DungeonManager.Dungeon.MaximumRoomSize, iy * (int)DungeonManager.Dungeon.MaximumRoomSize);
                    Room nextRoom = RoomSpawner.Spawn(nextRoomPosition, nextRoomPrefab);
                    nextRoom.Entrance = roomGroup.Rooms[j + k * roomGroup.ArraySize].Entrance;
                    nextRoom.Connection = roomGroup.Rooms[j + k * roomGroup.ArraySize].Connection;
                    DungeonManager.Dungeon.SetRoom(nextRoom, ix, iy);
                }
            }

            for (int ix = x - roomGroup.EntranceX, j = 0; ix < x + roomGroup.ArraySize - roomGroup.EntranceX; ix++, j++)
            {
                for (int iy = y - roomGroup.EntranceY, k = 0; iy < y + roomGroup.ArraySize - roomGroup.EntranceY; iy++, k++)
                {
                    Room room = DungeonManager.Dungeon.GetRoom(ix, iy);
                    if (room != null)
                    {
                        room.Create(ix, iy);
                    }
                }
            }
        }

        private static bool CheckRoomGroup(int x, int y, Side side, RoomGroup possibleNextRoom)
        {
            Debug.Log("Check if i can create room group");

            foreach (var item in possibleNextRoom.Rooms)
            {
                if (item.Room.Size > DungeonManager.Dungeon.MaximumRoomSize) return false;
            }

            possibleNextRoom.Rotate(side.Oposite());
            possibleNextRoom.RotateRooms();

            return possibleNextRoom.CanCreate(x, y);

        }

        public static void Create(int x, int y, Side side, List<RoomPrefabData> possibleRooms)
        {
            if (possibleRooms.Count == 1)
            {
                Create(x, y, side, possibleRooms[0]);
            }

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
                        if (nextRoomPrefabData.IsGroup)
                        {
                            CreateRoomGroup(x, y, nextRoomPrefabData);
                        }
                        else
                        {
                            CreateRoom(x, y, nextRoomPrefabData);
                        }
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
            DungeonManager.Dungeon.GetRoom(x, y).Create(x, y);
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
                else
                {
                    possibleNextRoom.Entrance = side.Oposite();
                }

                return possibleNextRoom.CanCreate(x, y);
            }
            else return false;
        }

        private static RoomPrefabData GetRandomRoom(int x, int y, Side side, List<RoomPrefabData> possibleRooms)
        {
            RoomPrefabData nextRoom = null;

            float total = 0;

            foreach (var item in possibleRooms) total += item.Chance;

            float chance = UnityEngine.Random.Range(0f, total);

            foreach (var room in possibleRooms)
            {
                float chanceMultiplier = room.IsPlug ? DungeonManager.Dungeon.PlugChance : DungeonManager.Dungeon.FillingChance;

                if (chance > 0 && chance <= (room.Chance * chanceMultiplier))
                {
                    if (room.IsGroup)
                    {
                        RoomGroup possibleNextRoom = room.Prefab.GetComponent<RoomGroup>();
                        if (CheckRoomGroup(x, y, side, possibleNextRoom)) nextRoom = room;
                    }
                    else
                    {
                        Room possibleNextRoom = room.Prefab.GetComponent<Room>();
                        if (CheckRoom(x, y, side, possibleNextRoom)) nextRoom = room;
                    }
                    break;
                }
                chance -= room.Chance;
            }
            return nextRoom;
        }
    }
}
