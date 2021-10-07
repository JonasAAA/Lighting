using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        enum GameState
        {
            menu,
            play,
            // TODO: pause
            pause,
            win,
            lose
        }

        private GameState gameState;

        private MenuState menuState;
        private PlayState playState;
        private WinState winState;
        private LoseState loseState;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = C.screenWidth;
            graphics.PreferredBackBufferHeight = C.screenHeight;
            //graphics.IsFullScreen = true;

            PlayState.EarlyInitialize(graphics);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PlayStateUnused.Initialize(GraphicsDevice, Content);
            PlayState.Initialize(GraphicsDevice, Content);
            Button.Initialize(Content, Color.White, Color.Yellow);

            //gameState = GameState.menu;
            //menuState = new MenuState();
            //IsMouseVisible = true;

            gameState = GameState.play;
            playState = new PlayState();
            IsMouseVisible = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (gameState)
            {
                case GameState.menu:
                    menuState.Update();
                    if (menuState.newGame.IsClicked)
                    {
                        gameState = GameState.play;
                        playState = new PlayState();
                        IsMouseVisible = false;
                    }
                    if (menuState.exit.IsClicked)
                        Exit();
                    break;
                case GameState.play:
                    playState.Update(elapsed);
                    if (playState.HasWon)
                    {
                        gameState = GameState.win;
                        winState = new WinState();
                        IsMouseVisible = true;
                    }
                    if (playState.HasLost)
                    {
                        gameState = GameState.lose;
                        loseState = new LoseState();
                        IsMouseVisible = true;
                    }
                    break;
                case GameState.win:
                    winState.Update();
                    if (winState.mainMenu.IsClicked)
                    {
                        gameState = GameState.menu;
                        menuState = new MenuState();
                        IsMouseVisible = true;
                    }
                    if (winState.restart.IsClicked)
                    {
                        gameState = GameState.play;
                        playState = new PlayState();
                        IsMouseVisible = false;
                    }
                    break;
                case GameState.lose:
                    loseState.Update();
                    if (loseState.mainMenu.IsClicked)
                    {
                        gameState = GameState.menu;
                        menuState = new MenuState();
                        IsMouseVisible = true;
                    }
                    if (loseState.restart.IsClicked)
                    {
                        gameState = GameState.play;
                        playState = new PlayState();
                        IsMouseVisible = false;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            switch (gameState)
            {
                case GameState.menu:
                    menuState.Draw(spriteBatch);
                    break;
                case GameState.play:
                    playState.Draw(spriteBatch);
                    break;
                case GameState.win:
                    winState.Draw(spriteBatch);
                    break;
                case GameState.lose:
                    loseState.Draw(spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
