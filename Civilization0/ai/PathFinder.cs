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
    }

    public static class PathFinder
    {

        public static int FindHScore(int x, int y, int xTarget, int yTarget)
        {
            return Math.Abs(xTarget - x) + Math.Abs(yTarget - y);
        }

        public static List<ALocation> FindPath(int xStart, int yStart, int xTarget, int yTarget)
        {
            ALocation current = null;
            ALocation start = new ALocation(xStart, yStart);
            ALocation target = new ALocation(xTarget, yTarget);
            List<ALocation> open = new List<ALocation>();
            List<ALocation> closed = new List<ALocation>();
            int g = 0;

            open.Add(start);

            while (open.Count > 0)
            {
                int lowest = open.Min(l => l.f);
                current = open.First(l => l.f == lowest);

                closed.Add(current);
                open.Remove(current);

                if (closed.FirstOrDefault(l => l.x == target.x && l.y == target.y) != null)
                    break;

                List<ALocation> adjacent = GetWalkableAdjacentSquares(current.x, current.y);
                g++;

                foreach(ALocation square in adjacent)
                {
                    if (closed.FirstOrDefault(l => l.x == square.x && l.y == square.y) != null)
                        continue;

                    if (open.FirstOrDefault(l => l.x == square.x && l.y == square.y) != null)
                    {
                        square.g = g;
                        square.h = FindHScore(square.x, square.y, target.x, target.y);
                        square.f = square.g + square.h;
                        square.parent = current;

                        open.Insert(0, square);
                    } else
                    {
                        if(g + square.h < square.f)
                        {
                            square.g = g;
                            square.f = square.g + square.h;
                            square.parent = current;
                        }
                    }
                }

            }

            List<ALocation> result = new List<ALocation>();
            while(current != null)
            {
                result.Add(current);
                current = current.parent;
            }
            return result;

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
                l => Game.instance.tiles[x,y].type == tiles.TileType.grass).ToList();
        }


    }
}
