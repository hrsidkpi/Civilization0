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

    public abstract class Unit
    {

        public int x, y;
        public UnitType type;
        private Texture2D sprite;
        public bool player;

        public int movesLeft;

        public int hp;

        public Unit(int x, int y, UnitType type, bool player)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            this.sprite = type.GetSprite();
            this.player = player;

            movesLeft = type.GetMaxMoves();
            hp = type.GetMaxHp();
        }

        public abstract void Initialize();
        public abstract void Update();

        public virtual void Click()
        {
            foreach (Button del in Game.instance.selectionButtons)
            {
                del.Delete();
            }
            Game.instance.selectionButtons.Clear();

            if (movesLeft == 0) return;

            GenerateMovementMoves();
            GenerateContextMenu();
        }

        private List<Button> movementButtons = new List<Button>();
        private List<Button> buildButtons = new List<Button>();

        public virtual void GenerateMovementMoves()
        {
            foreach (Move m in GetMoves())
            {
                if (!(m is MovementMove)) continue;
                MovementMove move = m as MovementMove;
                int xPixels = move.x * Tile.TILE_WIDTH + Game.instance.xScroll;
                int yPixels = move.y * Tile.TILE_HEIGHT + Game.instance.yScroll;


                if (CanMove(m.cost))
                {
                    Button select = new Button(new Rectangle(xPixels, yPixels, Tile.TILE_WIDTH, Tile.TILE_HEIGHT), Assets.greenHighlight, true);
                    select.Click += () =>
                    {
                        Game.instance.tiles[move.x, move.y].unitsOn.Add(this);
                        Game.instance.tiles[x / Tile.TILE_WIDTH, y / Tile.TILE_HEIGHT].unitsOn.Remove(this);
                        x = move.x * Tile.TILE_WIDTH;
                        y = move.y * Tile.TILE_WIDTH;

                        SubtractMove(m.cost);
                        foreach (Button b in movementButtons) b.Delete();
                    };
                    Game.instance.selectionButtons.Add(select);
                    movementButtons.Add(select);
                }
            }
        }

        public virtual void GenerateContextMenu()
        {
            int x = 0;
            foreach (UnitType t in GetBuildable())
            {
                Button b = new Button(new Rectangle(Game.GAME_WIDTH - 300 + x * 50, 0, 50, 50), t.GetSprite());
                b.Click += () =>
                {
                    foreach (Button del in movementButtons) del.Delete();
                    foreach (Move m in GetMoves())
                    {
                        if (!(m is BuildMove)) continue;
                        BuildMove move = m as BuildMove;

                        int xPixels = move.x * Tile.TILE_WIDTH + Game.instance.xScroll;
                        int yPixels = move.y * Tile.TILE_HEIGHT + Game.instance.yScroll;

                        if (move.unit != t) continue;

                        if (CanMove(move.cost))
                        {
                            Button place = new Button(new Rectangle(xPixels, yPixels, Tile.TILE_WIDTH, Tile.TILE_HEIGHT), Assets.blueHighlight, true);
                            place.Click += () =>
                            {
                                Game.instance.tiles[move.x, move.y].unitsOn.Add(t.BuildOnTile(move.x, move.y));
                                SubtractMove(m.cost);
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

        public virtual void Draw(SpriteBatch canvas)
        {
            Rectangle drawLocation = new Rectangle(x + Game.instance.xScroll, y + Game.instance.yScroll, Tile.TILE_WIDTH, Tile.TILE_HEIGHT);
            canvas.Draw(sprite, drawLocation, Color.White);
            canvas.DrawString(Assets.font, ""+hp, new Vector2(x + Game.instance.xScroll, y + Game.instance.yScroll), Color.White);
            if (movesLeft == 0) canvas.Draw(Assets.done, drawLocation, Color.White);
        }

        public abstract List<Move> GetMoves();
        public abstract List<UnitType> GetBuildable();

        private bool CanMove(int amount)
        {
            return amount <= movesLeft;
        }

        private void SubtractMove(int amount)
        {
            movesLeft -= amount;
            if (movesLeft == 0)
            {
                foreach (Button del in Game.instance.selectionButtons)
                {
                    del.Delete();
                }
                Game.instance.selectionButtons.Clear();
            }
        }
        public void NewTurn()
        {
            movesLeft = type.GetMaxMoves();
        }

    }
}
