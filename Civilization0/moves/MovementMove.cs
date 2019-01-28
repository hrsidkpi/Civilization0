using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
	public class MovementMove : Move
	{

		public Unit unit;
		public int x, y;

		public MovementMove(Unit unit, int x, int y) : base(Math.Abs(unit.x/Tile.TILE_WIDTH - x) + Math.Abs(unit.y/Tile.TILE_HEIGHT - y))
        {
			this.unit = unit;
			this.x = x;
			this.y = y;
		}

	}
}
