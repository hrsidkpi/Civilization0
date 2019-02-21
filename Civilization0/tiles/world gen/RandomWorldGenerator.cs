using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.tiles.world_gen
{
	class RandomWorldGenerator : IWorldGenerator
	{
		public Tile[,] Generate(int width, int height)
		{
			Tile[,] tiles = new Tile[width, height];
			Random r = new Random();
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					int uniformRandom = r.Next(20);
					int grassBias = (int)(((long)uniformRandom * (long)uniformRandom * (long)uniformRandom * (long)uniformRandom * (long)uniformRandom * (long)uniformRandom) / 10000000);
					TileType type = (TileType)grassBias;
					tiles[x, y] = new Tile(type, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);
				}
			}
			return tiles;
		}
	}
}
