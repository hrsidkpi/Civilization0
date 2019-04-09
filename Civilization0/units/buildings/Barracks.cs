using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;
using Civilization0.tiles;

namespace Civilization0.units.buildings
{
	public class Barracks : Unit
	{

		public Barracks(int x, int y, bool player, Tile[,] board) : base(x, y, UnitType.barracks, player, board)
        {

		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>() { UnitType.swordman, UnitType.spearman, UnitType.axeman}.Shuffle();
		}

		public override List<Move> GetMoves(Tile[,] board)
		{
            return this.BuildAroundMoveAll(Game.instance.tiles, 1);
        }

	}
}
