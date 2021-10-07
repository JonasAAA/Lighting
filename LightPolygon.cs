using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game1
{
    public class LightPolygon
    {
        private static GraphicsDevice GraphicsDevice;
        private static Camera camera;
        private static BasicEffect basicEffect;

        private Vector2 center;
        private List<Vector2> vertices;
        private VertexPositionColorTexture[] vertPosCol;
        private int[] ind;

        private readonly float strength;
        private readonly Color color;

        private const int maxWidth = 1024;

        static private RenderTarget2D renderTarget;

        public static void EarlyInitialize(GraphicsDeviceManager graphics)
        {
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(SetToPreserve);
        }

        public static void Initialize(GraphicsDevice newGraphicsDevice, Camera newCamera)
        {
            GraphicsDevice = newGraphicsDevice;

            renderTarget = new RenderTarget2D(GraphicsDevice, C.screenWidth, C.screenHeight);
            // I copied this. It makes so that when switching back to the default render target, it wouldn't erase what we did now. Not sure why this works without it.
            // new RenderTarget2D(GraphicsDevice, C.screenWidth, C.screenHeight, false, GraphicsDevice.PresentationParameters.BackBufferFormat, GraphicsDevice.PresentationParameters.DepthStencilFormat, GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);

            camera = newCamera;
            Texture2D texture = new Texture2D(GraphicsDevice, maxWidth, maxWidth);
            Color[] colorData = new Color[maxWidth * maxWidth];
            for (int i = 0; i < maxWidth; i++)
                for (int j = 0; j < maxWidth; j++)
                    colorData[i * maxWidth + j] = CalcColor(Vector2.Distance(new Vector2(maxWidth / 2, maxWidth / 2), new Vector2(i, j)));
            texture.SetData(colorData);

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
            basicEffect.Texture = texture;
        }

        public LightPolygon(Vector2 center, List<Vector2> vertices, float strength, Color color)
            : this(strength, color)
        {
            this.center = center;
            this.vertices = vertices;
            Update(center, vertices);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strength">a positive float which determins the radius of the lit area</param>
        /// <param name="color"></param>
        public LightPolygon(float strength, Color color)
        {
            vertices = new List<Vector2>();
            vertPosCol = new VertexPositionColorTexture[0];
            ind = new int[0];
            this.strength = strength;
            this.color = color;
        }

        public void Update(Vector2 center, List<Vector2> vertices)
        {
            this.center = center;
            this.vertices = vertices;
            int centerInd = vertices.Count;
            vertPosCol = new VertexPositionColorTexture[centerInd + 1];

            ind = new int[vertices.Count * 3];
            for (int i = 0; i < vertices.Count; i++)
            {
                // may need to swap the last two
                ind[3 * i] = centerInd;
                ind[3 * i + 1] = i;
                ind[3 * i + 2] = (i + 1) % vertices.Count;
            }
        }

        public static void BeginDraw()
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
        }

        public void Draw()
        {
            int centerInd = vertices.Count;
            vertPosCol[centerInd] = new VertexPositionColorTexture(Transform(center), color, new Vector2(0.5f, 0.5f));
            for (int i = 0; i < centerInd; i++)
                vertPosCol[i] = new VertexPositionColorTexture(Transform(vertices[i]), color, new Vector2(0.5f, 0.5f) + (vertices[i] - center) / maxWidth / strength);
            if (vertPosCol.Count() == 0)
                return;

            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            GraphicsDevice.RasterizerState = rasterizerState;

            GraphicsDevice.BlendState = BlendState.NonPremultiplied;

            //GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertPosCol, 0, vertPosCol.Count(), ind, 0, ind.Length / 3);
            }
        }

        public static void EndDraw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.SetRenderTarget(null);

            BlendState blendState = new BlendState()
            {
                AlphaBlendFunction = BlendState.AlphaBlend.AlphaBlendFunction,
                AlphaSourceBlend = BlendState.AlphaBlend.ColorSourceBlend,
                AlphaDestinationBlend = BlendState.AlphaBlend.AlphaDestinationBlend,
                BlendFactor = Color.White,
                ColorBlendFunction = BlendFunction.Add,//BlendFunction.Max,
                ColorSourceBlend = BlendState.AlphaBlend.ColorSourceBlend,
                ColorDestinationBlend = Blend.SourceAlpha, //BlendState.AlphaBlend.ColorDestinationBlend,
            };

            GraphicsDevice.BlendState = blendState;

            spriteBatch.Begin(SpriteSortMode.Deferred, blendState, null, null, null, null, null);

            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);

            spriteBatch.End();
        }

        private static Color CalcColor(float distFromLight)
        {
            //colorData[i * maxWidth + j] = Color.White * (float)Math.Exp(-relDist / 200);
            //colorData[i * maxWidth + j] = Color.White * (float)Math.Exp(-relDist * relDist / 50000);
            //colorData[i * maxWidth + j] = new Color(0f, 0f, 0f, (float)Math.Exp(-1 / relDist / relDist * 5000));

            float factor = (float)Math.Exp(-distFromLight * distFromLight / 50000);
            return new Color(0.2f, 0.2f, 0.2f, 1) * factor;
        }

        private static Vector3 Transform(Vector2 pos)
        {
            Vector2 transPos = Vector2.Transform(pos, camera.Transform);
            return new Vector3(2 * transPos.X / C.screenWidth - 1, 1 - 2 * transPos.Y / C.screenHeight, 0);
        }

        // I copied this. It makes so that switching to renderTarget doesn't clear the screen automatically
        private static void SetToPreserve(object sender, PreparingDeviceSettingsEventArgs eventargs)
        {
            eventargs.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }
    }
}
