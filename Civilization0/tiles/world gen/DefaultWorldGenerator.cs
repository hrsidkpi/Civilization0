using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.tiles.world_gen
{
	public class DefaultWorldGenerator : IWorldGenerator
	{
		public Tile[,] Generate(int width, int height)
		{
			Tile[,] tiles = new Tile[width, height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (x > width / 2 - 2 && width / 2 + 2 > x && y > height / 2 - 2 && height / 2 + 2 > y )
						tiles[x, y] = new Tile(TileType.water, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);

					else if((x == 3 && y == 2) || (x == 2 && y == 3) || (x == width - 4 && y == height - 3) || (x == width - 3 && y == height - 4))
						tiles[x, y] = new Tile(TileType.mountain, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);

					else if((x == 4 && y == 2) || (x == 2 && y == 4) || (x == width - 5 && y == height - 3) || (x == width - 3 && y == height - 5))
						tiles[x, y] = new Tile(TileType.forest, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);

					else
						tiles[x, y] = new Tile(TileType.grass, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);



				}
			}

			return tiles;
		}
	}
}
