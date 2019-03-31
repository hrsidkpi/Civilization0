using Civilization0.tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.moves
{

    /// <summary>
    /// Represents an action a unit can do.
    /// </summary>
	public abstract class Move
	{
        //The cost (in move points) of the move.
        public int cost;

        /// <summary>
        /// Create a new move
        /// </summary>
        /// <param name="cost">The cost of the move</param>
        public Move(int cost)
        {
            this.cost = cost;
        }

        /// <summary>
        /// Get the cost of this move on a different board from the one it was created on
        /// </summary>
        /// <param name="board">The board to calculate cost on</param>
        /// <returns>Cost of the move in another board in move points</returns>
        public abstract int CostBoard(Tile[,] board);

        /// <summary>
        /// Execute the move on the current game instance board.
        /// </summary>
        /// <param name="playerCall">True if the player triggered this move, and false if the computer did.</param>
        public virtual void Execute(bool playerCall)
        {
            Execute(playerCall, Game.instance.tiles);
        }

        /// <summary>
        /// Execute the move on a given board.
        /// </summary>
        /// <param name="playerCall">True if the player triggered this move, and false if the computer did.</param>
        /// <param name="tiles">The board to execute on</param>
        public abstract void Execute(bool playerCall, Tile[,] tiles);

    }
}
