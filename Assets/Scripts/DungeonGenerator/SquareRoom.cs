using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class SquareRoom : TemplateRoom
    {
        public override bool CanCreate(int x, int y)
        {
            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (topConnection.Bottom != ConnectionType.Door && topConnection.Bottom != ConnectionType.None) return false;
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (bottomConnection.Top != ConnectionType.Door && bottomConnection.Top != ConnectionType.None) return false;
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (leftConnection.Right != ConnectionType.Door && leftConnection.Right != ConnectionType.None) return false;
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (rightConnection.Left != ConnectionType.Door && rightConnection.Left != ConnectionType.None) return false;
            return true;
        }
    }
}
