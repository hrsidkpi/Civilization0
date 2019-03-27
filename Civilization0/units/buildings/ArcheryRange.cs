using Civilization0.moves;
using Civilization0.tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units.buildings
{
    public class ArcheryRange : Unit
    {

        public ArcheryRange(int x, int y, bool player, Tile[,] board) : base(x, y, UnitType.archeryRange, player, board)
        {

        }

        public override List<UnitType> GetBuildable()
        {
            return new List<UnitType>() { UnitType.archer, UnitType.levy, UnitType.crossbowman }.Shuffle();
        }

        public override List<Move> GetMoves(in Tile[,] board)
        {
            return this.BuildAroundMoveAll(board, 1);
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
        }
    }
}
