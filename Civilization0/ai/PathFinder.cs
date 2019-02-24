using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{

    public class ALocation
    {

        public ALocation() { }

        public ALocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x, y;
        public int f, g, h;

        public ALocation parent;

        public Tile Tile
        {
            get
            {
                return Game.instance.tiles[x, y];
            }
        }

        public override string ToString()
        {
            return "ALocation (x: " + x + ", y: " + y + ")";
        }
    }

    public static class PathFinder
    {

        public const int MAX_PATH_LENGTH = 15;

        public static int FindHScore(int x, int y, int xTarget, int yTarget)
        {
            return Math.Abs(xTarget - x) + Math.Abs(yTarget - y);
        }

        public static Dictionary<UnitType, int> CountAround(Unit counter, int range, bool player)
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
                    foreach (Unit u in test.Tile.unitsOn)
                    {
                        if (u.player != player) continue;
                        if (!res.ContainsKey(u.type)) res[u.type] = 0;
                        res[u.type]++;
                    }
                    foreach (ALocation l in GetAdjacentSquares(test.x, test.y))
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

        public static List<ALocation> PathToNearestTile(UnitType traveler, int xStart, int yStart, TileType tile)
        {
            ALocation start = new ALocation(xStart, yStart);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                foreach (ALocation test in current)
                {
                    if (Game.instance.tiles[test.x, test.y].type == tile && Game.instance.tiles[test.x, test.y].unitsOn.Count == 0)
                    {
                        List<ALocation> res = new List<ALocation>();
                        ALocation curr = test;
                        while (curr != null)
                        {
                            res.Add(curr);
                            curr = curr.parent;
                        }
                        foreach (Tile t in Game.instance.tiles) t.flag = false;
                        res.Reverse();
                        res.RemoveAt(0);
                        return res;
                    }
                    if (!traveler.CanPlaceOn(test.x, test.y) && test != start)
                        continue;

                    foreach (ALocation l in GetAdjacentSquares(test.x, test.y))
                        if (!Game.instance.tiles[l.x, l.y].flag)
                        {
                            l.parent = test;
                            next.Push(l);
                            Game.instance.tiles[l.x, l.y].flag = true;
                        }
                }
                current = next;
                next = new Stack<ALocation>();
            }
            foreach (Tile t in Game.instance.tiles) t.flag = false;

            return null;
        }

        public static List<ALocation> PathToNearestUnit(UnitType traveler, int xStart, int yStart, bool player)
        {
            ALocation start = new ALocation(xStart, yStart);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                foreach (ALocation test in current)
                {
                    Console.WriteLine(test);
                    foreach (Unit u in test.Tile.unitsOn)
                        if (u.player == player)
                        {
                            List<ALocation> res = new List<ALocation>();
                            ALocation curr = test;
                            while (curr != null)
                            {
                                res.Add(curr);
                                curr = curr.parent;
                            }
                            foreach (Tile t in Game.instance.tiles) t.flag = false;
                            res.Reverse();
                            res.RemoveAt(0);
                            return res;
                        }

                    if (!traveler.CanPlaceOn(test.x, test.y) && test != start)
                        continue;

                    foreach (ALocation l in GetAdjacentSquares(test.x, test.y))
                        if (!Game.instance.tiles[l.x, l.y].flag)
                        {
                            l.parent = test;
                            next.Push(l);
                            Game.instance.tiles[l.x, l.y].flag = true;
                        }
                }
                current = next;
                next = new Stack<ALocation>();
            }
            foreach (Tile t in Game.instance.tiles) t.flag = false;

            return null;
        }

        public static List<ALocation> PathToNearestUnit(UnitType traveler, int xStart, int yStart, UnitType unit, bool player)
        {
            ALocation start = new ALocation(xStart, yStart);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                foreach (ALocation test in current)
                {
                    Console.WriteLine(test);
                    foreach (Unit u in test.Tile.unitsOn)
                        if (u.player == player && u.type == unit)
                        {
                            List<ALocation> res = new List<ALocation>();
                            ALocation curr = test;
                            while (curr != null)
                            {
                                res.Add(curr);
                                curr = curr.parent;
                            }
                            foreach (Tile t in Game.instance.tiles) t.flag = false;
                            res.Reverse();
                            res.RemoveAt(0);
                            return res;
                        }

                    if (!traveler.CanPlaceOn(test.x, test.y) && test != start)
                        continue;

                    foreach (ALocation l in GetAdjacentSquares(test.x, test.y))
                        if (!Game.instance.tiles[l.x, l.y].flag)
                        {
                            l.parent = test;
                            next.Push(l);
                            Game.instance.tiles[l.x, l.y].flag = true;
                        }
                }
                current = next;
                next = new Stack<ALocation>();
            }
            foreach (Tile t in Game.instance.tiles) t.flag = false;

            return null;
        }

        public static List<ALocation> PathToNearestUnit(UnitType traveler, int xStart, int yStart, UnitType search)
        {
            ALocation start = new ALocation(xStart, yStart);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                foreach (ALocation test in current)
                {
                    foreach (Unit u in test.Tile.unitsOn)
                        if (u.type == search)
                        {
                            List<ALocation> res = new List<ALocation>();
                            ALocation curr = test;
                            while (curr != null)
                            {
                                res.Add(curr);
                                curr = curr.parent;
                            }
                            foreach (Tile t in Game.instance.tiles) t.flag = false;
                            res.Reverse();
                            res.RemoveAt(0);
                            return res;
                        }
                    if (!traveler.CanPlaceOn(test.x, test.y) && test != start)
                        continue;

                    foreach (ALocation l in GetAdjacentSquares(test.x, test.y))
                        if (!Game.instance.tiles[l.x, l.y].flag)
                        {
                            l.parent = test;
                            next.Push(l);
                            Game.instance.tiles[l.x, l.y].flag = true;
                        }
                }
                current = next;
                next = new Stack<ALocation>();
            }
            return null;
        }

        public static List<ALocation> GetWalkableAdjacentSquares(int x, int y)
        {
            List<ALocation> proposedLocations = new List<ALocation>()
                {
                    new ALocation { x = x, y = y - 1 },
                    new ALocation { x = x, y = y + 1 },
                    new ALocation { x = x - 1, y = y },
                    new ALocation { x = x + 1, y = y },
                };

            return proposedLocations.Where(
                l => l.x < Game.TILES_WIDTH && l.y < Game.TILES_HEIGHT && l.x >= 0 && l.y >= 0 && Game.instance.tiles[l.x, l.y].type == tiles.TileType.grass).ToList();
        }

        public static List<ALocation> GetAdjacentSquares(int x, int y)
        {
            List<ALocation> proposedLocations = new List<ALocation>()
                {
                    new ALocation { x = x, y = y - 1 },
                    new ALocation { x = x, y = y + 1 },
                    new ALocation { x = x - 1, y = y },
                    new ALocation { x = x + 1, y = y },
                };

            return proposedLocations.Where(
                l => l.x < Game.TILES_WIDTH && l.y < Game.TILES_HEIGHT && l.x >= 0 && l.y >= 0).ToList();
        }

    }
}
