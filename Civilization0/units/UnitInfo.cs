using Civilization0.ai;
using Civilization0.tiles;
using Civilization0.units.buildings;
using Civilization0.units.human;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units
{

    /// <summary>
    /// All possible types of units
    /// </summary>
    public enum UnitType
    {
        none,
        town,
        barracks, archeryRange, stable,
		farm, lumberhouse, mine,
        builder,
        swordman, spearman, axeman,
        archer, levy, crossbowman,
        cavelry, catapracht, chariot,

    }

    /// <summary>
    /// Static class with utility methods to get stats of different units
    /// </summary>
    public static class UnitTypeInfo
    {


        /// <summary>
        /// Checks if a unit type can be placed in a location on a board
        /// </summary>
        /// <param name="unit">The unit type to check</param>
        /// <param name="board">The board to check</param>
        /// <param name="x">The x position to check, in tiles</param>
        /// <param name="y">The y position to check, in tiles</param>
        /// <returns>True if the unit type can be placed on the location on the board</returns>
        public static bool CanPlaceOn(this UnitType unit, Tile[,] board, int x, int y)
        {
            Tile t = board[x, y];
            if (!unit.CanBeOn(t.type)) return false;
            if ((unit.IsBuilding() && t.buildingOn != null) || (unit.IsHuman() && t.unitOn != null)) return false;
            return true;
        }

        /// <summary>
        /// Checks if a unit type can be placed in a location on the board
        /// </summary>
        /// <param name="unit">The unit type to check</param>
        /// <param name="board">The boarad to check</param>
        /// <param name="x">The x position to check, in tiles</param>
        /// <param name="y">The y position to check, in tiles</param>
        /// <param name="player">The player index of the unit to check</param>
        /// <returns>True if the unit type can be placed on the location on the board</returns>
        public static bool CanPlaceOn(this UnitType unit, Tile[,] board, int x, int y, bool player)
        {
            Tile t = board[x, y];
            if (!unit.CanBeOn(t.type)) return false;
            if ((unit.IsBuilding() && t.buildingOn != null) || (unit.IsHuman() && t.unitOn != null)) return false;
            if (t.buildingOn != null && t.buildingOn.player != player) return false;
            return true;
        }

        /// <summary>
        /// Checks if a unit can be placed in a location on the board
        /// </summary>
        /// <param name="unit">The unit to check</param>
        /// <param name="board">The boarad to check</param>
        /// <param name="x">The x position to check, in tiles</param>
        /// <param name="y">The y position to check, in tiles</param>
        /// <returns>True if the unit can be placed on the location on the board</returns>
        public static bool CanPlaceOn(this Unit unit, Tile[,] board, int x, int y)
        {
            return CanPlaceOn(unit.type, board, x, y, unit.player);
        }


        /// <summary>
        /// Get the locations a unit can move to on a board
        /// </summary>
        /// <param name="t">The unit type that needs to move</param>
        /// <param name="board">The board to move on</param>
        /// <param name="x">The x position of the unit on the board</param>
        /// <param name="y">The y position of the unit on the board</param>
        /// <returns></returns>
        public static List<ALocation> AdjecentLocationsFrom(this UnitType t, Tile[,] board, int x, int y)
        {
            //Prepare the list
            List<ALocation> res = new List<ALocation>();

            for(int xx = x-t.GetRange(); xx <= x+t.GetRange(); xx++)
            {
                int yRange = t.GetRange() - (Math.Abs(xx - x));
                for(int yy = y-yRange; yy <= y+yRange; yy++)
                {
                    res.Add(new ALocation(xx, yy));
                }
            }
            //Return the possibilities
            return res;
        }

        /// <summary>
        /// Get the tiles the unit type can be built on.
        /// </summary>
        /// <param name="t">The unit to build</param>
        /// <returns></returns>
		public static List<TileType> BuildableTiles (this UnitType t)
		{
			switch(t)
			{
				case UnitType.farm:
					return new List<TileType>() { TileType.grass };
				case UnitType.lumberhouse:
					return new List<TileType>() { TileType.forest };
				case UnitType.mine:
					return new List<TileType>() { TileType.mountain };
			}
			return new List<TileType>() { TileType.grass };
		}

        /// <summary>
        /// Get the cost of the unit
        /// </summary>
        /// <param name="t">The unit to get the cost of</param>
        /// <returns>Resources object with the cost to build the unit</returns>
        public static Resources Cost(this UnitType t)
        {
            int food = 0, wood = 0, iron = 0;
            switch (t)
            {
                case UnitType.builder:
                    food = 50;
                    break;
                case UnitType.cavelry:
                    food = 120; iron = 30;
                    break;

                case UnitType.chariot:
                    food = 120; wood = 50;
                    break;

                case UnitType.catapracht:
                    food = 120; iron = 150;
                    break;

                case UnitType.swordman:
                    food = 80; iron = 30;
                    break;

                case UnitType.levy:
                    food = 80; wood = 40;
                    break;

                case UnitType.archer:
                    food = 80; wood = 50;
                    break;

                case UnitType.spearman:
                    food = 80; iron = 20;
                    break;

                case UnitType.axeman:
                    food = 80; iron = 60;
                    break;

                case UnitType.crossbowman:
                    food = 80; wood = 90;
                    break;

                case UnitType.town:
                    wood = 400;
                    break;
                case UnitType.barracks:
                case UnitType.stable:
                case UnitType.archeryRange:
                    wood = 200;
                    break;
                default:
                    break;

            }

            return new Resources()
            {
                wood = wood,
                food = food,
                iron = iron
            };
        }

        /// <summary>
        /// Checks if the unit type is a building
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>True if t is a building and false otherwise</returns>
        public static bool IsBuilding(this UnitType t)
        {
            return
                t == UnitType.town ||
                t == UnitType.barracks ||
                t == UnitType.farm ||
                t == UnitType.lumberhouse ||
                t == UnitType.mine ||
                t == UnitType.archeryRange ||
                t == UnitType.stable;
        }

        /// <summary>
        /// Checks if the unit type is human.
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>True if t is a human unit and false otherwise</returns>
        public static bool IsHuman(this UnitType t)
        {
            return !t.IsBuilding();
        }

        /// <summary>
        /// Checks if the unit can be on the tile type
        /// </summary>
        /// <param name="t">The unit to check</param>
        /// <param name="tile">The tile type to check</param>
        /// <returns>true if t can be on tile</returns>
        public static bool CanBeOn(this UnitType t, TileType tile)
        {

            if (t == UnitType.farm) return tile == TileType.grass;
            if (t == UnitType.mine) return tile == TileType.mountain;
            if (t == UnitType.lumberhouse) return tile == TileType.forest;

            return tile == TileType.grass;
        }

        /// <summary>
        /// Get the maximum amount of tiles the unit type can move in one turn
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>The amount of maximum moves he can move in one turn</returns>
        public static int GetMaxMoves(this UnitType t)
        {
            switch (t)
            {
                case UnitType.cavelry:
                case UnitType.chariot:
                    return 4;
                case UnitType.catapracht:
                case UnitType.swordman:
                case UnitType.levy:
                case UnitType.archer:
                    return 3;
                case UnitType.spearman:
                case UnitType.axeman:
                case UnitType.crossbowman:
                    return 2;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Get the starting hp of a unit type
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>The starting (maximal) amount of hp the unit type has</returns>
        public static int GetMaxHp(this UnitType t)
        {
            switch (t)
            {
                case UnitType.axeman:
                    return 6;
                case UnitType.swordman:
                    return 7;
                case UnitType.crossbowman:
                    return 5;
                case UnitType.catapracht:
                    return 8;
                case UnitType.archer:
                    return 3;
                case UnitType.spearman:
                    return 8;
                case UnitType.chariot:
                    return 4;
                case UnitType.levy:
                    return 2;
                case UnitType.cavelry:
                    return 3;
                case UnitType.town:
                    return 25;
                default:
                    return 4;
            }
        }

        /// <summary>
        /// Checks whether the unit type is a millitary training building
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>True if t is a barracks, archery range or stable.</returns>
        public static bool IsTraining(this UnitType t)
        {
            return t == UnitType.barracks || t == UnitType.archeryRange || t == UnitType.stable;
        }

        /// <summary>
        /// Gets the amount of damage the unit deals when attacking an enemy.
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>The amount of damage the unit deals per one attack on an enemy</returns>
        public static int GetDamage(this UnitType t)
        {
            switch (t)
            {
                case UnitType.axeman:
                    return 6;
                case UnitType.swordman:
                case UnitType.crossbowman:
                    return 5;
                case UnitType.catapracht:
                case UnitType.archer:
                case UnitType.spearman:
                case UnitType.chariot:
                case UnitType.levy:
                    return 3;
                case UnitType.cavelry:
                    return 2;
                default:
                    return 0;
            }
        }


        /// <summary>
        /// Get the amount of damage the unit reflects back to the attacker
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>Fraction of damage to be reflected to the attacking enemy.</returns>
        public static double GetReflect(this UnitType t)
        {
            switch (t)
            {
                case UnitType.swordman:
                    return 0.2;
                case UnitType.spearman:
                    return 0.6;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Get the range of a unit.
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>The maximum amount of tiles an enemy can be away from the unit and still be targeted</returns>
        public static int GetRange(this UnitType t)
        {
            switch(t)
            {
                case UnitType.chariot:
                case UnitType.levy:
                    return 2;
                case UnitType.archer:
                    return 3;
                case UnitType.crossbowman:
                    return 4;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Get the 2D sprite that corresponds to the unit type
        /// </summary>
        /// <param name="t">The unit type to check</param>
        /// <returns>A Monogame Texture2D object with the sprite of the unit type</returns>
        public static Texture2D GetSprite(this UnitType t)
        {
            switch (t)
            {
                case UnitType.town:
                    return Assets.town;
                case UnitType.builder:
                    return Assets.builder;
                case UnitType.barracks:
                    return Assets.barracks;
                case UnitType.swordman:
                    return Assets.swordman;
                case UnitType.axeman:
                    return Assets.axeman;
                case UnitType.spearman:
                    return Assets.spearman;
				case UnitType.farm:
					return Assets.farm;
				case UnitType.mine:
					return Assets.mine;
				case UnitType.lumberhouse:
					return Assets.forestry;
                case UnitType.archer:
                    return Assets.archer;
                case UnitType.archeryRange:
                    return Assets.archeryRange;
                case UnitType.levy:
                    return Assets.levy;
                case UnitType.crossbowman:
                    return Assets.crossbowman;
                case UnitType.catapracht:
                    return Assets.heavyCavalry;
                case UnitType.cavelry:
                    return Assets.lightCavalry;
                case UnitType.chariot:
                    return Assets.mountedArcher;
                case UnitType.stable:
                    return Assets.stable;

            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.GetSprite(this UnitType)");
        }

        /// <summary>
        /// Creates and places a new unit on a given board
        /// </summary>
        /// <param name="t">The unit type to create</param>
        /// <param name="x">The x position to create the unit in, in pixels</param>
        /// <param name="y">The y position to creaate the unit in, in pixels</param>
        /// <param name="board">The board to place the unit on</param>
        /// <returns>The unit that was created and placed</returns>
        public static Unit Build(this UnitType t, int x, int y, Tile[,] board)
        {
            switch (t)
            {
                case UnitType.town:
                    return new Town(x, y, true, board);
                case UnitType.builder:
                    return new Builder(x, y, true, board);
                case UnitType.barracks:
                    return new Barracks(x, y, true, board);
                case UnitType.swordman:
                    return new Axeman(x, y, true, board);
                case UnitType.archer:
                    return new Archer(x, y, true, board);
                case UnitType.archeryRange:
                    return new ArcheryRange(x, y, true, board);
                case UnitType.stable:
                    return new Stable(x, y, true, board);
            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.Build(this UnitType)");
        }

        /// <summary>
        /// Creates and places a new unit on a given board
        /// </summary>
        /// <param name="t">The unit type to create</param>
        /// <param name="xTile">The x position to create the unit in, in tiles</param>
        /// <param name="yTile">The y position to creaate the unit in, in tiles</param>
        /// <param name="board">The board to place the unit on</param>
        /// <param name="player">The player index to create the unit with</param>
        /// <returns>The unit that was created and placed</returns>
        public static Unit BuildOnTile(this UnitType t, int xTile, int yTile, Tile[,] board, bool player = true)
        {
            int x = xTile * Tile.TILE_WIDTH;
            int y = yTile * Tile.TILE_HEIGHT;
            switch (t)
            {
                case UnitType.town:
                    return new Town(x, y, player, board);
                case UnitType.builder:
                    return new Builder(x, y, player, board);
                case UnitType.barracks:
                    return new Barracks(x, y, player, board);
                case UnitType.swordman:
                    return new Swordman(x, y, player, board);
                case UnitType.spearman:
                    return new Spearman(x, y, player, board);
                case UnitType.axeman:
                    return new Axeman(x, y, player, board);
                case UnitType.farm:
					return new Farm(x, y, player, board);
                case UnitType.mine:
                    return new Mine(x, y, player, board);
                case UnitType.lumberhouse:
                    return new Lumberhouse(x, y, player, board);
                case UnitType.archer:
                    return new Archer(x, y, player, board);
                case UnitType.archeryRange:
                    return new ArcheryRange(x, y, player, board);
                case UnitType.stable:
                    return new Stable(x, y, player, board);
                case UnitType.catapracht:
                    return new Catapracht(x, y, player, board);
                case UnitType.levy:
                    return new Levy(x, y, player, board);
                case UnitType.crossbowman:
                    return new Crossbowman(x, y, player, board);
                case UnitType.cavelry:
                    return new Cavelry(x, y, player, board);
                case UnitType.chariot:
                    return new Chariot(x, y, player, board);

            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.Build(this UnitType)");
        }
    }
}
