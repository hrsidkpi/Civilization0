﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civilization0.moves;
using Civilization0.tiles;
using Civilization0.units;

namespace Civilization0.units.buildings
{
    public class Mine : Unit
    {
        public Mine(int x, int y, bool player, Tile[,] board) : base(x, y, UnitType.mine, player, board)
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
            (player ? Game.instance.player : Game.instance.computer).resources.iron += 20;
        }

    }
}
