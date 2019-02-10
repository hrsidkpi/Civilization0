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
            if (u is Town)
            {
                return u.GetMoves()[0];
            }

            return null;
        }

    }
}
