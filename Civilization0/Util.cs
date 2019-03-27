using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
    /**
     * Class containing utility static methods to be used anywhere. For example, shuffeling a list.
     */
	public static class Util
	{

        //Random number generator for utility methods requiring randomization.
		public static Random rng = new Random();

        //Select a random item from an array.
		public static T Random<T>(params T[] from)
		{
			int r = rng.Next(from.Length);
			return from[r];
		}

        //Extension method on a list. Shuffle the list- randomize the order of items in it, and return the list.
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

    }
}
