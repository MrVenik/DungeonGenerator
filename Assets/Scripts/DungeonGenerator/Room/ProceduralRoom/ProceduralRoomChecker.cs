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

        private int _currentAmountOfOpenConnections;

        public override bool CanCreate(int x, int y, RoomData room)
        {
            if (base.CanCreate(x, y, room))
            {
                _currentAmountOfOpenConnections = 0;

                ProceduralRoomData proceduralRoom = room as ProceduralRoomData;

                if (!CheckNeighbourRoom(x, y, Side.Top, proceduralRoom)) return false;
                if (!CheckNeighbourRoom(x, y, Side.Bottom, proceduralRoom)) return false;
                if (!CheckNeighbourRoom(x, y, Side.Left, proceduralRoom)) return false;
                if (!CheckNeighbourRoom(x, y, Side.Right, proceduralRoom)) return false;

                bool canCreate = (_currentAmountOfOpenConnections >= _minAmountOfOpenConnections && _currentAmountOfOpenConnections <= _maxAmountOfOpenConnections);

                _currentAmountOfOpenConnections = 0;

                return canCreate;
            }
            else return false;
        }

        private bool CheckNeighbourRoom(int x, int y, Side side, ProceduralRoomData roomData)
        {
            RoomData neighbourRoom = DungeonManager.Dungeon.GetRoom(x + side.X(), y + side.Y());
            ConnectionType neighbourConnectionType;
            if (neighbourRoom != null)
            {
                neighbourConnectionType = neighbourRoom.Connection.GetConnectionTypeBySide(side.Oposite());
                if (neighbourConnectionType == ConnectionType.None)
                {
                    if ((roomData.ShouldConnectToProceduralRooms && (neighbourRoom as ProceduralRoomData).ShouldConnectToProceduralRooms) || roomData.Entrance == side)
                    {
                        if (!CanConnect(roomData, neighbourRoom as ProceduralRoomData))
                        {
                            return false;
                        }
                        _currentAmountOfOpenConnections++;
                    }
                }
                else
                {
                    if (!roomData.ShouldConnectToTemplateRooms) return false;
                    if (!roomData.PossibleNextConnectionTypes.Exists(t => t.ConnectionType == neighbourConnectionType) && neighbourConnectionType != ConnectionType.Wall) return false;
                    if (neighbourConnectionType != ConnectionType.Wall && neighbourConnectionType != ConnectionType.Border) _currentAmountOfOpenConnections++;
                }
            }
            else
            {
                neighbourConnectionType = DungeonManager.Dungeon.GetRoomConnection(x + side.X(), y + side.Y()).GetConnectionTypeBySide(side.Oposite());
                if (neighbourConnectionType != ConnectionType.None && neighbourConnectionType != ConnectionType.Wall) return false;
                _currentAmountOfOpenConnections++;

            }
            return true;
        }

        private bool CanConnect(ProceduralRoomData room, ProceduralRoomData neighbourRoom)
        {
            foreach (var connectionData in room.PossibleNextConnectionTypes)
            {
                if (connectionData.Chance > 0 && neighbourRoom.PossibleNextConnectionTypes.Exists(t => t.ConnectionType == connectionData.ConnectionType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
