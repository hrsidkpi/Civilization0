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

        public AttackMove(Unit att, Unit def) : base(Math.Abs(att.x-def.x)/Tile.TILE_WIDTH +Math.Abs(att.y-def.y)/Tile.TILE_HEIGHT)
        {
            this.att = att;
            this.def = def;
        }

		public override void Execute(bool playerCall)
		{
			att.Charge(def);
            Console.WriteLine((playerCall ? "Human" : "Computer") + " player has attacked " + def + " with " + att);

        }

    }
}
