using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Game1
{
    public class EdgelessPolygon
    {
        private static GraphicsDevice GraphicsDevice;
        private static Camera camera;
        //public static int screenWidth, screenHeight;

        private Vector2 center;
        private List<Vector2> vertices;
        private VertexPositionColor[] vertPosCol;
        private int[] ind;

        private readonly BasicEffect basicEffect;
        private readonly Color color;

        public static void Initialize(GraphicsDevice newGraphicsDevice, Camera newCamera)
        {
            GraphicsDevice = newGraphicsDevice;
            camera = newCamera;
        }

        public EdgelessPolygon(Vector2 center, List<Vector2> vertices, Color color)
            : this(color)
        {
            this.center = center;
            this.vertices = vertices;
            Update(center, vertices);
        }

        public EdgelessPolygon(Color color)
        {
            vertices = new List<Vector2>();
            vertPosCol = new VertexPositionColor[0];
            ind = new int[0];
            basicEffect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };
            this.color = color;
        }

        public void Update(Vector2 center, List<Vector2> vertices)
        {
            this.center = center;
            this.vertices = vertices;
            int centerInd = vertices.Count;
            vertPosCol = new VertexPositionColor[centerInd + 1];
            //vertPosCol[centerInd] = new VertexPositionColor(Transform(center), color);
            //for (int i = 0; i < centerInd; i++)
            //    vertPosCol[i] = new VertexPositionColor(Transform(vertices[i]), color);

            ind = new int[vertices.Count * 3];
            for (int i = 0; i < vertices.Count; i++)
            {
                // may need to swap the last two
                ind[3 * i] = centerInd;
                ind[3 * i + 1] = i;
                ind[3 * i + 2] = (i + 1) % vertices.Count;
            }
        }

        public void Draw()
        {
            int centerInd = vertices.Count;
            vertPosCol[centerInd] = new VertexPositionColor(Transform(center), color);
            for (int i = 0; i < centerInd; i++)
                vertPosCol[i] = new VertexPositionColor(Transform(vertices[i]), color);

            if (vertPosCol.Count() == 0)
                return;

            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertPosCol, 0, vertPosCol.Count(), ind, 0, ind.Length / 3);
            }
        }

        private Vector3 Transform(Vector2 pos)
        {
            Vector2 transPos = Vector2.Transform(pos, camera.Transform);
            return new Vector3(2 * transPos.X / C.screenWidth - 1, 1 - 2 * transPos.Y / C.screenHeight, 0);
        }
    }
}
