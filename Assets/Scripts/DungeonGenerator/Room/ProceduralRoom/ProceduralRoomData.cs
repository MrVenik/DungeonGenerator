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
    [CreateAssetMenu(fileName = "New Room", menuName = "Rooms/Room/Procedural Room")]
    public class ProceduralRoomData : RoomData
    {
        [Range(0, 4)]
        [SerializeField] private int _amountOfNeededRooms;
        [Range(0, 4)]
        [SerializeField] private int _maximumAmountOfCreatedRooms;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float _chanceOfNextRoom;
        [SerializeField] private List<ConnectionData> _possibleNextConnectionTypes;

        public int AmountOfOpenConnections { get; private set; }
        public int AmountOfNeededRooms { get => _amountOfNeededRooms; private set => _amountOfNeededRooms = value; }
        public float ChanceOfNextRoom { get => _chanceOfNextRoom; private set => _chanceOfNextRoom = value; }
        public List<ConnectionData> PossibleNextConnectionTypes { get => _possibleNextConnectionTypes; private set => _possibleNextConnectionTypes = value; }
        public int MaximumAmountOfCreatedRooms { get => _maximumAmountOfCreatedRooms; private set => _maximumAmountOfCreatedRooms = value; }

        public override void Create(int x, int y)
        {
            Connection.Top = (Connection.Top == ConnectionType.None) ? ConnectTo(x, y, Side.Top) : Connection.Top;
            Connection.Bottom = (Connection.Bottom == ConnectionType.None) ? ConnectTo(x, y, Side.Bottom) : Connection.Bottom;
            Connection.Left = (Connection.Left == ConnectionType.None) ? ConnectTo(x, y, Side.Left) : Connection.Left;
            Connection.Right = (Connection.Right == ConnectionType.None) ? ConnectTo(x, y, Side.Right) : Connection.Right;

            if (Connection.Top.CanCreateNextRoom()) AmountOfOpenConnections++;
            if (Connection.Bottom.CanCreateNextRoom()) AmountOfOpenConnections++;
            if (Connection.Left.CanCreateNextRoom()) AmountOfOpenConnections++;
            if (Connection.Right.CanCreateNextRoom()) AmountOfOpenConnections++;

            CreateNextRooms(x, y);
        }

        private void CreateNextRooms(int x, int y)
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

                        if (Connection.Top == ConnectionType.None)
                        {
                            if (ShouldCreateNextRoom) CreateNextRoom(x, y + 1, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x, y + 1);
                            if (nextRoom == null) Connection.Top = ConnectionType.Wall;
                            else Connection.Top = nextRoom.Connection.Bottom;
                            if (Connection.Top.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    case Side.Bottom:

                        if (Connection.Bottom == ConnectionType.None)
                        {
                            if (ShouldCreateNextRoom) CreateNextRoom(x, y - 1, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x, y - 1);
                            if (nextRoom == null) Connection.Bottom = ConnectionType.Wall;
                            else Connection.Bottom = nextRoom.Connection.Top;
                            if (Connection.Bottom.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    case Side.Left:

                        if (Connection.Left == ConnectionType.None)
                        {
                            if (ShouldCreateNextRoom) CreateNextRoom(x - 1, y, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x - 1, y);
                            if (nextRoom == null) Connection.Left = ConnectionType.Wall;
                            else Connection.Left = nextRoom.Connection.Right;
                            if (Connection.Left.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    case Side.Right:

                        if (Connection.Right == ConnectionType.None)
                        {
                            if (ShouldCreateNextRoom) CreateNextRoom(x + 1, y, side);
                            nextRoom = DungeonManager.Dungeon.GetRoom(x + 1, y);
                            if (nextRoom == null) Connection.Right = ConnectionType.Wall;
                            else Connection.Right = nextRoom.Connection.Left;
                            if (Connection.Right.CanCreateNextRoom()) AmountOfOpenConnections++;
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        protected void CreateNextRoom(int x, int y, Side side)
        {
            if (AmountOfOpenConnections < MaximumAmountOfCreatedRooms)
            {
                if (AmountOfOpenConnections < AmountOfNeededRooms)
                {
                    Creator.Create(x, y, side);
                    RoomData nextRoomData = DungeonManager.Dungeon.GetRoom(x, y);
                    if (nextRoomData == null) CreateNextRoom(x, y, side);
                }
                else
                {
                    float chance = UnityEngine.Random.Range(0f, 1f);
                    if (chance <= ChanceOfNextRoom)
                    {
                        Creator.Create(x, y, side);
                    }
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
