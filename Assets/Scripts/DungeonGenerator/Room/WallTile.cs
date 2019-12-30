using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New Wall Tile", menuName = "Tiles/Wall Tile")]
    public class WallTile : RuleTile
    {
        public override bool RuleMatch(int neighbor, TileBase other)
        {
            if (other is RuleOverrideTile)
                other = (other as RuleOverrideTile).m_InstanceTile;

            switch (neighbor)
            {
                case TilingRule.Neighbor.This:
                    {
                        return other is WallTile;
                    }
                case TilingRule.Neighbor.NotThis:
                    {
                        return !(other is WallTile);
                    }
            }

            return base.RuleMatch(neighbor, other);
        }
    }
}