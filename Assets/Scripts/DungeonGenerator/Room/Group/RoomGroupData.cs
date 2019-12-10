using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [Serializable]
    public class GroupElementData
    {
        public RoomData RoomData;
        public Side Entrance;
    }

    [CreateAssetMenu(fileName = "New RoomGroup", menuName = "Rooms/RoomGroup")]
    public class RoomGroupData : CreatableData
    {
        [SerializeField] private string _name;
        [SerializeField] private Side _entrance;

        [SerializeField] private int _entranceX;
        [SerializeField] private int _entranceY;

        [SerializeField] private bool _isPlug;

        [SerializeField] private int _arraySize;

        [SerializeField] private GroupElementData[] _defaultElements;
        private GroupElementData[] _elements = null;

        public GroupElementData[] Elements 
        { 
            get
            {
                if (_elements == null)
                {
                    _elements = new GroupElementData[ArraySize * ArraySize];
                    for (int x = 0; x < ArraySize; x++)
                    {
                        for (int y = 0; y < ArraySize; y++)
                        {
                            _elements[x + y * ArraySize] = new GroupElementData
                            {
                                Entrance = _defaultElements[x + y * ArraySize].Entrance,
                                RoomData = Instantiate(_defaultElements[x + y * ArraySize].RoomData)
                            };
                        }
                    }
                }
                return _elements;
            }
            private set => _elements = value; 
        }

        public int EntranceX { get => _entranceX; private set => _entranceX = value; }
        public int EntranceY { get => _entranceY; private set => _entranceY = value; }
        public string Name { get => _name; private set => _name = value; }
        public Side Entrance { get => _entrance; private set => _entrance = value; }
        public int ArraySize { get => _arraySize; private set => _arraySize = value; }
        public override bool IsPlug { get => _isPlug; }

        public void OnDestroy()
        {
            foreach (var item in _elements)
            {
                Destroy(item.RoomData);
            }
        }

        public override void Create(int x, int y)
        {
            for (int ix = x - EntranceX, j = 0; ix < x + ArraySize - EntranceX; ix++, j++)
            {
                for (int iy = y - EntranceY, k = 0; iy < y + ArraySize - EntranceY; iy++, k++)
                {
                    if (Elements[j + k * ArraySize].RoomData != null)
                    {
                        RoomData room = Elements[j + k * ArraySize].RoomData;
                        room.Create(ix, iy);
                    }
                }
            }
        }

        public override bool CanCreate(int x, int y)
        {
            for (int ix = x - EntranceX, j = 0; ix < x + ArraySize - EntranceX; ix++, j++)
            {
                for (int iy = y - EntranceY, k = 0; iy < y + ArraySize - EntranceY; iy++, k++)
                {
                    if (Elements[j + k * ArraySize].RoomData != null)
                    {
                        RoomData room = Elements[j + k * ArraySize].RoomData;
                        if (!room.CanCreate(ix, iy)) return false;
                    }
                }
            }
            return true;
        }


        public override void Rotate(Side side)
        {
            switch (Entrance)
            {
                case Side.Top:
                    switch (side)
                    {
                        case Side.Top:
                            break;
                        case Side.Bottom:
                            Rotate(2);
                            break;
                        case Side.Left:
                            Rotate(1, false);
                            break;
                        case Side.Right:
                            Rotate(1, true);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Bottom:
                    switch (side)
                    {
                        case Side.Top:
                            Rotate(2);
                            break;
                        case Side.Bottom:
                            break;
                        case Side.Left:
                            Rotate(1, true);
                            break;
                        case Side.Right:
                            Rotate(1, false);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Left:
                    switch (side)
                    {
                        case Side.Top:
                            Rotate(1, true);
                            break;
                        case Side.Bottom:
                            Rotate(1, false);
                            break;
                        case Side.Left:
                            break;
                        case Side.Right:
                            Rotate(2);
                            break;
                        default:
                            break;
                    }
                    break;
                case Side.Right:
                    switch (side)
                    {
                        case Side.Top:
                            Rotate(1, false);
                            break;
                        case Side.Bottom:
                            Rotate(1, true);
                            break;
                        case Side.Left:
                            Rotate(2);
                            break;
                        case Side.Right:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            RotateRooms();
        }

        public void Rotate(int steps, bool clockwise = false)
        {
            for (int i = 0; i < steps; i++)
            {
                Rotate(clockwise);
            }
        }

        public void Rotate(bool clockwise = false)
        {
            RotateEntranceCoords(clockwise);

            Elements = Elements.Rotate(clockwise);

            Entrance = Entrance.Rotate(clockwise);

            foreach (var room in Elements)
            {
                room.Entrance = room.Entrance.Rotate(clockwise);
            }

        }

        private void RotateRooms()
        {
            foreach (var room in Elements)
            {
                room.RoomData.Rotate(room.Entrance);
            }
        }

        private void RotateEntranceCoords(bool clockwise = false)
        {
            int tempX = EntranceX;
            EntranceX = clockwise ? EntranceY : ArraySize - 1 - EntranceY;
            EntranceY = clockwise ? ArraySize - 1 - tempX : tempX;
        }
    }
}
