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
        [SerializeField] private FloorBuilder _floorBuilder;

        public FloorBuilder FloorBuilder { get => _floorBuilder; private set => _floorBuilder = value; }

        public abstract void Build(RoomData roomData, Transform transform);
    }
}
