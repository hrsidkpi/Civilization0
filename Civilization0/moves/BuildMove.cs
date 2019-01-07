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

		public BuildMove(int x, int y, UnitType type)
		{
			this.x = x;
			this.y = y;
			this.unit = type;
		}

	}
}
