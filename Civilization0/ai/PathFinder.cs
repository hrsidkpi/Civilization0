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

        public const int MAX_PATH_LENGTH = 221;

        /**
         */
        public static List<ALocation> PathToNearestTile(UnitType traveler,Tile[,] board, int xStart, int yStart, TileLookupConstraint constraint)
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

                    if (!traveler.CanPlaceOn(Game.instance.tiles, test.x, test.y) && test != start)
                        continue;

                    foreach (ALocation l in GetAdjacentSquares(traveler, board, test.x, test.y))
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

        public static List<ALocation> PathToNearestTile(UnitType traveler,Tile[,] board, int xStart, int yStart, TileType tile)
        {
            return PathToNearestTile(traveler, board, xStart, yStart, new TileLookupConstraint(new TileTypeConstraint(tile)));
        }

        public static List<ALocation> PathToCoordinates(UnitType traveler, Tile[,] board, int xStart, int yStart, int xGoal, int yGoal)
        {
            return PathToNearestTile(traveler, board, xStart, yStart, new TileLookupConstraint(new DistanceTileConstraint(xGoal, yGoal, 1)));
        }



        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, LookupConstraint lookup)
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
                            foreach (Tile t in board) t.flag = false;
                            res.Reverse();
                            res.RemoveAt(0);
                            return res;
                        }

                    if (!traveler.CanPlaceOn(board, test.x, test.y) && test != start)
                        continue;

                    foreach (ALocation l in GetAdjacentSquares(traveler, board, test.x, test.y))
                        if (!board[l.x, l.y].flag)
                        {
                            l.parent = test;
                            next.Push(l);
                            board[l.x, l.y].flag = true;
                        }
                }
                current = next;
                next = new Stack<ALocation>();
            }
            foreach (Tile t in board) t.flag = false;

            return null;
        }
                                                                          
        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, bool player)
        {
            return PathToNearestUnit(traveler,board, xStart, yStart, new LookupConstraint(new PlayerConstraint(player)));
        }
                                                                          
        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, UnitType unit, bool player)
        {
            return PathToNearestUnit(traveler, board,xStart, yStart, new LookupConstraint(new PlayerConstraint(player), new UnitTypeConstraint(unit)));
        }
                                                                          
        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, UnitType search)
        {
            return PathToNearestUnit(traveler,board, xStart, yStart, new LookupConstraint(new UnitTypeConstraint(search)));
        }



        public static List<ALocation> GetWalkableAdjacentSquares(UnitType traveler, Tile[,] board, int x, int y)
        {
            List<ALocation> proposedLocations = traveler.AdjecentLocationsFrom(board, x, y);

            return proposedLocations.Where(
                l => l.x < Game.TILES_WIDTH && l.y < Game.TILES_HEIGHT && l.x >= 0 && l.y >= 0 && Game.instance.tiles[l.x, l.y].type == tiles.TileType.grass).ToList();
        }

        public static List<ALocation> GetAdjacentSquares(UnitType traveler, Tile[,] board, int x, int y)
        {

            List<ALocation> proposedLocations = traveler.AdjecentLocationsFrom(board, x, y);
            return proposedLocations.Where(
                l => l.x < Game.TILES_WIDTH && l.y < Game.TILES_HEIGHT && l.x >= 0 && l.y >= 0).ToList();
        }

    }
}
