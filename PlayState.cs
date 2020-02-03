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

        public static void Initialize(GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            camera = new Camera();

            LightPolygon.Initialize(GraphicsDevice, camera);
            EdgelessPolygon.Initialize(GraphicsDevice, camera);
            Image.Initialize(Content);
        }

        public PlayState()
        {
            HasWon = false;
            HasLost = false;
            
            player = new LightDiskPlayer(new Vector2(600, 600), 16, 0, Keys.Up, Keys.Left, Keys.Down, Keys.Right, Color.Black * 0f, 1f, new Color(0, 255, 0));

            obstacles = new List<IObstacle>()
            {
                new DiskObst(new Vector2(500, 500), 32, 0, "disk", Color.Black/*White*/),
                new DiskObst(new Vector2(1700, 500), 64, 0, "disk", Color.Black/*Red*/),
                new SegmentObst(new Vector2(700, 700), new Vector2(700, 500), 2, Color.Black/*Green*/),
                new PolygonObst(new List<Vector2>() { new Vector2(1800, 900), new Vector2(1600, 800), new Vector2(1700, 700) }, 2, Color.Black)
            };

            lights = new List<Light>()
            {
                player.light,
                new Light(new Vector2(1920/2, 1080/2), 2f, Color.White),
                new RotatingLight(new Vector2(1920/2, 400), MathHelper.TwoPi * 0.75f, MathHelper.Pi * 0.125f, 1f, 2f, new Color(255, 0, 0)),
                new Light(new Vector2(1920, 1080/2), 1f, new Color(0, 0, 255)),
            };

            foreach (Light light in lights)
            {
                //if (light != player.light)
                //    light.AddObject(player);

                foreach (IObstacle obstacle in obstacles)
                    light.AddObject(obstacle);
            }
        }

        public void Update(float elapsed)
        {
            player.Update(elapsed);

            foreach (IObstacle obstacle in obstacles)
                obstacle.Collide(player);

            camera.Update(player);

            foreach (Light light in lights)
                light.Update(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Light light in lights)
                light.Draw();

            camera.BeginDraw(spriteBatch);

            player.Draw(spriteBatch);

            foreach (IObstacle obstacle in obstacles)
                obstacle.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
