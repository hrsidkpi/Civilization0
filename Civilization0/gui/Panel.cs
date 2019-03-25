using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.gui
{
	public class Panel
	{

        private Rectangle hitbox;
        public Texture2D texture;

        public bool scrolled = false;

        public Panel(Rectangle hitbox, Texture2D texture, bool add = true)
        {
            this.hitbox = hitbox;
            this.texture = texture;

            if (add) Game.instance.panels.Add(this);
        }

        public Panel(Rectangle hitbox, Texture2D texture, bool scrolled, bool add = true)
        {
            this.hitbox = hitbox;
            this.texture = texture;
            this.scrolled = scrolled;

            if (add) Game.instance.panels.Add(this);
        }

        internal void Draw(SpriteBatch canvas)
        {
            Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
            canvas.Draw(texture, scrolled ? scrollHitbox : hitbox, Color.White);
        }

        public Rectangle GetHitbox()
        {
            Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
            return scrolled ? scrollHitbox : hitbox;
        }

        public void Delete()
        {
            Game.instance.panels.Remove(this);
        }

    }
}
