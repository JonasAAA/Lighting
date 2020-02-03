using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        private float scale;

        public Camera()
        {
            Transform = new Matrix();
            scale = 1;
        }

        public void Update(DiskPlayer player)
        {
            Transform = Matrix.CreateTranslation(C.screenWidth * 0.5f, C.screenHeight * 0.5f, 0)
                        //* Matrix.CreateRotationZ(player.rotation)
                        * Matrix.CreateScale(scale)
                        * Matrix.CreateTranslation(-player.position.X, -player.position.Y, 0);
        }

        public void BeginDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Transform);
        }
    }
}
