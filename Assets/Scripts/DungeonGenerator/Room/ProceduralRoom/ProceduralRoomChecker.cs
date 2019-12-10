using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomChecker", menuName = "Rooms/Room Checkers/Procedural Room Checker")]
    public class ProceduralRoomChecker : RoomChecker
    {
        [Range(0, 4)]
        [SerializeField] private int _minAmountOfOpenConnections;
        [Range(0, 4)]
        [SerializeField] private int _maxAmountOfOpenConnections;

        [SerializeField] private List<ConnectionType> _possibleConnectionTypes;

        public List<ConnectionType> PossibleConnectionTypes { get => _possibleConnectionTypes; private set => _possibleConnectionTypes = value; }

        public override bool CanCreate(int x, int y, RoomData room)
        {
            if (base.CanCreate(x, y, room))
            {
                int connections = 0;

                Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
                if (!PossibleConnectionTypes.Contains(topConnection.Bottom) && topConnection.Bottom != ConnectionType.None && topConnection.Bottom != ConnectionType.Wall) return false;
                if (topConnection.Bottom != ConnectionType.Wall && topConnection.Bottom != ConnectionType.Border) connections++;

                Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
                if (!PossibleConnectionTypes.Contains(bottomConnection.Top) && bottomConnection.Top != ConnectionType.None && bottomConnection.Top != ConnectionType.Wall) return false;
                if (bottomConnection.Top != ConnectionType.Wall && bottomConnection.Top != ConnectionType.Border) connections++;

                Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
                if (!PossibleConnectionTypes.Contains(leftConnection.Right) && leftConnection.Right != ConnectionType.None && leftConnection.Right != ConnectionType.Wall) return false;
                if (leftConnection.Right != ConnectionType.Wall && leftConnection.Right != ConnectionType.Border) connections++;

                Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
                if (!PossibleConnectionTypes.Contains(rightConnection.Left) && rightConnection.Left != ConnectionType.None && rightConnection.Left != ConnectionType.Wall) return false;
                if (rightConnection.Left != ConnectionType.Wall && rightConnection.Left != ConnectionType.Border) connections++;

                bool canCreate = (connections >= _minAmountOfOpenConnections && connections <= _maxAmountOfOpenConnections);

                return canCreate;
            }
            else return false;
        }
    }
}
