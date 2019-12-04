using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class Corridor : ProceduralRoomBehaviour
    {
        protected override float ChanceOfNextRoom 
        { 
            get
            {
                if (AmountOfOpenConnections < 2) return 1.0f;
                return base.ChanceOfNextRoom;
            }
        }

        protected override void CreateNextRoom(int x, int y, Side side)
        {
            if (AmountOfOpenConnections < 4)
            {
                base.CreateNextRoom(x, y, side);

                RoomBehaviour nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

                if (nextRoom == null)
                {
                    CreateNextRoom(x, y, side);
                }
            }
        }
    }
}
