using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [Serializable]
    public class ConnectionData
    {
        public ConnectionType ConnectionType;
        public float Chance;
    }

    [CreateAssetMenu(fileName = "New RoomFactory", menuName = "Room Factory/Procedural RoomFactory")]
    public class ProceduralRoomFactory : RoomFacroty
    {
        [SerializeField] private int _amountOfOpenConnections;
        [SerializeField] private int _amountOfNeededRooms;
        [SerializeField] private float _chanceOfNextRoom;
        [SerializeField] private List<ConnectionData> _possibleNextConnectionTypes;

        public int AmountOfOpenConnections { get => _amountOfOpenConnections; private set => _amountOfOpenConnections = value; }
        public int AmountOfNeededRooms { get => _amountOfNeededRooms; private set => _amountOfNeededRooms = value; }
        public float ChanceOfNextRoom { get => _chanceOfNextRoom; private set => _chanceOfNextRoom = value; }
        public List<ConnectionData> PossibleNextConnectionTypes { get => _possibleNextConnectionTypes; private set => _possibleNextConnectionTypes = value; }

        public override void Create(int x, int y, RoomData roomData)
        {
            AmountOfOpenConnections = 0;

            roomData.Connection.Top = (roomData.Connection.Top == ConnectionType.None) ? ConnectTo(x, y, Side.Top) : roomData.Connection.Top;
            roomData.Connection.Bottom = (roomData.Connection.Bottom == ConnectionType.None) ? ConnectTo(x, y, Side.Bottom) : roomData.Connection.Bottom;
            roomData.Connection.Left = (roomData.Connection.Left == ConnectionType.None) ? ConnectTo(x, y, Side.Left) : roomData.Connection.Left;
            roomData.Connection.Right = (roomData.Connection.Right == ConnectionType.None) ? ConnectTo(x, y, Side.Right) : roomData.Connection.Right;

            if (roomData.Connection.Top.CanCreateNextRoom()) AmountOfOpenConnections++;
            if (roomData.Connection.Bottom.CanCreateNextRoom()) AmountOfOpenConnections++;
            if (roomData.Connection.Left.CanCreateNextRoom()) AmountOfOpenConnections++;
            if (roomData.Connection.Right.CanCreateNextRoom()) AmountOfOpenConnections++;

            CreateNextRooms(x, y, roomData);
        }

        public override void CreateNextRooms(int x, int y, RoomData roomData)
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
                RoomData nextRoom;
                switch (side)
                {
                    case Side.Top:

                        if (roomData.Connection.Top == ConnectionType.None)
                        {
                            if (roomData.ShouldCreateNextRoom) CreateNextRoom(x, y + 1, side, roomData);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x, y + 1);
                            if (nextRoom == null) roomData.Connection.Top = ConnectionType.Wall;
                            else roomData.Connection.Top = nextRoom.Connection.Bottom;
                            if (roomData.Connection.Top.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    case Side.Bottom:

                        if (roomData.Connection.Bottom == ConnectionType.None)
                        {
                            if (roomData.ShouldCreateNextRoom) CreateNextRoom(x, y - 1, side, roomData);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x, y - 1);
                            if (nextRoom == null) roomData.Connection.Bottom = ConnectionType.Wall;
                            else roomData.Connection.Bottom = nextRoom.Connection.Top;
                            if (roomData.Connection.Bottom.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    case Side.Left:

                        if (roomData.Connection.Left == ConnectionType.None)
                        {
                            if (roomData.ShouldCreateNextRoom) CreateNextRoom(x - 1, y, side, roomData);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x - 1, y);
                            if (nextRoom == null) roomData.Connection.Left = ConnectionType.Wall;
                            else roomData.Connection.Left = nextRoom.Connection.Right;
                            if (roomData.Connection.Left.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    case Side.Right:

                        if (roomData.Connection.Right == ConnectionType.None)
                        {
                            if (roomData.ShouldCreateNextRoom) CreateNextRoom(x + 1, y, side, roomData);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x + 1, y);
                            if (nextRoom == null) roomData.Connection.Right = ConnectionType.Wall;
                            else roomData.Connection.Right = nextRoom.Connection.Left;
                            if (roomData.Connection.Right.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        protected override void CreateNextRoom(int x, int y, Side side, RoomData roomData)
        {
            if (AmountOfOpenConnections < AmountOfNeededRooms)
            {
                roomData.Creator.Create(x, y, side);
                RoomData nextRoomData = DungeonManager.Dungeon.GetRoom(x, y);
                if (nextRoomData == null) CreateNextRoom(x, y, side, roomData);
            }
            else
            {
                float chance = UnityEngine.Random.Range(0f, 1f);
                if (chance <= ChanceOfNextRoom)
                {
                    roomData.Creator.Create(x, y, side);
                }
            }
        }

        private ConnectionType ConnectTo(int x, int y, Side side)
        {
            switch (side)
            {
                case Side.Top:
                    return ConnectToTop(x, y);

                case Side.Bottom:
                    return ConnectToBottom(x, y);

                case Side.Left:
                    return ConnectToLeft(x, y);

                case Side.Right:
                    return ConnectToRight(x, y);
                default:
                    break;
            }
            throw new Exception("Invalid side type: " + side);
        }

        private ConnectionType ConnectToRight(int x, int y)
        {
            RoomData rightRoom = DungeonManager.Dungeon.GetRoom(x + 1, y);
            ConnectionType rightConnection;
            if (rightRoom != null)
            {
                rightConnection = rightRoom.Connection.Left;
                if (rightConnection == ConnectionType.None)
                {
                    rightConnection = CreateRandomConnection();
                }
            }
            else
            {
                rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y).Left;
            }
            return rightConnection;
        }

        private ConnectionType ConnectToBottom(int x, int y)
        {
            RoomData bottomRoom = DungeonManager.Dungeon.GetRoom(x, y - 1);
            ConnectionType bottomConnection;
            if (bottomRoom != null)
            {
                bottomConnection = bottomRoom.Connection.Top;
                if (bottomConnection == ConnectionType.None)
                {
                    bottomConnection = CreateRandomConnection();
                }
            }
            else
            {
                bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1).Top;
            }
            return bottomConnection;
        }

        private ConnectionType ConnectToLeft(int x, int y)
        {
            RoomData leftRoom = DungeonManager.Dungeon.GetRoom(x - 1, y);
            ConnectionType leftConnection;
            if (leftRoom != null)
            {
                leftConnection = leftRoom.Connection.Right;
                if (leftConnection == ConnectionType.None)
                {
                    leftConnection = CreateRandomConnection();
                }
            }
            else
            {
                leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y).Right;
            }
            return leftConnection;
        }

        private ConnectionType ConnectToTop(int x, int y)
        {
            RoomData topRoom = DungeonManager.Dungeon.GetRoom(x, y + 1);
            ConnectionType topConnection;
            if (topRoom != null)
            {
                topConnection = topRoom.Connection.Bottom;
                if (topConnection == ConnectionType.None)
                {
                    topConnection = CreateRandomConnection();
                }
            }
            else
            {
                topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1).Bottom;
            }
            return topConnection;
        }

        private ConnectionType CreateRandomConnection()
        {
            if (PossibleNextConnectionTypes.Count > 0)
            {
                List<ConnectionType> nextConnections = new List<ConnectionType>();

                float chance = UnityEngine.Random.Range(0f, 1f);

                foreach (var connection in PossibleNextConnectionTypes)
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
                return CreateRandomConnection();
            }
            else throw new Exception("Possible connection types list is empty");
        }
    }
}
