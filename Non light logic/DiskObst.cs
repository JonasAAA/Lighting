using Microsoft.Xna.Framework;

namespace Game1
{
    public class DiskObst : Disk, IObstacle
    {
        public DiskObst(Vector2 position, float radius, float rotation, string imageName, Color color)
            : base(position, radius, rotation, imageName, color)
        { }

        public void Collide(DiskPlayer player)
        {
            if (Vector2.Distance(Position, player.Position) >= radius + player.radius)
                return;

            Vector2 dir = player.Position - Position;
            dir.Normalize();

            player.Position = Position + dir * (radius + player.radius);
            //player.velocity -= dir * Vector2.Dot(dir, player.velocity);
        }
    }
}
