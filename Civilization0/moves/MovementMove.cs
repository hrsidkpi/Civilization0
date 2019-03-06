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

		public override void Execute(bool playerCall)
		{
			Game.instance.tiles[unit.TileX, unit.TileY].unitOn = null;
            Game.instance.tiles[x, y].unitOn = unit;
			unit.TileX = x;
			unit.TileY = y;

            Console.WriteLine((playerCall ? "Human" : "Computer") + " player has moved " + unit);

        }

    }
}
