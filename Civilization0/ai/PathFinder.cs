using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{
    
    /// <summary>
    /// Utility class for representing a location on the board or in a path.
    /// </summary>
    public class ALocation
    {

        //The coordinates of the location in tiles.
        public int x, y;

        //Parent of the ALocation in the path, if used as a part of a path.
        public ALocation parent;

        /// <summary>
        /// Creates an ALocation in the given location.
        /// </summary>
        /// <param name="x">The x position in tiles</param>
        /// <param name="y">The y position in tiles</param>
        public ALocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Get the tile object in this location on a board
        /// </summary>
        /// <param name="board">The board to get the tile from</param>
        /// <returns>The tile object in this position on the given board</returns>
        public Tile Tile(Tile[,] board)
        {
            return board[x, y];
        }

        /// <summary>
        /// Get a string representation of this position
        /// </summary>
        /// <returns>The coordinates of this location in a string format</returns>
        public override string ToString()
        {
            return "ALocation (x: " + x + ", y: " + y + ")";
        }
    }

    /// <summary>
    /// Static class for finding paths (lists of ALocation objects from a point to another) to different objectives.
    /// </summary>
    public static class PathFinder
    {

        //Settings const for the maximum amount of tiles in a path the pathfinder can find. 221 is the longest possible path on the board so it is used here.
        public const int MAX_PATH_LENGTH = 221;

        /// <summary>
        /// Get a path to the nearest tile satisfying a TileLookupConstraint. The algorithm used is flood fill, meaning the optimal path is
        /// Gurrenteed.
        /// </summary>
        /// <param name="traveler">The unit type that attempts to travel</param>
        /// <param name="board">The board to search for a path in</param>
        /// <param name="xStart">The beggining position of the path in tiles</param>
        /// <param name="yStart">The beggining position of the path in tiles</param>
        /// <param name="constraint">The TileLookupConstraint object that decides if a tile is good</param>
        /// <returns>A List of ALocations starting at (xStart,yStart) and ending at the nearest tile satisfying the constraint</returns>
        public static List<ALocation> PathToNearestTile(UnitType traveler,Tile[,] board, int xStart, int yStart, TileLookupConstraint constraint)
        {
            ALocation start = new ALocation(xStart, yStart);

            //Current positions to check
            Stack<ALocation> current = new Stack<ALocation>();
            //Next positions to check
            Stack<ALocation> next = new Stack<ALocation>();

            //Start by checking the start
            current.Push(start);

            //Iteration i checks all the tiles in distance 1 from the beggining. Repeat until the maximum path length is reached.
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                //Check every tile in the current list
                foreach (ALocation test in current)
                {
                    //Check if the tile satisfies the constraint
                    if (constraint.Check(test.Tile(board)))
                    {
                        //If it does, trace the path using the parent property of each ALocation and return it.
                        Console.WriteLine("Found tile satisfying " + constraint + " in " + test.x + ", " + test.y);
                        List<ALocation> res = new List<ALocation>();
                        ALocation curr = test;
                        while (curr != null)
                        {
                            res.Add(curr);
                            curr = curr.parent;
                        }
                        //Clear the flags
                        foreach (Tile t in board) t.flag = false;
                        //Return the path
                        res.Reverse();
                        res.RemoveAt(0);
                        return res;
                    }

                    //If the traveler cannot walk on that tile, stop checking it, it cannot be path of a path.
                    if (!traveler.CanPlaceOn(board, test.x, test.y) && test != start)
                        continue;

                    //This tile is not final but may be in the shortest path. Check all tiles adjecent to it.
                    foreach (ALocation l in GetWalkableAdjacentSquares(traveler, board, test.x, test.y))
                        //Only check tiles that weren't checked yet.
                        if (!Game.instance.tiles[l.x, l.y].flag)
                        {
                            l.parent = test;
                            next.Push(l);

                            //Mark this tile as checked.
                            board[l.x, l.y].flag = true;
                        }
                }
                //Finished checking current tiles, switch on to the next batch.
                current = next;
                next = new Stack<ALocation>();
            }
            //If the maximum tile length is reached and no path was found, stop looking, unflag all tiles and return null.
            foreach (Tile t in board) t.flag = false;

            return null;
        }

        //Utility method for finding path to nearest tile of a type, instead of using a TileLookupConstraint.
        public static List<ALocation> PathToNearestTile(UnitType traveler,Tile[,] board, int xStart, int yStart, TileType tile)
        {
            return PathToNearestTile(traveler, board, xStart, yStart, new TileLookupConstraint(new TileTypeConstraint(tile)));
        }

        //Utility method of finding a path to coordinates, instead of using a TileLookupConstraint.
        public static List<ALocation> PathToCoordinates(UnitType traveler, Tile[,] board, int xStart, int yStart, int xGoal, int yGoal)
        {
            return PathToNearestTile(traveler, board, xStart, yStart, new TileLookupConstraint(new DistanceTileConstraint(xGoal, yGoal, 1)));
        }


        /// <summary>
        /// Get a path to the nearest location that has a unit on it that satisfies the given LookupConstraint.
        /// </summary>
        /// <param name="traveler">The unit that will travel the path</param>
        /// <param name="board">The board to search for a path in</param>
        /// <param name="xStart">The beggining x position of the path in tiles</param>
        /// <param name="yStart">The beggining y position of the path in tiles</param>
        /// <param name="lookup">The constraint to satisfy</param>
        /// <returns>A List of ALocations starting at (xStart,yStart) and ending at the nearest tile with a unit satisfying the constraint</returns>
        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, LookupConstraint lookup)
        {
            ALocation start = new ALocation(xStart, yStart);

            //Current positions to check
            Stack<ALocation> current = new Stack<ALocation>();
            //Next positions to check
            Stack<ALocation> next = new Stack<ALocation>();

            //Start by checking the start
            current.Push(start);

            //Iteration i checks all the tiles in distance 1 from the beggining. Repeat until the maximum path length is reached.
            for (int i = 0; i < MAX_PATH_LENGTH; i++)
            {
                //Check every tile in the current list
                foreach (ALocation test in current)
                {
                    //Check if there is a unit satisfying the constraint on this tile
                    foreach (Unit u in test.Tile(board).UnitsOn)
                        if (lookup.Check(u))
                        {
                            List<ALocation> res = new List<ALocation>();
                            ALocation curr = test;
                            while (curr != null)
                            {
                                res.Add(curr);
                                curr = curr.parent;
                            }
                            //Clear the flags
                            foreach (Tile t in board) t.flag = false;
                            //Return the path
                            res.Reverse();
                            res.RemoveAt(0);
                            return res;
                        }

                    //If the traveler cannot walk on that tile, stop checking it, it cannot be path of a path.
                    if (!traveler.CanPlaceOn(board, test.x, test.y) && test != start)
                        continue;

                    //This tile is not final but may be in the shortest path. Check all tiles adjecent to it.
                    foreach (ALocation l in GetWalkableAdjacentSquares(traveler, board, test.x, test.y))
                        //Only check tiles that weren't checked yet.
                        if (!Game.instance.tiles[l.x, l.y].flag)
                        {
                            l.parent = test;
                            next.Push(l);

                            //Mark this tile as checked.
                            board[l.x, l.y].flag = true;
                        }
                }
                //Finished checking current tiles, switch on to the next batch.
                current = next;
                next = new Stack<ALocation>();
            }
            //If the maximum tile length is reached and no path was found, stop looking, unflag all tiles and return null.
            foreach (Tile t in board) t.flag = false;

            return null;
        }
                
        //Utility method for finding path to the nearest unit of a sepcific player without using LookupConstraint
        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, bool player)
        {
            return PathToNearestUnit(traveler,board, xStart, yStart, new LookupConstraint(new PlayerConstraint(player)));
        }
                                                                          
        //Utility method for findign path to the nearest unit of a specific type and player without using LookupConstraint
        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, UnitType unit, bool player)
        {
            return PathToNearestUnit(traveler, board,xStart, yStart, new LookupConstraint(new PlayerConstraint(player), new UnitTypeConstraint(unit)));
        }
                                                                          
        //Utility method for finding path to the nearest unit of a specific type without using LookupConstraint
        public static List<ALocation> PathToNearestUnit(UnitType traveler,Tile[,] board, int xStart, int yStart, UnitType search)
        {
            return PathToNearestUnit(traveler,board, xStart, yStart, new LookupConstraint(new UnitTypeConstraint(search)));
        }


        /// <summary>
        /// Get a list of locations a unit can walk to from a position in a single turn.
        /// </summary>
        /// <param name="traveler">The unit that moves</param>
        /// <param name="board">The board to move on</param>
        /// <param name="x">The starting x location</param>
        /// <param name="y">The starting y location</param>
        /// <returns>A list of tiles the traveler can move to in one turn</returns>
        public static List<ALocation> GetWalkableAdjacentSquares(UnitType traveler, Tile[,] board, int x, int y)
        {
            List<ALocation> proposedLocations = traveler.AdjecentLocationsFrom(board, x, y);

            return proposedLocations.Where(
                l => l.x < Game.TILES_WIDTH && l.y < Game.TILES_HEIGHT && l.x >= 0 && l.y >= 0 && board[l.x, l.y].type == tiles.TileType.grass).ToList();
        }

    }
}
