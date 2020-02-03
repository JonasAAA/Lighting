using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Background : Image
    {
        private readonly Vector2 position;

        public Background(Point center, string imageName, Color color)
            : base(imageName, color)
        {
            position = center.ToVector2();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch, position);
        }
    }
}
