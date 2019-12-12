using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New FloorBuilder", menuName = "Rooms/Room Builders/Floor Builder")]
    public class FloorBuilder : ScriptableObject
    {
        [SerializeField] private List<GameObject> _tileVariants;

        public void Build(RoomData roomData, Transform transform)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;

            int diff = (maximumSize - roomSize) / 2;

            for (int x = 0; x < roomSize; x++)
            {
                for (int y = 0; y < roomSize; y++)
                {
                    GameObject tile = GetVariantFrom(_tileVariants);
                    Instantiate(tile, new Vector3(transform.position.x + diff + x, transform.position.y + diff + y, 1), transform.rotation, transform);
                }
            }


        }

        private GameObject GetVariantFrom(List<GameObject> variants)
        {
            if (variants != null && variants.Count > 0)
            {
                int rndIndex = UnityEngine.Random.Range(0, variants.Count);
                return variants[rndIndex];
            }
            return null;
        }
    }
}
