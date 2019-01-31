using Civilization0.gui;
using Civilization0.tiles;
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

        public int food = 0, wood = 0, iron = 0;

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
			tiles = new Tile[TILES_WIDTH, TILES_HEIGHT];
            Random r = new Random();
			for (int x = 0; x < TILES_WIDTH; x++)
			{
				for (int y = 0; y < TILES_HEIGHT; y++)
				{
                    int uniformRandom = r.Next(20);
                    int grassBias = (uniformRandom * uniformRandom) / 133;
                    TileType type = (TileType)grassBias;
					tiles[x, y] = new Tile(type, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);
				}
			}

			tiles[0, 0].unitsOn.Add(new Swordman(0, 0, true));
            tiles[1, 1].unitsOn.Add(new Town(1 * Tile.TILE_WIDTH, 1 * Tile.TILE_HEIGHT, false));
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


			playerTurn = true;
			turnButton.texture = Assets.myTurn;
		}

	}
}
