using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Game1
{
    public class Polygon : IShadowCastingObject
    {
        private readonly List<Segment> segments;
        private readonly EdgelessPolygon edgelessPoly;

        public static void Initialize(GraphicsDevice GraphicsDevice, Camera camera)
        {
            EdgelessPolygon.Initialize(GraphicsDevice, camera);
        }

        public Polygon(List<Vector2> vertices, float radius, Color color)
        {
            segments = new List<Segment>();
            for (int i = 0; i < vertices.Count; i++)
                segments.Add(new Segment(vertices[i], vertices[(i + 1) % vertices.Count], radius, color));

            edgelessPoly = new EdgelessPolygon(vertices[0], vertices, color);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            edgelessPoly.Draw();
            foreach (Segment segment in segments)
                segment.Draw(spriteBatch);
        }

        public IEnumerable<float> RelAngles(Vector2 lightPos)
        {
            return
                from segment in segments
                from angle in segment.RelAngles(lightPos)
                select angle;
            //foreach (Segment segment in segments)
            //    segment.RelAngles(lightPos, relAngles);
        }

        public IEnumerable<float> InterPoint(Vector2 lightPos, Vector2 lightDir)
        {
            return
                from segment in segments
                from interPoint in segment.InterPoint(lightPos, lightDir)
                select interPoint;
            //foreach (Segment segment in segments)
            //    segment.InterPoint(lightPos, lightDir, interPoints);
        }
    }
}
