using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
	public static class Assets
	{

		private static readonly string Path = @"..\bin\";

		public static Texture2D grass;
		public static Texture2D mountain;
		public static Texture2D water;
		public static Texture2D forest;
		public static Texture2D desert;
		public static Texture2D sheep;
		public static Texture2D horses;
		public static Texture2D iron;

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
		public static Texture2D ridingField;
		public static Texture2D targetPractice;
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

		public static void Load()
		{
			grass = Game.instance.Content.Load<Texture2D>(Path + "grass");
			mountain = Game.instance.Content.Load<Texture2D>(Path + "mountain");
			water = Game.instance.Content.Load<Texture2D>(Path + "water");

			town = Game.instance.Content.Load<Texture2D>(Path + "town");

			builder = Game.instance.Content.Load<Texture2D>(Path + "builder");

			menu = Game.instance.Content.Load<Texture2D>(Path + "menu");
			greenHighlight = Game.instance.Content.Load<Texture2D>(Path + "greenHighlight");
			blueHighlight = Game.instance.Content.Load<Texture2D>(Path + "blueHighlight");
			myTurn = Game.instance.Content.Load<Texture2D>(Path + "myTurn");
			enemyTurn = Game.instance.Content.Load<Texture2D>(Path + "enemyTurn");
		}

	}
}
