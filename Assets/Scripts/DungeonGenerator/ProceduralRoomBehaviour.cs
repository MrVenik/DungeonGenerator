using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class ProceduralRoomBehaviour : RoomBehaviour
    {
        [SerializeField] protected List<GameObject> WallConnectionVariants;
        [SerializeField] protected List<GameObject> SmallConnectionVariants;
        [SerializeField] protected List<GameObject> MediumConnectionVariants;
        [SerializeField] protected List<GameObject> BigConnectionVariants;
        [SerializeField] protected List<GameObject> SecretConnectionVariants;

        [SerializeField] public List<ConnectionType> PossibleConnectionTypes;
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
            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (!PossibleConnectionTypes.Contains(topConnection.Bottom) && topConnection.Bottom != ConnectionType.None) return false;
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (!PossibleConnectionTypes.Contains(bottomConnection.Top) && bottomConnection.Top != ConnectionType.None) return false;
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (!PossibleConnectionTypes.Contains(leftConnection.Right) && leftConnection.Right != ConnectionType.None) return false;
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (!PossibleConnectionTypes.Contains(rightConnection.Left) && rightConnection.Left != ConnectionType.None) return false;

            return true;
        }

        public override void Create(int x, int y)
        {
            AmountOfOpenConnections = 0;

            X = x;
            Y = y;

            Connection = new Connection
            {
                Top = ConnectTo(Side.Top),
                Bottom = ConnectTo(Side.Bottom),
                Left = ConnectTo(Side.Left),
                Right = ConnectTo(Side.Right)
            };

            if (CanCreateNextRoom(Connection.Top)) AmountOfOpenConnections++;
            if (CanCreateNextRoom(Connection.Bottom)) AmountOfOpenConnections++;
            if (CanCreateNextRoom(Connection.Left)) AmountOfOpenConnections++;
            if (CanCreateNextRoom(Connection.Right)) AmountOfOpenConnections++;

            CreateNextRooms();
        }

        protected override void CreateNextRooms()
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
                RoomBehaviour nextRoom = null;
                switch (side)
                {
                    case Side.Top:
                        
                        if (Connection.Top == ConnectionType.None)
                        {
                            CreateNextRoom(X, Y + 1, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(X, Y + 1);
                            if (nextRoom == null) Connection.Top = ConnectionType.Wall;
                            else Connection.Top = nextRoom.Connection.Bottom;
                            if (CanCreateNextRoom(Connection.Top)) AmountOfOpenConnections++;
                        }
                        
                        break;
                    case Side.Bottom:
                        
                        if (Connection.Bottom == ConnectionType.None)
                        {
                            CreateNextRoom(X, Y - 1, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(X, Y - 1);
                            if (nextRoom == null) Connection.Bottom = ConnectionType.Wall;
                            else Connection.Bottom = nextRoom.Connection.Top;
                            if (CanCreateNextRoom(Connection.Bottom)) AmountOfOpenConnections++;
                        }
                        
                        break;
                    case Side.Left:
                        
                        if (Connection.Left == ConnectionType.None)
                        {
                            CreateNextRoom(X - 1, Y, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(X - 1, Y);
                            if (nextRoom == null) Connection.Left = ConnectionType.Wall;
                            else Connection.Left = nextRoom.Connection.Right;
                            if (CanCreateNextRoom(Connection.Left)) AmountOfOpenConnections++;
                        }
                        
                        break;
                    case Side.Right:
                        
                        if (Connection.Right == ConnectionType.None)
                        {
                            CreateNextRoom(X + 1, Y, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(X + 1, Y);
                            if (nextRoom == null) Connection.Right = ConnectionType.Wall;
                            else Connection.Right = nextRoom.Connection.Left;
                            if (CanCreateNextRoom(Connection.Right)) AmountOfOpenConnections++;
                        }
                        
                        break;
                    default:
                        break;
                }
            }
        }

        private ConnectionType ConnectTo(Side side)
        {
            switch (side)
            {
                case Side.Top:
                    return ConnectToTop();

                case Side.Bottom:
                    return ConnectToBottom();

                case Side.Left:
                    return ConnectToLeft();

                case Side.Right:
                    return ConnectToRight();
                default:
                    break;
            }
            throw new Exception("Invalid side type: " + side);
        }

        private ConnectionType ConnectToRight()
        {
            RoomBehaviour rightRoom = DungeonManager.Dungeon.GetRoom(X + 1, Y);
            ConnectionType rightConnection;
            if (rightRoom != null)
            {
                rightConnection = rightRoom.Connection.Left;
                if (rightConnection == ConnectionType.None)
                {
                    rightConnection = CreateNewConnection(rightRoom as ProceduralRoomBehaviour);
                }
            }
            else
            {
                rightConnection = DungeonManager.Dungeon.GetRoomConnection(X + 1, Y).Left;
            }
            return rightConnection;
        }

        private ConnectionType ConnectToBottom()
        {
            RoomBehaviour bottomRoom = DungeonManager.Dungeon.GetRoom(X, Y - 1);
            ConnectionType bottomConnection;
            if (bottomRoom != null)
            {
                bottomConnection = bottomRoom.Connection.Top;
                if (bottomConnection == ConnectionType.None)
                {
                    bottomConnection = CreateNewConnection(bottomRoom as ProceduralRoomBehaviour);
                }
            }
            else
            {
                bottomConnection = DungeonManager.Dungeon.GetRoomConnection(X, Y - 1).Top;
            }
            return bottomConnection;
        }

        private ConnectionType ConnectToLeft()
        {
            RoomBehaviour leftRoom = DungeonManager.Dungeon.GetRoom(X - 1, Y);
            ConnectionType leftConnection;
            if (leftRoom != null)
            {
                leftConnection = leftRoom.Connection.Right;
                if (leftConnection == ConnectionType.None)
                {
                    leftConnection = CreateNewConnection(leftRoom as ProceduralRoomBehaviour);
                }
            }
            else
            {
                leftConnection = DungeonManager.Dungeon.GetRoomConnection(X - 1, Y).Right;
            }
            return leftConnection;
        }

        private ConnectionType ConnectToTop()
        {
            RoomBehaviour topRoom = DungeonManager.Dungeon.GetRoom(X, Y + 1);
            ConnectionType topConnection;
            if (topRoom != null)
            {
                topConnection = topRoom.Connection.Bottom;
                if (topConnection == ConnectionType.None)
                {
                    topConnection = CreateNewConnection(topRoom as ProceduralRoomBehaviour);
                }
            }
            else
            {
                topConnection = DungeonManager.Dungeon.GetRoomConnection(X, Y + 1).Bottom;
            }
            return topConnection;
        }

        protected virtual ConnectionType CreateNewConnection(ProceduralRoomBehaviour room)
        {
            List<ConnectionData> possibleConnections = new List<ConnectionData>();
            foreach (var connectionData in PossibleNextConnections)
            {
                if (room.PossibleConnectionTypes.Contains(connectionData.ConnectionType))
                {
                    possibleConnections.Add(connectionData);
                }
            }

            return GetRandomConnection(possibleConnections);
        }

        private ConnectionType GetRandomConnection(List<ConnectionData> possibleConnections)
        {
            if (possibleConnections.Count > 0)
            {
                List<ConnectionType> nextConnections = new List<ConnectionType>();

                float chance = UnityEngine.Random.Range(0f, 1f);

                foreach (var connection in possibleConnections)
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
                return GetRandomConnection(possibleConnections);
            }
            else throw new Exception("Possible connection types list is empty");
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
