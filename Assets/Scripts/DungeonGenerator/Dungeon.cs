using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{

    public class Dungeon : MonoBehaviour
    {
        [SerializeField] public List<RoomPrefabData> AllRooms;

        [SerializeField] public GameObject StartRoomPrefab;

        [SerializeField] public GameObject ColumnPrefab;

        [SerializeField] public RoomSize MaximumRoomSize;

        [SerializeField] private int _heigth = 0;
        [SerializeField] private int _width = 0;

        public int Heigth { get => _heigth; set => _heigth = value; }
        public int Width { get => _width; set => _width = value; }

        private RoomBehaviour[,] _rooms;

        public Transform Transform { get; private set; }

        [SerializeField] private float _dungeonFilling;
        [SerializeField] private int _maximumAmountOfRooms;
        [SerializeField] private int _predicatedAmountOfRooms;
        [SerializeField] private int _currentAmountOfRooms;

        public int PredicatedAmountOfRooms
        {
            get => _predicatedAmountOfRooms;
        }
        public int AmountOfRooms
        {
            get => _currentAmountOfRooms;
        }

        //public float PlugChance => _currentAmountOfRooms < (_predicatedAmountOfRooms / 4) ? 0.0f : 1.0f;
        //public float FillingChance => _currentAmountOfRooms > (_predicatedAmountOfRooms / 4) ? 0.0f : 1.0f;

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
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Heigth; y++)
                {
                    if (_rooms[x, y] != null)
                    {
                        _rooms[x, y].Build();
                    }
                }
            }
        }

        public void Awake()
        {
            Transform = transform;

            _maximumAmountOfRooms = Heigth * Width;
            _predicatedAmountOfRooms = (int)(_maximumAmountOfRooms * _dungeonFilling);

            _rooms = new RoomBehaviour[Width, Heigth];
        }

        public void CreateStartRoom(int x, int y, Side side)
        {
            RoomPrefabData startRoomData = new RoomPrefabData()
            {
                Name = "StartRoom",
                Prefab = StartRoomPrefab,
                Chance = 1f
            };

            RoomCreator.Create(x, y, side, startRoomData);
        }

        public RoomBehaviour GetRoom(int x, int y)
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

        public void SetRoom(RoomBehaviour room, int x, int y)
        {
            if (CheckBorders(x, y))
            {
                if (_rooms[x, y] == null)
                {
                    _rooms[x, y] = room;
                    //_rooms[x, y].Create(x, y);
                    _currentAmountOfRooms++;
                }
            }
        }

        private bool CheckBorders(int x, int y)
        {
            return (x >= 0 && x < Width) && (y >= 0 && y < Heigth);
        }
    }
}
