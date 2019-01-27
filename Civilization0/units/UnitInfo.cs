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
        builder,
        swordman, spearman, axeman,
        archer, levy, crossbowman,
        cavelry, catapracht, chariot,
        ram, catapult, airship,

    }

    public static class UnitTypeInfo
    {

        public static int GetMaxMoves(this UnitType t)
        {
            switch(t)
            {
                case UnitType.cavelry:
                    return 3;
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
            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.GetSprite(this UnitType)");
        }

        public static Unit Build(this UnitType t, int x, int y)
        {
            switch (t)
            {
                case UnitType.town:
                    return new Town(x, y);
                case UnitType.builder:
                    return new Builder(x, y);
                case UnitType.barracks:
                    return new Barracks(x, y);
                case UnitType.swordman:
                    return new Swordman(x, y);
            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.Build(this UnitType)");
        }

        public static Unit BuildOnTile(this UnitType t, int xTile, int yTile)
        {
            int x = xTile * Tile.TILE_WIDTH;
            int y = yTile * Tile.TILE_HEIGHT;
            switch (t)
            {
                case UnitType.town:
                    return new Town(x, y);
                case UnitType.builder:
                    return new Builder(x, y);
                case UnitType.barracks:
                    return new Barracks(x, y);
                case UnitType.swordman:
                    return new Swordman(x, y);
            }
            throw new Exception("UnitTypeNotConfigured Exception- add unit info to units.UnitTypeInfi.Build(this UnitType)");
        }
    }
}
