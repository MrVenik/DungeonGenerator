using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class Corridor : Room
    {
        protected override ConnectionType[] PossibleConnectionTypes { get; } = new ConnectionType[]
        {
            ConnectionType.Wall,
            ConnectionType.Door
        };

        protected override ConnectionType CreateNewConnection()
        {

            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance <= 0.5f)
            {
                return ConnectionType.Door;
            }
            else return ConnectionType.Wall;

            //int index = UnityEngine.Random.Range(0, PossibleConnectionTypes.Length);
            //return PossibleConnectionTypes[index];
        }

        public override void Build()
        {
            int size = DungeonManager.Dungeon.CorridorSize;
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + 1, Transform.position.y + 1), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + 1, Transform.position.y + size), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size, Transform.position.y + 1), Transform.rotation, Transform);
            Instantiate(DungeonManager.Dungeon.ColumnPrefab, new Vector3(Transform.position.x + size, Transform.position.y + size), Transform.rotation, Transform);


            if (Connection.Top == ConnectionType.Open)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size + 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.Open)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.Open)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.Open)
                Instantiate(GetConnectionGameObject(Connection.Right), new Vector3(Transform.position.x + size + 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);



            if (Connection.Top == ConnectionType.Door)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size + 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.Door)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.Door)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.Door)
                Instantiate(GetConnectionGameObject(Connection.Right), new Vector3(Transform.position.x + size + 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);



            if (Connection.Top == ConnectionType.Wall)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.Wall)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y + 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.Wall)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x + 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.Wall)
                Instantiate(GetConnectionGameObject(Connection.Right), new Vector3(Transform.position.x + size, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);
        }

        public override GameObject GetConnectionGameObject(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    break;
                case ConnectionType.Wall:
                    return DungeonManager.Dungeon.CorridorWallPrefab;
                case ConnectionType.Open:
                    return DungeonManager.Dungeon.CorridorOpenPrefab;
                    break;
                case ConnectionType.Door:
                    return DungeonManager.Dungeon.CorridorDoorPrefab;
                default:
                    break;
            }
            throw new Exception("Invalid connection type " + type);
        }
    }
}
