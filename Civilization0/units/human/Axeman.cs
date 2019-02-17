﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;

namespace Civilization0.units.human
{
	public class Axeman : Unit
	{

		public Axeman(int x, int y, bool player) : base(x, y, UnitType.axeman, player)
		{
			
		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>();
		}

		public override List<Move> GetMoves()
		{
            return this.DefaultMoveAroundMove().Union(this.DefaultAttackAroundMove()).ToList();
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
