using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.tiles.world_gen
{
	public interface IWorldGenerator
	{

		Tile[,] Generate(int width, int height);

	}
}
