using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomChecker", menuName = "Room Checkers/Square Room Checker")]
    public class SquareRoomChecker : TemplateRoomChecker
    {
        public override bool CanCreate(int x, int y, RoomBehaviour room)
        {
            if (base.CanCreate(x, y, room))
            {
                if (DungeonManager.Dungeon.GetRoom(x, y + 1) is SquareRoom) return false;
                if (DungeonManager.Dungeon.GetRoom(x, y - 1) is SquareRoom) return false;
                if (DungeonManager.Dungeon.GetRoom(x - 1, y) is SquareRoom) return false;
                if (DungeonManager.Dungeon.GetRoom(x + 1, y) is SquareRoom) return false;

                if (DungeonManager.Dungeon.GetRoom(x - 1, y + 1) is SquareRoom) return false;
                if (DungeonManager.Dungeon.GetRoom(x - 1, y - 1) is SquareRoom) return false;
                if (DungeonManager.Dungeon.GetRoom(x + 1, y + 1) is SquareRoom) return false;
                if (DungeonManager.Dungeon.GetRoom(x + 1, y - 1) is SquareRoom) return false;

                return true;
            }
            else return false;
        }
    }
}
