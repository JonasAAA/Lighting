using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Button
    {
        public static ContentManager Content;
        public static Color pasCol, actCol;
        public bool IsPressed { get; private set; }
        public bool IsClicked { get; private set; }

        private bool wasPressed, isActive;

        private readonly Texture2D picture;
        private readonly Vector2 origin;
        private readonly Rectangle destRect;

        public static void Initialize(ContentManager newContent, Color newPasCol, Color newActCol)
        {
            Content = newContent;
            pasCol = newPasCol;
            actCol = newActCol;
        }

        public Button(Point center, string imageName)
        {
            IsPressed = false;
            IsClicked = false;
            wasPressed = false;
            isActive = false;

            picture = Content.Load<Texture2D>(imageName);
            origin = Vector2.Zero;
            destRect = new Rectangle(center.X - picture.Width / 2, center.Y - picture.Height / 2, picture.Width, picture.Height);
        }

        public void Update()
        {
            wasPressed = IsPressed;

            MouseState mouseState = Mouse.GetState();
            if (destRect.Contains(mouseState.Position))
                isActive = true;
            else
                isActive = false;
    
            IsPressed = mouseState.LeftButton == ButtonState.Pressed && isActive;
            if (!IsPressed && wasPressed)
                IsClicked = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isActive)
                color = actCol;
            else
                color = pasCol;

            spriteBatch.Draw(picture, destRect, null, color, 0, origin, SpriteEffects.None, 0);
        }
    }
}
