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
            int size = DungeonManager.Dungeon.RoomSize;

            switch (Entrance)
            {
                case Side.Top:
                    Instantiate(_templatePrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y + size - 1), Quaternion.Euler(0, 0, 180), Transform);
                    break;
                case Side.Bottom:
                    Instantiate(_templatePrefab, new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 0), Transform);
                    break;
                case Side.Left:
                    Instantiate(_templatePrefab, new Vector3(Transform.position.x, Transform.position.y + size - 1), Quaternion.Euler(0, 0, 270), Transform);
                    break;
                case Side.Right:
                    Instantiate(_templatePrefab, new Vector3(Transform.position.x + size - 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);
                    break;
                default:
                    break;
            }
        }
    }
}