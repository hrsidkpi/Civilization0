using Civilization0.moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units.buildings
{
    public class Stable : Unit
    {

        public Stable(int x, int y, bool player) : base(x, y, UnitType.stable, player)
        {

        }

        public override List<UnitType> GetBuildable()
        {
            return new List<UnitType>() { UnitType.catapracht, UnitType.chariot, UnitType.cavelry  };
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
