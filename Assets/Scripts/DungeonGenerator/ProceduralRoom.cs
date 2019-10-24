using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
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
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x, _y + 1);
                    if (neighborConnection.Bottom != ConnectionType.None) return neighborConnection.Bottom;
                    else return CreateNewConnection();
                case Side.Bottom:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x, _y - 1);
                    if (neighborConnection.Top != ConnectionType.None) return neighborConnection.Top;
                    else return CreateNewConnection();
                case Side.Left:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x - 1, _y);
                    if (neighborConnection.Right != ConnectionType.None) return neighborConnection.Right;
                    else return CreateNewConnection();
                case Side.Right:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x + 1, _y);
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

        struct NextRoomData
        {
            public readonly int X;
            public readonly int Y;
            public readonly Side Side;

            public NextRoomData(int x, int y, Side side)
            {
                X = x;
                Y = y;
                Side = side;
            }
        }

        protected override void CreateNextRooms()
        {
            /*
            if (CanCreateNextRoom(Connection.Top)) CreateNextRoom(_x, _y + 1);
            if (CanCreateNextRoom(Connection.Bottom)) CreateNextRoom(_x, _y - 1);
            if (CanCreateNextRoom(Connection.Left)) CreateNextRoom(_x - 1, _y);
            if (CanCreateNextRoom(Connection.Right)) CreateNextRoom(_x + 1, _y);
            */
            
            List<NextRoomData> queue = new List<NextRoomData>();
            if (CanCreateNextRoom(Connection.Top)) queue.Add(new NextRoomData(_x, _y + 1, Side.Top));
            if (CanCreateNextRoom(Connection.Bottom)) queue.Add(new NextRoomData(_x, _y - 1, Side.Bottom));
            if (CanCreateNextRoom(Connection.Left)) queue.Add(new NextRoomData(_x - 1, _y, Side.Left));
            if (CanCreateNextRoom(Connection.Right)) queue.Add(new NextRoomData(_x + 1, _y, Side.Right));
            queue.Shuffle();
            foreach (var room in queue)
            {
                //Debug.Log(room);
                CreateNextRoom(room.X, room.Y, room.Side);
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
