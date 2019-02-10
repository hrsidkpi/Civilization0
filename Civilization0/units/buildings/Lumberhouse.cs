using Civilization0.moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units.buildings
{
    public class Lumberhouse : Unit
    {
        public Lumberhouse(int x, int y, bool player) : base(x, y, UnitType.lumberhouse, player)
        {

        }

        public override List<UnitType> GetBuildable()
        {
            return new List<UnitType>();
        }

        public override List<Move> GetMoves()
        {
            return new List<Move>();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
        }

        public override void NewTurn()
        {
            base.NewTurn();
            Game.instance.resources.wood += 20;
        }

    }
}
