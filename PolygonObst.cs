using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1
{
    public class PolygonObst : Polygon, IObstacle
    {
        private readonly List<SegmentObst> segments;

        public PolygonObst(List<Vector2> vertices, float radius, Color color)
            : base(vertices, radius, color)
        {
            segments = new List<SegmentObst>();
            for (int i = 0; i < vertices.Count; i++)
                segments.Add(new SegmentObst(vertices[i], vertices[(i + 1) % vertices.Count], radius, color));
        }

        public void Collide(DiskPlayer player)
        {
            foreach (SegmentObst segment in segments)
                segment.Collide(player);
        }
    }
}
