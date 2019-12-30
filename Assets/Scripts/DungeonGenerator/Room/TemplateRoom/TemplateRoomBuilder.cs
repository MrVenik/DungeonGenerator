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
        [SerializeField] private List<TileBase> _wallVariants;
        [SerializeField] private List<TileBase> _floorVariants;

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

        private TileBase _roomWall;

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
    }
}
