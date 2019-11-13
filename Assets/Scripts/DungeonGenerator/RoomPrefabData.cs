using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [Serializable]
    public class RoomPrefabData
    {
        public string Name;
        public GameObject Prefab;
        public float Chance;
        public bool IsPlug;
    }
}
