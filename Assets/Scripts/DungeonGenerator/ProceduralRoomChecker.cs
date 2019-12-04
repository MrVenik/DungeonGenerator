using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomChecker", menuName = "Room Checkers/Procedural Room Checker")]
    public class ProceduralRoomChecker : RoomChecker
    {
        public override bool CanCreate(int x, int y, RoomBehaviour room)
        {
            if (base.CanCreate(x, y, room))
            {
                ProceduralRoomBehaviour proceduralRoom = room as ProceduralRoomBehaviour;

                Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(topConnection.Bottom) && topConnection.Bottom != ConnectionType.None) return false;
                Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(bottomConnection.Top) && bottomConnection.Top != ConnectionType.None) return false;
                Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(leftConnection.Right) && leftConnection.Right != ConnectionType.None) return false;
                Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(rightConnection.Left) && rightConnection.Left != ConnectionType.None) return false;

                return true;
            }
            else return false;
        }
    }
}
