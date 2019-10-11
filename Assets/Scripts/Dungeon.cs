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

        [SerializeField] public int RoomSize;
        [SerializeField] public int CorridorSize;

        [SerializeField] private int _heigth;
        [SerializeField] private int _width;

        private IRoom[,] _rooms;

        public Transform Transform { get; private set; }

        public void BuildDungeon()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigth; y++)
                {
                    _rooms[x, y].Build();
                }
            }
        }

        public void Awake()
        {
            Transform = transform;

            _rooms = new IRoom[_width, _heigth];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigth; y++)
                {
                    _rooms[x, y] = new EmptyRoom();
                }
            }
        }

        public void CreateStartRoom(int x, int y)
        {
            IRoom startRoom = Instantiate(StartRoomPrefab, Transform.position, Transform.rotation, Transform).GetComponent<StartRoom>();
            SetRoom(startRoom, x, y);
        }

        internal IRoom GetRoom(int x, int y)
        {
            if (CheckBorders(x, y))
            {
                return _rooms[x, y];
            }
            else return new BorderRoom();
        }

        internal void SetRoom(IRoom room, int x, int y)
        {
            if (CheckBorders(x, y))
            {
                if (_rooms[x, y] is EmptyRoom)
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
