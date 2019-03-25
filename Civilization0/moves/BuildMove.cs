using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
	public class BuildMove : Move
	{

		public UnitType unit;
		public int x, y;

        public BuildMove(Unit u, int x, int y, UnitType type) : base(Math.Abs(u.px/Tile.TILE_WIDTH - x) + Math.Abs(u.py/Tile.TILE_HEIGHT - y))
		{
			this.x = x;
			this.y = y;
			this.unit = type;
		}

        public override int CostBoard(Tile[,] board)
        {
            return cost;
        }

        public override void Execute(bool playerCall, Tile[,] board)
		{
            Unit u = unit.BuildOnTile(x, y, board, playerCall);
            Console.WriteLine((playerCall ? "Human" : "Computer") + " player built " + u);
        }
    }
}
