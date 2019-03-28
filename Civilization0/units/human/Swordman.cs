using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;
using Civilization0.tiles;
using Microsoft.Xna.Framework.Graphics;

namespace Civilization0.units.human
{
	public class Swordman : Unit
	{

		public Swordman(int x, int y, bool player, Tile[,] board) : base(x, y, UnitType.swordman, player, board)
        {
			
		}

        public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>();
		}

		public override List<Move> GetMoves(Tile[,] board)
		{
            return this.DefaultMoveAroundMove(board).Union(this.DefaultAttackAroundMove(board)).ToList();
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
