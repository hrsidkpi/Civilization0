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

        public const int MAX_PATH_LENGTH = 25;

        public static int FindHScore(int x, int y, int xTarget, int yTarget)
        {
            return Math.Abs(xTarget - x) + Math.Abs(yTarget - y);
        }


        public static Dictionary<UnitType, int> Count(LookupConstraint constraint)
        {
            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();

            foreach (Tile t in Game.instance.tiles)
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

                    foreach (ALocation l in GetAdjacentSquares(counter.type, test.x, test.y))
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

        public static List<ALocation> PathToNearestTile(UnitType traveler, int xStart, int yStart, TileLookupConstraint constraint)
        {
            if (traveler == UnitType.swordman)
                traveler = UnitType.swordman;
            ALocation start = new ALocation(xStart, yStart);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                foreach (ALocation test in current)
                {
                    if (constraint.Check(test.Tile))
                    {
                        Console.WriteLine("Found tile satisfying " + constraint + " in " + test.x + ", " + test.y);
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

                    foreach (ALocation l in GetAdjacentSquares(traveler, test.x, test.y))
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
                    if (Game.instance.tiles[test.x, test.y].type == tile && test.Tile.buildingOn == null)
                    {
                        Console.WriteLine("Found " + tile + " in " + test.x + ", " + test.y);
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

                    foreach (ALocation l in GetAdjacentSquares(traveler, test.x, test.y))
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

        public static List<ALocation> PathToCoordinates(UnitType traveler, int xStart, int yStart, int xGoal, int yGoal)
        {
            ALocation start = new ALocation(xStart, yStart);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                foreach (ALocation test in current)
                {
                    if (test.x == xGoal && test.y == yGoal)
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

                    foreach (ALocation l in GetAdjacentSquares(traveler, test.x, test.y))
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

        public static List<ALocation> PathToNearestUnit(UnitType traveler, int xStart, int yStart, LookupConstraint lookup)
        {
            ALocation start = new ALocation(xStart, yStart);

            Stack<ALocation> current = new Stack<ALocation>();
            Stack<ALocation> next = new Stack<ALocation>();

            current.Push(start);
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                foreach (ALocation test in current)
                {
                    foreach (Unit u in test.Tile.UnitsOn)
                        if (lookup.Check(u))
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

                    foreach (ALocation l in GetAdjacentSquares(traveler, test.x, test.y))
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
            return PathToNearestUnit(traveler, xStart, yStart, new LookupConstraint(new PlayerConstraint(player)));
        }

        public static List<ALocation> PathToNearestUnit(UnitType traveler, int xStart, int yStart, UnitType unit, bool player)
        {
            return PathToNearestUnit(traveler, xStart, yStart, new LookupConstraint(new PlayerConstraint(player), new UnitTypeConstraint(unit)));
        }

        public static List<ALocation> PathToNearestUnit(UnitType traveler, int xStart, int yStart, UnitType search)
        {
            return PathToNearestUnit(traveler, xStart, yStart, new LookupConstraint(new UnitTypeConstraint(search)));
        }

        public static List<ALocation> GetWalkableAdjacentSquares(UnitType traveler, int x, int y)
        {
            List<ALocation> proposedLocations = traveler.AdjecentLocationsFrom(x, y);

            return proposedLocations.Where(
                l => l.x < Game.TILES_WIDTH && l.y < Game.TILES_HEIGHT && l.x >= 0 && l.y >= 0 && Game.instance.tiles[l.x, l.y].type == tiles.TileType.grass).ToList();
        }

        public static List<ALocation> GetAdjacentSquares(UnitType traveler, int x, int y)
        {

            List<ALocation> proposedLocations = traveler.AdjecentLocationsFrom(x, y);
            return proposedLocations.Where(
                l => l.x < Game.TILES_WIDTH && l.y < Game.TILES_HEIGHT && l.x >= 0 && l.y >= 0).ToList();
        }

    }
}
