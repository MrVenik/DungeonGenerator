using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    public class ExampleRoom : TemplateRoom
    {
        public override bool CanCreate(int x, int y)
        {
            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (topConnection.Bottom != Connection.Top && topConnection.Bottom != ConnectionType.None) return false;
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (bottomConnection.Top != Connection.Bottom && bottomConnection.Top != ConnectionType.None) return false;
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (leftConnection.Right != Connection.Left && leftConnection.Right != ConnectionType.None) return false;
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (rightConnection.Left != Connection.Right && rightConnection.Left != ConnectionType.None) return false;
            return true;
        }
    }
}
