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

        public enum Task
        {
            improve, forward, improveForward, defend,
            cleanup, retreat, push, raid, destroy, trade
        }

        public const bool COMPUTER_INFINATE_RESOURCES = false;

        public static readonly List<UnitType> BUILD_ORDER_GAME = new List<UnitType>()
        {
            UnitType.builder, UnitType.farm, UnitType.builder, UnitType.farm, UnitType.farm,
            UnitType.builder, UnitType.farm,
            UnitType.lumberhouse, UnitType.mine, UnitType.lumberhouse,
            UnitType.farm, UnitType.mine, UnitType.farm, UnitType.barracks,
            UnitType.swordman, UnitType.spearman
        };

        public static readonly List<UnitType> BUILD_ORDER_DEBUG = new List<UnitType>()
        {
            UnitType.builder, UnitType.barracks, UnitType.axeman
        };

        public static readonly List<UnitType> BUILD_ORDER = BUILD_ORDER_GAME;
        public static readonly Dictionary<UnitType, int> BUILD_ORDER_DICT = AIUtil.ListToDict(BUILD_ORDER);

        public static Dictionary<UnitType, int> buildingThisTurn = new Dictionary<UnitType, int>();

        public static List<Move> BestMoves()
        {
            List<Move> moves = new List<Move>();
            Game game = Game.instance;

            Task task = GetTask();
            Console.WriteLine("AI is trying to " + task);

            foreach (Tile t in game.tiles)
            {
                if (t.unitOn != null && !t.unitOn.player)
                {
                    Move m = BestMove(t.unitOn, task);
                    if (m != null)
                        moves.Add(m);
                }

                if (t.buildingOn != null && !t.buildingOn.player)
                {
                    Move m = BestMove(t.buildingOn, task);
                    if (m != null)
                        moves.Add(m);
                }
            }

            buildingThisTurn.Clear();
            return moves;
        }

        public static Move BestMove(Unit u, Task task)
        {

            if (task == Task.improve)
            {
                Dictionary<UnitType, int> friendlies = PathFinder.Count(new LookupConstraint(new PlayerConstraint(false)));
                friendlies.Add(buildingThisTurn);

                UnitType build = UnitType.builder;
                bool needToBuild = false;
                foreach (UnitType test in BUILD_ORDER)
                {
                    if (!friendlies.ContainsKey(test))
                    {
                        build = test;
                        needToBuild = true;
                        break;
                    }
                    if (friendlies[test] == 0)
                    {
                        build = test;
                        needToBuild = true;
                        break;
                    }
                    friendlies[test]--;
                }
                if (!needToBuild) return null;

                if (!(build.Cost() <= Game.instance.computer.resources || COMPUTER_INFINATE_RESOURCES)) return null;
                if (u.GetBuildable().Contains(build))
                {
                    List<Move> options = u.BuildAroundMove(build, 1);
                    options.RemoveAll((o) => { return o.cost > u.movesLeft; });
                    if (options.Count > 0)
                    {
                        Move res = options[0];
                        if (buildingThisTurn.ContainsKey(build)) buildingThisTurn[build]++;
                        else buildingThisTurn.Add(build, 1);
                        return res;
                    }
                    else
                    {
                        if (u.type == UnitType.builder)
                        {
                            List<ALocation> path = PathFinder.PathToNearestTile(u.type, u.px / Tile.TILE_WIDTH, u.py / Tile.TILE_HEIGHT, build.BuildableTiles()[0]);
                            if (path != null && path.Count != 0)
                                return new MovementMove(u, path[0].x, path[0].y);
                        }
                    }
                }
            }

            if (task == Task.forward)
            {
                if (u.type == UnitType.builder)
                {
                    Tile target = null;
                    if (Game.instance.tiles[3, 2].buildingOn == null)
                        target = Game.instance.tiles[3, 2];
                    if (Game.instance.tiles[4, 2].buildingOn == null)
                        target = Game.instance.tiles[4, 2];
                    if (Game.instance.tiles[2, 3].buildingOn == null)
                        target = Game.instance.tiles[2, 3];
                    if (Game.instance.tiles[2, 4].buildingOn == null)
                        target = Game.instance.tiles[2, 4];

                    if (target != null)
                    {
                        List<Move> options = u.BuildAroundMove(target.type.GetHarvesterType(), 1);
                        options.RemoveAll((o) => { return o.cost > u.movesLeft; });
                        if (options.Count > 0)
                        {
                            Move res = options[0];
                            return res;
                        }
                        else
                        {
                            List<ALocation> path = PathFinder.PathToNearestTile(u.type, u.px / Tile.TILE_WIDTH, u.py / Tile.TILE_HEIGHT, target.type);
                            if (path != null && path.Count != 0)
                                return new MovementMove(u, path[0].x, path[0].y);

                        }
                    }


                }
            }

            if (task == Task.defend)
            {
                if (u.type.GetDamage() > 0)
                {
                    if (10 - u.TileX > 4 || 10 - u.TileY > 4)
                    {
                        List<ALocation> path = PathFinder.PathToNearestTile(u.type, u.TileX, u.TileY, new TileLookupConstraint(new DistanceTileConstraint(10, 10, 2)));
                        return new MovementMove(u, path[0].x, path[0].y);

                    }
                    else
                    {
                        List<Move> attacks = u.DefaultAttackAroundMove();
                        if (attacks.Count > 0)
                            return attacks[0];

                        List<ALocation> path = PathFinder.PathToNearestUnit(u.type, u.TileX, u.TileY, new LookupConstraint(new PlayerConstraint(true), new DistanceConstraint(10,10,4)));
                        if (path != null && path.Count > 0)
                            return new MovementMove(u, path[0].x, path[0].y);
                    }
                }
            }

            if (task == Task.push)
            {
                if (u.type.GetDamage() > 0)
                {
                    List<Move> attacks = u.DefaultAttackAroundMove();
                    if (attacks.Count > 0) return attacks[0];

                    List<ALocation> pathToEnemy = PathFinder.PathToNearestUnit(u.type, u.TileX, u.TileY, true);
                    if (pathToEnemy != null && pathToEnemy.Count < 2)
                    {
                        return new MovementMove(u, pathToEnemy[0].x, pathToEnemy[0].y);
                    }
                    else
                    {
                        List<ALocation> path = PathFinder.PathToNearestTile(u.type, u.TileX, u.TileY, new TileLookupConstraint(new DistanceTileConstraint(0, 0, 3)));
                        if(path != null && path.Count != 0) return new MovementMove(u, path[0].x, path[0].y);
                    }
                }
            }

            return null;
        }

        public static Task GetTask()
        {

            Dictionary<UnitType, int> counts = PathFinder.Count(new LookupConstraint());
            Dictionary<UnitType, int> friendlies = PathFinder.Count(new LookupConstraint(new PlayerConstraint(false)));
            Dictionary<UnitType, int> enemies = PathFinder.Count(new LookupConstraint(new PlayerConstraint(true)));

            if (!friendlies.Add(buildingThisTurn).GetMissingUnits(BUILD_ORDER_DICT).NoUnits())
                return Task.improve;

            Dictionary<UnitType, int> enemiesNearBase = PathFinder.Count(new LookupConstraint(new PlayerConstraint(true), new DistanceConstraint(10, 10, 4)));
            if (enemiesNearBase.Sum((kv) => { return kv.Value; }) > 0)
                return Task.defend;

            if (Game.instance.tiles[3, 2].buildingOn == null || Game.instance.tiles[4, 2].buildingOn == null ||
               Game.instance.tiles[2, 3].buildingOn == null || Game.instance.tiles[2, 4].buildingOn == null)
                return Task.forward;


            return Task.push;

        }

    }
}
