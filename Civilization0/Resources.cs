using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
    public struct Resources
    {

        public int wood, food, iron;

        public static bool operator <(Resources r1, Resources r2)
        {
            return r1.wood < r2.wood && r1.food < r2.food && r1.iron < r2.iron;
        }

        public static bool operator >(Resources r1, Resources r2)
        {
            return r1.wood > r2.wood && r1.food > r2.food && r1.iron > r2.iron;
        }

        public static bool operator <=(Resources r1, Resources r2)
        {
            return r1.wood <= r2.wood && r1.food <= r2.food && r1.iron <= r2.iron;
        }

        public static bool operator >=(Resources r1, Resources r2)
        {
            return r1.wood >= r2.wood && r1.food >= r2.food && r1.iron >= r2.iron;
        }

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
