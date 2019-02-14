using Civilization0.tiles;
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

    }
}
