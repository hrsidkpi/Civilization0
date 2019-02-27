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

        public static bool CanPlaceOn(this UnitType unit, int x, int y)
        {
            Tile t = Game.instance.tiles[x, y];
            if (!unit.CanBeOn(t.type)) return false;
            if ((unit.IsBuilding() && t.buildingOn != null) || (unit.IsHuman() && t.unitOn != null)) return false;
            return true;
        }

        public static bool CanPlaceOn(this UnitType unit, int x, int y, bool player)
        {
            Tile t = Game.instance.tiles[x, y];
            if (!unit.CanBeOn(t.type)) return false;
            if ((unit.IsBuilding() && t.buildingOn != null) || (unit.IsHuman() && t.unitOn != null)) return false;
            if (t.buildingOn != null && t.buildingOn.player != player) return false;
            return true;
        }

        public static bool CanPlaceOn(this Unit unit, int x, int y)
        {
            return CanPlaceOn(unit.type, x, y, unit.player);
        }



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
                    Tile t = Game.instance.tiles[x, y];

                    if (type.CanPlaceOn(x, y))
                        moves.Add(new BuildMove(unit, x, y, type));

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
                    Tile t = Game.instance.tiles[x, y];

                    foreach (UnitType type in unit.GetBuildable())
                        if (type.CanPlaceOn(x, y))
                            moves.Add(new BuildMove(unit, x, y, type));

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
                    if (unit.CanPlaceOn(x, y))
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

        public static List<Move> AttackAroundMove(this Unit unit, int distance)
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
                    if (t.UnitsOn.Count == 0) continue;
                    Unit target = t.UnitsOn[0];
                    if (target.player == unit.player) continue;
                    moves.Add(new AttackMove(unit, target));
                }
            }
            return moves;
        }

        public static List<Move> DefaultAttackAroundMove(this Unit unit)
        {
            return AttackAroundMove(unit, 1);
        }

        public static List<Move> ShootAroundMove(this Unit unit, int distance)
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
                    if (t.unitOn == null) continue;
                    Unit target = t.unitOn;
                    if (target.player == unit.player) continue;
                    moves.Add(new ShootMove(unit, target));
                }
            }
            return moves;
        }

        public static List<Move> DefaultShootAroundMove(this Unit unit)
        {
            return ShootAroundMove(unit, unit.type.GetRange());
        }

    }
}
