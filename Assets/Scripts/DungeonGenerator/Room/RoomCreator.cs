using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [Serializable]
    public class RandomRoomData
    {
        public RoomData RoomData;
        [Range(0.0f, 1.0f)]
        public float Chance;
    }

    [CreateAssetMenu(fileName = "New RoomCreator", menuName = "Room Creator")]
    public class RoomCreator : ScriptableObject
    {
        [SerializeField] private List<RandomRoomData> _possibleNextRooms;

        private float _totalWeight = 0;

        public List<RandomRoomData> PossibleNextRooms { get => _possibleNextRooms; private set => _possibleNextRooms = value; }

        private float TotalWeight
        {
            get
            {
                if (_totalWeight == 0)
                {
                    foreach (var item in PossibleNextRooms) _totalWeight += item.Chance;
                }
                return _totalWeight;
            }
        }


        public void Create(int x, int y, Side side)
        {
            if (DungeonManager.Dungeon.GetRoom(x, y) == null)
            {
                if (PossibleNextRooms != null)
                {
                    if (PossibleNextRooms.Count > 0)
                    {
                        RoomData nextRoomData = (PossibleNextRooms.Count == 1) ? PossibleNextRooms[0].RoomData : GetRandomRoom();
                        Debug.Log(nextRoomData);

                        if (nextRoomData != null)
                        {
                            RoomData roomData = Instantiate(nextRoomData);
                            roomData.Rotate(side.Oposite());
                            if (roomData.CanCreate(x, y))
                            {
                                DungeonManager.Dungeon.SetRoom(x, y, roomData);
                                roomData.Create(x, y);
                            }
                            else Destroy(roomData);
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

            foreach (var room in PossibleNextRooms)
            {
                float chanceMultiplier = 1;//room.ShouldCreateNextRoom ? DungeonManager.Dungeon.PlugChance : DungeonManager.Dungeon.FillingChance;

                if (chance > 0 && chance <= (room.Chance * chanceMultiplier))
                {
                    nextRoom = room.RoomData;
                    break;
                }
                chance -= room.Chance;
            }
            return nextRoom;
        }
    }
}
