using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
	public static class Util
	{

		public static Random random = new Random();

		public static T Random<T>(params T[] from)
		{
			int r = random.Next(from.Length);
			return from[r];
		}

	}
}
