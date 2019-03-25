using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
    public class ShootMove : Move
    {
        public Unit att, def;

        public ShootMove(Unit att, Unit def) : base(att.type.GetMaxMoves())
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

            Unit att1 = att;
            Unit def1 = def;

            if (board != Game.instance.tiles)
            {
                att1 = board[att.TileX, att.TileY].unitOn;
                def1 = board[def.TileX, def.TileY].unitOn;
            }

            att1.Shoot(def1);
            Console.WriteLine((playerCall ? "Human" : "Computer") + " player has shot " + def1 + " with " + att1);
        }

    }
}
