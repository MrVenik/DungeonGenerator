using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGenerator
{
    public class Corridor : ProceduralRoom
    {
        public override bool CanCreate(int x, int y)
        {
            int connections = 0;

            Connection topConnection = DungeonManager.Dungeon.GetRoomConnection(x, y + 1);
            if (topConnection.Bottom != ConnectionType.Wall && topConnection.Bottom != ConnectionType.Border) connections++;
            Connection bottomConnection = DungeonManager.Dungeon.GetRoomConnection(x, y - 1);
            if (bottomConnection.Top != ConnectionType.Wall && bottomConnection.Top != ConnectionType.Border) connections++;
            Connection leftConnection = DungeonManager.Dungeon.GetRoomConnection(x - 1, y);
            if (leftConnection.Right != ConnectionType.Wall && leftConnection.Right != ConnectionType.Border) connections++;
            Connection rightConnection = DungeonManager.Dungeon.GetRoomConnection(x + 1, y);
            if (rightConnection.Left != ConnectionType.Wall && rightConnection.Left != ConnectionType.Border) connections++;

            Debug.Log(connections >= 2);
            return connections >= 2;
        }

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



            if (Connection.Top == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size + 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.SecretRoomDoor)
                Instantiate(GetConnectionGameObject(Connection.Right), new Vector3(Transform.position.x + size + 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);




            if (Connection.Top == ConnectionType.Wall || Connection.Top == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Top), new Vector3(Transform.position.x, Transform.position.y + size), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Bottom == ConnectionType.Wall || Connection.Bottom == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Bottom), new Vector3(Transform.position.x, Transform.position.y + 1), Quaternion.Euler(0, 0, 0), Transform);

            if (Connection.Left == ConnectionType.Wall || Connection.Left == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Left), new Vector3(Transform.position.x + 1, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);

            if (Connection.Right == ConnectionType.Wall || Connection.Right == ConnectionType.Border)
                Instantiate(GetConnectionGameObject(Connection.Right), new Vector3(Transform.position.x + size, Transform.position.y), Quaternion.Euler(0, 0, 90), Transform);
        }

        protected override GameObject GetConnectionGameObject(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    return DungeonManager.Dungeon.CorridorWallPrefab;
                case ConnectionType.Wall:
                    return DungeonManager.Dungeon.CorridorWallPrefab;
                case ConnectionType.Open:
                    return DungeonManager.Dungeon.CorridorOpenPrefab;
                case ConnectionType.Door:
                    return DungeonManager.Dungeon.CorridorDoorPrefab;
                case ConnectionType.SecretRoomDoor:
                    return DungeonManager.Dungeon.SecretRoomDoorPrefab;
                default:
                    break;
            }
            throw new Exception("Invalid connection type " + type);
        }
    }
}
