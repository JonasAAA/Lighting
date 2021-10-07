using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1
{
    public interface IShadowCastingObject
    {
        IEnumerable<float> RelAngles(Vector2 lightPos);

        IEnumerable<float> InterPoint(Vector2 lightPos, Vector2 lightDir);

        void Draw(SpriteBatch spriteBatch);
    }
}
