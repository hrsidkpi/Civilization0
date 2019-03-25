using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{
    public class MovementMove : Move
    {

        public Unit unit;
        public int x, y;

        public MovementMove(Unit unit, int x, int y) : base(Math.Abs(unit.px / Tile.TILE_WIDTH - x) + Math.Abs(unit.py / Tile.TILE_HEIGHT - y))
        {
            this.unit = unit;
            this.x = x;
            this.y = y;
        }

        public override int CostBoard(Tile[,] board)
        {
            Unit unit1 = unit;

            if (board != Game.instance.tiles)
            {
                if (unit.type.IsBuilding())
                {
                    if (board[unit.TileX, unit.TileY].buildingOn == null)
                    {
                        throw new Exception("Units out of sync with board");
                    }
                    unit1 = board[unit.TileX, unit.TileY].buildingOn;
                }
                else
                {
                    if (board[unit.TileX, unit.TileY].unitOn == null)
                    {
                        throw new Exception("Units out of sync with board");
                    }
                    unit1 = board[unit.TileX, unit.TileY].unitOn;
                }

            }


            return Math.Abs(unit1.px / Tile.TILE_WIDTH - x) + Math.Abs(unit1.py / Tile.TILE_HEIGHT - y);
        }

        public override void Execute(bool playerCall, Tile[,] board)
        {

            Unit unit1 = unit;

            if (board != Game.instance.tiles)
            {
                if (unit.type.IsBuilding())
                {
                    if (board[unit.TileX, unit.TileY].buildingOn == null)
                    {
                        throw new Exception("Units out of sync with board");
                    }
                    unit1 = board[unit.TileX, unit.TileY].buildingOn;
                }
                else
                {
                    if (board[unit.TileX, unit.TileY].unitOn == null)
                    {
                        throw new Exception("Units out of sync with board");
                    }
                    unit1 = board[unit.TileX, unit.TileY].unitOn;
                }

            }

            board[unit1.TileX, unit1.TileY].unitOn = null;
            board[x, y].unitOn = unit1;
            unit1.TileX = x;
            unit1.TileY = y;

            if (board == Game.instance.tiles)
                Console.WriteLine((playerCall ? "Human" : "Computer") + " player has moved " + unit1);

        }

    }
}
