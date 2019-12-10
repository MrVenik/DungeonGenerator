using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    public class TemplateRoomFactory : RoomFacroty
    {
        public override void CreateNextRooms(int x, int y, RoomData roomData)
        {
            if (ShouldCreateNextRoom)
            {
                var queue = new List<(int, int, Side)>();
                if (roomData.Connection.Top.CanCreateNextRoom()) queue.Add((x, y + 1, Side.Top));
                if (roomData.Connection.Bottom.CanCreateNextRoom()) queue.Add((x, y - 1, Side.Bottom));
                if (roomData.Connection.Left.CanCreateNextRoom()) queue.Add((x - 1, y, Side.Left));
                if (roomData.Connection.Right.CanCreateNextRoom()) queue.Add((x + 1, y, Side.Right));
                queue.Shuffle();
                foreach (var roomPos in queue)
                {
                    CreateNextRoom(roomPos.Item1, roomPos.Item2, roomPos.Item3, roomData);
                }
            }
        }

        protected override void CreateNextRoom(int x, int y, Side side, RoomData roomData)
        {
            roomData.Creator.Create(x, y, side);
            RoomData nextRoomData = DungeonManager.Dungeon.GetRoom(x, y);
            if (nextRoomData == null) CreateNextRoom(x, y, side, roomData);
        }
    }
}
