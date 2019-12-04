using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomChecker", menuName = "Room Checkers/Template Room Checker")]
    public class TemplateRoomChecker : RoomChecker
    {
        public override bool CanCreate(int x, int y, RoomBehaviour room)
        {
            if (base.CanCreate(x, y, room))
            {
                Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
                if (topConnection.Bottom != room.Connection.Top && topConnection.Bottom != ConnectionType.None) return false;
                Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
                if (bottomConnection.Top != room.Connection.Bottom && bottomConnection.Top != ConnectionType.None) return false;
                Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
                if (leftConnection.Right != room.Connection.Left && leftConnection.Right != ConnectionType.None) return false;
                Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
                if (rightConnection.Left != room.Connection.Right && rightConnection.Left != ConnectionType.None) return false;
                return true;
            }
            else return false;
        }
    }
}
