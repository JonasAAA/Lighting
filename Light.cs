using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Light
    {
        public Vector2 position;

        protected float mainAngle;

        private readonly float maxAngleDiff;
        private readonly List<IShadowCastingObject> castObjects;
        private readonly LightPolygon polygon;

        public static void EarlyInitialize(GraphicsDeviceManager graphics)
        {
            LightPolygon.EarlyInitialize(graphics);
        }

        public static void Initialize(GraphicsDevice GraphicsDevice, Camera camera)
        {
            LightPolygon.Initialize(GraphicsDevice, camera);
        }

        public Light(Vector2 position, float strength, Color color)
            : this(position, 0, MathHelper.TwoPi, strength, color)
        { }

        public Light(Vector2 position, float mainAngle, float maxAngleDiff, float strength, Color color)
        {
            this.position = position;

            this.mainAngle = mainAngle;

            this.maxAngleDiff = maxAngleDiff;
            castObjects = new List<IShadowCastingObject>();
            polygon = new LightPolygon(strength, color);
        }

        public void AddObject(IShadowCastingObject castObject)
        {
            castObjects.Add(castObject);
        }

        public virtual void Update(float elapsed)
        {
            //the current approach (with IEnumerable) may be less efficient
            List<float> angles = new List<float>();
            foreach (var castObject in castObjects)
                angles.AddRange(castObject.RelAngles(position));
            //foreach (var castObject in castObjects)
            //    castObject.RelAngles(position, angles);

            const float small = 0.0001f;
            int oldAngleCount = angles.Count;

            for (int i = 0; i < oldAngleCount; i++)
            {
                angles.Add(angles[i] + small);
                angles.Add(angles[i] - small);
            }

            PrepareAngles(ref angles);

            List<Vector2> vertices = new List<Vector2>();

            float maxDist = 2000;
            for (int i = 0; i < angles.Count; i++)
            {
                float angle = angles[i];
                Vector2 rayDir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                List<float> dists = new List<float>();
                float minDist = maxDist;
                foreach (var castObject in castObjects)
                    dists.AddRange(castObject.InterPoint(position, rayDir));
                //foreach (var castObject in castObjects)
                //    castObject.InterPoint(position, rayDir, dists);
                foreach (float dist in dists)
                {
                    float d = dist/* + 1f*/;
                    if (d >= 0 && d < minDist)
                        minDist = d;
                }
                vertices.Add(position + minDist * rayDir);
            }

            if (maxAngleDiff * 2 < MathHelper.TwoPi)
                vertices.Add(position);

            polygon.Update(position, vertices);
        }

        public static void BeginDraw()
        {
            LightPolygon.BeginDraw();
        }

        public void Draw()
        {
            polygon.Draw();
        }

        public static void EndDraw(SpriteBatch spriteBatch)
        {
            LightPolygon.EndDraw(spriteBatch);
        }

        private void PrepareAngles(ref List<float> angles)
        {
            for (int i = 0; i < 4; i++)
                angles.Add(i * MathHelper.TwoPi / 4);

            List<float> prepAngles = new List<float>();
            foreach (float angle in angles)
            {
                float prepAngle = MathHelper.WrapAngle(angle - mainAngle);
                if (Math.Abs(prepAngle) <= maxAngleDiff)
                    prepAngles.Add(prepAngle + mainAngle);
            }
            prepAngles.Add(mainAngle + MathHelper.WrapAngle(maxAngleDiff));
            prepAngles.Add(mainAngle - MathHelper.WrapAngle(maxAngleDiff));
            prepAngles.Sort();

            angles = new List<float>();
            for (int i = 0; i < prepAngles.Count; i++)
            {
                if (i == 0 || prepAngles[i - 1] != prepAngles[i])
                    angles.Add(prepAngles[i]);
            }
        }
    }
}
