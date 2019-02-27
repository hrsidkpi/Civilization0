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
    public enum UnitType
    {
        town, barracks,
		farm, lumberhouse, mine,
        builder,
        swordman, spearman, axeman,
        archer, levy, crossbowman,
        cavelry, catapracht, chariot,
        ram, catapult, airship,

    }

    public static class UnitTypeInfo
    {

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

                case UnitType.catapult:
                    food = 60; wood = 200;
                    break;

                case UnitType.ram:
                    food = 60; wood = 200; iron = 100;
                    break;

                case UnitType.airship:
                    food = 200; wood = 400; iron = 300;
                    break;

                case UnitType.town:
                    wood = 400;
                    break;
                case UnitType.barracks:
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

        public static bool IsLand(this UnitType t)
        {
            return true;
        }

        public static bool IsFlying(this UnitType t)
        {
            return t == UnitType.airship;
        }

		public static bool IsWater(this UnitType t)
		{
			return false;
		}

        public static bool IsBuilding(this UnitType t)
        {
            return
                t == UnitType.town ||
                t == UnitType.barracks ||
                t == UnitType.farm ||
                t == UnitType.lumberhouse ||
                t == UnitType.mine;
        }

        public static bool IsHuman(this UnitType t)
        {
            return !t.IsBuilding();
        }

        public static bool CanBeOn(this UnitType t, TileType tile)
        {

            if (t == UnitType.farm) return tile == TileType.grass;
            if (t == UnitType.mine) return tile == TileType.mountain;
            if (t == UnitType.lumberhouse) return tile == TileType.forest;

			if (t.IsLand() && !t.IsFlying()) return tile == TileType.grass;
			if (t.IsFlying()) return true;
			if (t.IsWater()) return tile == TileType.water;

            return true;
        }

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
                case UnitType.catapult:
                case UnitType.ram:
                case UnitType.airship:
                default:
                    return 1;
            }
        }

        public static int GetMaxHp(this UnitType t)
        {
            switch (t)
            {
                case UnitType.swordman:
                    return 3;
                case UnitType.spearman:
                    return 5;
                default:
                    return 2;
            }
        }

        public static int GetDamage(this UnitType t)
        {
            switch (t)
            {
                case UnitType.airship:
                    return 20;
                case UnitType.axeman:
                    return 6;
                case UnitType.swordman:
                case UnitType.crossbowman:
                    return 5;
                case UnitType.catapracht:
                case UnitType.archer:
                case UnitType.catapult:
                    return 4;
                case UnitType.spearman:
                case UnitType.chariot:
                case UnitType.levy:
                    return 3;
                case UnitType.cavelry:
                case UnitType.ram:
                    return 2;
                default:
                    return 0;
            }
        }

        public static int GetArmor(this UnitType t)
        {
            switch (t)
            {
                case UnitType.swordman:
                    return 2;
                case UnitType.spearman:
                    return 1;
                default:
                    return 0;
            }
        }

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

        public static int GetRange(this UnitType t)
        {
            switch(t)
            {
                case UnitType.chariot:
                case UnitType.levy:
                    return 4;
                case UnitType.archer:
                    return 5;
                case UnitType.crossbowman:
                    return 6;
                case UnitType.catapult:
                    return 8;
                default:
                    return 1;
            }
        }

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

            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.GetSprite(this UnitType)");
        }

        public static Unit Build(this UnitType t, int x, int y)
        {
            switch (t)
            {
                case UnitType.town:
                    return new Town(x, y, true);
                case UnitType.builder:
                    return new Builder(x, y, true);
                case UnitType.barracks:
                    return new Barracks(x, y, true);
                case UnitType.swordman:
                    return new Axeman(x, y, true);
                case UnitType.archer:
                    return new Archer(x, y, true);
            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.Build(this UnitType)");
        }

        public static Unit BuildOnTile(this UnitType t, int xTile, int yTile, bool player = true)
        {
            int x = xTile * Tile.TILE_WIDTH;
            int y = yTile * Tile.TILE_HEIGHT;
            switch (t)
            {
                case UnitType.town:
                    return new Town(x, y, player);
                case UnitType.builder:
                    return new Builder(x, y, player);
                case UnitType.barracks:
                    return new Barracks(x, y, player);
                case UnitType.swordman:
                    return new Swordman(x, y, player);
                case UnitType.spearman:
                    return new Spearman(x, y, player);
                case UnitType.axeman:
                    return new Axeman(x, y, player);
				case UnitType.farm:
					return new Farm(x, y, player);
                case UnitType.mine:
                    return new Mine(x, y, player);
                case UnitType.lumberhouse:
                    return new Lumberhouse(x, y, player);
                case UnitType.archer:
                    return new Archer(x, y, player);
            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.Build(this UnitType)");
        }
    }
}
