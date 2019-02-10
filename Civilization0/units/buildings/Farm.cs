using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;

namespace Civilization0.units.buildings
{
	public class Farm : Unit
	{
		public Farm(int x, int y, bool player) : base(x, y, UnitType.farm, player)
		{
		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>();
		}

		public override List<Move> GetMoves()
		{
			return new List<Move>();
		}

		public override void Initialize()
		{
			throw new NotImplementedException();
		}

		private int counter = 0;
		public override void Update()
		{
			counter++;
			if(counter >= 60)
			{
				Game.instance.resources.food += 10;
				counter = 0;
			}
		}
	}
}
