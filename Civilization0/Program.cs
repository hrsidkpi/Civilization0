using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{

    /*
     * Main class, only creates a new game class in Main and runs it.
     */
	class Program
	{

        //Main method, creates a new Game object and runs it.
		[STAThread]
		static void Main(string[] args)
		{
			using(Game game = new Game())
			{
				game.Run();
			}
		}
	}
}
