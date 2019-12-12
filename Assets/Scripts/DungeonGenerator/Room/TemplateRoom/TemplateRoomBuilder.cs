using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomBuilder", menuName = "Rooms/Room Builders/Template Room Builder")]
    public class TemplateRoomBuilder : RoomBuilder
    {
        [SerializeField] private GameObject _templatePrefab;
        public override void Build(RoomData roomData, Transform transform)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;


            int diff = (maximumSize - roomSize) / 2;

            switch (roomData.Entrance)
            {
                case Side.Top:
                    Instantiate(_templatePrefab, new Vector3(transform.position.x + maximumSize - diff, transform.position.y + maximumSize - diff), Quaternion.Euler(0, 0, 180), transform);
                    break;
                case Side.Bottom:
                    Instantiate(_templatePrefab, new Vector3(transform.position.x + diff, transform.position.y + diff), Quaternion.Euler(0, 0, 0), transform);
                    break;
                case Side.Left:
                    Instantiate(_templatePrefab, new Vector3(transform.position.x + diff, transform.position.y + maximumSize - diff), Quaternion.Euler(0, 0, 270), transform);
                    break;
                case Side.Right:
                    Instantiate(_templatePrefab, new Vector3(transform.position.x + maximumSize - diff, transform.position.y + diff), Quaternion.Euler(0, 0, 90), transform);
                    break;
                default:
                    break;
            }
        }
    }
}
