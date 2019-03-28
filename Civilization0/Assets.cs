using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
    /// <summary>
    /// Static class with all the assets used in the game.
    /// </summary>
	public static class Assets
	{
		public static Texture2D grass;
		public static Texture2D mountain;
		public static Texture2D water;
		public static Texture2D forest;
		public static Texture2D desert;
		public static Texture2D sheep;
		public static Texture2D horses;
		public static Texture2D ironMine;

		public static Texture2D town;
		public static Texture2D road;
		public static Texture2D path;
		public static Texture2D pasture;
		public static Texture2D barn;
		public static Texture2D fisherman;
		public static Texture2D mine;
		public static Texture2D forestry;
		public static Texture2D farm;
		public static Texture2D vineyard;
		public static Texture2D tavern;
		public static Texture2D barracks;
		public static Texture2D stable;
		public static Texture2D archeryRange;
		public static Texture2D factory;
		public static Texture2D shipyard;
		public static Texture2D tower;
		public static Texture2D wall;
		public static Texture2D gate;

		public static Texture2D builder;

		public static Texture2D spearman;
		public static Texture2D swordman;
		public static Texture2D axeman;

		public static Texture2D lightCavalry;
		public static Texture2D heavyCavalry;
		public static Texture2D mountedArcher;

		public static Texture2D archer;
		public static Texture2D levy;
		public static Texture2D crossbowman;

		public static Texture2D scorpio;
		public static Texture2D catapult;
		public static Texture2D ram;
		public static Texture2D airship;
							  
		public static Texture2D ramShip;
		public static Texture2D arrowShip;
		public static Texture2D boardingShip;
		public static Texture2D siegeShip;

		public static Texture2D menu;
		public static Texture2D button;
		public static Texture2D greenHighlight;
		public static Texture2D blueHighlight;
		public static Texture2D yellowHighlight;
		public static Texture2D myTurn;
		public static Texture2D enemyTurn;
        public static Texture2D done;

        public static Texture2D food;
        public static Texture2D wood;
        public static Texture2D iron;

        public static Texture2D win;
        public static Texture2D lose;


        public static SpriteFont font;

        /// <summary>
        /// Load all the content.
        /// </summary>
		public static void Load()
		{
            grass = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/grass.jpg", FileMode.Open));
            grass.Tag = "Grass Texture";
			mountain = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/mountain.jpg", FileMode.Open));
            mountain.Tag = "mountain Texture";
            water = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/water.jpg", FileMode.Open));
            water.Tag = "water Texture";
            forest = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/forest.jpg", FileMode.Open));
            forest.Tag = "forest Texture";

            town = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/town.png", FileMode.Open));
            town.Tag = "town Texture";
            barracks = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/barracks.png", FileMode.Open));
            barracks.Tag = "barracks Texture";
            archeryRange = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/archeryRange.png", FileMode.Open));
            archeryRange.Tag = "archeryRange Texture";
            stable = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/stable.png", FileMode.Open));
            stable.Tag = "stable Texture";

            builder = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/builder.png", FileMode.Open));
            builder.Tag = "builder Texture";

            swordman = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/swordman.png", FileMode.Open));
            swordman.Tag = "swordman Texture";
            axeman = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/axeman.png", FileMode.Open));
            axeman.Tag = "axeman Texture";
            spearman = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/spearman.png", FileMode.Open));
            spearman.Tag = "spearman Texture";

            archer = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/archer.png", FileMode.Open));
            archer.Tag = "archer Texture";
            levy = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/levy.png", FileMode.Open));
            levy.Tag = "levy Texture";
            crossbowman = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/crossbowman.png", FileMode.Open));
            crossbowman.Tag = "crossbowman Texture";

            lightCavalry = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/lightCavalry.png", FileMode.Open));
            lightCavalry.Tag = "lightCavalry Texture";
            heavyCavalry = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/heavyCavalry.png", FileMode.Open));
            heavyCavalry.Tag = "heavyCavalry Texture";
            mountedArcher = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/mountedArcher.png", FileMode.Open));
            mountedArcher.Tag = "mountedArcher Texture";

            catapult = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/catapult.png", FileMode.Open));
            catapult.Tag = "catapult Texture";
            ram = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/ram.png", FileMode.Open));
            ram.Tag = "ram Texture";
            airship = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/airship.png", FileMode.Open));
            airship.Tag = "airship Texture";

            menu = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/menu.png", FileMode.Open));
            menu.Tag = "menu Texture";
            greenHighlight = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/greenHighlight.png", FileMode.Open));
            greenHighlight.Tag = "greenHighlight Texture";
            blueHighlight = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/blueHighlight.png", FileMode.Open));
            blueHighlight.Tag = "blueHighlight Texture";
            yellowHighlight = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/yellowHighlight.png", FileMode.Open));
            yellowHighlight.Tag = "yellowHighlight Texture";
            myTurn = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/myTurn.png", FileMode.Open));
            myTurn.Tag = "myTurn Texture";
            enemyTurn = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/enemyTurn.png", FileMode.Open));
            enemyTurn.Tag = "enemyTurn Texture";
            done = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/done.png", FileMode.Open));
            done.Tag = "done Texture";

            food = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/food.png", FileMode.Open));
            food.Tag = "food Texture";
            wood = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/wood.png", FileMode.Open));
            wood.Tag = "wood Texture";
            iron = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/iron.png", FileMode.Open));
            iron.Tag = "iron Texture";

			mine = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/mine.png", FileMode.Open));
			mine.Tag = "mine Texture";
			forestry = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/lumberhouse.png", FileMode.Open));
			forestry.Tag = "forestry Texture";
			farm = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/granery.png", FileMode.Open));
			farm.Tag = "farm Texture";

            font = Game.instance.Content.Load<SpriteFont>("../res/HpFont");

            win = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/win.png", FileMode.Open));
            win.Tag = "Win texture";
            lose = Texture2D.FromStream(Game.instance.GraphicsDevice, new FileStream("res/lose.png", FileMode.Open));
            lose.Tag = "Lose texture";
        }

	}
}
