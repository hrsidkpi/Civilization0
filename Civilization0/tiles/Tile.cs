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

    /// <summary>
    /// Possible tile types
    /// </summary>
	public enum TileType
	{
		 grass, mountain, water, forest
	}

    /// <summary>
    /// Static class with extension methods for TileType to get information about a tile type
    /// </summary>
    public static class TileTypeInfo
    {

        /// <summary>
        /// Get the unit type that harvests the resource on a tile
        /// </summary>
        /// <param name="type">The tile type to check</param>
        /// <returns>The unit type that is capable of harvesting a resource from this tile type</returns>
        public static UnitType GetHarvesterType(this TileType type)
        {
            if (type == TileType.forest) return UnitType.lumberhouse;
            if (type == TileType.grass) return UnitType.farm;
            if (type == TileType.mountain) return UnitType.mine;
            return UnitType.none;
        }
    }

    /// <summary>
    /// Represents a tile on the board
    /// </summary>
	public class Tile
	{

        //Setting consts, the width and height of a single tile in pixels.
		public const int TILE_WIDTH = 50;
		public const int TILE_HEIGHT = 50;

        //The type of the tile
        public TileType type;
        //The 2D image of the tile
		public Texture2D texture;

        //The location of the tile, in pixels
		public int x, y;

        //The unit and the building that are placed on this tile. Can be null if no unit/building are placed.
        public Unit unitOn;
        public Unit buildingOn;

        /// <summary>
        /// Gets a list of units on this tile (human and buildings). 
        /// </summary>
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

        /// <summary>
        /// Creates a new tile
        /// </summary>
        /// <param name="type">The type of tile to create</param>
        /// <param name="x">The x position to create the tile in in pixels</param>
        /// <param name="y">The y position to create the tile in in pixels</param>
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


        /// <summary>
        /// Draw the tile on the screen
        /// </summary>
        /// <param name="canvas">Monogame's Canvas object to draw on</param>
		public void Draw(SpriteBatch canvas)
		{
			canvas.Draw(texture, new Rectangle(x + Game.instance.xScroll, y + Game.instance.yScroll, TILE_WIDTH, TILE_HEIGHT), Color.White);
            if (unitOn != null) unitOn.Draw(canvas);
            if (buildingOn != null) buildingOn.Draw(canvas);
		}

        /// <summary>
        /// Get the hitbox of the tile (square of pixels that the tile is in).
        /// </summary>
        /// <returns>A rectangle around the tile</returns>
		public Rectangle GetHitbox()
		{
			return new Rectangle(x + Game.instance.xScroll, y + Game.instance.yScroll, TILE_WIDTH, TILE_HEIGHT);
		}

        //Get the location of the tile in tiles
		public int TileX
		{
			get
			{
				return x / Tile.TILE_WIDTH;
			}
		}

        //Get the location of the tile in tiles
		public int TileY
		{
			get
			{
				return y / Tile.TILE_HEIGHT;
			}
		}

	}
}
