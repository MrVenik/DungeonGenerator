using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGenerator
{
    public class TileHider : MonoBehaviour
    {
        private Tilemap _tilemap;

        public Transform Transform { get; private set; }


        private List<Vector3Int> _hidedTiles;


        private void Awake()
        {
            _tilemap = DungeonManager.Dungeon.TilemapData.WallLayer;
            Transform = transform;
            _hidedTiles = new List<Vector3Int>();
        }

        private void Update()
        {
            if (_tilemap != null)
            {
                HideTiles();
                ShowTiles();
            }
        }

        private void HideTiles()
        {
            for (int iy = -6; iy <= 0; iy++)
            {
                for (int ix = -1; ix >= -6; ix--)
                {
                    Vector3Int tilePos = new Vector3Int((int)Transform.position.x + ix, (int)Transform.position.y + iy, (int)Transform.position.z);
                    if (_tilemap.HasTile(tilePos))
                    {
                        if (CanHide(tilePos))
                        {
                            if (!_hidedTiles.Contains(tilePos))
                            {
                                HideTile(tilePos);
                                _hidedTiles.Add(tilePos);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }                  
                }
                for (int ix = 0; ix <= 6; ix++)
                {
                    Vector3Int tilePos = new Vector3Int((int)Transform.position.x + ix, (int)Transform.position.y + iy, (int)Transform.position.z);
                    if (_tilemap.HasTile(tilePos))
                    {
                        if (CanHide(tilePos))
                        {
                            if (!_hidedTiles.Contains(tilePos))
                            {
                                HideTile(tilePos);
                                _hidedTiles.Add(tilePos);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }                  
                }
            }
        }

        private bool CanHide(Vector3Int tilePos)
        {

                if (_tilemap.GetTile(tilePos) is WallTile)
                {
                    for (int iy = 1; iy <= 3; iy++)
                    {
                        Vector3Int topTile = new Vector3Int(tilePos.x, tilePos.y + iy, tilePos.z);
                        if (!_tilemap.HasTile(topTile))
                        {
                            return true;
                        }
                        else if (_tilemap.GetTile(topTile) != _tilemap.GetTile(tilePos))
                        {
                            return true;
                        }
                    }
                }
            
            return false;
        }

        private void ShowTiles()
        {
            List<Vector3Int> toShow = new List<Vector3Int>();
            Vector3Int currentPosition = new Vector3Int((int)Transform.position.x, (int)Transform.position.y, (int)Transform.position.z);
            foreach (var tilePos in _hidedTiles)
            {
                float distance = Vector3Int.Distance(currentPosition, tilePos);
                if (distance > 6)
                {
                    ShowTile(tilePos);
                    toShow.Add(tilePos);
                }
            }

            foreach (var tilePos in toShow)
            {
                _hidedTiles.Remove(tilePos);
            }
        }

        private void HideTile(Vector3Int tilePos)
        {
            Color color = _tilemap.GetColor(tilePos);
            color.a = 0.25f;
            _tilemap.SetColor(tilePos, color);
            _tilemap.SetTileFlags(tilePos, TileFlags.None);
        }

        private void ShowTile(Vector3Int tilePos)
        {
            if (_tilemap.HasTile(tilePos))
            {
                Color color = _tilemap.GetColor(tilePos);
                color.a = 1.0f;
                _tilemap.SetTileFlags(tilePos, TileFlags.None);
                _tilemap.SetColor(tilePos, color);
            }
        }
    }
}
