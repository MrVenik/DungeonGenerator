using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public abstract class RoomBuilder : ScriptableObject
    {
        public abstract void Build(int roomX, int roomY, RoomData roomData, Transform transform);
    }
}
