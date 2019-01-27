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

		public Town(int x, int y) : base(x, y, Assets.town)
		{
			
		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>() { UnitType.builder };
		}

		public override List<Move> GetMoves()
		{
            return this.BuildAroundMove(UnitType.builder, 1);
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
			throw new NotImplementedException();
		}
	}
}
