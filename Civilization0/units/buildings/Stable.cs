﻿using Civilization0.moves;
using Civilization0.tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units.buildings
{
    public class Stable : Unit
    {

        public Stable(int x, int y, bool player, Tile[,] board) : base(x, y, UnitType.stable, player, board)
        {

        }

        public override List<UnitType> GetBuildable()
        {
            return new List<UnitType>() { UnitType.catapracht, UnitType.chariot, UnitType.cavelry  }.Shuffle();
        }

        public override List<Move> GetMoves(Tile[,] board)
        {
            return this.BuildAroundMoveAll(board, 1);
        }


    }
}
