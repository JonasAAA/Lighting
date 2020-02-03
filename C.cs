using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public static class C
    {
        public static readonly int screenWidth, screenHeight;

        static C()
        {
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }
    }
}
