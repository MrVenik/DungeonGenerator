using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomBuilder", menuName = "Room Builders/Template Room Builder")]
    public class TemplateRoomBuilder : RoomBuilder
    {
        public override void Build(RoomBehaviour room)
        {
            TemplateRoomBehaviour templateRoom = room as TemplateRoomBehaviour;
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)templateRoom.Size;


            int diff = (maximumSize - roomSize) / 2;

            switch (templateRoom.Entrance)
            {
                case Side.Top:
                    Instantiate(templateRoom.TemplatePrefab, new Vector3(0.5f + templateRoom.Transform.position.x + maximumSize - 1 - diff, 0.5f + templateRoom.Transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 180), templateRoom.Transform);
                    break;
                case Side.Bottom:
                    Instantiate(templateRoom.TemplatePrefab, new Vector3(0.5f + templateRoom.Transform.position.x + diff, 0.5f + templateRoom.Transform.position.y + diff), Quaternion.Euler(0, 0, 0), templateRoom.Transform);
                    break;
                case Side.Left:
                    Instantiate(templateRoom.TemplatePrefab, new Vector3(0.5f + templateRoom.Transform.position.x + diff, 0.5f + templateRoom.Transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 270), templateRoom.Transform);
                    break;
                case Side.Right:
                    Instantiate(templateRoom.TemplatePrefab, new Vector3(0.5f + templateRoom.Transform.position.x + maximumSize - 1 - diff, 0.5f + templateRoom.Transform.position.y + diff), Quaternion.Euler(0, 0, 90), templateRoom.Transform);
                    break;
                default:
                    break;
            }
        }
    }
}
