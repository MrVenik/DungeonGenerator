using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [Serializable]
    public class RoomGroupData
    {
        public string Name;
        public Side Entrance;
        public Connection Connection;
        public GameObject Prefab;
        private RoomBehaviour _room = null;
        public RoomBehaviour Room
        {
            get
            {
                if (_room == null) _room = Prefab.GetComponent<RoomBehaviour>();
                return _room;
            }
        }
    }

    public class RoomGroup : MonoBehaviour
    {
        public int ArraySize;
        public int EntranceX;
        public int EntranceY;
        public Side Entrance;
        public RoomGroupData[] Rooms;

        public bool CanCreate(int x, int y)
        {
            for (int ix = x - EntranceX, j = 0; ix < x + ArraySize - EntranceX; ix++, j++)
            {
                for (int iy = y - EntranceY, k = 0; iy < y + ArraySize - EntranceY; iy++, k++)
                {
                    if (DungeonManager.Dungeon.GetRoom(ix, iy) != null) return false;
                    if (DungeonManager.Dungeon.GetRoomConnection(ix, iy) != Connection.None) return false;
                    Rooms[j + k * ArraySize].Room.Entrance = Rooms[j + k * ArraySize].Entrance;
                    Rooms[j + k * ArraySize].Room.Connection = Rooms[j + k * ArraySize].Connection;
                    if (!Rooms[j + k * ArraySize].Room.CanCreate(ix, iy)) return false;
                }
            }
            return true;
        }

        public void Rotate(Side side)
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

            Rooms = Rooms.Rotate(clockwise);

            Entrance = Entrance.Rotate(clockwise);
            foreach (var room in Rooms)
            {
                room.Entrance = room.Entrance.Rotate(clockwise);
            }

        }

        public void RotateRooms()
        {
            foreach (var room in Rooms)
            {
                //room.Room.Rotate(Entrance);
                room.Room.Rotate(room.Entrance);
                room.Connection = room.Room.Connection;
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
