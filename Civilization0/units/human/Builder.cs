using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;
using Civilization0.tiles;
using Microsoft.Xna.Framework.Graphics;

namespace Civilization0.units.human
{
	class Builder : Unit
	{
        public Builder(int x, int y, bool player, Tile[,] board) : base(x, y, UnitType.builder, player, board)
        {
		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>() { UnitType.town,
                UnitType.barracks, UnitType.archeryRange, UnitType.stable,
                UnitType.farm, UnitType.mine, UnitType.lumberhouse };
		}

        public override List<Move> GetMoves(Tile[,] board)
        {
            List<Move> moves = this.MoveAroundMove(board, 1);
            foreach (UnitType t in GetBuildable())
                moves.AddRange( this.BuildAroundMove(board, t, 1));
            return moves;
		}

		public override void Initialize()
		{
		}

		public override void Update()
		{
		}
	}
}
