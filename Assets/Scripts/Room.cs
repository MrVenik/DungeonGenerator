using System;
using UnityEngine;

namespace DungeonGenerator
{
    public enum Side
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public class Room : MonoBehaviour, IRoom
    {
        [SerializeField] protected Connection Connection;

        public Transform Transform { get; private set; }

        protected virtual ConnectionType[] PossibleConnectionTypes { get; } = new ConnectionType[]
        {
            ConnectionType.Wall,
            ConnectionType.Door,
            ConnectionType.Open
        };

        public Connection GetConnection()
        {
            return Connection;
        }

        protected int _x, _y;

        private void Awake()
        {
            Transform = transform;
        }

        public virtual void Create(int x, int y)
        {
            _x = x;
            _y = y;

            Connection = new Connection();
            CreateConnections();
            CreateNextRooms();
        }

        protected virtual void CreateConnections()
        {
            Connection.Top = CreateConnection(Side.Top);
            Connection.Bottom = CreateConnection(Side.Bottom);
            Connection.Left = CreateConnection(Side.Left);
            Connection.Right = CreateConnection(Side.Right);
        }

        protected virtual ConnectionType CreateConnection(Side side)
        {
            Connection neighborConnection;
            switch (side)
            {
                case Side.Top:
                    neighborConnection = GetConnectionFromNeighborRoom(_x, _y + 1);
                    return ChooseConnectionByType(neighborConnection.Bottom);
                case Side.Bottom:
                    neighborConnection = GetConnectionFromNeighborRoom(_x, _y - 1);
                    return ChooseConnectionByType(neighborConnection.Top);
                case Side.Left:
                    neighborConnection = GetConnectionFromNeighborRoom(_x - 1, _y);
                    return ChooseConnectionByType(neighborConnection.Right);
                case Side.Right:
                    neighborConnection = GetConnectionFromNeighborRoom(_x + 1, _y);
                    return ChooseConnectionByType(neighborConnection.Left);
                default:
                    break;
            }
            throw new Exception("Invalid side type");
        }

        protected virtual ConnectionType ChooseConnectionByType(ConnectionType type)
        {
            if (type == ConnectionType.Border) return ConnectionType.Wall;
            if (type == ConnectionType.CorridorWall) return ConnectionType.Wall;
            if (type != ConnectionType.None) return type;
            else return CreateNewConnection();
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

        protected virtual Connection GetConnectionFromNeighborRoom(int x, int y)
        {
            IRoom neighborRoom = DungeonManager.Dungeon.GetRoom(x, y);
            return neighborRoom.GetConnection();
        }

        protected virtual void CreateNextRooms()
        {
            if (CanCreateNextRoom(Connection.Top)) CreateNextRoom(_x, _y + 1, Connection.Top);
            if (CanCreateNextRoom(Connection.Bottom)) CreateNextRoom(_x, _y - 1, Connection.Bottom);
            if (CanCreateNextRoom(Connection.Left)) CreateNextRoom(_x - 1, _y, Connection.Left);
            if (CanCreateNextRoom(Connection.Right)) CreateNextRoom(_x + 1, _y, Connection.Right);
        }

        protected virtual bool CanCreateNextRoom(ConnectionType type)
        {
            return type != ConnectionType.Wall 
                && type != ConnectionType.None 
                && type != ConnectionType.Border
                && type != ConnectionType.CorridorWall;
        }

        protected virtual void CreateNextRoom(int x, int y, ConnectionType previousConnectionType)
        {
            IRoom nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom is EmptyRoom)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);
                if (previousConnectionType == ConnectionType.SecretRoomDoor)
                {
                    Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                    nextRoom = Instantiate(DungeonManager.Dungeon.SecretRoomPrefab, nextRoomPosition, Transform.rotation, DungeonManager.Dungeon.Transform).GetComponent<SecretRoom>();
                    DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
                }
                else
                {
                    if (chance >= 0.5)
                    {
                        Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                        nextRoom = Instantiate(DungeonManager.Dungeon.CorridorPrefab, nextRoomPosition, Transform.rotation, DungeonManager.Dungeon.Transform).GetComponent<Corridor>();
                        DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
                    }
                    else
                    {
                        Vector3 nextRoomPosition = new Vector3(x * DungeonManager.Dungeon.RoomSize, y * DungeonManager.Dungeon.RoomSize);
                        nextRoom = Instantiate(DungeonManager.Dungeon.RoomPrefab, nextRoomPosition, Transform.rotation, DungeonManager.Dungeon.Transform).GetComponent<Room>();
                        DungeonManager.Dungeon.SetRoom(nextRoom, x, y);
                    }
                }
            }
        }

        public virtual void Build()
        {
            int size = DungeonManager.Dungeon.RoomSize;
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x, Transform.position.y), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x, Transform.position.y + size - 1), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y + size - 1), Transform.rotation, Transform);

            if (Connection.Top == ConnectionType.Wall || Connection.Top == ConnectionType.Door || Connection.Top == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size - 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.Wall || Connection.Bottom == ConnectionType.Door || Connection.Bottom == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.Wall || Connection.Left == ConnectionType.Door || Connection.Left == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.Wall || Connection.Right == ConnectionType.Door || Connection.Right == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Right), new Vector3(Transform.position.x + size - 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

        }

        public virtual GameObject GetConnectionGameObject(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    break;
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