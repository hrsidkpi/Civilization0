using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
    public class ShootMove : Move
    {
        public Unit att, def;

        public ShootMove(Unit att, Unit def) : base(att.type.GetMaxMoves())
        {
            this.att = att;
            this.def = def;
        }

        public override void Execute(bool playerCall)
        {
            att.Shoot(def);
            Console.WriteLine((playerCall ? "Human" : "Computer") + " player has shot " + def + " with " + att);
        }

    }
}
