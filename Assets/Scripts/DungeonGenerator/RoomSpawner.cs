using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public static class RoomSpawner
    {
        public static RoomBehaviour Spawn(Vector3 position, GameObject room)
        {
            RoomBehaviour spawnedRoom = UnityEngine.Object.Instantiate(room, position, Quaternion.identity, DungeonManager.Dungeon.Transform).GetComponent<RoomBehaviour>();
            spawnedRoom.name = room.name;
            return spawnedRoom;
        }
    }
}
