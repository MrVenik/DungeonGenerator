using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class TemplateRoomBehaviour : RoomBehaviour
    {
        protected override void CreateNextRoom(int x, int y, Side side)
        {
            base.CreateNextRoom(x, y, side);

            RoomBehaviour nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom == null)
            {
                CreateNextRoom(x, y, side);
            }
        }
    }
}