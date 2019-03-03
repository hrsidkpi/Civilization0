using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
    public class Player
    {
        public const int STARTING_FOOD = 150, STARTING_WOOD = 100, STARTING_IRON = 0;
        public const bool INFINATE_RESOURCES = false;

        public Resources resources = new Resources() { food = STARTING_FOOD, wood = STARTING_WOOD, iron = STARTING_IRON };
        
        public Player()
        {
            if (INFINATE_RESOURCES) resources = new Resources() { food = 100000, wood = 100000, iron = 100000 };
        }

    }
}
