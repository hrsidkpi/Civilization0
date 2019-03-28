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
		 grass, mountain, water, forest
	}

    public static class TileTypeInfo
    {
        public static UnitType GetHarvesterType(this TileType type)
        {
            if (type == TileType.forest) return UnitType.lumberhouse;
            if (type == TileType.grass) return UnitType.farm;
            if (type == TileType.mountain) return UnitType.mine;
            return UnitType.none;
        }
    }

	public class Tile
	{
		public const int TILE_WIDTH = 50;
		public const int TILE_HEIGHT = 50;

        public TileType type;
		public Texture2D texture;

		public int x, y;

        public Unit unitOn;
        public Unit buildingOn;

        public List<Unit> UnitsOn
        {
            get {
                var res =  new List<Unit>();
                if (unitOn != null) res.Add(unitOn);
                if (buildingOn != null) res.Add(buildingOn);
                return res;
            }
        }

		//Used for pathfinding
		public bool flag = false;

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
                case TileType.forest:
                    texture = Assets.forest;
                    break;
				default:
					texture = Assets.grass;
					break;
			}
            this.type = type;
			this.x = x;
			this.y = y;
		}

		public void Initialize()
		{

		}

		public void Update()
		{
            if(unitOn != null) unitOn.Update();
            if(buildingOn != null) buildingOn.Update();
		}

		public void Draw(SpriteBatch canvas)
		{
			canvas.Draw(texture, new Rectangle(x + Game.instance.xScroll, y + Game.instance.yScroll, TILE_WIDTH, TILE_HEIGHT), Color.White);
            if (unitOn != null) unitOn.Draw(canvas);
            if (buildingOn != null) buildingOn.Draw(canvas);
		}

		public Rectangle GetHitbox()
		{
			return new Rectangle(x + Game.instance.xScroll, y + Game.instance.yScroll, TILE_WIDTH, TILE_HEIGHT);
		}

        public bool CanStandOn(Unit u)
        {
            switch(type)
            {
                case TileType.grass:
                    return true;
            }
            return false;
        }

		public int TileX
		{
			get
			{
				return x / Tile.TILE_WIDTH;
			}
		}

		public int TileY
		{
			get
			{
				return y / Tile.TILE_HEIGHT;
			}
		}

	}
}
