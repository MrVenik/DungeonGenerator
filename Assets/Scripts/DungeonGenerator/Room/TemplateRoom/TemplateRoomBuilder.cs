using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New RoomBuilder", menuName = "Rooms/Room Builders/Template Room Builder")]
    public class TemplateRoomBuilder : RoomBuilder
    {
        [SerializeField] private List<WallData> _wallVariants;
        [SerializeField] private List<Tile> _floorVariants;

        private string[] _template = {  "###___###",
                                        "#_______#",
                                        "#_______#",
                                        "___###___",
                                        "___# #___",
                                        "___###___",
                                        "#_______#",
                                        "#_______#",
                                        "###___###" };



        private RoomCellData[,] _roomCells;

        private WallData _roomWall;

        public void PlaceCells(RoomData roomData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            _roomCells = new RoomCellData[maximumSize, maximumSize];

            for (int x = 0; x < maximumSize; x++)
            {
                for (int y = 0; y < maximumSize; y++)
                {
                    if (_template[x][y] == ' ')
                    {
                        _roomCells[x, y] = RoomCellData.None;
                    }
                    else if (_template[x][y] == '#')
                    {
                        _roomCells[x, y] = RoomCellData.Wall;
                    }
                    else if (_template[x][y] == '_')
                    {
                        _roomCells[x, y] = RoomCellData.Floor;
                    }
                }
            }

        }

        public override void Build(RoomData roomData, Vector3Int position, TilemapData tilemapData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;

            _roomWall = _wallVariants.GetRandomElement();

            PlaceCells(roomData);

            for (int x = 0; x < maximumSize; x++)
            {
                for (int y = 0; y < maximumSize; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, 0);

                    if (_roomCells[x, y] == RoomCellData.Wall)
                    {
                        PlaceWall(x, y, position, tilemapData.WallLayer);
                    }
                    else if (_roomCells[x, y] == RoomCellData.Floor)
                    {
                        if (!tilemapData.FloorLayer.HasTile(tilePosition))
                        {
                            tilemapData.FloorLayer.SetTile(tilePosition, _floorVariants.GetRandomElement());
                        }
                    }
                }
            }
        }

        private void PlaceWall(int x, int y, Vector3Int roomPosition, Tilemap wallLayer)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;

            // placing top wall
            if (y - 1 >= 0 && _roomCells[x, y - 1] == RoomCellData.Floor)
            {
                if (y + 1 >= maximumSize || _roomCells[x, y + 1] != RoomCellData.Floor)
                {
                    // Placing brick
                    Vector3Int brickTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y, 0);
                    if (!wallLayer.HasTile(brickTilePosition))
                    {
                        wallLayer.SetTile(brickTilePosition, _roomWall.Brick);
                    }
                    // Placing top left inner
                    if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                    {
                        Vector3Int topTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(topTilePosition))
                        {
                            wallLayer.SetTile(topTilePosition, _roomWall.TopInnerLeft);
                        }
                    }
                    // Placing top right inner
                    else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                    {
                        Vector3Int topTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(topTilePosition))
                        {
                            wallLayer.SetTile(topTilePosition, _roomWall.TopInnerRight);
                        }
                    }
                    // Placing top
                    else
                    {
                        Vector3Int topTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(topTilePosition))
                        {
                            wallLayer.SetTile(topTilePosition, _roomWall.Top);
                        }
                    }
                }
            }

            // placing bottom wall
            if (y + 1 < maximumSize && _roomCells[x, y + 1] == RoomCellData.Floor)
            {
                // Placing bottom left inner
                if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.BottomInnerLeft);
                    }
                }
                // Placing bottom right inner
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.BottomInnerRight);
                    }
                }
                // Placing bottom
                else
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.Bottom);
                    }
                }
            }

            // placing side walls
            if (y - 1 >= 0 && y + 1 < maximumSize)
            {
                if (_roomCells[x, y - 1] == RoomCellData.Wall && _roomCells[x, y + 1] == RoomCellData.Wall)
                {
                    // Placing left
                    if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                    {
                        Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(bottomTilePosition))
                        {
                            wallLayer.SetTile(bottomTilePosition, _roomWall.TopLeft);
                        }
                    }
                    // Placing right
                    else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                    {
                        Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(bottomTilePosition))
                        {
                            wallLayer.SetTile(bottomTilePosition, _roomWall.TopRight);
                        }
                    }
                }
            }
            // placing top corners
            if (y + 1 >= maximumSize || _roomCells[x, y + 1] == RoomCellData.None)
            {
                // Placing left outer
                if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Wall)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.TopOuterLeft);
                    }
                }
                // Placing left 
                else if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.TopLeft);
                    }
                }
                // Placing right outer
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Wall)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.TopOuterRight);
                    }
                }
                // Placing right 
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.TopRight);
                    }
                }
            }
            // placing bottom corners
            if (y - 1 < 0 || _roomCells[x, y - 1] == RoomCellData.None)
            {
                // Placing left outer
                if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Wall)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.BottomOuterLeft);
                    }
                }
                // Placing left 
                else if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.TopLeft);
                    }
                }
                // Placing right outer
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Wall)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.BottomOuterRight);
                    }
                }
                // Placing right 
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _roomWall.TopRight);
                    }
                }
            }
        }
    }
}
