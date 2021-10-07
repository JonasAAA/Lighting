using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1
{
    public class PlayState
    {
        private static Camera camera;

        public bool HasWon { get; private set; }
        public bool HasLost { get; private set; }

        private readonly List<Light> lights;
        private readonly List<IObstacle> obstacles;
        private readonly LightDiskPlayer player;
        private readonly Background background;

        public static void EarlyInitialize(GraphicsDeviceManager graphics)
        {
            Light.EarlyInitialize(graphics);
        }

        public static void Initialize(GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            camera = new Camera();

            Light.Initialize(GraphicsDevice, camera);
            Polygon.Initialize(GraphicsDevice, camera);
            Image.Initialize(Content);
        }

        public PlayState()
        {
            HasWon = false;
            HasLost = false;

            player = new LightDiskPlayer(new Vector2(600, 600), 16, 0, Keys.Up, Keys.Left, Keys.Down, Keys.Right, Color.Black * 0f, 1f, Color.Yellow /*new Color(0, 255, 0)*/);

            obstacles = new List<IObstacle>()
            {
                new DiskObst(new Vector2(500, 500), 32, 0, "disk", Color.Black/*White*/),
                new DiskObst(new Vector2(1700, 500), 64, 0, "disk", Color.Black/*Red*/),
                new SegmentObst(new Vector2(700, 700), new Vector2(700, 500), 2, Color.Black/*Green*/),
                new PolygonObst(new List<Vector2>() { new Vector2(1800, 900), new Vector2(1600, 800), new Vector2(1700, 700) }, 0f, Color.Black)
            };

            lights = new List<Light>()
            {
                new Light(new Vector2(1920/2, 1080/2), 0.5f, Color.White),
                new RotatingLight(new Vector2(1920/2, 300), MathHelper.TwoPi * 0.75f, MathHelper.Pi * 0.125f, 1f, 1f, Color.Magenta /*new Color(255, 0, 0)*/),
                new Light(new Vector2(1920, 1080/2), 1f, Color.Cyan /*new Color(0, 0, 255)*/),
                player.light,
            };

            foreach (Light light in lights)
            {
                //if (light != player.light)
                //    light.AddObject(player);

                foreach (IObstacle obstacle in obstacles)
                    light.AddObject(obstacle);
            }

            background = new Background(new Point(C.screenWidth / 2, C.screenHeight / 2), "random background", Color.White);
        }

        public void Update(float elapsed)
        {
            player.Update(elapsed);

            foreach (IObstacle obstacle in obstacles)
                obstacle.Collide(player);

            camera.Update(player.Position);

            foreach (Light light in lights)
                light.Update(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            camera.BeginDraw(spriteBatch);
            background.Draw(spriteBatch);
            spriteBatch.End();


            Light.BeginDraw();
            foreach (Light light in lights)
                light.Draw();
            Light.EndDraw(spriteBatch);
            

            camera.BeginDraw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (IObstacle obstacle in obstacles)
                obstacle.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
