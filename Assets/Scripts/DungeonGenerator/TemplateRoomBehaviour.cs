using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class TemplateRoomBehaviour : RoomBehaviour
    {
        [SerializeField] public GameObject TemplatePrefab;


        protected override float ChanceOfNextRoom => 1.0f;

        public override bool CanCreate(int x, int y)
        {
            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (topConnection.Bottom != Connection.Top && topConnection.Bottom != ConnectionType.None) return false;
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (bottomConnection.Top != Connection.Bottom && bottomConnection.Top != ConnectionType.None) return false;
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (leftConnection.Right != Connection.Left && leftConnection.Right != ConnectionType.None) return false;
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (rightConnection.Left != Connection.Right && rightConnection.Left != ConnectionType.None) return false;
            return true;
        }

        public override void Create(int x, int y)
        {
            X = x;
            Y = y;

            CreateNextRooms();
        }

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