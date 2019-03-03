using Civilization0.moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units.buildings
{
    public class ArcheryRange : Unit
    {

        public ArcheryRange(int x, int y, bool player) : base(x, y, UnitType.archeryRange, player)
        {

        }

        public override List<UnitType> GetBuildable()
        {
            return new List<UnitType>() { UnitType.archer, UnitType.levy, UnitType.crossbowman };
        }

        public override List<Move> GetMoves()
        {
            return this.BuildAroundMoveAll(1);
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
