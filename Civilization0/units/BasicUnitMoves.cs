using Civilization0.moves;
using Civilization0.tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units
{
    public static class BasicUnitMoves
    {

        public static List<Move> BuildAroundMove(this Unit unit, UnitType type, int distance)
        {
            List<Move> moves = new List<Move>();

            //Get the 9 tiles around this building
            int xStart = Math.Max(0, unit.x / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.x / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.y / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.y / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    if (x == unit.x / Tile.TILE_WIDTH && y == unit.y / Tile.TILE_HEIGHT) continue;
                    Tile t = Game.instance.tiles[x, y];
                    if (t.unitsOn.Count == 0)
                    {
                        moves.Add(new BuildMove(unit, x, y, type));
                    }
                }
            }

            return moves;
        }

        public static List<Move> BuildAroundMoveAll(this Unit unit, int distance)
        {
            List<Move> moves = new List<Move>();

            //Get the 9 tiles around this building
            int xStart = Math.Max(0, unit.x / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.x / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.y / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.y / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    if (x == unit.x / Tile.TILE_WIDTH && y == unit.y / Tile.TILE_HEIGHT) continue;
                    Tile t = Game.instance.tiles[x, y];
                    if (t.unitsOn.Count == 0)
                    {
                        foreach(UnitType type in unit.GetBuildable())
                            moves.Add(new BuildMove(unit, x, y, type));
                    }
                }
            }

            return moves;
        }
        public static List<Move> MoveAroundMove(this Unit unit, int distance)
        {
            List<Move> moves = new List<Move>();

            int xStart = Math.Max(0, unit.x / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.x / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.y / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.y / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    if (x == unit.x / Tile.TILE_WIDTH && y == unit.y / Tile.TILE_HEIGHT) continue;
                    Tile t = Game.instance.tiles[x, y];
                    if (t.unitsOn.Count == 0)
                    {
                        moves.Add(new MovementMove(unit, x, y));
                    }
                }
            }
            return moves;
        }

        public static List<Move> DefaultMoveAroundMove(this Unit unit)
        {
            return MoveAroundMove(unit, unit.type.GetMaxMoves());
        }

    }
}
