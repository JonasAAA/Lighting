using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Image
    {
        private static ContentManager Content;
        public readonly Color color;

        private readonly Texture2D picture;
        private readonly Vector2 origin;
        private readonly Vector2 scale;

        public static void Initialize(ContentManager newContent)
        {
            Content = newContent;
        }

        public Image(string imageName, Color color)
        {
            picture = Content.Load<Texture2D>(imageName);
            origin = new Vector2(picture.Width * 0.5f, picture.Height * 0.5f);
            scale = new Vector2(1);

            this.color = color;
        }

        public Image(string imageName, float width, Color color)
        {
            picture = Content.Load<Texture2D>(imageName);
            origin = new Vector2(picture.Width * 0.5f, picture.Height * 0.5f);
            scale = new Vector2(width / picture.Width);

            this.color = color;
        }

        public Image(string imageName, float width, float height, Color color)
        {
            picture = Content.Load<Texture2D>(imageName);
            origin = new Vector2(picture.Width * 0.5f, picture.Height * 0.5f);
            scale = new Vector2(width / picture.Width, height / picture.Height);
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation = 0)
        {
            spriteBatch.Draw(picture, position, null, color, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}
