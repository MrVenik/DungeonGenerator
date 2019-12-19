using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGenerator
{

    public class Dungeon : MonoBehaviour
    {
        [SerializeField] public RoomData StartRoom;

        [SerializeField] public RoomSize MaximumRoomSize;

        [SerializeField] private int _heigth = 0;
        [SerializeField] private int _width = 0;

        public int Heigth { get => _heigth; set => _heigth = value; }
        public int Width { get => _width; set => _width = value; }

        public TilemapData TilemapData { get; set; }

        private RoomData[,] _rooms;
        public Transform Transform { get; private set; }

        [SerializeField] private float _dungeonFilling;
        [SerializeField] private int _maximumAmountOfRooms;
        [SerializeField] private int _predicatedAmountOfRooms;
        [SerializeField] private int _currentAmountOfRooms;
        [Range(0.2f, 1.0f)]
        [SerializeField] private float _maximumDeviation;

        [SerializeField] private bool _builded;

        public int PredicatedAmountOfRooms
        {
            get => _predicatedAmountOfRooms;
        }
        public int AmountOfRooms
        {
            get => _currentAmountOfRooms;
        }
        public float MaximumDeviation
        {
            get => _maximumDeviation;
        }

        public float PlugChance
        {
            get
            {
                if (_currentAmountOfRooms < _predicatedAmountOfRooms)
                {
                    return (_currentAmountOfRooms / _predicatedAmountOfRooms);
                }
                else return 1.0f;
            }
        }

        public float FillingChance
        {
            get
            {
                if (_currentAmountOfRooms > _predicatedAmountOfRooms)
                {
                    return ((_currentAmountOfRooms - _predicatedAmountOfRooms) / _predicatedAmountOfRooms);
                }
                else return 1.0f;
            }
        }

        public void BuildDungeon()
        {
            if (!_builded)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Heigth; y++)
                    {
                        if (_rooms[x, y] != null)
                        {
                            Vector3Int roomPosition = new Vector3Int(x * (int)DungeonManager.Dungeon.MaximumRoomSize, y * (int)DungeonManager.Dungeon.MaximumRoomSize, 0);
                            _rooms[x, y].Build(_rooms[x, y], roomPosition, TilemapData);
                            //DestroyImmediate(_rooms[x, y]);
                        }
                    }
                }
                _builded = true;
            }

        }

        public void Awake()
        {
            _builded = false;
            Create();
        }

        public void Create()
        {
            Transform = transform;

            _maximumAmountOfRooms = Heigth * Width;
            _predicatedAmountOfRooms = (int)(_maximumAmountOfRooms * _dungeonFilling);

            _rooms = new RoomData[Width, Heigth];
        }

        public void CreateStartRoom(int x, int y, Side side)
        {
            RoomData startRoom = Instantiate(StartRoom);
            startRoom.Rotate(side.Oposite());
            SetRoom(x, y, startRoom);
            startRoom.Create(x, y);
        }

        public RoomData GetRoom(int x, int y)
        {
            if (CheckBorders(x, y))
            {
                return _rooms[x, y];
            }
            else return null;
        }

        public Connection GetRoomConnection(int x, int y)
        {
            if (CheckBorders(x, y))
            {
                if (_rooms[x, y] != null)
                {
                    return _rooms[x, y].Connection;
                }
                else return Connection.None;
            }
            else return Connection.Border;
        }

        public void ReserveRoom(int x, int y, Side side, CreatableData data)
        {
            CreatableData roomData = Instantiate(data);
            roomData.Rotate(side.Oposite());
            if (roomData.CanCreate(x, y))
            {
                if (roomData is RoomGroupData)
                {
                    RoomGroupData roomGroup = roomData as RoomGroupData;
                    for (int ix = x - roomGroup.EntranceX, j = 0; ix < x + roomGroup.ArraySize - roomGroup.EntranceX; ix++, j++)
                    {
                        for (int iy = y - roomGroup.EntranceY, k = 0; iy < y + roomGroup.ArraySize - roomGroup.EntranceY; iy++, k++)
                        {
                            GroupElementData groupElement = roomGroup.Elements[j + k * roomGroup.ArraySize];
                            if (groupElement != null && groupElement.RoomData != null)
                            {
                                RoomData room = groupElement.RoomData;
                                DungeonManager.Dungeon.SetRoom(ix, iy, room);
                            }
                        }
                    }
                }
                else
                {
                    DungeonManager.Dungeon.SetRoom(x, y, roomData as RoomData);
                }
                //roomData.Create(x, y);
            }
            else DestroyImmediate(roomData);
        }

        [SerializeField] private RoomData _pathRoom;
        public void BuildPath(int fromX, int fromY, Side fromSide, int toX, int toY, Side toSide)
        {
            int currentX = fromX + fromSide.X();
            int currentY = fromY + fromSide.Y();
            int endX = toX + toSide.X();
            int endY = toY + toSide.Y();

            if (fromX == toX && fromY == toY)
            {
                return;
            }

            ReserveRoom(currentX, currentY, fromSide, _pathRoom);

            if (currentX == endX && currentY == endY)
            {
                return;
            }

            switch (fromSide)
            {
                case Side.Top:
                    if (currentY + fromSide.Y() > endY)
                    {
                        bool clockwise = ((currentX - endX) < 0);
                        fromSide = fromSide.Rotate(clockwise);
                    }
                    else if ((currentX - endX) != 0)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            bool clockwise = ((currentX - endX) < 0);
                            fromSide = fromSide.Rotate(clockwise);
                        }
                    }
                    break;

                case Side.Bottom:
                    if (currentY + fromSide.Y() < endY)
                    {
                        bool clockwise = ((currentX - endX) > 0);
                        fromSide = fromSide.Rotate(clockwise);
                        Debug.Log("New Side - " + fromSide);
                    }
                    else if ((currentX - endX) != 0)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            bool clockwise = ((currentX - endX) > 0);
                            fromSide = fromSide.Rotate(clockwise);
                        }
                    }

                    break;
                case Side.Left:
                    if (currentX + fromSide.X() < endX)
                    {
                        bool clockwise = (currentY - endY) < 0;
                        fromSide = fromSide.Rotate(clockwise);
                    }
                    else if ((currentY - endY) != 0)
                    {
                        bool clockwise = ((currentY - endY) < 0);
                        fromSide = fromSide.Rotate(clockwise);
                    }
                    break;
                case Side.Right:
                    if (currentX + fromSide.X() > endX)
                    {
                        bool clockwise = (currentY - endY) > 0;
                        fromSide = fromSide.Rotate(clockwise);
                    }
                    else if ((currentY - endY) != 0)
                    {
                        bool clockwise = ((currentY - endY) > 0);
                        fromSide = fromSide.Rotate(clockwise);
                    }
                    break;
                default:
                    break;
            }
            BuildPath(currentX, currentY, fromSide, toX, toY, toSide);

        }

        public void SetRoom(int x, int y, RoomData roomData)
        {
            if (CheckBorders(x, y))
            {
                if (_rooms[x, y] == null)
                {
                    _rooms[x, y] = roomData;
                    _currentAmountOfRooms++;
                    _rooms[x, y].ID = _currentAmountOfRooms;
                }
            }
        }

        private bool CheckBorders(int x, int y)
        {
            return (x >= 0 && x < Width) && (y >= 0 && y < Heigth);
        }
    }
}
