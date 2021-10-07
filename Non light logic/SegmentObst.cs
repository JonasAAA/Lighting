using Microsoft.Xna.Framework;

namespace Game1
{
    public class SegmentObst : Segment, IObstacle
    {
        private readonly DiskObst startDisk, endDisk;

        public SegmentObst(Vector2 startPos, Vector2 endPos, float radius, Color color)
            : base(startPos, endPos, radius, color)
        {
            startDisk = new DiskObst(startPos, radius, 0, "disk", color);
            endDisk = new DiskObst(endPos, radius, 0, "disk", color);
        }

        public void Collide(DiskPlayer player)
        {
            float t = Vector2.Dot(dir, player.Position - startPos);
            if (t < 0)
            {
                startDisk.Collide(player);
                return;
            }
            if (t > length)
            { 
                endDisk.Collide(player);
                return;
            }
            DiskObst temp = new DiskObst(startPos + t * dir, radius, 0, "disk", Color.White);
            temp.Collide(player);
        }
    }
}
