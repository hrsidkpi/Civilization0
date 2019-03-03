﻿using Civilization0.moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units.human
{
    public class Levy : Unit
    {

        public Levy(int x, int y, bool player) : base(x, y, UnitType.levy, player)
        {

        }

        public override List<UnitType> GetBuildable()
        {
            return new List<UnitType>();
        }

        public override List<Move> GetMoves()
        {
            return this.DefaultMoveAroundMove().Union(this.DefaultShootAroundMove()).ToList();
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
