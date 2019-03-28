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
    public static class Minmax
    {

        public const bool DEBUG_CONTROLS = false;

        public static Move move(this Tuple<Move,int> t)
        {
            return t.Item1;
        }
        public static int control(this Tuple<Move, int> t)
        {
            return t.Item2;
        }


        public static Move GetBestMoveMin(Unit u, List<Move> pending)
        {
            return GetBestMoveMin(u, Game.instance.tiles, pending, 3).move();
        }

        public static Tuple<Move,int> GetBestMoveMin(Unit u, Tile[,] board, List<Move> pending, int level)
        {
            Tile[,] clone;

            int minControl = 1000;
            Move best = null;

            foreach (Move m in u.GetMoves(board))
            {
                clone = CopyBoard(board);
                foreach (Move p in pending) p.Execute(false, clone);
                Unit cloneUnit = clone[u.TileX, u.TileY].UnitsOn[0];

                if (cloneUnit.type.GetMaxMoves() < m.CostBoard(clone)) continue;
                m.Execute(false, clone);

                int control = 0;
                if (level == 1) control = CalculateMapControl(clone);
                else control = GetBestMoveMin(cloneUnit, clone, new List<Move>(), level - 1).control();
                if (control < minControl)
                {
                    minControl = control;
                    best = m;
                }
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
            return new Tuple<Move, int>(best, minControl);
        }

        public static Tile[,] CopyBoard(Tile[,] board)
        {
            Tile[,] clone = new Tile[Game.TILES_WIDTH, Game.TILES_HEIGHT];
            for (int x = 0; x < Game.TILES_WIDTH; x++)
            {
                for (int y = 0; y < Game.TILES_HEIGHT; y++)
                {
                    Tile t = board[x, y];
                    clone[x, y] = new Tile(t.type, t.x, t.y);
                }
            }
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

        public static int CalculateMapControl(Tile[,] tiles)
        {

            if (DEBUG_CONTROLS) Game.instance.panels.Clear();

            int[,] control = new int[tiles.GetLength(0), tiles.GetLength(1)];
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
