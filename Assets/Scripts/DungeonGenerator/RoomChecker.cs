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
        public virtual bool CanCreate(int x, int y, RoomBehaviour room)
        {
            if (DungeonManager.Dungeon.GetRoom(x, y) != null) return false;
            if (room.Size > DungeonManager.Dungeon.MaximumRoomSize) return false;
            return true;
        }
    }
}
