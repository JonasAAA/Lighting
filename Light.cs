using Microsoft.Xna.Framework;
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
            {
                //float maxMoveDist = 200;
                //KeyboardState keyState = Keyboard.GetState();
                //Vector2 moveDir = Vector2.Zero;

                //if (keyState.IsKeyDown(Keys.W))
                //    moveDir.Y--;
                //if (keyState.IsKeyDown(Keys.A))
                //    moveDir.X--;
                //if (keyState.IsKeyDown(Keys.S))
                //    moveDir.Y++;
                //if (keyState.IsKeyDown(Keys.D))
                //    moveDir.X++;

                //if (moveDir != Vector2.Zero)
                //    moveDir.Normalize();

                //position += moveDir * maxMoveDist * 1 / 60f;
            }
            List<float> angles = new List<float>();
            foreach (var castObject in castObjects)
                castObject.RelAngles(position, angles);

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
                    castObject.InterPoint(position, rayDir, dists);
                foreach (float dist in dists)
                {
                    float d = dist + 1f;
                    if (d >= 0 && d < minDist)
                        minDist = d;
                }
                vertices.Add(position + minDist * rayDir);
            }

            if (maxAngleDiff * 2 < MathHelper.TwoPi)
                vertices.Add(position);

            polygon.Update(position, vertices);
        }

        public void Draw()
        {
            polygon.Draw();
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

        //public void Update()
        //{
        //    int angleCount = 10000;
        //    List<float> angles = new List<float>();
        //    for (int i = 0; i < angleCount; i++)
        //        angles.Add(MathHelper.WrapAngle(i * MathHelper.TwoPi / angleCount));

        //    int centerInd = angles.Count;

        //    vertPosCol = new VertexPositionColor[angles.Count + 1];
        //    vertPosCol[centerInd].Position = Transform(lightSource);

        //    float maxDist = 2000;
        //    for (int i = 0; i < angles.Count; i++)
        //    {
        //        float angle = angles[i];
        //        Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        //        float dist = maxDist;
        //        foreach (var castObject in castObjects)
        //        {
        //            float? curDist = castObject.InterPoint(lightSource, dir);
        //            if (curDist.HasValue && curDist.Value < dist)
        //                dist = curDist.Value;
        //        }
        //        vertPosCol[i].Position = Transform(lightSource - dist * dir);
        //    }

        //    for (int i = 0; i < vertPosCol.Count(); i++)
        //        vertPosCol[i].Color = Color.White;

        //    ind = new int[angles.Count * 3];
        //    for (int i = 0; i < angles.Count; i++)
        //    {
        //        // may need to swap the last two
        //        ind[3 * i] = centerInd;
        //        ind[3 * i + 1] = i;
        //        ind[3 * i + 2] = (i + 1) % angles.Count;
        //    }
        //}
    }
}
