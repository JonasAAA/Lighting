using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class LightDiskPlayer : DiskPlayer
    {
        public readonly Light light;

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                if (light != null)
                    light.position = base.Position;
            }

        }

        public LightDiskPlayer(Vector2 position, float radius, float rotation, Keys up, Keys left, Keys down, Keys right, Color avatarColor, float lightStrength, Color lightColor)
            : base(position, radius, rotation, up, left, down, right, avatarColor)
        {
            light = new Light(position, lightStrength, lightColor);
        }
    }
}
