using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{
    public static class AIUtil
    {

        public static Tuple<int, int> NearestTileOfType(int xStart, int yStart, TileType tile)
        {
            int dist = 0;
            while(true)
            {
                //top and bottom rows
                for(int x = Math.Max(0,xStart - dist); x < Math.Min(xStart + dist, Game.instance.tiles.GetLength(0)); x++)
                {
                    if (yStart - dist >= 0)
                        if (Game.instance.tiles[x, yStart - dist].type == tile) return new Tuple<int, int>(x, yStart - dist);
                    
                    if (yStart + dist < Game.instance.tiles.GetLength(0))
                        if(Game.instance.tiles[x, yStart + dist].type == tile) return new Tuple<int, int>(x, yStart + dist);
                }
                //left and right columns
                for (int y = Math.Max(0, yStart - dist); y < Math.Min(yStart + dist, Game.instance.tiles.GetLength(0)); y++)
                {
                    if (xStart - dist >= 0)
                        if (Game.instance.tiles[xStart - dist, y].type == tile) return new Tuple<int, int>(xStart - dist, y);

                    if (xStart + dist < Game.instance.tiles.GetLength(0))
                        if (Game.instance.tiles[xStart + dist, y].type == tile) return new Tuple<int, int>(xStart + dist, y);
                }
                dist++;
            }
        }

        public static Dictionary<UnitType,int> Add(this Dictionary<UnitType,int> a, Dictionary<UnitType,int> b)
        {
            foreach(KeyValuePair<UnitType, int> p in b)
            {
                if (a.ContainsKey(p.Key)) a[p.Key] += p.Value;
                else a.Add(p.Key, p.Value);
            }
            return a;
        }

        public static Dictionary<UnitType,int> GetMissingUnits(this Dictionary<UnitType,int> container, Dictionary<UnitType,int> contained)
        {
            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();

            foreach(KeyValuePair<UnitType,int> pair in contained)
            {
                if (!container.ContainsKey(pair.Key)) res.Add(pair.Key, pair.Value);
                else if (container[pair.Key] < pair.Value) res[pair.Key] = pair.Value - container[pair.Key];
            }
            return res;
        }

        public static bool NoUnits(this Dictionary<UnitType, int> dict)
        {
            foreach (int v in dict.Values) if (v != 0) return false;
            return true;
        }

        public static Dictionary<UnitType, int> ListToDict(List<UnitType> units)
        {
            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();
            foreach (UnitType t in units)
                if (res.ContainsKey(t)) res[t]++;
                else res[t] = 1;
            return res;
        }

        public static List<Unit> UnitsFromBoard(Tile[,] board)
        {
            List<Unit> res = new List<Unit>();
            foreach (Tile t in board) foreach (Unit u in t.UnitsOn) res.Add(u);
            return res;
        }


        public static Dictionary<UnitType, int> Count(Tile[,] board, LookupConstraint constraint)
        {
            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();

            foreach (Tile t in board)
                foreach (Unit u in t.UnitsOn)
                {
                    if (constraint.Check(u))
                    {
                        if (!res.ContainsKey(u.type)) res[u.type] = 0;
                        res[u.type]++;
                    }
                }
            return res;

        }

        public static Dictionary<UnitType, int> CountAround(Unit counter, Tile[,] board, int range, bool player)
        {

            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();

            ALocation start = new ALocation(counter.TileX, counter.TileY);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < range; i++)
            {
                foreach (ALocation test in current)
                {

                    if (test.Tile.unitOn != null && test.Tile.unitOn.player == player)
                    {
                        if (!res.ContainsKey(test.Tile.unitOn.type)) res[test.Tile.unitOn.type] = 0;
                        res[test.Tile.unitOn.type]++;
                    }

                    if (test.Tile.buildingOn != null && test.Tile.buildingOn.player == player)
                    {
                        if (!res.ContainsKey(test.Tile.buildingOn.type)) res[test.Tile.buildingOn.type] = 0;
                        res[test.Tile.buildingOn.type]++;
                    }

                    foreach (ALocation l in PathFinder.GetAdjacentSquares(counter.type, board, test.x, test.y))
                    {
                        if (!l.Tile.flag)
                        {
                            l.parent = test;
                            next.Push(l);
                            l.Tile.flag = true;
                        }
                    }
                }
                current = next;
                next = new Stack<ALocation>();
            }

            foreach (Tile t in Game.instance.tiles) t.flag = false;

            return res;
        }

    }
}
