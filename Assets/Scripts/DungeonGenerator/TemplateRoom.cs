using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class TemplateRoom : Room
    {
        [SerializeField] private GameObject _templatePrefab;

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

            Room nextRoom = DungeonManager.Dungeon.GetRoom(x, y);

            if (nextRoom == null)
            {
                CreateNextRoom(x, y, side);
            }
        }

        public override void Build()
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)Size;


            int diff = (maximumSize - roomSize) / 2;

            switch (Entrance)
            {
                case Side.Top:
                    Instantiate(_templatePrefab, new Vector3(0.5f + Transform.position.x + maximumSize - 1 - diff, 0.5f + Transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 180), Transform);
                    break;
                case Side.Bottom:
                    Instantiate(_templatePrefab, new Vector3(0.5f + Transform.position.x + diff, 0.5f + Transform.position.y + diff), Quaternion.Euler(0, 0, 0), Transform);
                    break;
                case Side.Left:
                    Instantiate(_templatePrefab, new Vector3(0.5f + Transform.position.x + diff, 0.5f + Transform.position.y + maximumSize - 1 - diff), Quaternion.Euler(0, 0, 270), Transform);
                    break;
                case Side.Right:
                    Instantiate(_templatePrefab, new Vector3(0.5f + Transform.position.x + maximumSize - 1 - diff, 0.5f + Transform.position.y + diff), Quaternion.Euler(0, 0, 90), Transform);
                    break;
                default:
                    break;
            }
        }
    }
}