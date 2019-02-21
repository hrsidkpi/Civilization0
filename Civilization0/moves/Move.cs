using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
	public abstract class Move
	{
        public int cost;

        public Move(int cost)
        {
            this.cost = cost;
        }

		public abstract void Execute(bool playerCall);

    }
}
