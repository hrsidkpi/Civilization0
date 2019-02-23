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
			UnitType.builder, UnitType.farm, UnitType.farm, UnitType.farm,
			UnitType.lumberhouse, UnitType.mine, UnitType.lumberhouse, UnitType.lumberhouse,
			UnitType.farm, UnitType.mine, UnitType.farm, UnitType.mine, UnitType.barracks,
			UnitType.swordman, UnitType.spearman, UnitType.axeman
		};
		public static bool buildOrderDone = false;
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

			//Try to build the next thing in the build order
			if (!buildOrderDone)
			{
				UnitType build = BUILD_ORDER[buildOrderPosition];
				if (u.GetBuildable().Contains(build))
				{
					List<Move> options = u.BuildAroundMove(BUILD_ORDER[buildOrderPosition], 1);
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
							if(path != null && path.Count != 0)
								return new MovementMove(u, path[0].x, path[0].y);
						}
					}
				}
			}

			//Fighting unit AI
			if(u.type.GetDamage() > 0)
			{
                Dictionary<UnitType, int> enemiesAround = PathFinder.CountAround(u, 4, true);
                Dictionary<UnitType, int> friendliesAround = PathFinder.CountAround(u, 4, true);
                int diff = 0;
                foreach (KeyValuePair<UnitType, int> t in enemiesAround)
                {
                    diff -= t.Value * t.Key.GetDamage();
                }
                //No enemies around, move towards closest enemy
                if(diff == 0)
                {
                    List<ALocation> path = PathFinder.PathToNearestUnit(u.type, u.TileX, u.TileY, true);
                    if (path != null && path.Count != 0)
                        return new MovementMove(u, path[0].x, path[0].y);
                }


                foreach (KeyValuePair<UnitType, int> t in friendliesAround)
                {
                    diff += t.Value * t.Key.GetDamage();
                }
                
			}

			return null;
		}

	}
}
