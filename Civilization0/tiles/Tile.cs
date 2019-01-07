using Civilization0.units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.tiles
{

	public enum TileType
	{
		 grass, mountain, water, 
	}

	public class Tile
	{
		public const int TILE_WIDTH = 50;
		public const int TILE_HEIGHT = 50;

		public Texture2D texture;

		public int x, y;

		public List<Unit> unitsOn;

		public Tile(TileType type, int x, int y)
		{
			switch(type)
			{
				case TileType.grass:
					texture = Assets.grass;
					break;
				case TileType.mountain:
					texture = Assets.mountain;
					break;
				case TileType.water:
					texture = Assets.water;
					break;
				default:
					texture = Assets.grass;
					break;
			}
			this.x = x;
			this.y = y;

			unitsOn = new List<Unit>();
		}

		public void Initialize()
		{

		}

		public void Update()
		{
			foreach (Unit u in unitsOn) u.Update();
		}

		public void Draw(SpriteBatch canvas)
		{
			canvas.Draw(texture, new Rectangle(x + Game.instance.xScroll, y + Game.instance.yScroll, TILE_WIDTH, TILE_HEIGHT), Color.White);
			foreach (Unit u in unitsOn) u.Draw(canvas);
		}

		public Rectangle GetHitbox()
		{
			return new Rectangle(x + Game.instance.xScroll, y + Game.instance.yScroll, TILE_WIDTH, TILE_HEIGHT);
		}


	}
}
