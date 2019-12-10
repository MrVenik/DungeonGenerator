using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public abstract class RoomChecker : ScriptableObject
    {
        [SerializeField] private int _radius;
        public virtual bool CanCreate(int x, int y, RoomData room)
        {
            if (DungeonManager.Dungeon.GetRoom(x, y) != null) return false;
            if (room.Size > DungeonManager.Dungeon.MaximumRoomSize) return false;

            if (_radius > 0)
            {
                for (int ix = x - _radius; ix <= x + _radius; ix++)
                {
                    for (int iy = y - _radius; iy <= y + _radius; iy++)
                    {
                        RoomData nextRoom = DungeonManager.Dungeon.GetRoom(ix, iy);
                        if (nextRoom != null && nextRoom.Name == room.Name) return false;
                    }
                }
            }

            return true;
        }
    }
}
