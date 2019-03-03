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

		public Barracks(int x, int y, bool player) : base(x, y, UnitType.barracks, player)
		{

		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>() { UnitType.swordman, UnitType.spearman, UnitType.axeman};
		}

		public override List<Move> GetMoves()
		{
            return this.BuildAroundMoveAll(1);
        }

        public override void Initialize()
		{
			throw new NotImplementedException();
		}

		public override void Update()
		{
		}
	}
}
