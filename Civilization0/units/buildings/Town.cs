using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;
using Civilization0.tiles;
using Microsoft.Xna.Framework.Graphics;
using Civilization0.units;

namespace Civilization0.units.buildings
{
	public class Town : Unit
	{

		public Town(int x, int y, bool player, Tile[,] board) : base(x, y, UnitType.town, player, board)
        {
			
		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>() { UnitType.builder };
		}

		public override List<Move> GetMoves(Tile[,] board)
		{
            return this.BuildAroundMove(board, UnitType.builder, 1);
        }

		public override void Initialize()
		{
			throw new NotImplementedException();
		}

		public override void Draw(SpriteBatch canvas)
		{
			base.Draw(canvas);
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override void Update()
		{
		}
	}
}
