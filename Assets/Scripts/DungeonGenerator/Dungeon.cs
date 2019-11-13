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

        private Room[,] _rooms;

        public Transform Transform { get; private set; }

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

            _rooms = new Room[Width, Heigth];
        }

        public void CreateStartRoom(int x, int y)
        {
            RoomPrefabData startRoomData = new RoomPrefabData()
            {
                Name = "StartRoom",
                Prefab = StartRoomPrefab,
                Chance = 1f
            };

            RoomCreator.Create(x, y, Side.Top, startRoomData);
        }

        public Room GetRoom(int x, int y)
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

        public void SetRoom(Room room, int x, int y)
        {
            if (CheckBorders(x, y))
            {
                if (_rooms[x, y] == null)
                {
                    _rooms[x, y] = room;
                    _rooms[x, y].Create(x, y);
                }
            }
        }

        private bool CheckBorders(int x, int y)
        {
            return (x >= 0 && x < Width) && (y >= 0 && y < Heigth);
        }
    }
}
