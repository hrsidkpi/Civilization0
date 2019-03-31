using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.tiles.world_gen
{

    /// <summary>
    /// Interface for world generators- classes that create an array of tiles.
    /// </summary>
	public interface IWorldGenerator
	{

        /// <summary>
        /// Generate a 2D array of tiles as a board for a game.
        /// </summary>
        /// <param name="width">The amount of tiles in the width of the board to create</param>
        /// <param name="height">The amount of tiles in the height of the board to create</param>
        /// <returns>A generated 2D array of tiles.</returns>
		Tile[,] Generate(int width, int height);

	}
}
