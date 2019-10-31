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
        [SerializeField] protected List<GameObject> WallConnectionVariants;
        [SerializeField] protected List<GameObject> SmallConnectionVariants;
        [SerializeField] protected List<GameObject> MediumConnectionVariants;
        [SerializeField] protected List<GameObject> BigConnectionVariants;
        [SerializeField] protected List<GameObject> SecretConnectionVariants;

        [SerializeField] protected List<ConnectionData> PossibleNextConnections;

        [SerializeField] private int _amountOfOpenConnections;
        public int AmountOfOpenConnections
        {
            get => _amountOfOpenConnections;
            protected set
            {
                if (value > 4) throw new Exception("Amount of open connections cant be more than 4");
                else _amountOfOpenConnections = value;
            }
        }

        public override bool CanCreate(int x, int y)
        {
            return true;
        }

        public override void Create(int x, int y)
        {
            AmountOfOpenConnections = 0;

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

                    if (CanCreateNextRoom(Connection.Top)) AmountOfOpenConnections++;

                    return Connection.Top;
                case Side.Bottom:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x, _y - 1);

                    if (neighborConnection.Top != ConnectionType.None) Connection.Bottom = neighborConnection.Top;
                    else Connection.Bottom = CreateNewConnection();

                    if (CanCreateNextRoom(Connection.Bottom)) AmountOfOpenConnections++;

                    return Connection.Bottom;
                case Side.Left:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x - 1, _y);

                    if (neighborConnection.Right != ConnectionType.None) Connection.Left = neighborConnection.Right;
                    else Connection.Left = CreateNewConnection();

                    if (CanCreateNextRoom(Connection.Left)) AmountOfOpenConnections++;

                    return Connection.Left;
                case Side.Right:
                    neighborConnection = DungeonManager.Dungeon.GetRoomConnection(_x + 1, _y);

                    if (neighborConnection.Left != ConnectionType.None) Connection.Right = neighborConnection.Left;
                    else Connection.Right = CreateNewConnection();

                    if (CanCreateNextRoom(Connection.Right)) AmountOfOpenConnections++;

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
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)Size;

            int diff = (maximumSize - roomSize) / 2;

            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + Transform.position.x + diff, 0.5f + Transform.position.y + diff), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + Transform.position.x + diff, 0.5f + Transform.position.y + maximumSize - 1 - diff), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + Transform.position.x + maximumSize - 1 - diff, 0.5f + Transform.position.y + diff), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + Transform.position.x + maximumSize - 1 - diff, 0.5f + Transform.position.y + maximumSize - 1 - diff), Transform.rotation, Transform);

            GameObject element;
            // Top connection
            element = GetConnectionGameObject(Connection.Top);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + Transform.position.x + 1 + diff, 0.5f + Transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 0), Transform);
            // Bottom connection
            element = GetConnectionGameObject(Connection.Bottom);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + Transform.position.x + maximumSize - 2 - diff, 0.5f + Transform.position.y + diff), Quaternion.Euler(0, 0, 180), Transform);
            // Left connection
            element = GetConnectionGameObject(Connection.Left);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + Transform.position.x + diff, 0.5f + Transform.position.y + 1 + diff), Quaternion.Euler(0, 0, 90), Transform);
            // Ritght connection
            element = GetConnectionGameObject(Connection.Right);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + Transform.position.x + maximumSize - 1 - diff, 0.5f + Transform.position.y + maximumSize - 2 - diff), Quaternion.Euler(0, 0, -90), Transform);
        }

        protected virtual GameObject GetConnectionGameObject(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    return GetVariantFrom(WallConnectionVariants);
                case ConnectionType.Wall:
                    return GetVariantFrom(WallConnectionVariants);
                case ConnectionType.Medium:
                    return GetVariantFrom(MediumConnectionVariants);
                case ConnectionType.Small:
                    return GetVariantFrom(SmallConnectionVariants);
                case ConnectionType.Big:
                    return GetVariantFrom(BigConnectionVariants);
                case ConnectionType.SecretRoomDoor:
                    return GetVariantFrom(SecretConnectionVariants);
                default:
                    break;
            }
            throw new Exception("Invalid connection type " + type);
        }

        protected virtual GameObject GetVariantFrom(List<GameObject> variants)
        {
            if (variants != null && variants.Count > 0)
            {
                int rndIndex = UnityEngine.Random.Range(0, variants.Count);
                return variants[rndIndex];
            }
            return null;
        }
    }
}
