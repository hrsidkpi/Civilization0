using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;
using Microsoft.Xna.Framework.Graphics;

namespace Civilization0.units.human
{
	public class Swordman : Unit
	{

		public Swordman(int x, int y, bool player) : base(x, y, UnitType.swordman, player)
		{
			
		}

        public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>();
		}

		public override List<Move> GetMoves()
		{
            return this.DefaultMoveAroundMove();
        }

        public override void Initialize()
		{
			throw new NotImplementedException();
		}

		public override void Update()
		{
			throw new NotImplementedException();
		}
	}
}
