using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGenerator
{
    public abstract class RoomBuilder : ScriptableObject
    {
        public abstract void Build(RoomData roomData, Vector3Int position, TilemapData tilemapData);
    }
}
