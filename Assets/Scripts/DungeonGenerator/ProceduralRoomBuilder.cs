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
            element = proceduralRoom.GetConnectionGameObject(proceduralRoom.Connection.Top);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + 1 + diff, 0.5f + proceduralRoom.Transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 0), proceduralRoom.Transform);
            // Bottom connection
            element = proceduralRoom.GetConnectionGameObject(proceduralRoom.Connection.Bottom);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + maximumSize - 2 - diff, 0.5f + proceduralRoom.Transform.position.y + diff), Quaternion.Euler(0, 0, 180), proceduralRoom.Transform);
            // Left connection
            element = proceduralRoom.GetConnectionGameObject(proceduralRoom.Connection.Left);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + diff, 0.5f + proceduralRoom.Transform.position.y + 1 + diff), Quaternion.Euler(0, 0, 90), proceduralRoom.Transform);
            // Ritght connection
            element = proceduralRoom.GetConnectionGameObject(proceduralRoom.Connection.Right);
            if (element != null)
                Instantiate(element, new Vector3(0.5f + proceduralRoom.Transform.position.x + maximumSize - 1 - diff, 0.5f + proceduralRoom.Transform.position.y + maximumSize - 2 - diff), Quaternion.Euler(0, 0, -90), proceduralRoom.Transform);
        }
    }
}
