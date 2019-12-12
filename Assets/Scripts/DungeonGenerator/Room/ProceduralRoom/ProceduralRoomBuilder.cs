using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomBuilder", menuName = "Rooms/Room Builders/Procedural Room Builder")]
    public class ProceduralRoomBuilder : RoomBuilder
    {
        [SerializeField] private List<GameObject> _columnVariants;
        [SerializeField] private List<GameObject> _wallConnectionVariants;
        [SerializeField] private List<GameObject> _smallConnectionVariants;
        [SerializeField] private List<GameObject> _mediumConnectionVariants;
        [SerializeField] private List<GameObject> _bigConnectionVariants;
        [SerializeField] private List<GameObject> _secretConnectionVariants;

        public override void Build(RoomData roomData, Transform transform)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;

            int diff = (maximumSize - roomSize) / 2;

            Instantiate(GetVariantFrom(_columnVariants), new Vector3(0.5f + transform.position.x + diff, 0.5f + transform.position.y + diff), transform.rotation, transform);
            Instantiate(GetVariantFrom(_columnVariants), new Vector3(0.5f + transform.position.x + diff, 0.5f + transform.position.y + maximumSize - 1 - diff), transform.rotation, transform);
            Instantiate(GetVariantFrom(_columnVariants), new Vector3(0.5f + transform.position.x + maximumSize - 1 - diff, 0.5f + transform.position.y + diff), transform.rotation, transform);
            Instantiate(GetVariantFrom(_columnVariants), new Vector3(0.5f + transform.position.x + maximumSize - 1 - diff, 0.5f + transform.position.y + maximumSize - 1 - diff), transform.rotation, transform);

            GameObject element;
            // Top connection
            element = GetConnectionGameObject(roomData.Connection.Top);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + transform.position.x + 1 + diff, 0.5f + transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 0), transform);
            // Bottom connection
            element = GetConnectionGameObject(roomData.Connection.Bottom);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + transform.position.x + maximumSize - 2 - diff, 0.5f + transform.position.y + diff), Quaternion.Euler(0, 0, 180), transform);
            // Left connection
            element = GetConnectionGameObject(roomData.Connection.Left);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + transform.position.x + diff, 0.5f + transform.position.y + 1 + diff), Quaternion.Euler(0, 0, 90), transform);
            // Ritght connection
            element = GetConnectionGameObject(roomData.Connection.Right);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + transform.position.x + maximumSize - 1 - diff, 0.5f + transform.position.y + maximumSize - 2 - diff), Quaternion.Euler(0, 0, -90), transform);
        }

        private GameObject GetConnectionGameObject(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.None:
                    return null;
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
