using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class DiskPlayer : Disk
    {
        private readonly float maxMoveDist;
        private readonly Keys up, left, down, right;

        public DiskPlayer(Vector2 position, float radius, float rotation, Keys up, Keys left, Keys down, Keys right, Color color)
            : base(position, radius, rotation, "disk", color)
        {
            this.up = up;
            this.left = left;
            this.down = down;
            this.right = right;
            maxMoveDist = 200;
        }

        public void Update(float elapsed)
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector2 moveDir = Vector2.Zero;

            if (keyState.IsKeyDown(up))
                moveDir.Y--;
            if (keyState.IsKeyDown(left))
                moveDir.X--;
            if (keyState.IsKeyDown(down))
                moveDir.Y++;
            if (keyState.IsKeyDown(right))
                moveDir.X++;

            if (moveDir != Vector2.Zero)
                moveDir.Normalize();

            position += moveDir * maxMoveDist * elapsed;
        }
    }
}
