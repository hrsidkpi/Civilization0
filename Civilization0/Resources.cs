using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
    /*
     * Class representing the resources storage of a player- the amount of food, wood and iron the player has.
     */
    public struct Resources
    {

        public int wood, food, iron;

        //returns true if for each resource, r1 has less of it than r2.
        public static bool operator <(Resources r1, Resources r2)
        {
            return r1.wood < r2.wood && r1.food < r2.food && r1.iron < r2.iron;
        }

        //returns true if for each resource, r1 has less or the same amount of it than r2.
        public static bool operator >(Resources r1, Resources r2)
        {
            return r1.wood > r2.wood && r1.food > r2.food && r1.iron > r2.iron;
        }

        //returns true if for each resource, r2 has less of it than r1.
        public static bool operator <=(Resources r1, Resources r2)
        {
            return r1.wood <= r2.wood && r1.food <= r2.food && r1.iron <= r2.iron;
        }

        //returns true if for each resource, r2 has less or the same amount of it than r1.
        public static bool operator >=(Resources r1, Resources r2)
        {
            return r1.wood >= r2.wood && r1.food >= r2.food && r1.iron >= r2.iron;
        }

        //Subtracts every resource amount r2 has from r1 and return the result as a new Resources object.
        public static Resources operator -(Resources r1, Resources r2)
        {
            return new Resources()
            {
                wood = r1.wood - r2.wood,
                food = r1.food - r2.food,
                iron = r1.iron - r2.iron
            };
        }

    }
}
