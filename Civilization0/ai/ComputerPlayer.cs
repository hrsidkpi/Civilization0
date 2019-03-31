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
    /// <summary>
    /// Static class used for calculating the computer player moves.
    /// </summary>
    public static class ComputerPlayer
    {

        /// <summary>
        /// The types of tasks units can be given. The AI will decide what is the best task for a unit, and then what
        /// the best move for that task would be.
        /// </summary>
        public enum Task
        {
            improve,    //Build according to build order
            forward,    //Build buildings on the resources on the enemy side
            defend,     //Attack units near friendly base
            retreat,    //Retreat back to friendly base
            push,       //Advance to gain as much land control as possible (depth first search)
            destroy,    //Attack units at the enemy base
        }

        //Debug setting const. If set to true, the computer has infinate resources.
        public const bool COMPUTER_INFINATE_RESOURCES = false;

        //Setting const, build order for the AI. He will build buildings in his base in this order.
        public static readonly List<UnitType> BUILD_ORDER = new List<UnitType>()
        {
            UnitType.builder, UnitType.farm, UnitType.builder, UnitType.farm, UnitType.farm,
            UnitType.builder, UnitType.farm,
            UnitType.mine, UnitType.mine, UnitType.lumberhouse,
            UnitType.lumberhouse, UnitType.barracks, UnitType.farm, UnitType.farm,
        };

        //The build order in Dictionary form. Counts the amount of each unit type in the build order.
        public static readonly Dictionary<UnitType, int> BUILD_ORDER_DICT = AIUtil.ListToDict(BUILD_ORDER);

        /// <summary>
        /// Get a list of the best moves for each unit the computer player has.
        /// </summary>
        /// <returns>A list of moves, one per each unit the computer player has, decided by the AI.</returns>
        public static List<Move> BestMoves()
        {
            List<Move> moves = new List<Move>();
            Game game = Game.instance;

            //Clone the board to get a disposable board to work with.
            Tile[,] clone = DepthSearch.CopyBoard(Game.instance.tiles);

            //Get the best move for each unit
            foreach (Unit u in game.GetUnits())
            {
                if (u.player) continue;

                Task t = GetTask(u, clone);
                Move m = BestMove(u, clone, t);
                if (m != null)
                {
                    moves.Add(m);
                    //Execute the move on the cloned board so next units know what move was chosen beforehand. 
                    m.Execute(false, clone);
                }
            }

            //Return the list of moves found.
            return moves;
        }

        /// <summary>
        /// Find the best move for a unit.
        /// </summary>
        /// <param name="u">The unit to move</param>
        /// <param name="board">The board to find units on</param>
        /// <param name="task">The task the unit should do</param>
        /// <returns>A move that is decided as the best move for this unit this turn.</returns>
        public static Move BestMove(Unit u, Tile[,] board, Task task)
        {

            if (task == Task.improve)
            {
                //Get dictionary of amount of friendly units of type.
                Dictionary<UnitType, int> friendlies = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(false)));

                UnitType build = UnitType.builder; //The unit that needs to be built
                bool needToBuild = false; //Whether there is a unit to build

                //Find the first type in the build order that is not built yet.
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

                //Check if the build target can be built (resources wise)
                if (!(build.Cost() <= Game.instance.computer.resources || COMPUTER_INFINATE_RESOURCES)) return null;

                //Checks if the current unit is able to build the type
                if (u.GetBuildable().Contains(build))
                {
                    //Find all possibilites of building
                    List<Move> options = u.BuildAroundMove(board, build, 1);

                    //Make sure there is enough moves left for the unit to build
                    options.RemoveAll((o) => { return o.cost > u.movesLeft; });

                    //If there is an option to build the target, do it.
                    if (options.Count > 0)
                    {
                        Move res = options[0];
                        return res;
                    }
                    //Otherwise, if the unit is a builder, move to the nearest tile where building will be possible.
                    else
                    {
                        if (u.type == UnitType.builder)
                        {
                            //Find path to the nearest tile where the build target can be built
                            List<ALocation> path = PathFinder.PathToNearestTile(u.type, board, u.TileX, u.TileY, build.BuildableTiles()[0]);
                            //If a non empty path is found, walk it
                            if (path != null && path.Count != 0)
                                return new MovementMove(u, path[0].x, path[0].y);
                        }
                    }
                }
            }

            if (task == Task.forward)
            {

                //Find the resource tile that is open
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
                    //Find all build options of the harvester of the open tile.
                    List<Move> options = u.BuildAroundMove(board, target.type.GetHarvesterType(), 1);

                    //If a good build option is found, return it.
                    options.RemoveAll((o) => { return o.cost > u.movesLeft; });
                    if (options.Count > 0)
                    {
                        Move res = options[0];
                        return res;
                    }
                    //Otherwise, find a path to that tile.
                    else
                    {
                        List<ALocation> path = PathFinder.PathToNearestTile(u.type, board, u.px / Tile.TILE_WIDTH, u.py / Tile.TILE_HEIGHT, target.type);
                        if (path != null && path.Count != 0)
                            return new MovementMove(u, path[0].x, path[0].y);

                    }
                }



            }

            if (task == Task.defend)
            {

                //First see if the unit can attack. If so, attack.
                List<Move> attacks = u.DefaultAttackAroundMove(board);
                if (attacks.Count > 0)
                    return attacks[0];

                //If the unit can't attack, find a path to the nearest enemy unit in the friendly base.
                List<ALocation> path = PathFinder.PathToNearestUnit(u.type, board, u.TileX, u.TileY, new LookupConstraint(new PlayerConstraint(true), new DistanceConstraint(10, 10, 4)));
                if (path != null && path.Count > 0)
                {
                    //If a path is found, move along it.
                    int i = Math.Min(Math.Max(0, path.Count - 2), u.movesLeft - 1);
                    return new MovementMove(u, path[i].x, path[i].y);
                }

                //If no path is found, push instead of defending.
                return BestMove(u, board, Task.push);

            }

            if (task == Task.push)
            {
                if (u.type.GetDamage() > 0)
                {
                    //Find the move that maximizes map control, calculating 3 moves ahead.
                    return DepthSearch.GetBestMoveMin(u, board);
                }
                if (u.type.IsTraining())
                {
                    //Build a unit.
                    List<Move> options = u.BuildAroundMove(board, u.GetBuildable()[0], 1).Where(m => m.cost <= u.movesLeft).ToList();
                    if (options.Count == 0) return null;
                    Move build = options[0];
                    return build;
                }
            }

            if (task == Task.destroy)
            {
                //Try to attack a nearby unit.
                List<Move> attacks = u.DefaultAttackAroundMove(board);
                if (attacks.Count > 0) return attacks[0];

                //If no nearby units, find the nearest unit and go towards it.
                List<ALocation> path = PathFinder.PathToNearestUnit(u.type, board, u.TileX, u.TileY, true);
                if (path == null || path.Count == 0) return null;
                int i = Math.Min(Math.Max(0, path.Count - 2), u.movesLeft - 1);
                return new MovementMove(u, path[i].x, path[i].y);
            }

            if (task == Task.retreat)
            {
                //Find a path to the friendly base.
                List<ALocation> path = PathFinder.PathToCoordinates(u.type, board, u.TileX, u.TileY, 10, 10);
                if (path == null || path.Count == 0)
                {
                    //If no path is found, find a path to a tile that is close to the friendly base.
                    List<ALocation> path2 = PathFinder.PathToNearestTile(u.type, board, u.TileX, u.TileY, new TileLookupConstraint(new DistanceTileConstraint(10, 10, 3)));
                    if (path2 == null || path2.Count == 0) return null;
                    int i2 = Math.Min(Math.Max(0, path2.Count - 2), u.movesLeft - 1);
                    return new MovementMove(u, path2[i2].x, path2[i2].y);
                }
                int i = Math.Min(Math.Max(0, path.Count - 2), u.movesLeft - 1);
                return new MovementMove(u, path[i].x, path[i].y);
            }

            return null;
        }

        /// <summary>
        /// Get the best task for a unit.
        /// </summary>
        /// <param name="u">The unit to find a task for</param>
        /// <param name="board">The board to find a task on</param>
        /// <returns></returns>
        public static Task GetTask(Unit u, Tile[,] board)
        {

            Dictionary<UnitType, int> counts = AIUtil.Count(board, new LookupConstraint());
            Dictionary<UnitType, int> friendlies = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(false)));
            Dictionary<UnitType, int> enemies = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(true)));

            //If there are missing units from the build order, and this unit can build them, the task is to improve.
            if (!friendlies.GetMissingUnits(BUILD_ORDER_DICT).NoUnits() && (u.type == UnitType.builder || u.type == UnitType.town))
                return Task.improve;

            //If there are enemies near the base, and this unit can fight, defend.
            Dictionary<UnitType, int> enemiesNearBase = AIUtil.Count(board, new LookupConstraint(new PlayerConstraint(true), new DistanceConstraint(10, 10, 4)));
            if (enemiesNearBase.Sum((kv) => { return kv.Value; }) > 0 && u.type.GetDamage() > 0)
                return Task.defend;

            //If there are available resources on the enemy side of the board, forward.
            if (u.type == UnitType.builder && (board[3, 2].buildingOn == null || board[4, 2].buildingOn == null ||
               board[2, 3].buildingOn == null || board[2, 4].buildingOn == null))
                return Task.forward;

            //If there are much more friendly units than enemies, destroy.
            Dictionary<UnitType, int> fFighters = new Dictionary<UnitType, int>();
            foreach (KeyValuePair<UnitType, int> kvp in friendlies) if (kvp.Key.GetDamage() > 0) fFighters.Add(kvp.Key, kvp.Value);
            Dictionary<UnitType, int> eFighters = new Dictionary<UnitType, int>();
            foreach (KeyValuePair<UnitType, int> kvp in enemies) if (kvp.Key.GetDamage() > 0) eFighters.Add(kvp.Key, kvp.Value);
            if (u.type.GetDamage() > 0 && fFighters.Sum(uu => uu.Value) > eFighters.Sum(uu => uu.Value) + 2)
                return Task.destroy;

            //If there are much less friendly units than enemies, retreat.
            if (u.type.GetDamage() > 0 && fFighters.Sum(uu => uu.Value) < eFighters.Sum(uu => uu.Value) - 2)
                return Task.retreat;

            //If there are no special events, push.
            return Task.push;

        }

    }
}
