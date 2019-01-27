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

		public Swordman(int x, int y) : base(x, y, Assets.swordman)
		{
			
		}

        public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>();
		}

		public override List<Move> GetMoves()
		{
			throw new NotImplementedException();
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
