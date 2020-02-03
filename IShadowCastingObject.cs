using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1
{
    public interface IShadowCastingObject
    {
        void RelAngles(Vector2 lightPos, List<float> relAngles);

        void InterPoint(Vector2 lightPos, Vector2 lightDir, List<float> interPoints);

        void Draw(SpriteBatch spriteBatch);
    }
}
