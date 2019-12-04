using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class TemplateRoomBehaviour : RoomBehaviour
    {
        [SerializeField] public GameObject TemplatePrefab;


        protected override float ChanceOfNextRoom => 1.0f;

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