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
        [SerializeField] protected List<ConnectionData> PossibleNextConnections;
        [SerializeField] protected int AmmountOfOpenConnections = 0;

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
            List<Side> queue = new List<Side>
            {
                Side.Top,
                Side.Bottom,
                Side.Left,
                Side.Right
            };

            queue.Shuffle();
            foreach (var side in queue)
            {
                GetOrCreateConnection(side);
            }
        }

        protected virtual ConnectionType GetOrCreateConnection(Side side)
        {
            Connection neighborConnection;
            switch (side)
            {
                case Side.Top:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x, _y + 1);

                    if (neighborConnection.Bottom != ConnectionType.None) Connection.Top = neighborConnection.Bottom;
                    else Connection.Top = CreateNewConnection();

                    if (CanCreateNextRoom(Connection.Top)) AmmountOfOpenConnections++;

                    return Connection.Top;
                case Side.Bottom:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x, _y - 1);

                    if (neighborConnection.Top != ConnectionType.None) Connection.Bottom = neighborConnection.Top;
                    else Connection.Bottom = CreateNewConnection();

                    if (CanCreateNextRoom(Connection.Bottom)) AmmountOfOpenConnections++;

                    return Connection.Bottom;
                case Side.Left:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x - 1, _y);

                    if (neighborConnection.Right != ConnectionType.None) Connection.Left = neighborConnection.Right;
                    else Connection.Left = CreateNewConnection();

                    if (CanCreateNextRoom(Connection.Left)) AmmountOfOpenConnections++;

                    return Connection.Left;
                case Side.Right:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x + 1, _y);

                    if (neighborConnection.Left != ConnectionType.None) Connection.Right = neighborConnection.Left;
                    else Connection.Right = CreateNewConnection();

                    if (CanCreateNextRoom(Connection.Right)) AmmountOfOpenConnections++;

                    return Connection.Right;
                default:
                    break;
            }
            throw new Exception("Invalid side type");
        }

        protected virtual ConnectionType CreateNewConnection()
        {
            List<ConnectionType> nextConnections = new List<ConnectionType>();

            float chance = UnityEngine.Random.Range(0f, 1f);

            foreach (var connection in PossibleNextConnections)
            {
                if (chance < connection.Chance)
                {
                    ConnectionType possibleNextConnection = connection.ConnectionType;
                    nextConnections.Add(possibleNextConnection);
                }
            }

            if (nextConnections.Count > 0)
            {
                int rndIndex = UnityEngine.Random.Range(0, nextConnections.Count);
                return nextConnections[rndIndex];
            }
            else throw new Exception("There no connection to create");
        }

        public override void Build()
        {
            int size = DungeonManager.Dungeon.RoomSize;
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x, Transform.position.y), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x, Transform.position.y + size - 1), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y + size - 1), Transform.rotation, Transform);

            if (Connection.Top == ConnectionType.Wall || Connection.Top == ConnectionType.Small || Connection.Top == ConnectionType.SecretRoomDoor || Connection.Top == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size - 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.Wall || Connection.Bottom == ConnectionType.Small || Connection.Bottom == ConnectionType.SecretRoomDoor || Connection.Bottom == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.Wall || Connection.Left == ConnectionType.Small || Connection.Left == ConnectionType.SecretRoomDoor || Connection.Left == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.Wall || Connection.Right == ConnectionType.Small || Connection.Right == ConnectionType.SecretRoomDoor || Connection.Right == ConnectionType.Border)
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
                case ConnectionType.Medium:
                    break;
                case ConnectionType.Small:
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
