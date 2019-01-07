using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilization0.gui
{
	public class Button
	{

		private Rectangle hitbox;
		public Texture2D texture;

		public bool scrolled = false;

		public delegate void ClickEvent();

		public ClickEvent Click;

		public Button(Rectangle hitbox, Texture2D texture)
		{
			this.hitbox = hitbox;
			this.texture = texture;

			Game.instance.buttons.Add(this);
		}

		public Button(Rectangle hitbox, Texture2D texture, bool scrolled)
		{
			this.hitbox = hitbox;
			this.texture = texture;
			this.scrolled = scrolled;

			Game.instance.buttons.Add(this);
		}

		internal void Draw(SpriteBatch canvas)
		{
			Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
			canvas.Draw(texture, scrolled?scrollHitbox:hitbox, Color.White);
		}

		public Rectangle GetHitbox()
		{
			Rectangle scrollHitbox = new Rectangle(hitbox.X + Game.instance.xScroll, hitbox.Y + Game.instance.yScroll, hitbox.Width, hitbox.Height);
			return scrolled ? scrollHitbox : hitbox;
		}

		public void Delete()
		{
			Game.instance.buttons.Remove(this);
		}
	}
}
