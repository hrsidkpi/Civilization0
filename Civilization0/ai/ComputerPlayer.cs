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
            cleanup, retreat, push, destroy,
        }

        public const bool COMPUTER_INFINATE_RESOURCES = false;

        public static readonly List<UnitType> BUILD_ORDER_GAME = new List<UnitType>()
        {
            UnitType.builder, UnitType.farm, UnitType.builder, UnitType.farm, UnitType.farm,
            UnitType.builder, UnitType.farm,
            UnitType.mine, UnitType.mine, UnitType.lumberhouse,
            UnitType.lumberhouse, UnitType.barracks, UnitType.farm, UnitType.farm,
        };

        public static readonly List<UnitType> BUILD_ORDER_DEBUG = new List<UnitType>()
        {
            UnitType.builder, UnitType.barracks, UnitType.axeman
        };

        public static readonly List<UnitType> BUILD_ORDER = BUILD_ORDER_GAME;
        public static readonly Dictionary<UnitType, int> BUILD_ORDER_DICT = AIUtil.ListToDict(BUILD_ORDER);

        public static Dictionary<UnitType, int> buildingThisTurn = new Dictionary<UnitType, int>();

        public static List<Move> pendingMoves = new List<Move>();

        public static List<Move> BestMoves()
        {
            List<Move> moves = new List<Move>();
            Game game = Game.instance;

            Tile[,] clone = Minmax.CopyBoard(Game.instance.tiles);

            pendingMoves.Clear();

            foreach (Unit u in game.GetUnits())
            {
                if (u.player) continue;

                Move m = BestMove(u, clone, GetTask(u, clone));
                if (m != null)
                {
                    moves.Add(m);
                    pendingMoves.Add(m);
                    m.Execute(false, clone);
                }
            }

            buildingThisTurn.Clear();
            return moves;
        }

        public static Move BestMove(Unit u, Tile[,] board, Task task)
        {

            if (task == Task.improve)
            {
                Dictionary<UnitType, int> friendlies = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(false)));
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
                    List<Move> options = u.BuildAroundMove(board, build, 1);
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
                            List<ALocation> path = PathFinder.PathToNearestTile(u.type, board, u.px / Tile.TILE_WIDTH, u.py / Tile.TILE_HEIGHT, build.BuildableTiles()[0]);
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
                    if (board[3, 2].buildingOn == null)
                        target = board[3, 2];
                    if (board[4, 2].buildingOn == null)
                        target = board[4, 2];
                    if (board[2, 3].buildingOn == null)
                        target = board[2, 3];
                    if (board[2, 4].buildingOn == null)
                        target = board[2, 4];

                    if (target != null)
                    {
                        List<Move> options = u.BuildAroundMove(board, target.type.GetHarvesterType(), 1);
                        options.RemoveAll((o) => { return o.cost > u.movesLeft; });
                        if (options.Count > 0)
                        {
                            Move res = options[0];
                            return res;
                        }
                        else
                        {
                            List<ALocation> path = PathFinder.PathToNearestTile(u.type, board, u.px / Tile.TILE_WIDTH, u.py / Tile.TILE_HEIGHT, target.type);
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

                    List<Move> attacks = u.DefaultAttackAroundMove(board);
                    if (attacks.Count > 0)
                        return attacks[0];

                    List<ALocation> path = PathFinder.PathToNearestUnit(u.type,board, u.TileX, u.TileY, new LookupConstraint(new PlayerConstraint(true), new DistanceConstraint(10, 10, 4)));
                    if (path != null && path.Count > 0)
                    {
                        int i = Math.Min(Math.Max(0, path.Count - 2), u.movesLeft - 1);
                        return new MovementMove(u, path[i].x, path[i].y);
                    }

                    return BestMove(u, board, Task.push);
                }
            }

            if (task == Task.push)
            {
                if (u.type.GetDamage() > 0)
                {
                    return Minmax.GetBestMoveMin(u, pendingMoves);
                }
                if (u.type.IsTraining())
                {
                    List<Move> options = u.BuildAroundMove(board, u.GetBuildable()[0], 1).Where(m => m.cost <= u.movesLeft).ToList();
                    if (options.Count == 0) return null;
                    Move build = options[0];
                    return build;
                }
            }

            if (task == Task.destroy)
            {
                List<Move> attacks = u.DefaultAttackAroundMove(board);
                if (attacks.Count > 0) return attacks[0];

                List<ALocation> path = PathFinder.PathToNearestUnit(u.type, board, u.TileX, u.TileY, true);
                if (path == null || path.Count == 0) return null;
                int i = Math.Min(Math.Max(0,path.Count - 2), u.movesLeft - 1);
                return new MovementMove(u, path[i].x, path[i].y);
            }

            if(task == Task.retreat)
            {
                List<ALocation> path = PathFinder.PathToCoordinates(u.type, board, u.TileX, u.TileY, 10, 10);
                if(path == null || path.Count == 0)
                {
                    List<ALocation> path2 = PathFinder.PathToNearestTile(u.type, board, u.TileX, u.TileY, new TileLookupConstraint(new DistanceTileConstraint(10, 10, 3)));
                    if (path2 == null || path2.Count == 0) return null;
                    int i2 = Math.Min(Math.Max(0,path2.Count - 2), u.movesLeft - 1);
                    return new MovementMove(u, path2[i2].x, path2[i2].y);
                }
                int i = Math.Min(Math.Max(0,path.Count - 2), u.movesLeft - 1);
                return new MovementMove(u, path[i].x, path[i].y);
            }

            return null;
        }

        public static Task GetTask(Unit u, Tile[,] board)
        {

            Dictionary<UnitType, int> counts = AIUtil.Count(board, new LookupConstraint());
            Dictionary<UnitType, int> friendlies = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(false)));
            Dictionary<UnitType, int> enemies = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(true)));

            if (!friendlies.Add(buildingThisTurn).GetMissingUnits(BUILD_ORDER_DICT).NoUnits() && (u.type == UnitType.builder || u.type == UnitType.town))
                return Task.improve;

            Dictionary<UnitType, int> enemiesNearBase = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(true), new DistanceConstraint(10, 10, 4)));
            if (enemiesNearBase.Sum((kv) => { return kv.Value; }) > 0 && u.type.GetDamage() > 0)
                return Task.defend;

            if (u.type == UnitType.builder && (board[3, 2].buildingOn == null || board[4, 2].buildingOn == null ||
               board[2, 3].buildingOn == null || board[2, 4].buildingOn == null))
                return Task.forward;

            Dictionary<UnitType, int> fFighters = new Dictionary<UnitType, int>();
            foreach (KeyValuePair<UnitType, int> kvp in friendlies) if (kvp.Key.GetDamage() > 0) fFighters.Add(kvp.Key, kvp.Value);
            Dictionary<UnitType, int> eFighters = new Dictionary<UnitType, int>();
            foreach (KeyValuePair<UnitType, int> kvp in enemies) if (kvp.Key.GetDamage() > 0) eFighters.Add(kvp.Key, kvp.Value);
            if (u.type.GetDamage() > 0 && fFighters.Sum(uu => uu.Value) > eFighters.Sum(uu => uu.Value))
                return Task.destroy;

            if (u.type.GetDamage() > 0 && fFighters.Sum(uu => uu.Value) < eFighters.Sum(uu => uu.Value) - 2)
                return Task.retreat;

            return Task.push;

        }

    }
}
