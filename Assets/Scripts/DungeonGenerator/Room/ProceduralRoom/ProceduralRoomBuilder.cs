using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGenerator
{
    public class RoomTilemapData
    {
        public int Size { get; private set; }
        public Tile[,] FloorLayer;
        public Tile[,] WallLayer;


        public RoomTilemapData(int size)
        {
            Size = size;
            FloorLayer = new Tile[Size, Size];
            WallLayer = new Tile[Size, Size];
        }
    }

    public enum RoomCellData
    {
        None,
        Wall,
        Floor
    }

    [Serializable]
    public class WallData
    {
        public Tile Top;
        public Tile Brick;
        public Tile Bottom;

        public Tile TopLeft;
        public Tile TopRight;

        public Tile TopInnerLeft;
        public Tile TopInnerRight;
        public Tile TopOuterLeft;
        public Tile TopOuterRight;

        public Tile BottomInnerLeft;
        public Tile BottomInnerRight;
        public Tile BottomOuterLeft;
        public Tile BottomOuterRight;
    }

    [CreateAssetMenu(fileName = "New RoomBuilder", menuName = "Rooms/Room Builders/Procedural Room Builder")]
    public class ProceduralRoomBuilder : RoomBuilder
    {
        [SerializeField] private List<WallData> _wallVariants;
        [SerializeField] private List<Tile> _floorVariants;
        [SerializeField] private GameObject _shadow;

        private RoomTilemapData _tilemapData;

        private RoomCellData[,] _roomCells;

        private void BuildFloor(RoomData roomData, Vector3Int position, TilemapData tilemapData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;

            int roomOffset = (maximumSize - roomSize) / 2;

            for (int x = position.x + roomOffset + 1; x < position.x + maximumSize - roomOffset - 1; x++)
            {
                for (int y = position.y + roomOffset + 1; y < position.y + maximumSize - roomOffset - 1; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    if (!tilemapData.FloorLayer.HasTile(tilePosition))
                    {
                        tilemapData.FloorLayer.SetTile(tilePosition, _floorVariants.GetRandomElement());
                    }
                }
            }

        }

        private void BuildSide(RoomData roomData, Side side, Vector3Int position, TilemapData tilemapData)
        {
            switch (roomData.Connection.GetConnectionTypeBySide(side))
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    break;
                case ConnectionType.Wall:
                    BuildWall(roomData, side, position, tilemapData);
                    break;
                case ConnectionType.Small:
                    BuildOpen(roomData, side, 2, position, tilemapData);
                    break;
                case ConnectionType.Medium:
                    BuildOpen(roomData, side, 3, position, tilemapData);
                    break;
                case ConnectionType.Big:
                    BuildOpen(roomData, side, 4, position, tilemapData);
                    break;
                case ConnectionType.SecretRoomDoor:
                    break;
                default:
                    break;
            }
        }

        private void BuildWall(RoomData roomData, Side side, Vector3Int position, TilemapData tilemapData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;
            int offset = (maximumSize - roomSize) / 2;

            switch (side)
            {
                case Side.Top:
                    for (int x = position.x + offset; x < position.x + maximumSize - offset; x++)
                    {
                        if (x > position.x + offset && x < position.x + maximumSize - offset - 1)
                        {
                            int y = position.y + (maximumSize - offset - 1);
                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                            {
                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Top);
                            }

                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                            {
                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                            }
                        }
                    }
                    break;

                case Side.Bottom:
                    for (int x = position.x + offset; x < position.x + maximumSize - offset; x++)
                    {
                        if (x > position.x + offset && x < position.x + maximumSize - offset - 1)
                        {
                            int y = position.y + offset + 1;
                            Vector3Int bottomTilePosition = new Vector3Int(x, y, 0);
                            if (!tilemapData.WallLayer.HasTile(bottomTilePosition))
                            {
                                tilemapData.WallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().Bottom);
                            }
                        }
                    }
                    break;

                case Side.Left:
                    for (int y = position.y + offset; y < position.y + maximumSize - offset; y++)
                    {
                        if (y == position.y + offset + 1)
                        {
                            if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                            {
                                int x = position.x + offset;
                                Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                {
                                    tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                }
                            }
                            else
                            {
                                int x = position.x + offset;
                                Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                {
                                    tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().BottomOuterLeft);
                                }
                            }
                        }
                        else if (y == position.y + maximumSize - offset - 1)
                        {
                            int x = position.x + offset;
                            Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                            {
                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                            }
                            if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                            {
                                Vector3Int leftOuterTilePosition = new Vector3Int(x, y + 1, 0);
                                if (!tilemapData.WallLayer.HasTile(leftOuterTilePosition))
                                {
                                    tilemapData.WallLayer.SetTile(leftOuterTilePosition, _wallVariants.GetRandomElement().TopOuterLeft);
                                }
                            }
                        }
                        else if (y > position.y + offset && y < position.y + maximumSize - offset - 1)
                        {
                            int x = position.x + offset;
                            Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                            {
                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                            }
                        }
                    }
                    break;

                case Side.Right:
                    for (int y = position.y + offset + 1; y < position.y + maximumSize - offset; y++)
                    {
                        if (y == position.y + offset + 1)
                        {
                            if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                            {
                                int x = position.x + maximumSize - offset - 1;
                                Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                {
                                    tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                                }
                            }
                            else
                            {
                                int x = position.x + maximumSize - offset - 1;
                                Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                {
                                    tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().BottomOuterRight);
                                }
                            }
                        }
                        else if (y == position.y + maximumSize - offset - 1)
                        {
                            int x = position.x + maximumSize - offset - 1;
                            Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                            if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                            {
                                tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                            }

                            if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                            {
                                Vector3Int rightOuterTilePosition = new Vector3Int(x, y + 1, 0);
                                if (!tilemapData.WallLayer.HasTile(rightOuterTilePosition))
                                {
                                    tilemapData.WallLayer.SetTile(rightOuterTilePosition, _wallVariants.GetRandomElement().TopOuterRight);
                                }
                            }
                        }
                        else if (y > position.y + offset && y < position.y + maximumSize - offset - 1)
                        {
                            int x = position.x + maximumSize - offset - 1;
                            Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                            if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                            {
                                tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void BuildOpen(RoomData roomData, Side side, int connectionOffset, Vector3Int position, TilemapData tilemapData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;
            int offset = (maximumSize - roomSize) / 2;
            int centerPoint = (maximumSize - 1) / 2;


            switch (side)
            {
                case Side.Top:
                    for (int y = position.y + maximumSize - offset - 1; y < position.y + maximumSize; y++)
                    {
                        for (int x = position.x + offset; x < position.x + maximumSize - offset; x++)
                        {
                            if (y == position.y + maximumSize - offset - 1)
                            {
                                // placing floor between walls
                                if (x > position.x + centerPoint - connectionOffset && x < position.x + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                                else if (x == position.x + centerPoint - connectionOffset || x == position.x + centerPoint + connectionOffset)
                                {
                                    if (x == position.x + centerPoint - connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerLeft);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }

                                    }
                                    else if (x == position.x + centerPoint + connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerRight);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }
                                    }

                                }
                                else if (x > position.x + offset && x < position.x + maximumSize - offset - 1)
                                {
                                    Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                    if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                    {
                                        tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Top);
                                    }

                                    Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                    {
                                        tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                    }
                                }
                            }
                            else
                            {
                                if (x == position.x + centerPoint - connectionOffset || x == position.x + centerPoint + connectionOffset)
                                {
                                    if (y != position.y + maximumSize - offset)
                                    {
                                        if (x == position.x + centerPoint - connectionOffset)
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                            }
                                        }
                                        else if (x == position.x + centerPoint + connectionOffset)
                                        {
                                            Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (x == position.x + centerPoint - connectionOffset)
                                        {
                                            if (roomData.Connection.Left == ConnectionType.Wall)
                                            {
                                                Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                                if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                                {
                                                    tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                                }
                                            }
                                        }
                                        else if (x == position.x + centerPoint + connectionOffset)
                                        {
                                            if (roomData.Connection.Right == ConnectionType.Wall)
                                            {
                                                Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                                if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                                {
                                                    tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (x > position.x + centerPoint - connectionOffset && x < position.x + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                            }


                        }
                    }
                    break;

                case Side.Bottom:
                    for (int y = position.y; y < position.y + offset + 1; y++)
                    {
                        for (int x = position.x + offset; x < position.x + maximumSize - offset; x++)
                        {
                            if (y == position.y + offset)
                            {
                                // placing floor between walls
                                if (x > position.x + centerPoint - connectionOffset && x < position.x + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                                else if (x == position.x + centerPoint - connectionOffset || x == position.x + centerPoint + connectionOffset)
                                {
                                    if (x == position.x + centerPoint - connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                                        {
                                            Vector3Int bottomTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(bottomTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().BottomInnerRight);
                                            }
                                            Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                            }
                                        }
                                        else if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Left))
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                            }
                                        }

                                    }
                                    else if (x == position.x + centerPoint + connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().BottomInnerLeft);
                                            }
                                            Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                                            }
                                        }
                                        else if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Right))
                                        {
                                            Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                                            }
                                        }
                                    }

                                }
                                else if (x > position.x + offset && x < position.x + maximumSize - offset - 1)
                                {
                                    Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                    if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                    {
                                        tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Bottom);
                                    }
                                }
                            }
                            else
                            {
                                if (x == position.x + centerPoint - connectionOffset || x == position.x + centerPoint + connectionOffset)
                                {
                                    if (x == position.x + centerPoint - connectionOffset)
                                    {
                                        Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                        if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                        }
                                    }
                                    else if (x == position.x + centerPoint + connectionOffset)
                                    {
                                        Vector3Int rightTilePosition = new Vector3Int(x, y, 0);
                                        if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().TopRight);
                                        }
                                    }
                                }
                                else if (x > position.x + centerPoint - connectionOffset && x < position.x + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                            }


                        }
                    }
                    break;
                case Side.Left:
                    for (int x = position.x; x < position.x + offset + 1; x++)
                    {
                        for (int y = position.y + offset; y < position.y + maximumSize - offset; y++)
                        {
                            if (x == position.x + offset)
                            {
                                if (y > position.y + centerPoint - connectionOffset && y < position.y + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                                else if (y == position.y + centerPoint - connectionOffset || y == position.y + centerPoint + connectionOffset)
                                {
                                    if (y == position.y + centerPoint + connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Left))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerLeft);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }
                                        else if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerLeft);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Top);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }
                                    }
                                    else if (y == position.y + centerPoint - connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Left))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().BottomInnerRight);
                                            }
                                        }
                                        else if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().BottomInnerRight);
                                            }
                                            Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Bottom);
                                            }
                                        }
                                    }
                                }
                                else 
                                {
                                    if (y > position.y + offset && y < position.y + maximumSize - offset - 1)
                                    {
                                        Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                        if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                        }
                                    }
                                    else if (y == position.y + offset)
                                    {
                                        if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().BottomOuterLeft);
                                            }
                                        }
                                    }
                                    else if (y == position.y + maximumSize - offset - 1)
                                    {
                                        if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopLeft);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopOuterLeft);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (y == position.y + centerPoint - connectionOffset || y == position.y + centerPoint + connectionOffset)
                                {
                                    if (y == position.y + centerPoint - connectionOffset)
                                    {
                                        Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                        if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().Bottom);
                                        }
                                    }
                                    else if (y == position.y + centerPoint + connectionOffset)
                                    {
                                        Vector3Int rightTilePosition = new Vector3Int(x, y + 1, 0);
                                        if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().Top);
                                        }
                                        Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                        if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                        }
                                    }

                                }
                                else if (y > position.y + centerPoint - connectionOffset && y < position.y + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                            }

                        }
                    }
                    break;
                case Side.Right:
                    for (int x = position.x + maximumSize - offset - 1; x < position.x + maximumSize; x++)
                    {
                        for (int y = position.y + offset; y < position.y + maximumSize - offset; y++)
                        {

                            if (x == position.x + maximumSize - offset - 1)
                            {
                                if (y > position.y + centerPoint - connectionOffset && y < position.y + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                                else if (y == position.y + centerPoint - connectionOffset || y == position.y + centerPoint + connectionOffset)
                                {
                                    if (y == position.y + centerPoint + connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Right))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerRight);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }
                                        else if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerRight);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Top);
                                            }
                                            Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                            }
                                        }

                                    }
                                    else if (y == position.y + centerPoint - connectionOffset)
                                    {
                                        if (!roomData.Size.IsEqualsToConnectionType(roomData.Connection.Right))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().BottomInnerLeft);
                                            }
                                        }
                                        else if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().BottomInnerLeft);
                                            }
                                            Vector3Int leftTilePosition = new Vector3Int(x, y, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopRight);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int topTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(topTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Bottom);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (y > position.y + offset && y < position.y + maximumSize - offset - 1)
                                    {
                                        Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                        if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopRight);
                                        }
                                    }
                                    else if (y == position.y + offset)
                                    {
                                        if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Bottom))
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopRight);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().BottomOuterRight);
                                            }
                                        }
                                    }
                                    else if (y == position.y + maximumSize - offset - 1)
                                    {
                                        if (roomData.Size.IsEqualsToConnectionType(roomData.Connection.Top))
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopRight);
                                            }
                                        }
                                        else
                                        {
                                            Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                            if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                            {
                                                tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().TopOuterRight);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (y == position.y + centerPoint - connectionOffset || y == position.y + centerPoint + connectionOffset)
                                {
                                    if (y == position.y + centerPoint - connectionOffset)
                                    {
                                        Vector3Int leftTilePosition = new Vector3Int(x, y + 1, 0);
                                        if (!tilemapData.WallLayer.HasTile(leftTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(leftTilePosition, _wallVariants.GetRandomElement().Bottom);
                                        }
                                    }
                                    else if (y == position.y + centerPoint + connectionOffset)
                                    {
                                        Vector3Int rightTilePosition = new Vector3Int(x, y + 1, 0);
                                        if (!tilemapData.WallLayer.HasTile(rightTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(rightTilePosition, _wallVariants.GetRandomElement().Top);
                                        }
                                        Vector3Int brickTilePosition = new Vector3Int(x, y, 0);
                                        if (!tilemapData.WallLayer.HasTile(brickTilePosition))
                                        {
                                            tilemapData.WallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                                        }
                                    }
                                }
                                else if (y > position.y + centerPoint - connectionOffset && y < position.y + centerPoint + connectionOffset)
                                {
                                    Vector3Int floorTilePosition = new Vector3Int(x, y, 0);
                                    if (!tilemapData.FloorLayer.HasTile(floorTilePosition))
                                    {
                                        tilemapData.FloorLayer.SetTile(floorTilePosition, _floorVariants.GetRandomElement());
                                    }
                                }
                            }

                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Build(RoomData roomData, Vector3Int position, TilemapData tilemapData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;

            BuildSide(roomData, Side.Top, position, tilemapData);
            BuildSide(roomData, Side.Bottom, position, tilemapData);
            BuildSide(roomData, Side.Left, position, tilemapData);
            BuildSide(roomData, Side.Right, position, tilemapData);

            BuildFloor(roomData, position, tilemapData);


        }

        private RoomElementData GetRandomElement(List<RoomElementData> possibleElements)
        {
            List<RoomElementData> elements = new List<RoomElementData>();

            float chance = UnityEngine.Random.Range(0f, 1f);

            foreach (var element in possibleElements)
            {
                if (chance < element.Chance)
                {
                    elements.Add(element);
                }
            }

            if (elements.Count > 0)
            {
                int rndIndex = UnityEngine.Random.Range(0, elements.Count);
                return elements[rndIndex];
            }
            return null;
        }
    }
}
