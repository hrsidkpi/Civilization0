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
		public Builder(int x, int y, bool player) : base(x, y, UnitType.builder, player)
		{
		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>() { UnitType.town, UnitType.barracks };
		}

		public override List<Move> GetMoves()
		{
            List<Move> moves = this.MoveAroundMove(1);
            foreach (UnitType t in GetBuildable())
                moves.AddRange( this.BuildAroundMove(t, 1));
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
