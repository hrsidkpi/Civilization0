using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{

    /*
     * Represents a player's state. Contains the amount of resources he has.
     */
    public class Player
    {
        //Setting constatnts, the amount of each resource a player starts with.
        public const int STARTING_FOOD = 150, STARTING_WOOD = 100, STARTING_IRON = 0;
        //Setting constant, used for debug. If set to true, players will start the game with infinate resources.
        public const bool INFINATE_RESOURCES = false;

        //The amount of resources the player has.
        public Resources resources;
        
        //Create a new player with resources according to the settings.
        public Player()
        {
            if (INFINATE_RESOURCES) resources = new Resources() { food = 100000, wood = 100000, iron = 100000 };
            else resources = new Resources() { food = STARTING_FOOD, wood = STARTING_WOOD, iron = STARTING_IRON };
        }

    }
}
