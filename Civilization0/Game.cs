using Civilization0.ai;
using Civilization0.gui;
using Civilization0.moves;
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

    /// <summary>
    /// Represents an instance of a game of Civilization. Contains the player objects, the board state, and updates and draws the game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {

        //Setting consts. The window size settings of the game.
        public const int GAME_SCALE = 100;
        public const int GAME_WIDTH = (int)(9.5 * GAME_SCALE);
        public const int GAME_HEIGHT = (int)(6.3 * GAME_SCALE);

        //Setting consts. The amount of tiles in the board.
        public const int TILES_WIDTH = 11;
        public const int TILES_HEIGHT = 11;

        //Setting const. Used for scrolling the map when the map is more than 11x11.
        public const int SCROLL_SPEED = 3;

        //Setting const. True if the player is first to move.
        public const bool PLAYER_START = true;

        //Static variable used for accessing the current instance of the game anywhere, using Game.instance.
        //The constructor of Game sets this variale to itself.
        public static Game instance;

        //Objects used by the Monogame framework for graphics.
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //2D array containing the tiles of the board.
        public Tile[,] tiles;

        //The amount of pixels scrolled by the player.
        public int xScroll;
        public int yScroll;

        //List of buttons on the screen. The Draw method will draw them on the screen and the Update method will check for input.
        public List<Button> buttons = new List<Button>();
        //List of buttons of possible moves on the screen. This is a seperate list because deselection of a unit only removes those buttons.
        public List<Button> selectionButtons = new List<Button>();

        //List of panels (static images) on the screen.The draw method will draw them.
        public List<Panel> panels = new List<Panel>();

        //The button used to end the turn.
        public Button turnButton;

        //True if currently its the player's turn. Starts initialized as PLAYER_START.
        public bool playerTurn = PLAYER_START;

        //Player objects for the human player and the computer player.
        public Player player;
        public Player computer;

        //Bools for storing whether the game is started over, and who won if it is.
        public bool gameStarted = false;
        public bool gameOver = false;
        public bool playerWon = false;
        public bool playerLost = false;


        /// <summary>
        /// Create a new game. Set the instance variable to it, and create the Monogame objects.
        /// </summary>
        public Game()
        {
            instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Initialize graphics settings (screen size etc'), and call SetupGame.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;
            graphics.ApplyChanges();

            this.IsMouseVisible = true;
            SetupGame();
        }

        /// <summary>
        /// Called by monogame when the framework is ready to load images. Assets.Load() loads all the assets used for the game.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Load();
        }

		/// <summary>
		/// Generates the default world map with the given width and height
		/// </summary>
		/// <param name="width">The width of the map in tiles</param>
		/// <param name="height">The height of the map in tiles</param>
		/// <returns></returns>
		public Tile[,] GenerateWorld(int width, int height)
		{
			Tile[,] tiles = new Tile[width, height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (x > width / 2 - 2 && width / 2 + 2 > x && y > height / 2 - 2 && height / 2 + 2 > y)
						tiles[x, y] = new Tile(TileType.water, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);

					else if ((x == 3 && y == 2) || (x == 2 && y == 3) || (x == width - 4 && y == height - 3) || (x == width - 3 && y == height - 4))
						tiles[x, y] = new Tile(TileType.mountain, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);

					else if ((x == 4 && y == 2) || (x == 2 && y == 4) || (x == width - 5 && y == height - 3) || (x == width - 3 && y == height - 5))
						tiles[x, y] = new Tile(TileType.forest, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);

					else
						tiles[x, y] = new Tile(TileType.grass, x * Tile.TILE_WIDTH, y * Tile.TILE_HEIGHT);



				}
			}

			return tiles;
		}

		/// <summary>
		/// Setup the board and the units.
		/// </summary>
		private void SetupGame()
        {
			tiles = GenerateWorld(TILES_WIDTH, TILES_HEIGHT);

            playerTurn = PLAYER_START;
            buttons.Clear();

            new Town(0, 0, true, tiles).NewTurn();
            new Town((TILES_WIDTH - 1) * Tile.TILE_WIDTH, (TILES_HEIGHT - 1) * Tile.TILE_HEIGHT, false, tiles);
            turnButton = new Button(new Rectangle(GAME_WIDTH - 300 - 80, GAME_HEIGHT - 80, 80, 80), PLAYER_START ? Assets.myTurn : Assets.enemyTurn);
            turnButton.Click += SwitchTurn;

            player = new Player();
            computer = new Player();
        }

        /// <summary>
        /// Called by the framework 60 times per second. Used for getting input from the player.
        /// </summary>
        /// <param name="gameTime">The amount of time that passed since the launch of the game.</param>
        protected override void Update(GameTime gameTime)
        {

            if (!gameStarted && Mouse.GetState().LeftButton == ButtonState.Pressed && lReleased)
            {
                gameStarted = true;
                lReleased = false;
            }

            //If the game is over there is no need to get input.
            if (gameOver && lReleased)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    gameStarted = false;
                    SetupGame();
                    gameOver = playerWon = playerLost = false;
                    lReleased = false;
                }
            }



            //Escape press exits the game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //Scroll buttons update
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

            //Get mouse position and state (buttons pressed)
            MouseState mouse = Mouse.GetState();
            Point mousePos = mouse.Position;

            //Call mouse events
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                MouseDown(mousePos);
            }
            if (mouse.LeftButton == ButtonState.Released)
            {
                lReleased = true;
            }
            if (mouse.RightButton == ButtonState.Pressed)
            {
                MouseRightDown(mousePos);
            }
            if (mouse.RightButton == ButtonState.Released)
                rReleased = true;

            //Call Monogame's update
            base.Update(gameTime);
        }

        //Used for making sure buttons are pressed once.
        private bool lReleased;
        private bool rReleased;

        /// <summary>
        /// Event called the first time the left mouse button is pressed. Handles button clicks and unit selections.
        /// </summary>
        /// <param name="mousePos">The position of the mouse on the screen.</param>
        private void MouseDown(Point mousePos)
        {
            //Check that the button has been released (so only the first click counts).
            if (!lReleased) return;

            //Check click on buttons
            for (int i = 0; i < buttons.Count; i++)
            {
                Button b = buttons[i];
                if (b.GetHitbox().Contains(mousePos))
                {
                    b.Click?.Invoke();
                }
            }

            //Check click on units (selecting unit)
            foreach (Tile t in tiles)
            {
                if (t.GetHitbox().Contains(mousePos))
                {
                    if (t.unitOn != null && t.unitOn.player) t.unitOn.Click();
                    else if (t.buildingOn != null && t.buildingOn.player) t.buildingOn.Click();
                }
            }

            //Set released to false, so that no new events can be fired until the button is released.
            lReleased = false;
        }


        /// <summary>
        /// Event called the first time the right mouse button is pressed. Handles selecting moves.
        /// </summary>
        /// <param name="mousePos">The position of the mouse on the screen.</param>
        private void MouseRightDown(Point mousePos)
        {
            //Check that the button has been released (so only the first click counts).
            if (!rReleased) return;

            //Check click on buttons
            for (int i = 0; i < buttons.Count; i++)
            {
                Button b = buttons[i];
                if (b.GetHitbox().Contains(mousePos))
                {
                    b.RightClick?.Invoke();
                }
            }

            //Set released to false, so that no new events can be fired until the button is released.
            rReleased = false;
        }


        /// <summary>
        /// Draw the game. Called by the framework 60 times per second.
        /// </summary>
        /// <param name="gameTime">The amount of time since the beggining of the game</param>
        protected override void Draw(GameTime gameTime)
        {

            //Clear the previous frame
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Insruct the framework to begin drawing
            spriteBatch.Begin();

            //Used for scrolling- the corners of the board that are inside the current view.
            int xTileStart = Math.Max(0, -xScroll / Tile.TILE_WIDTH - 1);
            int yTileStart = Math.Max(0, -yScroll / Tile.TILE_HEIGHT - 1);
            int xTileEnd = Math.Min(TILES_WIDTH, xTileStart + GAME_WIDTH / Tile.TILE_WIDTH + 1);
            int yTileEnd = Math.Min(TILES_HEIGHT, yTileStart + GAME_HEIGHT / Tile.TILE_HEIGHT + 2);

            //Game instruction screen
            if (!gameStarted)
            {
                spriteBatch.Draw(Assets.instructions, new Rectangle(0, 0, GAME_WIDTH, GAME_HEIGHT), Color.White);
            }
            //Game over screen
            else if (gameOver)
            {
                Texture2D t = playerWon ? Assets.win : Assets.lose;
                spriteBatch.Draw(t, new Rectangle(170, 150, 500, 300), Color.White);
                spriteBatch.DrawString(Assets.font, "Press the left mouse button anywhere to start again...", new Vector2(230, 350), Color.Black);
            }
            //The actual game rendering
            else
            {
                //Draw the tiles. Tile.Draw also draws the units on the tile.
                for (int x = xTileStart; x < xTileEnd; x++)
                {
                    for (int y = yTileStart; y < yTileEnd; y++)
                    {
                        tiles[x, y].Draw(spriteBatch);
                    }
                }

                //Draw the menu
                spriteBatch.Draw(Assets.menu, new Rectangle(GAME_WIDTH - 400, 0, 400, GAME_HEIGHT), Color.White);
                spriteBatch.Draw(Assets.menu, new Rectangle(0, GAME_HEIGHT - 80, GAME_WIDTH - 400, 80), Color.White);

                spriteBatch.Draw(Assets.instructionsSide, new Rectangle(Game.GAME_WIDTH - 395, 80, 390, 450), Color.White);

                //Draw the buttons and panels
                foreach (Button b in buttons)
                {
                    b.Draw(spriteBatch);
                }
                foreach (Panel p in panels)
                {
                    p.Draw(spriteBatch);
                }

                //Draw the resources of the player
                spriteBatch.DrawString(Assets.font, "" + player.resources.food, new Vector2(70, GAME_HEIGHT - 80 + 60), Color.Black);
                spriteBatch.Draw(Assets.food, new Rectangle(60, GAME_HEIGHT - 80 + 10, 50, 50), Color.White);
                spriteBatch.DrawString(Assets.font, "" + player.resources.wood, new Vector2(140, GAME_HEIGHT - 80 + 60), Color.Black);
                spriteBatch.Draw(Assets.wood, new Rectangle(130, GAME_HEIGHT - 80 + 10, 50, 50), Color.White);
                spriteBatch.DrawString(Assets.font, "" + player.resources.iron, new Vector2(210, GAME_HEIGHT - 80 + 60), Color.Black);
                spriteBatch.Draw(Assets.iron, new Rectangle(200, GAME_HEIGHT - 80 + 10, 50, 50), Color.White);
            }
            //Finish drawing
            spriteBatch.End();

            //Call the framwork's draw method.
            base.Draw(gameTime);
        }

        public void DrawString(string s, int x, int y, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Assets.font, s, new Vector2(x, y), Color.Black);
        }

        /// <summary>
        /// Called when the player click the "end turn" button. Switches the turn to the computer player.
        /// </summary>
        public void SwitchTurn()
        {
            playerTurn = false;
            turnButton.texture = Assets.enemyTurn;

            //Reset the turns left per turn variables.
            foreach (Tile t in tiles)
            {
                if (t.unitOn != null) t.unitOn.NewTurn();
                if (t.buildingOn != null) t.buildingOn.NewTurn();
            }

            //Delete all move selection buttons.
            foreach (Button b in selectionButtons) b.Delete();
            selectionButtons.Clear();


            Console.WriteLine("Player turn ended, player map control: " + DepthSearch.CalculateMapControl(tiles));
            DoComputerTurn();
        }

        //Find the best move to do and execute it on the board
        public void DoComputerTurn()
        {
            //Find the list of the best moves to do (1 per unit).
            List<Move> moves = ComputerPlayer.BestMoves();


            foreach (Move m in moves)
            {
                //Ignore duplicate movement moves
                if (m is MovementMove)
                {
                    MovementMove mm = m as MovementMove;
                    if (tiles[mm.x, mm.y].unitOn != null) continue;
                }

                //Execute every move
                m.Execute(false);
            }

            playerTurn = true;
            turnButton.texture = Assets.myTurn;

            Console.WriteLine("Computer turn ended, player map control: " + DepthSearch.CalculateMapControl(tiles));
        }

        /// <summary>
        /// Check if the game has ended, and if it did who won.
        /// </summary>
        public void CheckWin()
        {
            bool playerLost = true;
            bool computerLost = true;
            foreach (Unit u in tiles.Cast<Tile>().Select((t) => { return t.buildingOn; }))
                if (u != null && u.type == UnitType.town)
                {
                    if (u.player) playerLost = false;
                    else computerLost = false;
                }

            if (playerLost)
            {
                gameOver = true;
                playerLost = true;
            }
            else if (computerLost)
            {
                gameOver = true;
                playerWon = true;
            }

        }

        /// <summary>
        /// Utility method, gets the units on the board.
        /// </summary>
        /// <returns>A list of all units on the board</returns>
        public List<Unit> GetUnits()
        {
            List<Unit> res = new List<Unit>();
            foreach (Tile t in tiles) foreach (Unit u in t.UnitsOn) res.Add(u);
            return res;
        }

    }
}
