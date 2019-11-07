using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class SquareRoom : TemplateRoom
    {
        public override bool CanCreate(int x, int y)
        {
            if (DungeonManager.Dungeon.GetRoom(x, y + 1) is SquareRoom) return false;
            if (DungeonManager.Dungeon.GetRoom(x, y - 1) is SquareRoom) return false;
            if (DungeonManager.Dungeon.GetRoom(x - 1, y) is SquareRoom) return false;
            if (DungeonManager.Dungeon.GetRoom(x + 1, y) is SquareRoom) return false;
            return base.CanCreate(x, y);
        }
    }
}
