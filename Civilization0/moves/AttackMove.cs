using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
    public class AttackMove : Move
    {

        public Unit att, def;

        public AttackMove(Unit att, Unit def) : base(Math.Abs(att.px-def.px)/Tile.TILE_WIDTH +Math.Abs(att.py-def.py)/Tile.TILE_HEIGHT)
        {
            this.att = att;
            this.def = def;
        }

		public override void Execute(bool playerCall)
		{
			att.Charge(def);
            att.movesLeft = 0;
            Console.WriteLine((playerCall ? "Human" : "Computer") + " player has attacked " + def + " with " + att);

        }

    }
}
