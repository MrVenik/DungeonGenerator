using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New Room", menuName = "Rooms/Room/Template Room")]
    public class TemplateRoomData : RoomData
    {
        public override void Create(int x, int y)
        {
            CreateNextRooms(x, y);
        }

        private void CreateNextRooms(int x, int y)
        {
            if (ShouldCreateNextRoom)
            {
                var queue = new List<(int, int, Side)>();
                if (Connection.Top.CanCreateNextRoom()) queue.Add((x, y + 1, Side.Top));
                if (Connection.Bottom.CanCreateNextRoom()) queue.Add((x, y - 1, Side.Bottom));
                if (Connection.Left.CanCreateNextRoom()) queue.Add((x - 1, y, Side.Left));
                if (Connection.Right.CanCreateNextRoom()) queue.Add((x + 1, y, Side.Right));
                queue.Shuffle();
                foreach (var roomPos in queue)
                {
                    CreateNextRoom(roomPos.Item1, roomPos.Item2, roomPos.Item3);
                }
            }
        }

        private void CreateNextRoom(int x, int y, Side side)
        {
            Creator.Create(x, y, side);
            RoomData nextRoomData = DungeonManager.Dungeon.GetRoom(x, y);
            if (nextRoomData == null) CreateNextRoom(x, y, side);
        }
    }
}
