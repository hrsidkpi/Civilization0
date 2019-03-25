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

        public int px, py;
        public int TileX { get { return px / Tile.TILE_WIDTH; } set { px = value * Tile.TILE_WIDTH; } }
        public int TileY { get { return py / Tile.TILE_HEIGHT; } set { py = value * Tile.TILE_HEIGHT; } }

        public UnitType type;
        private Texture2D sprite;
        public bool player;

        public int movesLeft;

        public int hp;

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

        public abstract void Initialize();
        public abstract void Update();

        public virtual void RightClick() { }

        public virtual void Click()
        {
            foreach (Button del in Game.instance.selectionButtons)
            {
                del.Delete();
            }
            Game.instance.selectionButtons.Clear();

            if (movesLeft == 0) return;
            if (!player) return;

            GenerateMoves();
            GenerateContextMenu();

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

        private List<Button> movementButtons = new List<Button>();
        private List<Button> buildButtons = new List<Button>();


        public virtual void GenerateMoves()
        {
            foreach (Move m in GetMoves())
            {
                if (m is MovementMove) GenerateMovementButton(m as MovementMove);
                if (m is AttackMove) GenerateAttackButton(m as AttackMove);
                if (m is ShootMove) GenerateShootButton(m as ShootMove);
            }
        }

        private void GenerateMovementButton(MovementMove move)
        {
            int xPixels = move.x * Tile.TILE_WIDTH + Game.instance.xScroll;
            int yPixels = move.y * Tile.TILE_HEIGHT + Game.instance.yScroll;


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

        public void Charge(Unit unit)
        {
            int RealDamage = type.GetDamage() - unit.type.GetArmor();
            unit.Damage(RealDamage);

            int reflect = (int)(RealDamage * unit.type.GetReflect());
            Damage(reflect);
        }

        public void Shoot(Unit unit)
        {
            int RealDamage = type.GetDamage() - unit.type.GetArmor();
            unit.Damage(RealDamage);
        }

        public void Damage(int amount)
        {
            hp -= amount;
            if (hp <= 0)
            {
                if (type.IsHuman()) Game.instance.tiles[px / Tile.TILE_WIDTH, py / Tile.TILE_HEIGHT].unitOn = null;
                else Game.instance.tiles[px / Tile.TILE_WIDTH, py / Tile.TILE_HEIGHT].buildingOn = null;

                if (type == UnitType.town) Game.instance.CheckWin();
            }
        }

        public virtual void GenerateContextMenu()
        {
            int x = 0;
            foreach (UnitType t in GetBuildable())
            {
                if (!(t.Cost() <= Game.instance.player.resources)) continue;
                Button b = new Button(new Rectangle(Game.GAME_WIDTH - 300 + x * 50, 0, 50, 50), t.GetSprite());
                b.Click += () =>
                {
                    foreach (Button del in movementButtons) del.Delete();
                    foreach (Button del in buildButtons) del.Delete();

                    foreach (Move m in GetMoves())
                    {
                        if (!(m is BuildMove)) continue;
                        BuildMove move = m as BuildMove;

                        int xPixels = move.x * Tile.TILE_WIDTH + Game.instance.xScroll;
                        int yPixels = move.y * Tile.TILE_HEIGHT + Game.instance.yScroll;

                        if (move.unit != t) continue;

                        if (CanMove(move.cost) && move.unit.CanBeOn(Game.instance.tiles[move.x, move.y].type))
                        {
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

        public virtual void Draw(SpriteBatch canvas)
        {
            Rectangle drawLocation = new Rectangle(px + Game.instance.xScroll, py + Game.instance.yScroll, Tile.TILE_WIDTH, Tile.TILE_HEIGHT);
            canvas.Draw(sprite, drawLocation, Color.White);
            canvas.DrawString(Assets.font, "" + hp, new Vector2(px + Game.instance.xScroll, py + Game.instance.yScroll), player ? Color.Blue : Color.Red);
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
        public virtual void NewTurn()
        {
            movesLeft = type.GetMaxMoves();
        }

        public override string ToString()
        {
            return type.ToString() + " belonging to " + (player ? "human" : "computer") + " player on " + (TileX+1) + "," + (TileY+1);
        }

    }
}
