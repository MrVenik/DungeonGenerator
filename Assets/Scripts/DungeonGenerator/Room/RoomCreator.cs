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
        public CreatableData Data;
        [Range(0.0f, 1.0f)]
        public float Chance;
    }

    [CreateAssetMenu(fileName = "New RoomCreator", menuName = "Rooms/Room Creator")]
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
                        CreatableData nextRoomData = (PossibleNextRooms.Count == 1) ? PossibleNextRooms[0].Data : GetRandomRoom();

                        if (nextRoomData != null)
                        {
                            CreatableData roomData = Instantiate(nextRoomData);
                            roomData.Rotate(side.Oposite());
                            if (roomData.CanCreate(x, y))
                            {
                                if (roomData is RoomGroupData)
                                {
                                    RoomGroupData roomGroup = roomData as RoomGroupData;
                                    for (int ix = x - roomGroup.EntranceX, j = 0; ix < x + roomGroup.ArraySize - roomGroup.EntranceX; ix++, j++)
                                    {
                                        for (int iy = y - roomGroup.EntranceY, k = 0; iy < y + roomGroup.ArraySize - roomGroup.EntranceY; iy++, k++)
                                        {
                                            GroupElementData groupElement = roomGroup.Elements[j + k * roomGroup.ArraySize];
                                            if (groupElement != null && groupElement.RoomData != null)
                                            {
                                                RoomData room = groupElement.RoomData;
                                                DungeonManager.Dungeon.SetRoom(ix, iy, room);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    DungeonManager.Dungeon.SetRoom(x, y, roomData as RoomData);
                                }
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

        private CreatableData GetRandomRoom()
        {
            CreatableData nextRoom = null;

            float chance = UnityEngine.Random.Range(0f, TotalWeight);

            foreach (var room in PossibleNextRooms)
            {
                float chanceMultiplier = room.Data.IsPlug ? DungeonManager.Dungeon.PlugChance : DungeonManager.Dungeon.FillingChance;

                if (chance > 0 && chance <= (room.Chance * chanceMultiplier))
                {
                    nextRoom = room.Data;
                    break;
                }
                chance -= room.Chance;
            }
            return nextRoom;
        }
    }
}
