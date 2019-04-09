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
    /// A 2D rectangle for drawing a sprite on the screen
    /// </summary>
	public class Panel
	{

        //A rectangle around the panel
        private Rectangle hitbox;

        //The texture of the panel
        public Texture2D texture;

        //True if the panel should be scrolled with the board, false if its in a fixed position.
        public bool scrolled = false;

        /// <summary>
        /// Creates a new panel
        /// </summary>
        /// <param name="hitbox">The rectnagle to create the panel in (location and size)</param>
        /// <param name="texture">The texture to draw the panel with</param>
        /// <param name="scrolled">True if the panel should scroll with the board or false if it should stay fixed</param>
        /// <param name="add">True to add the panel to the screen automatically, false to ignore</param>
        public Panel(Rectangle hitbox, Texture2D texture, bool add = true)
        {
            this.hitbox = hitbox;
            this.texture = texture;

            if (add) Game.instance.panels.Add(this);
        }

        /// <summary>
        /// Draws the panel on the screen
        /// </summary>
        /// <param name="canvas">Monogame's Canvas object to draw on</param>
        internal void Draw(SpriteBatch canvas)
        {
            Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
            canvas.Draw(texture, scrolled ? scrollHitbox : hitbox, Color.White);
        }

        /// <summary>
        /// Get the hitbox of the panel.
        /// </summary>
        /// <returns>Rectangle object of the hitbox of the panel</returns>
        public Rectangle GetHitbox()
        {
            Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
            return scrolled ? scrollHitbox : hitbox;
        }

        /// <summary>
        /// Deletes the panel from the screen
        /// </summary>
        public void Delete()
        {
            Game.instance.panels.Remove(this);
        }

    }
}
