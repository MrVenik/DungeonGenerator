using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    internal class TemplateRoom : Room
    {
        public override bool CanCreate(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public override void Create(int x, int y)
        {
            _x = x;
            _y = y;

            Connection = new Connection();
            CreateConnections();
            CreateNextRooms();
        }

        protected override void CreateConnections()
        {
            Connection = Connection.Start;
        }

        protected override void CreateNextRooms()
        {
            if (CanCreateNextRoom(Connection.Top)) CreateNextRoom(_x, _y + 1);
            if (CanCreateNextRoom(Connection.Bottom)) CreateNextRoom(_x, _y - 1);
            if (CanCreateNextRoom(Connection.Left)) CreateNextRoom(_x - 1, _y);
            if (CanCreateNextRoom(Connection.Right)) CreateNextRoom(_x + 1, _y);
        }

        protected virtual bool CanCreateNextRoom(ConnectionType type)
        {
            return type != ConnectionType.Wall
                && type != ConnectionType.None
                && type != ConnectionType.Border
                && type != ConnectionType.CorridorWall;
        }

        protected override void CreateNextRoom(int x, int y)
        {
            IRoom nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom is EmptyRoom)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);

                //if (PossibleNextRooms == null)
                //{
                PossibleNextRooms = DungeonManager.Dungeon.AllRooms;
                //}

                List<RoomPrefabData> nextRooms = new List<RoomPrefabData>();

                foreach (var room in PossibleNextRooms)
                {
                    if (chance < room.Chance)
                    {
                        IRoom possibleNextRoom = room.Prefab.GetComponent<Room>();
                        if (possibleNextRoom.CanCreate(x, y))
                        {
                            nextRooms.Add(room);
                        }
                    }
                }

                int rndIndex = UnityEngine.Random.Range(0, nextRooms.Count);
                GameObject nextRoomPrefab = nextRooms[rndIndex].Prefab;

                Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                nextRoom = Instantiate(nextRoomPrefab, nextRoomPosition, Transform.rotation, Transform.parent).GetComponent<Room>();
                DungeonManager.Dungeon.SetRoom(nextRoom, x, y);

                /*
                ConnectionType previousConnectionType = default;
                if (previousConnectionType == ConnectionType.SecretRoomDoor)
                {
                    Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                    nextRoom = Instantiate(DungeonManager.Dungeon.SecretRoomPrefab, nextRoomPosition, Transform.rotation, Transform.parent).GetComponent<Room>();
                    DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
                }
                else
                {
                    if (chance >= 0.5)
                    {
                        Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                        nextRoom = Instantiate(DungeonManager.Dungeon.CorridorPrefab, nextRoomPosition, Transform.rotation, Transform.parent).GetComponent<Room>();
                        DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
                    }
                    else
                    {
                        Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                        nextRoom = Instantiate(DungeonManager.Dungeon.RoomPrefab, nextRoomPosition, Transform.rotation, Transform.parent).GetComponent<Room>();
                        DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
                    }
                }*/
            }
        }

        public override void Build()
        {
            //Instantiate(DungeonManager.Dungeon.StartRoomPrefab, new Vector3(Transform.position.x, Transform.position.y), Transform.rotation, Transform);
        }
    }
}