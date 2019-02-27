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

        public const bool COMPUTER_INFINATE_RESOURCES = false;

        public static readonly List<UnitType> BUILD_ORDER_GAME = new List<UnitType>()
        {
            UnitType.builder, UnitType.farm, UnitType.farm, UnitType.farm,
            UnitType.lumberhouse, UnitType.mine, UnitType.lumberhouse, UnitType.lumberhouse,
            UnitType.farm, UnitType.mine, UnitType.farm, UnitType.mine, UnitType.barracks,
            UnitType.swordman, UnitType.spearman, UnitType.axeman
        };

        public static readonly List<UnitType> BUILD_ORDER_DEBUG = new List<UnitType>()
        {
            UnitType.builder, UnitType.barracks, UnitType.axeman
        };

        public static readonly List<UnitType> BUILD_ORDER = BUILD_ORDER_DEBUG;

        public static bool buildOrderDone = false;
        public static int buildOrderPosition = 0;

        public static List<Move> BestMoves()
        {
            List<Move> moves = new List<Move>();
            Game game = Game.instance;

            foreach (Tile t in game.tiles)
            {
                if (t.unitOn != null && !t.unitOn.player)
                {
                    Move m = BestMove(t.unitOn);
                    if (m != null)
                        moves.Add(m);
                }

                if (t.buildingOn != null && !t.buildingOn.player)
                {
                    Move m = BestMove(t.buildingOn);
                    if (m != null)
                        moves.Add(m);
                }
            }

            return moves;
        }

        public static Move BestMove(Unit u)
        {

            //Try to build the next thing in the build order
            if (!buildOrderDone)
            {
                UnitType build = BUILD_ORDER[buildOrderPosition];
                if (!(build.Cost() <= Game.instance.computer.resources || COMPUTER_INFINATE_RESOURCES)) return null;
                if (u.GetBuildable().Contains(build))
                {
                    List<Move> options = u.BuildAroundMove(BUILD_ORDER[buildOrderPosition], 1);
                    options.RemoveAll((o) => { return o.cost > u.movesLeft; });
                    if (options.Count > 0)
                    {
                        Move res = options[0];
                        if (res != null)
                        {
                            buildOrderPosition++;
                            if (buildOrderPosition == BUILD_ORDER.Count) buildOrderDone = true;
                            return res;
                        }
                    }
                    else
                    {
                        if (u.type == UnitType.builder)
                        {
                            List<ALocation> path = PathFinder.PathToNearestTile(u.type, u.x / Tile.TILE_WIDTH, u.y / Tile.TILE_HEIGHT, build.BuildableTiles()[0]);
                            if (path != null && path.Count != 0)
                                return new MovementMove(u, path[0].x, path[0].y);
                        }
                    }
                }
            }

            //Fighting unit AI
            if (u.type.GetDamage() > 0)
            {
                Dictionary<UnitType, int> enemiesAround = PathFinder.CountAround(u, 4, true);
                int diff = 0;
                foreach (KeyValuePair<UnitType, int> t in enemiesAround)
                {
                    diff -= t.Value * t.Key.GetDamage();
                }
                //No enemies around, move towards closest enemy
                if (enemiesAround.Count == 0)
                {
                    List<ALocation> path = PathFinder.PathToNearestUnit(u.type, u.TileX, u.TileY, true);
                    if (path != null && path.Count != 0)
                        return new MovementMove(u, path[0].x, path[0].y);
                }


                Dictionary<UnitType, int> friendliesAround = PathFinder.CountAround(u, 4, false);
                foreach (KeyValuePair<UnitType, int> t in friendliesAround)
                {
                    diff += t.Value * t.Key.GetDamage();
                }

                //More friendlies around than enemies, attack
                if (diff > 0)
                {
                    List<ALocation> path = PathFinder.PathToNearestUnit(u.type, u.TileX, u.TileY, true);
                    if (path != null && path.Count <= u.movesLeft)
                    {
                        AttackMove move = new AttackMove(u, path.Last().Tile.UnitsOn[0]);
                        return move;
                    }
                    if(path != null) return new MovementMove(u, path[0].x, path[0].y);
                }

                //More enemies around than friendlies, run to closest friendly
                else
                {
                    List<ALocation> path = PathFinder.PathToNearestUnit(u.type, u.TileX, u.TileY, UnitType.town, false);
                    return new MovementMove(u, path[0].x, path[0].y);
                }
            }

            return null;
        }

    }
}
