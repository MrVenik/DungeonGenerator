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
        [Range(0.0f, 1.0f)]
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
        [SerializeField] private bool _shouldConnectToOtherProceduralRooms;

        public int AmountOfOpenConnections { get; private set; }
        public int AmountOfNeededRooms { get => _amountOfNeededRooms; private set => _amountOfNeededRooms = value; }
        public float ChanceOfNextRoom { get => _chanceOfNextRoom; private set => _chanceOfNextRoom = value; }
        public List<ConnectionData> PossibleNextConnectionTypes { get => _possibleNextConnectionTypes; private set => _possibleNextConnectionTypes = value; }
        public int MaximumAmountOfCreatedRooms { get => _maximumAmountOfCreatedRooms; private set => _maximumAmountOfCreatedRooms = value; }
        public bool ShouldConnectToOtherProceduralRooms { get => _shouldConnectToOtherProceduralRooms; private set => _shouldConnectToOtherProceduralRooms = value; }

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
                if (Connection.GetConnectionTypeBySide(side) == ConnectionType.None)
                {
                    if (ShouldCreateNextRoom) CreateNextRoom(x + side.X(), y + side.Y(), side);
                    nextRoom = DungeonManager.Dungeon.GetRoom(x + side.X(), y + side.Y());
                    if (nextRoom == null) Connection.SetConnectionTypeBySide(ConnectionType.Wall, side);
                    else Connection.SetConnectionTypeBySide(nextRoom.Connection.GetConnectionTypeBySide(side.Oposite()), side);
                    if (Connection.GetConnectionTypeBySide(side).CanCreateNextRoom()) AmountOfOpenConnections++;
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
            RoomData neighbourRoom = DungeonManager.Dungeon.GetRoom(x + side.X(), y + side.Y());
            ConnectionType neighbourConnectionType;
            if (neighbourRoom != null)
            {
                neighbourConnectionType = neighbourRoom.Connection.GetConnectionTypeBySide(side.Oposite());
                if (neighbourConnectionType == ConnectionType.None)
                {
                    if ((ShouldConnectToOtherProceduralRooms && (neighbourRoom as ProceduralRoomData).ShouldConnectToOtherProceduralRooms) || Entrance == side)
                    {
                        neighbourConnectionType = CreateRandomConnection();
                    }
                    else neighbourConnectionType = ConnectionType.Wall;
                }
            }
            else
            {
                neighbourConnectionType = DungeonManager.Dungeon.GetRoomConnection(x + side.X(), y + side.Y()).GetConnectionTypeBySide(side.Oposite());
            }
            return neighbourConnectionType;
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
