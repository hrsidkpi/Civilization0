using Civilization0.moves;
using Civilization0.tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units
{

    /// <summary>
    /// Static class for generating common lists of moves, like movement in range moves.
    /// </summary>
    public static class BasicUnitMoves
    {


        /// <summary>
        /// Return build moves around the unit of the selected type and in a specified maximum distance
        /// </summary>
        /// <param name="unit">The unit that will build</param>
        /// <param name="board">The board to build on</param>
        /// <param name="type">The type of unit to build</param>
        /// <param name="distance">The maximum distance to build in</param>
        /// <returns>A list of movement moves that unit can do, of units of type 'type' and in distance 'distance'</returns>
        public static List<Move> BuildAroundMove(this Unit unit, Tile[,] board, UnitType type, int distance)
        {
            List<Move> moves = new List<Move>();

            //Get the 9 tiles around this building
            int xStart = Math.Max(0, unit.px / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.px / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.py / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.py / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {

                    if (type.CanPlaceOn(board, x, y))
                        moves.Add(new BuildMove(unit, x, y, type));

                }
            }

            return moves;
        }

        /// <summary>
        /// Return build moves around the unit of all possible types and in a specified maximum distance
        /// </summary>
        /// <param name="unit">The unit that will build</param>
        /// <param name="board">The board to build on</param>
        /// <param name="type">The type of unit to build</param>
        /// <param name="distance">The maximum distance to build in</param>
        /// <returns>A list of movement moves that unit can do, of all types the unit can build, and in distance 'distance'</returns>
        public static List<Move> BuildAroundMoveAll(this Unit unit, Tile[,] board, int distance)
        {
            List<Move> moves = new List<Move>();

            //Get the 9 tiles around this building
            int xStart = Math.Max(0, unit.px / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.px / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.py / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.py / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {

                    foreach (UnitType type in unit.GetBuildable())
                        if (type.CanPlaceOn(board, x, y))
                            moves.Add(new BuildMove(unit, x, y, type));

                }
            }

            return moves;
        }

        /// <summary>
        /// Get movement moves around a unit in a specified distance.
        /// </summary>
        /// <param name="unit">The moving unit</param>
        /// <param name="board">The board to creaate moves on</param>
        /// <param name="distance">The maximum distance for moving</param>
        /// <returns></returns>
        public static List<Move> MoveAroundMove(this Unit unit, Tile[,] board, int distance)
        {
            List<Move> moves = new List<Move>();

            int xStart = Math.Max(0, unit.px / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.px / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.py / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.py / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    if (x == unit.px / Tile.TILE_WIDTH && y == unit.py / Tile.TILE_HEIGHT) continue;
                    Tile t = Game.instance.tiles[x, y];
                    if (unit.CanPlaceOn(board, x, y))
                    {
                        moves.Add(new MovementMove(unit, x, y));
                    }
                }
            }
            return moves;
        }

        /// <summary>
        /// Get the default movement moves for a unit on a board
        /// </summary>
        /// <param name="unit">The unit to generate moves for</param>
        /// <param name="board">The board to generate moves on</param>
        /// <returns>A list of movement moves around the unit with maximum distance based on the unit's max moves.</returns>
        public static List<Move> DefaultMoveAroundMove(this Unit unit, Tile[,] board)
        {
            return MoveAroundMove(unit, board, unit.type.GetMaxMoves());
        }

        /// <summary>
        /// Get attack moves on all enemies around a unit in a specified maximum distance on a board
        /// </summary>
        /// <param name="unit">The unit that will attack</param>
        /// <param name="board">The board to generate moves on</param>
        /// <param name="distance">The maximum distance an enemy can be in from the unit to generate attack move on him</param>
        /// <returns>A list of attack moves on enemies around the unit in a specified maximum distance.</returns>
        public static List<Move> AttackAroundMove(this Unit unit, Tile[,] board, int distance)
        {
            List<Move> moves = new List<Move>();

            int xStart = Math.Max(0, unit.px / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.px / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.py / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.py / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    if (x == unit.px / Tile.TILE_WIDTH && y == unit.py / Tile.TILE_HEIGHT) continue;
                    Tile t = board[x, y];
                    if (t.UnitsOn.Count == 0) continue;
                    Unit target = t.UnitsOn[0];
                    if (target.player == unit.player) continue;
                    moves.Add(new AttackMove(unit, target));
                }
            }
            return moves;
        }

        /// <summary>
        /// Get the default attack moves for a unit on a board
        /// </summary>
        /// <param name="unit">The unit to generate moves for</param>
        /// <param name="board">The board to generate moves on</param>
        /// <returns>A list of attack moves around the unit with melee attack range of 1</returns>
        public static List<Move> DefaultAttackAroundMove(this Unit unit, Tile[,] board)
        {
            return unit.AttackAroundMove(board, 1);
        }

        /// <summary>
        /// Get shoot moves on all enemies around a unit in a specified maximum distance on a board
        /// </summary>
        /// <param name="unit">The unit that will attack</param>
        /// <param name="board">The board to generate moves on</param>
        /// <param name="distance">The maximum distance an enemy can be in from the unit to generate shoot move on him</param>
        /// <returns>A list of shoot moves on enemies around the unit in a specified maximum distance.</returns>
        public static List<Move> ShootAroundMove(this Unit unit, Tile[,] board, int distance)
        {
            List<Move> moves = new List<Move>();

            int xStart = Math.Max(0, unit.px / Tile.TILE_WIDTH - distance);
            int xEnd = Math.Min(Game.TILES_WIDTH - 1, unit.px / Tile.TILE_WIDTH + distance);
            int yStart = Math.Max(0, unit.py / Tile.TILE_HEIGHT - distance);
            int yEnd = Math.Min(Game.TILES_HEIGHT - 1, unit.py / Tile.TILE_HEIGHT + distance);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    if (x == unit.TileX && y == unit.TileY) continue;
                    Tile t = board[x, y];
                    if (t.unitOn != null)
                    {
                        Unit target = t.unitOn;
                        if (target.player == unit.player) continue;
                        moves.Add(new ShootMove(unit, target));
                    }
                    else if(t.buildingOn != null)
                    {
                        Unit target = t.buildingOn;
                        if (target.player == unit.player) continue;
                        moves.Add(new ShootMove(unit, target));
                    }
                }
            }
            return moves;
        }

        /// <summary>
        /// Get the default shoot moves for a unit on a board
        /// </summary>
        /// <param name="unit">The unit to generate moves for</param>
        /// <param name="board">The board to generate moves on</param>
        /// <returns>A list of shoot moves around the unit in maximum distance based on the unit's range</returns>
        public static List<Move> DefaultShootAroundMove(this Unit unit, Tile[,] board)
        {
            return unit.ShootAroundMove(board, unit.type.GetRange());
        }

    }
}
