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
	class Builder : Unit
	{
		public Builder(int x, int y) : base(x, y, Assets.builder)
		{
		}

		public override List<UnitType> GetBuildable()
		{
			return new List<UnitType>() { UnitType.town };
		}

		public override List<Move> GetMoves()
		{
			List<Move> moves = new List<Move>();

			int xStart = Math.Max(0, this.x / Tile.TILE_WIDTH - 1);
			int xEnd = Math.Min(Game.TILES_WIDTH - 1, this.x / Tile.TILE_WIDTH + 1);
			int yStart = Math.Max(0, this.y / Tile.TILE_HEIGHT - 1);
			int yEnd = Math.Min(Game.TILES_HEIGHT - 1, this.y / Tile.TILE_HEIGHT + 1);

			for (int x = xStart; x <= xEnd; x++)
			{
				for (int y = yStart; y <= yEnd; y++)
				{
					if (x == this.x / Tile.TILE_WIDTH && y == this.y / Tile.TILE_HEIGHT) continue;
					Tile t = Game.instance.tiles[x, y];
					if (t.unitsOn.Count == 0)
					{
						moves.Add(new MovementMove(this, t.x, t.y));
						moves.Add(new BuildMove(t.x, t.y, UnitType.town));
					}
				}
			}
			return moves;
		}

		public override void Initialize()
		{
		}

		public override void Update()
		{
		}
	}
}
