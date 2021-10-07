using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class WinState
    {
        public readonly Button restart, mainMenu;

        private readonly Background background;

        public WinState()
        {
            background = new Background(new Point(C.screenWidth / 2, C.screenHeight / 4), "you win", Color.Yellow);
            restart = new Button(new Point(C.screenWidth / 2, C.screenHeight * 2 / 4), "restart");
            mainMenu = new Button(new Point(C.screenWidth / 2, C.screenHeight * 3 / 4), "main menu");
        }

        public void Update()
        {
            restart.Update();
            mainMenu.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            background.Draw(spriteBatch);
            restart.Draw(spriteBatch);
            mainMenu.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
