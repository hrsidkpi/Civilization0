using Civilization0.moves;
using Civilization0.tiles;
using Civilization0.units;
using Civilization0.units.buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{
    public static class ComputerPlayer
    {

        public static readonly List<UnitType> BUILD_ORDER = new List<UnitType>()
        {
            UnitType.builder, UnitType.lumberhouse, UnitType.builder, UnitType.farm, UnitType.farm,
            UnitType.lumberhouse, UnitType.mine, UnitType.lumberhouse, UnitType.lumberhouse,
            UnitType.farm, UnitType.mine, UnitType.farm, UnitType.mine, UnitType.barracks
        };

        public static int buildOrderPosition = 0;

        public static List<Move> BestMoves()
        {
            List<Move> moves = new List<Move>();
            Game game = Game.instance;

            foreach (Tile t in game.tiles)
            {
                foreach (Unit u in t.unitsOn)
                {
                    if (!u.player)
                    {
                        Move m = BestMove(u);
                        if (m != null)
                            moves.Add(m);
                    }
                }
            }

            return moves;
        }

        public static Move BestMove(Unit u)
        {
            if (u.GetBuildable().Contains(BUILD_ORDER[buildOrderPosition]))
            {
                List<Move> options = u.BuildAroundMove(BUILD_ORDER[buildOrderPosition], 1);
                if (options.Count > 0)
                {
                    Move res = options[0];
                    if (res != null)
                    {
                        buildOrderPosition++;
                        return res;
                    }
                }
                else
                {
                    if(u.type == UnitType.builder)
                    {
                        Tuple<int, int> target = AIUtil.NearestTileOfType(u.x / Tile.TILE_WIDTH, u.y / Tile.TILE_HEIGHT, TileType.forest);
                        List<ALocation> path = PathFinder.FindPath(u.x / Tile.TILE_WIDTH, u.y / Tile.TILE_HEIGHT, target.Item1, target.Item2);
                        return new MovementMove(u, path[0].x, path[0].y);
                    }
                }
            }

            return null;
        }

    }
}
