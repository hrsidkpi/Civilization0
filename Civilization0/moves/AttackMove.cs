using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
    public class AttackMove : Move
    {

        public Unit att, def;

        public AttackMove(Unit att, Unit def) : base(Math.Abs(att.px - def.px) / Tile.TILE_WIDTH + Math.Abs(att.py - def.py) / Tile.TILE_HEIGHT)
        {
            this.att = att;
            this.def = def;
        }

        public override int CostBoard(Tile[,] board)
        {
            Unit att1 = att;
            Unit def1 = def;

            if (board != Game.instance.tiles)
            {
                att1 = board[att.TileX, att.TileY].unitOn;
                def1 = board[def.TileX, def.TileY].UnitsOn[0];
            }

            return Math.Abs(att1.px - def1.px) / Tile.TILE_WIDTH + Math.Abs(att1.py - def1.py) / Tile.TILE_HEIGHT;
        }

        public override void Execute(bool playerCall, Tile[,] board)
        {
            if (board[att.TileX, att.TileY].unitOn == null || board[def.TileX, def.TileY].UnitsOn.Count == 0) return;

            //The unit may have been a clone if the move was found in depth search, so find the corresponding unit on the board given
            Unit att1 = board[att.TileX, att.TileY].unitOn;
            Unit def1 = board[def.TileX, def.TileY].UnitsOn[0];

            att1.Charge(board, def1);
            att1.movesLeft = 0;
            if (board == Game.instance.tiles)
                Console.WriteLine((playerCall ? "Human" : "Computer") + " player has attacked " + def1 + " with " + att1);

        }

    }
}
