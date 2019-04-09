using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.gui
{

    /// <summary>
    /// Represents a button on the screen that can be left clicked and right clicked
    /// </summary>
    public class Button
	{

        //The hitbox of the button (location and size).
		private Rectangle hitbox;
        //The texture of the button.
		public Texture2D texture;

        //True if the button should scroll with the board, false if it should be in a fixed position.
		public bool scrolled = false;

        //A delegate for handeling events.
		public delegate void ClickEvent();

        //Events for left click and right click.
		public ClickEvent Click;
        public ClickEvent RightClick;


        /// <summary>
        /// Create a new button
        /// </summary>
        /// <param name="hitbox">The hitbox (location and size) of the button</param>
        /// <param name="texture">The texture to draw the button with</param>
        /// <param name="add">True if the button should be added automatically to the screen</param>
		public Button(Rectangle hitbox, Texture2D texture, bool add = true)
		{
			this.hitbox = hitbox;
			this.texture = texture;

			if(add) Game.instance.buttons.Add(this);
		}

        /// <summary>
        /// Draws the button on the screen.
        /// </summary>
        /// <param name="canvas">The Monogam's Canvas object to draw on</param>
		internal void Draw(SpriteBatch canvas)
		{
			Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
			canvas.Draw(texture, scrolled?scrollHitbox:hitbox, Color.White);
		}

        /// <summary>
        /// Gets the hitbox of the button
        /// </summary>
        /// <returns>The rectangle object of the hitbox of the button</returns>
		public Rectangle GetHitbox()
		{
			Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
			return scrolled ? scrollHitbox : hitbox;
		}

        /// <summary>
        /// Deletes the button from the screen.
        /// </summary>
		public void Delete()
		{
			Game.instance.buttons.Remove(this);
		}
	}
}
