using Civilization0.gui;
using Civilization0.moves;
using Civilization0.tiles;
using Civilization0.units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{
    public static class DepthSearch
    {
        //Debug setting. Used to debug the process of calculating map control of a board.
        public const bool DEBUG_CONTROLS = false;

        //Get the move property from a Tuple<Move,int>
        public static Move Move(this Tuple<Move,int> t)
        {
            return t.Item1;
        }

        //Get the control (the int) property from a Tuple<Move,int>
        public static int Control(this Tuple<Move, int> t)
        {
            return t.Item2;
        }

        /// <summary>
        /// Utility method to call GetBestMoveMin(Unit, board, level) and get the best move
        /// </summary>
        /// <param name="u">The unit to move</param>
        /// <param name="board">The board to move on</param>
        /// <returns>The best move to execute for unit u on board</returns>
        public static Move GetBestMoveMin(Unit u, Tile[,] board)
        {
            return GetBestMoveMin(u, board, 3).Move();
        }

        /// <summary>
        /// Find the best move for a unit to execute on a board, using Depth first search, forcasting a few moves ahead.
        /// </summary>
        /// <param name="u">The unit to move</param>
        /// <param name="board">The board for the unit to move on</param>
        /// <param name="level">The amount of levels the algorithm still needs to go through</param>
        /// <returns>The best move for unit u and the amount of map control this move will yield eventually (using DFS)</returns>
        public static Tuple<Move,int> GetBestMoveMin(Unit u, Tile[,] board, int level)
        {
            Tile[,] clone;

            int minControl = 1000;
            Move best = null;

            //Get all moves possible and check each one
            foreach (Move m in u.GetMoves(board))
            {
                //Clone the boarad
                clone = CopyBoard(board);
                //Find the cloned unit
                Unit cloneUnit = clone[u.TileX, u.TileY].UnitsOn[0];

                //Check that the move can actually be executed. If it can, execute it on the clone.
                if (cloneUnit.type.GetMaxMoves() < m.CostBoard(clone)) continue;
                m.Execute(false, clone);


                int control;

                //If this is the final search level, find the map control of the current state.
                if (level == 1) control = CalculateMapControl(clone);
                //Otherwise, get the best move after this one and the control will be the result from there.
                else control = GetBestMoveMin(cloneUnit, clone, level - 1).Control();

                //If the control of this move is better than the best one found so far, switch them.
                if (control < minControl)
                {
                    minControl = control;
                    best = m;
                }

                //If the current move is equal control-wise to the best so far, prioritize attack moves, or movement moves to places
                //Closer to the enemy base.
                else if (control == minControl)
                {
                    if (m is AttackMove)
                    {
                        minControl = control;
                        best = m;
                    }
                    else if (best is MovementMove && m is MovementMove)
                    {
                        MovementMove bb = best as MovementMove;
                        MovementMove mm = m as MovementMove;

                        if (mm.x + mm.y < bb.x + bb.y)
                        {
                            minControl = control;
                            best = m;
                        }
                    }
                }
            }
            //Return the best move found and the control it yields.
            return new Tuple<Move, int>(best, minControl);
        }

        /// <summary>
        /// Copy a board and all the units on it.
        /// </summary>
        /// <param name="board">The board to copy</param>
        /// <returns>A deep copy of the board- all tiles will be copied, and all units will be copied and placed on the clone</returns>
        public static Tile[,] CopyBoard(Tile[,] board)
        {
            Tile[,] clone = new Tile[Game.TILES_WIDTH, Game.TILES_HEIGHT];
            //Copy tiles
            for (int x = 0; x < Game.TILES_WIDTH; x++)
            {
                for (int y = 0; y < Game.TILES_HEIGHT; y++)
                {
                    Tile t = board[x, y];
                    clone[x, y] = new Tile(t.type, t.x, t.y);
                }
            }
            //Copy units
            for (int x = 0; x < Game.TILES_WIDTH; x++)
            {
                for (int y = 0; y < Game.TILES_HEIGHT; y++)
                {
                    Tile t = board[x, y];
                    if (t.unitOn != null)
                        clone[x, y].unitOn = t.unitOn.type.BuildOnTile(t.unitOn.TileX, t.unitOn.TileY, clone, t.unitOn.player);
                    if (t.buildingOn != null)
                        clone[x, y].buildingOn = t.buildingOn.type.BuildOnTile(t.buildingOn.TileX, t.buildingOn.TileY, clone, t.buildingOn.player);
                }
            }
            return clone;
        }

        /// <summary>
        /// Calculate the map control value of a board. High map control is good for the human player, and low map control is good for
        /// the computer player.
        /// </summary>
        /// <param name="tiles">The board to check</param>
        /// <returns>The amount of tiles the player controls (has more, stronger units nearby) minus the amount of tiles the 
        /// computer player controls. </returns>
        public static int CalculateMapControl(Tile[,] tiles)
        {

            if (DEBUG_CONTROLS) Game.instance.panels.Clear();

            int[,] control = new int[tiles.GetLength(0), tiles.GetLength(1)];

            //For each unit, mark the tiles around it as affected by its presence. 
            foreach (Tile t in tiles) foreach (Unit u in t.UnitsOn)
                {
                    control[u.TileX, u.TileY] += u.hp * (u.player ? 1 : -1);
                    if (DEBUG_CONTROLS) Console.WriteLine(u + " is controlling tile at " + u.TileX + ", " + u.TileY + ". Self location.");

                    int curr = u.hp + 1;
                    int radius = u.type.GetMaxMoves() + u.type.GetRange() - 1;
                    int drop = u.hp / radius;
                    for (int i = 1; i <= radius; i++)
                    {
                        curr -= drop;
                        for (int j = -i; j <= i; j++)
                        {
                            int x = u.TileX - j;
                            int y1 = u.TileY - Math.Abs(j) + i;
                            int y2 = u.TileY + Math.Abs(j) - i;
                            if (!(x >= 0 && x < Game.TILES_WIDTH)) continue;

                            if (y1 >= 0 && y1 < Game.TILES_HEIGHT)
                            {
                                control[x, y1] += curr * (u.player ? 1 : -1);
                                if (DEBUG_CONTROLS) Console.WriteLine(u + " is controlling tile at " + x + ", " + y1 + ". r: " + radius + " i: " + i + " j: " + j + " threat: " + curr);
                            }
                            if (y1 != y2 && y2 >= 0 && y2 < Game.TILES_HEIGHT)
                            {
                                control[x, y2] += curr * (u.player ? 1 : -1);
                                if (DEBUG_CONTROLS) Console.WriteLine(u + " is controlling tile at " + x + ", " + y2 + ". r: " + radius + " i: " + i + " j: " + j + " threat: " + curr);
                            }
                        }
                    }
                }

            //For each tile, if there is more human player presence than computer player, increase map control by 1.
            //If the computer player has more presence, decrease the map control by 1. Otherwise don't change it.
            int MapControl = 0;
            for (int x = 0; x < Game.TILES_WIDTH; x++)
                for (int y = 0; y < Game.TILES_HEIGHT; y++)
                {
                    int i = control[x, y];
                    if (i > 0)
                    {
                        MapControl++;
                        if (DEBUG_CONTROLS) new Panel(new Rectangle(x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT, 50, 50), Assets.myTurn);
                    }
                    else if (i < 0)
                    {
                        MapControl--;
                        if (DEBUG_CONTROLS) new Panel(new Rectangle(x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT, 50, 50), Assets.enemyTurn);
                    }
                    else
                    {
                        if (DEBUG_CONTROLS) new Panel(new Rectangle(x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT, 50, 50), Assets.wood);
                    }
                }
            return MapControl;
        }

    }
}
