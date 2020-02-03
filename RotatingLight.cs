using Microsoft.Xna.Framework;

namespace Game1
{
    public class RotatingLight : Light
    {
        private readonly float rotSpeed;

        public RotatingLight(Vector2 position, float mainAngle, float maxAngleDiff, float rotSpeed, float strength, Color color)
            : base(position, mainAngle, maxAngleDiff, strength, color)
        {
            this.rotSpeed = rotSpeed;
        }

        public override void Update(float elapsed)
        {
            mainAngle += rotSpeed * elapsed;
            mainAngle = MathHelper.WrapAngle(mainAngle);
            base.Update(elapsed);
        }
    }
}
