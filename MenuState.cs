using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class MenuState
    {
        public readonly Button newGame, exit;

        public MenuState()
        {
            newGame = new Button(new Point(C.screenWidth / 2, C.screenHeight / 3), "new game");
            exit = new Button(new Point(C.screenWidth / 2, C.screenHeight * 2 / 3), "exit");
        }

        public void Update()
        {
            newGame.Update();
            exit.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            newGame.Draw(spriteBatch);
            exit.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
