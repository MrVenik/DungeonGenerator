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

        private void PlaceCells(RoomData roomData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            _roomCells = new RoomCellData[maximumSize, maximumSize];

            BuildSide(roomData, Side.Top);
            BuildSide(roomData, Side.Bottom);
            BuildSide(roomData, Side.Left);
            BuildSide(roomData, Side.Right);

            BuildFloor(roomData);
        }

        private void BuildFloor(RoomData roomData)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;

            int roomOffset = (maximumSize - roomSize) / 2;

            for (int x = roomOffset + 1; x < maximumSize - roomOffset - 1; x++)
            {
                for (int y = roomOffset + 1; y < maximumSize - roomOffset - 1; y++)
                {
                    if (_roomCells[x, y] == RoomCellData.None)
                    {
                        _roomCells[x, y] = RoomCellData.Floor;
                    }
                }
            }

        }

        private void BuildSide(RoomData roomData, Side side)
        {
            switch (roomData.Connection.GetConnectionTypeBySide(side))
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.Border:
                    break;
                case ConnectionType.Wall:
                    BuildWall(roomData, side);
                    break;
                case ConnectionType.Small:
                    BuildOpen(roomData, side, 2);
                    break;
                case ConnectionType.Medium:
                    BuildOpen(roomData, side, 3);
                    break;
                case ConnectionType.Big:
                    BuildOpen(roomData, side, 4);
                    break;
                case ConnectionType.SecretRoomDoor:
                    break;
                default:
                    break;
            }
        }

        private void BuildWall(RoomData roomData, Side side)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;
            int offset = (maximumSize - roomSize) / 2;

            switch (side)
            {
                case Side.Top:
                    for (int x = offset; x < maximumSize - offset; x++)
                    {
                        if (_roomCells[x, maximumSize - offset - 1] == RoomCellData.None)
                        {
                            _roomCells[x, maximumSize - offset - 1] = RoomCellData.Wall;
                        }
                    }


                    break;
                case Side.Bottom:
                    for (int x = offset; x < maximumSize - offset; x++)
                    {
                        if (_roomCells[x, offset] == RoomCellData.None)
                        {
                            _roomCells[x, offset] = RoomCellData.Wall;
                        }
                    }

                    break;
                case Side.Left:
                    for (int y = offset; y < maximumSize - offset; y++)
                    {
                        if (_roomCells[offset, y] == RoomCellData.None)
                        {
                            _roomCells[offset, y] = RoomCellData.Wall;
                        }
                    }

                    break;
                case Side.Right:
                    for (int y = offset; y < maximumSize - offset; y++)
                    {
                        if (_roomCells[maximumSize - offset - 1, y] == RoomCellData.None)
                        {
                            _roomCells[maximumSize - offset - 1, y] = RoomCellData.Wall;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void BuildOpen(RoomData roomData, Side side, int connectionOffset)
        {
            int maximumSize = (int)DungeonManager.Dungeon.MaximumRoomSize;
            int roomSize = (int)roomData.Size;
            int offset = (maximumSize - roomSize) / 2;
            int centerPoint = (maximumSize - 1) / 2;


            switch (side)
            {
                case Side.Top:
                    for (int y = maximumSize - offset - 1; y < maximumSize; y++)
                    {
                        for (int x = offset; x < maximumSize - offset; x++)
                        {
                            if (_roomCells[x, y] == RoomCellData.None)
                            {
                                if (y == maximumSize - offset - 1)
                                {
                                    if (x > centerPoint - connectionOffset && x < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
                                    }
                                    else
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                }
                                else
                                {
                                    if (x == centerPoint - connectionOffset || x == centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                    else if (x > centerPoint - connectionOffset && x < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case Side.Bottom:
                    for (int y = 0; y < offset + 1; y++)
                    {
                        for (int x = offset; x < maximumSize - offset; x++)
                        {
                            if (_roomCells[x, y] == RoomCellData.None)
                            {
                                if (y == offset)
                                {
                                    if (x > centerPoint - connectionOffset && x < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
                                    }
                                    else
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                }
                                else
                                {
                                    if (x == centerPoint - connectionOffset || x == centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                    else if (x > centerPoint - connectionOffset && x < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case Side.Left:
                    for (int x = 0; x < offset + 1; x++)
                    {
                        for (int y = offset; y < maximumSize - offset; y++)
                        {
                            if (_roomCells[x, y] == RoomCellData.None)
                            {
                                if (x == offset)
                                {
                                    if (y > centerPoint - connectionOffset && y < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
                                    }
                                    else
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                }
                                else
                                {
                                    if (y == centerPoint - connectionOffset || y == centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                    else if (y > centerPoint - connectionOffset && y < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case Side.Right:
                    for (int x = maximumSize - offset - 1; x < maximumSize; x++)
                    {
                        for (int y = offset; y < maximumSize - offset; y++)
                        {
                            if (_roomCells[x, y] == RoomCellData.None)
                            {
                                if (x == maximumSize - offset - 1)
                                {
                                    if (y > centerPoint - connectionOffset && y < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
                                    }
                                    else
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                }
                                else
                                {
                                    if (y == centerPoint - connectionOffset || y == centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Wall;
                                    }
                                    else if (y > centerPoint - connectionOffset && y < centerPoint + connectionOffset)
                                    {
                                        _roomCells[x, y] = RoomCellData.Floor;
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
                        wallLayer.SetTile(brickTilePosition, _wallVariants.GetRandomElement().Brick);
                    }
                    // Placing top left inner
                    if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                    {
                        Vector3Int topTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(topTilePosition))
                        {
                            wallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerLeft);
                        }
                    }
                    // Placing top right inner
                    else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                    {
                        Vector3Int topTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(topTilePosition))
                        {
                            wallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().TopInnerRight);
                        }
                    }
                    // Placing top
                    else
                    {
                        Vector3Int topTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(topTilePosition))
                        {
                            wallLayer.SetTile(topTilePosition, _wallVariants.GetRandomElement().Top);
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
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().BottomInnerLeft);
                    }
                }
                // Placing bottom right inner
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().BottomInnerRight);
                    }
                }
                // Placing bottom
                else
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().Bottom);
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
                            wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopLeft);
                        }
                    }
                    // Placing right
                    else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                    {
                        Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                        if (!wallLayer.HasTile(bottomTilePosition))
                        {
                            wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopRight);
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
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopOuterLeft);
                    }
                }
                // Placing left 
                else if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopLeft);
                    }
                }
                // Placing right outer
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Wall)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopOuterRight);
                    }
                }
                // Placing right 
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopRight);
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
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().BottomOuterLeft);
                    }
                }
                // Placing left 
                else if (x + 1 < maximumSize && _roomCells[x + 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopLeft);
                    }
                }
                // Placing right outer
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Wall)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().BottomOuterRight);
                    }
                }
                // Placing right 
                else if (x - 1 >= 0 && _roomCells[x - 1, y] == RoomCellData.Floor)
                {
                    Vector3Int bottomTilePosition = new Vector3Int(roomPosition.x + x, roomPosition.y + y + 1, 0);
                    if (!wallLayer.HasTile(bottomTilePosition))
                    {
                        wallLayer.SetTile(bottomTilePosition, _wallVariants.GetRandomElement().TopRight);
                    }
                }
            }
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
