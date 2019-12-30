using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGenerator
{
    public enum RoomCellData
    {
        None,
        Wall,
        Floor
    }

    [CreateAssetMenu(fileName = "New RoomBuilder", menuName = "Rooms/Room Builders/Procedural Room Builder")]
    public class ProceduralRoomBuilder : RoomBuilder
    {
        [SerializeField] private List<TileBase> _wallVariants;
        [SerializeField] private List<TileBase> _floorVariants;
        [SerializeField] private GameObject _shadow;

        private TileBase _roomWall;

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

            _roomWall = _wallVariants.GetRandomElement();

            PlaceCells(roomData);

            for (int x = 0; x < maximumSize; x++)
            {
                for (int y = 0; y < maximumSize; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, 0);

                    if (_roomCells[x, y] == RoomCellData.Wall)
                    {
                        if (!tilemapData.WallLayer.HasTile(tilePosition))
                        {
                            tilemapData.WallLayer.SetTile(tilePosition, _roomWall);
                        }
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
