using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    static class MyExtensions
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
    }

    public class ProceduralRoom : Room
    {
        public override bool CanCreate(int x, int y)
        {
            return true;
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
            Connection.Top = GetOrCreateConnection(Side.Top);
            Connection.Bottom = GetOrCreateConnection(Side.Bottom);
            Connection.Left = GetOrCreateConnection(Side.Left);
            Connection.Right = GetOrCreateConnection(Side.Right);
        }

        protected virtual ConnectionType GetOrCreateConnection(Side side)
        {
            Connection neighborConnection;
            switch (side)
            {
                case Side.Top:
                    neighborConnection = DungeonManager.Dungeon.GetRoom(_x, _y + 1).Connection;
                    if (neighborConnection.Bottom != ConnectionType.None) return neighborConnection.Bottom;
                    else return CreateNewConnection();
                case Side.Bottom:
                    neighborConnection = DungeonManager.Dungeon.GetRoom(_x, _y - 1).Connection;
                    if (neighborConnection.Top != ConnectionType.None) return neighborConnection.Top;
                    else return CreateNewConnection();
                case Side.Left:
                    neighborConnection = DungeonManager.Dungeon.GetRoom(_x - 1, _y).Connection;
                    if (neighborConnection.Right != ConnectionType.None) return neighborConnection.Right;
                    else return CreateNewConnection();
                case Side.Right:
                    neighborConnection = DungeonManager.Dungeon.GetRoom(_x + 1, _y).Connection;
                    if (neighborConnection.Left != ConnectionType.None) return neighborConnection.Left;
                    else return CreateNewConnection();
                default:
                    break;
            }
            throw new Exception("Invalid side type");
        }

        protected virtual ConnectionType CreateNewConnection()
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance <= 0.5f)
            {
                chance = UnityEngine.Random.Range(0f, 1f);
                if (chance >= 0.50f)
                {
                    return ConnectionType.Open;
                }
                else if (chance >= 0.10f)
                {
                    return ConnectionType.Door;
                }
                else
                {
                    return ConnectionType.SecretRoomDoor;
                }
            }
            else return ConnectionType.Wall;

            //int index = UnityEngine.Random.Range(0, PossibleConnectionTypes.Length);
            //return PossibleConnectionTypes[index];
        }

        protected override void CreateNextRooms()
        {
            /*
            if (CanCreateNextRoom(Connection.Top)) CreateNextRoom(_x, _y + 1);
            if (CanCreateNextRoom(Connection.Bottom)) CreateNextRoom(_x, _y - 1);
            if (CanCreateNextRoom(Connection.Left)) CreateNextRoom(_x - 1, _y);
            if (CanCreateNextRoom(Connection.Right)) CreateNextRoom(_x + 1, _y);
            */
            
            List<Vector2Int> queue = new List<Vector2Int>();
            if (CanCreateNextRoom(Connection.Top)) queue.Add(new Vector2Int(_x, _y + 1));//CreateNextRoom(_x, _y + 1);
            if (CanCreateNextRoom(Connection.Bottom)) queue.Add(new Vector2Int(_x, _y - 1));//CreateNextRoom(_x, _y - 1);
            if (CanCreateNextRoom(Connection.Left)) queue.Add(new Vector2Int(_x - 1, _y));//CreateNextRoom(_x - 1, _y);
            if (CanCreateNextRoom(Connection.Right)) queue.Add(new Vector2Int(_x + 1, _y));//CreateNextRoom(_x + 1, _y);
            queue.Shuffle();
            foreach (var room in queue)
            {
                //Debug.Log(room);
                CreateNextRoom(room.x, room.y);
            }
        }

        protected virtual bool CanCreateNextRoom(ConnectionType type)
        {
            return type != ConnectionType.Wall
                && type != ConnectionType.None
                && type != ConnectionType.Border;
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
            int size = DungeonManager.Dungeon.RoomSize;
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x, Transform.position.y), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x, Transform.position.y + size - 1), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y + size - 1), Transform.rotation, Transform);

            if (Connection.Top == ConnectionType.Wall || Connection.Top == ConnectionType.Door || Connection.Top == ConnectionType.SecretRoomDoor || Connection.Top == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size - 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.Wall || Connection.Bottom == ConnectionType.Door || Connection.Bottom == ConnectionType.SecretRoomDoor || Connection.Bottom == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.Wall || Connection.Left == ConnectionType.Door || Connection.Left == ConnectionType.SecretRoomDoor || Connection.Left == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.Wall || Connection.Right == ConnectionType.Door || Connection.Right == ConnectionType.SecretRoomDoor || Connection.Right == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Right), new Vector3(Transform.position.x + size - 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

        }

        protected virtual GameObject GetConnectionGameObject(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    return DungeonManager.Dungeon.WallPrefab;
                case ConnectionType.Wall:
                    return DungeonManager.Dungeon.WallPrefab;
                case ConnectionType.Open:
                    break;
                case ConnectionType.Door:
                    return DungeonManager.Dungeon.DoorPrefab;
                case ConnectionType.SecretRoomDoor:
                    return DungeonManager.Dungeon.SecretRoomDoorPrefab;
                default:
                    break;
            }
            throw new Exception("Invalid connection type " + type);
        }
    }
}
