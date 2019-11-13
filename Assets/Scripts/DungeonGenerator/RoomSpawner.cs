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
        public static Room Spawn(Vector3 position, GameObject room)
        {
            Room spawnedRoom = UnityEngine.Object.Instantiate(room, position, Quaternion.identity, DungeonManager.Dungeon.Transform).GetComponent<Room>();
            spawnedRoom.name = room.name;
            return spawnedRoom;
        }
    }
}
