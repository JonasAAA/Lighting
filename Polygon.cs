using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1
{
    public class Polygon : IShadowCastingObject
    {
        private readonly List<Segment> segments;
        private readonly EdgelessPolygon edgelessPoly;

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

        public void RelAngles(Vector2 lightPos, List<float> relAngles)
        {
            foreach (Segment segment in segments)
                segment.RelAngles(lightPos, relAngles);
        }

        public void InterPoint(Vector2 lightPos, Vector2 lightDir, List<float> interPoints)
        {
            foreach (Segment segment in segments)
                segment.InterPoint(lightPos, lightDir, interPoints);
        }
    }
}
