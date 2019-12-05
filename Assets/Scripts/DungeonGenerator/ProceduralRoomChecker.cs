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
        [Range(0, 4)]
        [SerializeField] private int _minAmountOfOpenConnections;
        [Range(0, 4)]
        [SerializeField] private int _maxAmountOfOpenConnections;

        public override bool CanCreate(int x, int y, RoomBehaviour room)
        {
            if (base.CanCreate(x, y, room))
            {
                ProceduralRoomBehaviour proceduralRoom = room as ProceduralRoomBehaviour;
                int connections = 0;


                Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(topConnection.Bottom) && topConnection.Bottom != ConnectionType.None) return false;
                if (topConnection.Bottom != ConnectionType.Wall && topConnection.Bottom != ConnectionType.Border) connections++;

                Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(bottomConnection.Top) && bottomConnection.Top != ConnectionType.None) return false;
                if (bottomConnection.Top != ConnectionType.Wall && bottomConnection.Top != ConnectionType.Border) connections++;

                Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(leftConnection.Right) && leftConnection.Right != ConnectionType.None) return false;
                if (leftConnection.Right != ConnectionType.Wall && leftConnection.Right != ConnectionType.Border) connections++;

                Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
                if (!proceduralRoom.PossibleConnectionTypes.Contains(rightConnection.Left) && rightConnection.Left != ConnectionType.None) return false;
                if (rightConnection.Left != ConnectionType.Wall && rightConnection.Left != ConnectionType.Border) connections++;

                bool canCreate = (connections >= _minAmountOfOpenConnections && connections <= _maxAmountOfOpenConnections);

                return canCreate;
            }
            else return false;
        }
    }
}
