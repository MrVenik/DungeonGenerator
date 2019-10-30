using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class TemplateRoom : Room
    {
        [SerializeField] private GameObject _templatePrefab;

        public override bool CanCreate(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public override void Create(int x, int y)
        {
            _x = x;
            _y = y;
            CreateNextRooms();
        }

        protected override void CreateConnections()
        {
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