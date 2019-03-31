using Civilization0.tiles;
using Civilization0.units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.ai
{

    /// <summary>
    /// Static class with utility methods used by the AI.
    /// </summary>
    public static class AIUtil
    {


        /// <summary>
        /// Get a dictionary with the units that need to be added to container so that it will contain all units in contained.
        /// </summary>
        /// <param name="container">The dictionary that units need to be added to</param>
        /// <param name="contained">The dictionary with the goal amount of units</param>
        /// <returns></returns>
        public static Dictionary<UnitType, int> GetMissingUnits(this Dictionary<UnitType, int> container, Dictionary<UnitType, int> contained)
        {
            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();

            foreach (KeyValuePair<UnitType, int> pair in contained)
            {
                if (!container.ContainsKey(pair.Key)) res.Add(pair.Key, pair.Value);
                else if (container[pair.Key] < pair.Value) res[pair.Key] = pair.Value - container[pair.Key];
            }
            return res;
        }

        /// <summary>
        /// Checks if the dictionary represents no units
        /// </summary>
        /// <param name="dict">The dictionary to check</param>
        /// <returns>True if the dictionary has no units</returns>
        public static bool NoUnits(this Dictionary<UnitType, int> dict)
        {
            foreach (int v in dict.Values) if (v != 0) return false;
            return true;
        }

        /// <summary>
        /// Creates a dictionary of unit types from a list of units
        /// </summary>
        /// <param name="units">The list of units to convert</param>
        /// <returns>A dictionary where the keys are the unit types in the list of units and the values are how many units there are from the type</returns>
        public static Dictionary<UnitType, int> ListToDict(List<UnitType> units)
        {
            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();
            foreach (UnitType t in units)
                if (res.ContainsKey(t)) res[t]++;
                else res[t] = 1;
            return res;
        }

        /// <summary>
        /// Gets the units on a board
        /// </summary>
        /// <param name="board">The 2D array of tiles of the board</param>
        /// <returns>A list of all units on tiles on the board</returns>
        public static List<Unit> UnitsFromBoard(Tile[,] board)
        {
            List<Unit> res = new List<Unit>();
            foreach (Tile t in board) foreach (Unit u in t.UnitsOn) res.Add(u);
            return res;
        }

        /// <summary>
        /// Counts the units on a board that satisfy a constraint
        /// </summary>
        /// <param name="board">The board to count units on</param>
        /// <param name="constraint">The constraint units need to satisfy</param>
        /// <returns>Dictionary with the amount of each unit type of units that satisfied the constraint on the board.</returns>
        public static Dictionary<UnitType, int> Count(Tile[,] board, LookupConstraint constraint)
        {
            Dictionary<UnitType, int> res = new Dictionary<UnitType, int>();

            foreach (Tile t in board)
                foreach (Unit u in t.UnitsOn)
                {
                    if (constraint.Check(u))
                    {
                        if (!res.ContainsKey(u.type)) res[u.type] = 0;
                        res[u.type]++;
                    }
                }
            return res;

        }


    }
}
