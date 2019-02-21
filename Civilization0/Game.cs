using Civilization0.ai;
using Civilization0.gui;
using Civilization0.moves;
using Civilization0.tiles;
using Civilization0.tiles.world_gen;
using Civilization0.units;
using Civilization0.units.buildings;
using Civilization0.units.human;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0
{
	public class Game : Microsoft.Xna.Framework.Game
	{

		public const int GAME_SCALE = 100;
		public const int GAME_WIDTH = 16 * GAME_SCALE;
		public const int GAME_HEIGHT = 9 * GAME_SCALE;

		public const int TILES_WIDTH = 10;
		public const int TILES_HEIGHT = 10;

		public const int SCROLL_SPEED = 3;

		public const bool PLAYER_START = true;

        public const int STARTING_FOOD = 150, STARTING_WOOD = 100, STARTING_IRON = 0;

		public static Game instance;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public Tile[,] tiles;
		public int xScroll;
		public int yScroll;

		public List<Button> buttons = new List<Button>();
        public List<Button> selectionButtons = new List<Button>();

		public Button turnButton;
		public bool playerTurn = PLAYER_START;

        public Resources resources = new Resources() { food=STARTING_FOOD, wood=STARTING_WOOD, iron=STARTING_IRON };

		public Game()
		{
			instance = this;

			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

		}

		protected override void Initialize()
		{
			base.Initialize();

			graphics.PreferredBackBufferWidth = GAME_WIDTH;
			graphics.PreferredBackBufferHeight = GAME_HEIGHT;
			graphics.ApplyChanges();

			this.IsMouseVisible = true;
			SetupGame();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Assets.Load();
		}

		protected override void UnloadContent()
		{

		}

		private void SetupGame()
		{
			IWorldGenerator generator = new DefaultWorldGenerator();
			tiles = generator.Generate(TILES_WIDTH, TILES_HEIGHT);

			tiles[0, 0].unitsOn.Add(new Town(0, 0, true));
            tiles[9, 9].unitsOn.Add(new Town(9 * Tile.TILE_WIDTH, 9 * Tile.TILE_HEIGHT, false));
			turnButton = new Button(new Rectangle(GAME_WIDTH - 300 - 80, GAME_HEIGHT - 80, 80, 80),PLAYER_START?Assets.myTurn:Assets.enemyTurn);
            turnButton.Click += SwitchTurn;
		}

		protected override void Update(GameTime gameTime)
		{

			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				xScroll += SCROLL_SPEED;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				xScroll -= SCROLL_SPEED;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				yScroll += SCROLL_SPEED;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				yScroll -= SCROLL_SPEED;
			}

			foreach (Tile t in tiles) foreach (Unit u in t.unitsOn) u.Update();

			MouseState mouse = Mouse.GetState();
			Point mousePos = mouse.Position;
			if (mouse.LeftButton == ButtonState.Pressed)
			{
				MouseDown(mousePos);
			}
			if(mouse.LeftButton == ButtonState.Released)
			{
				released = true;
			}
			base.Update(gameTime);
		}

		private bool released;
		private void MouseDown(Point mousePos)
		{
			if (!released) return;
			foreach (Tile t in tiles)
			{
				if (t.GetHitbox().Contains(mousePos))
				{
					foreach (Unit u in t.unitsOn)
					{
						if(u.player) u.Click();
					}
				}
			}

			for (int i = 0; i < buttons.Count; i++)
			{
				Button b = buttons[i];
				if (b.GetHitbox().Contains(mousePos))
				{
					b.Click();
				}
			}

			released = false;
		}

		protected override void Draw(GameTime gameTime)
		{

			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			int xTileStart = Math.Max(0, -xScroll / Tile.TILE_WIDTH - 1);
			int yTileStart = Math.Max(0, -yScroll / Tile.TILE_HEIGHT - 1);
			int xTileEnd = Math.Min(TILES_WIDTH, xTileStart + GAME_WIDTH / Tile.TILE_WIDTH + 1);
			int yTileEnd = Math.Min(TILES_HEIGHT, yTileStart + GAME_HEIGHT / Tile.TILE_HEIGHT + 2);

			for (int x = xTileStart; x < xTileEnd; x++)
			{
				for (int y = yTileStart; y < yTileEnd; y++)
				{
					tiles[x, y].Draw(spriteBatch);
				}
			}

			spriteBatch.Draw(Assets.menu, new Rectangle(GAME_WIDTH - 300, 0, 300, GAME_HEIGHT), Color.White);
			spriteBatch.Draw(Assets.menu, new Rectangle(0, GAME_HEIGHT-80, GAME_WIDTH-300, 80), Color.White);
			foreach(Button b in buttons)
			{
				b.Draw(spriteBatch);
			}

            spriteBatch.DrawString(Assets.font, ""+resources.food, new Vector2(70, GAME_HEIGHT - 80 + 60), Color.Black);
            spriteBatch.Draw(Assets.food, new Rectangle(60, GAME_HEIGHT - 80 + 10, 50, 50), Color.White);
            spriteBatch.DrawString(Assets.font, "" + resources.wood, new Vector2(140, GAME_HEIGHT - 80 + 60), Color.Black);
            spriteBatch.Draw(Assets.wood, new Rectangle(130, GAME_HEIGHT - 80 + 10, 50, 50), Color.White);
            spriteBatch.DrawString(Assets.font, "" + resources.iron, new Vector2(210, GAME_HEIGHT - 80 + 60), Color.Black);
            spriteBatch.Draw(Assets.iron, new Rectangle(200, GAME_HEIGHT - 80 + 10, 50, 50), Color.White);

            spriteBatch.End();

			base.Draw(gameTime);
		}

		public void SwitchTurn()
		{
			playerTurn = false;
			turnButton.texture = Assets.enemyTurn;

            foreach(Tile t in tiles)
            {
                foreach(Unit u in t.unitsOn)
                {
                    u.NewTurn();
                }
            }

			DoComputerTurn();
		}

		public void DoComputerTurn()
		{
            List<Move> moves = ComputerPlayer.BestMoves();
            foreach(Move m in moves)
            {
				m.Execute(false);
            }

			playerTurn = true;
			turnButton.texture = Assets.myTurn;
		}

	}
}
