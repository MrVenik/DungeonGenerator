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

        [SerializeField] public GameObject RoomPrefab;
        [SerializeField] public GameObject StartRoomPrefab;
        [SerializeField] public GameObject CorridorPrefab;
        [SerializeField] public GameObject SecretRoomPrefab;

        [SerializeField] public GameObject ColumnPrefab;
        [SerializeField] public GameObject WallPrefab;
        [SerializeField] public GameObject DoorPrefab;
        [SerializeField] public GameObject CorridorWallPrefab;
        [SerializeField] public GameObject CorridorDoorPrefab;
        [SerializeField] public GameObject CorridorOpenPrefab;
        [SerializeField] public GameObject SecretRoomDoorPrefab;
        [SerializeField] public GameObject StartRoomBuildingPrefab;

        [SerializeField] public int RoomSize;
        [SerializeField] public int CorridorSize;

        [SerializeField] private int _heigth;
        [SerializeField] private int _width;

        private Room[,] _rooms;

        public Transform Transform { get; private set; }

        public void BuildDungeon()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigth; y++)
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

            _rooms = new Room[_width, _heigth];
        }

        public void CreateStartRoom(int x, int y)
        {
            RoomPrefabData startRoomData = new RoomPrefabData()
            {
                Name = "StartRoom",
                Prefab = StartRoomPrefab,
                Chance = 1f
            };

            RoomSpawnPoint.Spawn(x, y, startRoomData, Side.Bottom);
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
            return (x >= 0 && x < _width) && (y >= 0 && y < _heigth);
        }
    }
}
