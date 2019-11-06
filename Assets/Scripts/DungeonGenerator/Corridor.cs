using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class Corridor : ProceduralRoom
    {
        public override bool CanCreate(int x, int y)
        {
            int connections = 0;

            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (topConnection.Bottom != ConnectionType.Wall && topConnection.Bottom != ConnectionType.Border) connections++;
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (bottomConnection.Top != ConnectionType.Wall && bottomConnection.Top != ConnectionType.Border) connections++;
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (leftConnection.Right != ConnectionType.Wall && leftConnection.Right != ConnectionType.Border) connections++;
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (rightConnection.Left != ConnectionType.Wall && rightConnection.Left != ConnectionType.Border) connections++;

            return connections >= 2 && connections < 4;
        }

        protected override void CreateNextRoom(int x, int y, Side side)
        {
            if (AmountOfOpenConnections < 4)
            {
                base.CreateNextRoom(x, y, side);

                Room nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

                if (AmountOfOpenConnections < 2 && nextRoom == null)
                {
                    CreateNextRoom(x, y, side);
                }
            }
        }
    }
}
