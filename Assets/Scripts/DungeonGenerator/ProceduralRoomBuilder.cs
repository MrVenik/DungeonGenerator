using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomBuilder", menuName = "Room Builders/Procedural Room Builder")]
    public class ProceduralRoomBuilder : RoomBuilder
    {
        [SerializeField] private List<GameObject> _wallConnectionVariants;
        [SerializeField] private List<GameObject> _smallConnectionVariants;
        [SerializeField] private List<GameObject> _mediumConnectionVariants;
        [SerializeField] private List<GameObject> _bigConnectionVariants;
        [SerializeField] private List<GameObject> _secretConnectionVariants;

        public override void Build(RoomBehaviour room)
        {
            ProceduralRoomBehaviour proceduralRoom = room as ProceduralRoomBehaviour;

            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)proceduralRoom.Size;

            int diff = (maximumSize - roomSize) / 2;

            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + proceduralRoom.Transform.position.x + diff, 0.5f + proceduralRoom.Transform.position.y + diff), proceduralRoom.Transform.rotation, proceduralRoom.Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + proceduralRoom.Transform.position.x + diff, 0.5f + proceduralRoom.Transform.position.y + maximumSize - 1 - diff), proceduralRoom.Transform.rotation, proceduralRoom.Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + proceduralRoom.Transform.position.x + maximumSize - 1 - diff, 0.5f + proceduralRoom.Transform.position.y + diff), proceduralRoom.Transform.rotation, proceduralRoom.Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(0.5f + proceduralRoom.Transform.position.x + maximumSize - 1 - diff, 0.5f + proceduralRoom.Transform.position.y + maximumSize - 1 - diff), proceduralRoom.Transform.rotation, proceduralRoom.Transform);

            GameObject element;
            // Top connection
            element = GetConnectionGameObject(proceduralRoom.Connection.Top);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + 1 + diff, 0.5f + proceduralRoom.Transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 0), proceduralRoom.Transform);
            // Bottom connection
            element = GetConnectionGameObject(proceduralRoom.Connection.Bottom);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + maximumSize - 2 - diff, 0.5f + proceduralRoom.Transform.position.y + diff), Quaternion.Euler(0, 0, 180), proceduralRoom.Transform);
            // Left connection
            element = GetConnectionGameObject(proceduralRoom.Connection.Left);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + diff, 0.5f + proceduralRoom.Transform.position.y + 1 + diff), Quaternion.Euler(0, 0, 90), proceduralRoom.Transform);
            // Ritght connection
            element = GetConnectionGameObject(proceduralRoom.Connection.Right);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + maximumSize - 1 - diff, 0.5f + proceduralRoom.Transform.position.y + maximumSize - 2 - diff), Quaternion.Euler(0, 0, -90), proceduralRoom.Transform);
        }

        private GameObject GetConnectionGameObject(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    return GetVariantFrom(_wallConnectionVariants);
                case ConnectionType.Wall:
                    return GetVariantFrom(_wallConnectionVariants);
                case ConnectionType.Medium:
                    return GetVariantFrom(_mediumConnectionVariants);
                case ConnectionType.Small:
                    return GetVariantFrom(_smallConnectionVariants);
                case ConnectionType.Big:
                    return GetVariantFrom(_bigConnectionVariants);
                case ConnectionType.SecretRoomDoor:
                    return GetVariantFrom(_secretConnectionVariants);
                default:
                    break;
            }
            throw new Exception("Invalid connection type " + type);
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
