using Civilization0.gui;
using Civilization0.moves;
using Civilization0.tiles;
using Civilization0.units.buildings;
using Civilization0.units.human;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.units
{
    /// <summary>
    /// Represents a single unit on the board
    /// </summary>
    public abstract class Unit
    {

        //The location of the unit on the board in pixels
        public int px, py;

        //The location of the unit on the board in tiles
        public int TileX { get { return px / Tile.TILE_WIDTH; } set { px = value * Tile.TILE_WIDTH; } }
        public int TileY { get { return py / Tile.TILE_HEIGHT; } set { py = value * Tile.TILE_HEIGHT; } }

        //The type of the unit
        public UnitType type;

        //The 2D image of the unit
        private readonly Texture2D sprite;

        //True if the unit belongs to the human player, and false if it belongs to the computer player.
        public bool player;

        //The amount of tiles the unit can move this turn.
        public int movesLeft;

        //The amount of hp the unit currently has.
        public int hp;

        /// <summary>
        /// Creates a new unit and places it on the board
        /// </summary>
        /// <param name="x">The location to place the unit in, in pixels</param>
        /// <param name="y">The location to place the unit in, in pixels</param>
        /// <param name="type">The type of unit to create</param>
        /// <param name="player">The player index of the unit</param>
        /// <param name="board">The board to place the unit on</param>
        public Unit(int x, int y, UnitType type, bool player, Tile[,] board)
        {
            this.px = x;
            this.py = y;
            this.type = type;
            this.sprite = type.GetSprite();
            this.player = player;

            hp = type.GetMaxHp();

            if (type.IsBuilding()) board[x / Tile.TILE_WIDTH, y / Tile.TILE_HEIGHT].buildingOn = this;
            else board[x / Tile.TILE_WIDTH, y / Tile.TILE_HEIGHT].unitOn = this;

            movesLeft = 0;
        }

        /// <summary>
        /// Required by Monogame for all graphics objects, unused.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Called 30 times per second, handles logic.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called when the unit is right clicked.
        /// </summary>
        public virtual void RightClick() { }

        /// <summary>
        /// Called when the unit is left clicked.
        /// </summary>
        public virtual void Click()
        {
            //Deselect the previously selected unit.
            foreach (Button del in Game.instance.selectionButtons)
            {
                del.Delete();
            }
            Game.instance.selectionButtons.Clear();

            //If the player shouldn't move this unit, don't select it.
            if (movesLeft == 0) return;
            if (!player) return;

            //Generate context menu and possible moves for the unit (the movement buttons around it, the units it can build).
            GenerateMoves(Game.instance.tiles);
            GenerateContextMenu();

            //Create the deselect button.
            Button deselect = new Button(new Rectangle(Game.GAME_WIDTH - 200, Game.GAME_HEIGHT - 50, 50, 50), Assets.done);
            deselect.Click += () => {

                foreach (Button del in Game.instance.selectionButtons)
                {
                    del.Delete();
                }
                Game.instance.selectionButtons.Clear();
                deselect.Delete();
            };
        }

        //List with pointers to the buttons that trigger a movement move.
        private List<Button> movementButtons = new List<Button>();
        //List with pointers to the buttons that trigger a build move.
        private List<Button> buildButtons = new List<Button>();


        /// <summary>
        /// Generates the move buttons for possible moves the unit can perform.
        /// </summary>
        /// <param name="board">The board to check for moves in</param>
        public virtual void GenerateMoves(Tile[,] board)
        {
            foreach (Move m in GetMoves(board))
            {
                if (m is MovementMove) GenerateMovementButton(m as MovementMove);
                if (m is AttackMove) GenerateAttackButton(m as AttackMove);
                if (m is ShootMove) GenerateShootButton(m as ShootMove);
            }
        }

        /// <summary>
        /// Create a button for a movement move.
        /// </summary>
        /// <param name="move">The movement move to create a button to trigger.</param>
        private void GenerateMovementButton(MovementMove move)
        {
            //The location for drawing the button.
            int xPixels = move.x * Tile.TILE_WIDTH + Game.instance.xScroll;
            int yPixels = move.y * Tile.TILE_HEIGHT + Game.instance.yScroll;

            //Only create the button if there are enough moves left for making the move.
            if (CanMove(move.cost))
            {
                Button select = new Button(new Rectangle(xPixels, yPixels, Tile.TILE_WIDTH, Tile.TILE_HEIGHT), Assets.greenHighlight, true);
                select.RightClick += () =>
                {
                    move.Execute(true);

                    SubtractMove(move.cost);
                    foreach (Button b in movementButtons) b.Delete();
                };
                Game.instance.selectionButtons.Add(select);
                movementButtons.Add(select);
            }
        }

        /// <summary>
        /// Create a button for a attack move.
        /// </summary>
        /// <param name="move">The attack move to create a button to trigger.</param>
        private void GenerateAttackButton(AttackMove move)
        {
            int xPixels = move.def.px + Game.instance.xScroll;
            int yPixels = move.def.py + Game.instance.yScroll;

            if (CanMove(move.cost))
            {
                Button select = new Button(new Rectangle(xPixels, yPixels, Tile.TILE_WIDTH, Tile.TILE_HEIGHT), Assets.yellowHighlight, true);
                select.RightClick += () =>
                {
                    move.Execute(true);

                    SubtractMove(move.cost);
                    foreach (Button b in movementButtons) b.Delete();
                };
                Game.instance.selectionButtons.Add(select);
                movementButtons.Add(select);
            }
        }

        /// <summary>
        /// Create a button for a shoot move.
        /// </summary>
        /// <param name="move">The shoot move to create a button to trigger.</param>
        private void GenerateShootButton(ShootMove move)
        {
            int xPixels = move.def.px + Game.instance.xScroll;
            int yPixels = move.def.py + Game.instance.yScroll;

            if (CanMove(move.cost))
            {
                Button select = new Button(new Rectangle(xPixels, yPixels, Tile.TILE_WIDTH, Tile.TILE_HEIGHT), Assets.yellowHighlight, true);
                select.RightClick += () =>
                {
                    move.Execute(true);

                    SubtractMove(move.cost);
                    foreach (Button b in movementButtons) b.Delete();
                };
                Game.instance.selectionButtons.Add(select);
                movementButtons.Add(select);
            }
        }

        /// <summary>
        /// Called when this unit attacks another unit.
        /// </summary>
        /// <param name="board">The board to attack in</param>
        /// <param name="unit">The unit to attack</param>
        public void Charge(Tile[,] board, Unit unit)
        {
            int RealDamage = type.GetDamage();
            unit.Damage(board, RealDamage);

            int reflect = (int)(RealDamage * unit.type.GetReflect());
            Damage(board, reflect);
        }

        /// <summary>
        /// Shoot a unit with this unit
        /// </summary>
        /// <param name="board">The board to shoot in</param>
        /// <param name="unit">The unit to shoot</param>
        public void Shoot(Tile[,] board, Unit unit)
        {
            int RealDamage = type.GetDamage();
            unit.Damage(board, RealDamage);
        }

        /// <summary>
        /// Inflict damage to this unit
        /// </summary>
        /// <param name="board">The board to inflict the damage in</param>
        /// <param name="amount">The amount of damage to inflict</param>
        public void Damage(Tile[,] board, int amount)
        {
            hp -= amount;

            //Check for death
            if (hp <= 0)
            {
                if (type.IsHuman()) board[px / Tile.TILE_WIDTH, py / Tile.TILE_HEIGHT].unitOn = null;
                else board[px / Tile.TILE_WIDTH, py / Tile.TILE_HEIGHT].buildingOn = null;

                if (type == UnitType.town) Game.instance.CheckWin();
            }
        }

        /// <summary>
        /// Generate a context menu for this unit (buttons in the side menu for building new units).
        /// </summary>
        public virtual void GenerateContextMenu()
        {
            //Used for placing the buttons in an order
            int x = 0;
            //Get all buildable types
            foreach (UnitType t in GetBuildable())
            {
                //Make sure the type can be built with the owned resources
                if (!(t.Cost() <= Game.instance.player.resources)) continue;

                //Create the type to build button
                Button b = new Button(new Rectangle(Game.GAME_WIDTH - 300 + x * 50, 0, 50, 50), t.GetSprite());
                //Create an event for when the button is clicked:
                b.Click += () =>
                {
                    //Remove all other move buttons
                    foreach (Button del in movementButtons) del.Delete();
                    foreach (Button del in buildButtons) del.Delete();

                    //Get the building moves for this type
                    foreach (Move m in GetMoves(Game.instance.tiles))
                    {
                        if (!(m is BuildMove)) continue;
                        BuildMove move = m as BuildMove;

                        //Get the location in pixels to draw the execute build button
                        int xPixels = move.x * Tile.TILE_WIDTH + Game.instance.xScroll;
                        int yPixels = move.y * Tile.TILE_HEIGHT + Game.instance.yScroll;

                        if (move.unit != t) continue;

                        //Check that the unit can be built and can be placed on the target tile
                        if (CanMove(move.cost) && move.unit.CanBeOn(Game.instance.tiles[move.x, move.y].type))
                        {
                            //If all checks are good, create the final build button on the screen.
                            Button place = new Button(new Rectangle(xPixels, yPixels, Tile.TILE_WIDTH, Tile.TILE_HEIGHT), Assets.blueHighlight, true);
                            place.RightClick += () =>
                            {
                                move.Execute(true);

                                SubtractMove(m.cost);
                                Game.instance.player.resources -= t.Cost();
                                foreach (Button del in buildButtons) del.Delete();
                            };
                            buildButtons.Add(place);
                            Game.instance.selectionButtons.Add(place);
                        }
                    }
                };
                x++;
                buildButtons.Add(b);
                Game.instance.selectionButtons.Add(b);
            }
        }

        /// <summary>
        /// Draw the unit on the screen
        /// </summary>
        /// <param name="canvas">Monogame Canvas object for drawing the sprite on the screen</param>
        public virtual void Draw(SpriteBatch canvas)
        {
            //The location to draw the unit on
            Rectangle drawLocation = new Rectangle(px + Game.instance.xScroll, py + Game.instance.yScroll, Tile.TILE_WIDTH, Tile.TILE_HEIGHT);
            //Draw the unit's sprite on the screen
            canvas.Draw(sprite, drawLocation, Color.White);
            //Draw the amount of hp the unit has
            canvas.DrawString(Assets.font, "" + hp, new Vector2(px + Game.instance.xScroll, py + Game.instance.yScroll), player ? Color.Blue : Color.Red);

            //If the unit exhausted its moves per turn, draw an icon to signify that fact.
            if (movesLeft == 0) canvas.Draw(Assets.done, drawLocation, Color.White);
        }

        /// <summary>
        /// Get all the possible moves this unit can execute this turn
        /// </summary>
        /// <param name="board">The board to check for moves in.</param>
        /// <returns>List of Move, the moves this unit can execute.</returns>
        public abstract List<Move> GetMoves(Tile[,] board);

        /// <summary>
        /// Get all the possible unit types this unit can build.
        /// </summary>
        /// <returns>List of UnitType, the types this unit can build.</returns>
        public abstract List<UnitType> GetBuildable();

        /// <summary>
        /// Checks if the unit can move a specified amount of tiles
        /// </summary>
        /// <param name="amount">The amount to check</param>
        /// <returns>True if the unit has enough moves left this turn for the amount requested.</returns>
        private bool CanMove(int amount)
        {
            return amount <= movesLeft;
        }

        /// <summary>
        /// Subtract the amount from the moves left this turn
        /// </summary>
        /// <param name="amount">The amount of move points to subtract</param>
        private void SubtractMove(int amount)
        {
            movesLeft -= amount;

            //Remove leftover move buttons if all moves left are exhausted.
            if (movesLeft == 0)
            {
                foreach (Button del in Game.instance.selectionButtons)
                {
                    del.Delete();
                }
                Game.instance.selectionButtons.Clear();
            }
        }

        /// <summary>
        /// Clears this unit's moves left this turn variable.
        /// </summary>
        public virtual void NewTurn()
        {
            movesLeft = type.GetMaxMoves();
        }

        /// <summary>
        /// Gets a string representation of this unit.
        /// </summary>
        /// <returns>A string representation of this unit, including type, owning player and location.</returns>
        public override string ToString()
        {
            return type.ToString() + " belonging to " + (player ? "human" : "computer") + " player on " + (TileX+1) + "," + (TileY+1);
        }

    }
}
