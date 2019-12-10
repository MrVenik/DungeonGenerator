using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomCreator", menuName = "Room Creator")]
    public class RoomCreator : ScriptableObject
    {
        [SerializeField] private List<RoomData> _possibleNextRooms;

        private float _totalWeight = 0;
        private float TotalWeight
        {
            get
            {
                if (_totalWeight == 0)
                {
                    foreach (var item in _possibleNextRooms) _totalWeight += item.Chance;
                }
                return _totalWeight;
            }
        }


        public void Create(int x, int y, Side side)
        {
            if (DungeonManager.Dungeon.GetRoom(x, y) == null)
            {
                if (_possibleNextRooms != null)
                {
                    if (_possibleNextRooms.Count > 0)
                    {
                        RoomData nextRoomData = (_possibleNextRooms.Count == 1) ? _possibleNextRooms[0] : GetRandomRoom();

                        if (nextRoomData != null)
                        {
                            nextRoomData.Rotate(side.Oposite());
                            //if (nextRoomData.CanCreate(x, y)) DungeonManager.Dungeon.CreateRoom(x, y, nextRoom);  
                        }
                    }
                    else throw new Exception("Possible Next Rooms list is empty");
                }
                else throw new Exception("Possible Next Rooms list is null");
            }
        }

        private RoomData GetRandomRoom()
        {
            RoomData nextRoom = null;

            float chance = UnityEngine.Random.Range(0f, TotalWeight);

            foreach (var room in _possibleNextRooms)
            {
                float chanceMultiplier = room.ShouldCreateNextRoom ? DungeonManager.Dungeon.PlugChance : DungeonManager.Dungeon.FillingChance;

                if (chance > 0 && chance <= (room.Chance * chanceMultiplier))
                {
                    nextRoom = room;
                    break;
                }
                chance -= room.Chance;
            }
            return nextRoom;
        }
    }
}
