﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game1
{
    public class Disk : IShadowCastingObject
    {
        public virtual Vector2 Position
        {
            get;
            set;
        }
        public float radius, rotation;

        private readonly Image image;

        public Disk(Vector2 position, float radius, float rotation, string imageName, Color color)
        {
            Position = position;
            this.radius = radius;

            this.rotation = rotation;

            image = new Image(imageName, 2 * radius, color);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch, Position, rotation);
        }

        public IEnumerable<float> RelAngles(Vector2 lightPos)
        {
            float dist = Vector2.Distance(lightPos, Position);
            if (dist <= radius)
                yield break;

            float a = radius / Vector2.Distance(lightPos, Position),
                  b = (float)Math.Sqrt(1 - a * a);
            Vector2 center = Position * b * b + lightPos * a * a,
                    diff = Position - lightPos,
                    orth = new Vector2(diff.Y, -diff.X),
                    point1 = center + orth * a * b - lightPos,
                    point2 = center - orth * a * b - lightPos;
            yield return (float)Math.Atan2(point1.Y, point1.X);
            yield return (float)Math.Atan2(point2.Y, point2.X);
            //float angle1 = (float)Math.Atan2(point1.Y, point1.X),
            //      angle2 = (float)Math.Atan2(point2.Y, point2.X);
            //relAngles.Add(angle1);
            //relAngles.Add(angle2);

            //if (image.color.A != 255)
            //{
            //    int rayCount = 100;
            //    for (int i = 1; i <= rayCount - 1; i++)
            //        relAngles.Add((angle1 * i + (rayCount - i) * angle2) / rayCount);
            //}
        }

        public IEnumerable<float> InterPoint(Vector2 lightPos, Vector2 lightDir)
        {
            //float dist = Vector2.Distance(lightPos, position);
            //if (dist <= radius)
            //    return;

            Vector2 d = lightPos - Position;
            float e = Vector2.Dot(lightDir, d), f = Vector2.Dot(d, d) - radius * radius, g = e * e - f;
            if (g < 0)
                yield break;

            float h = (float)Math.Sqrt(g);

            if (float.IsNaN(h))
                yield break;

            yield return -e + h + 1f;
            yield return -e - h + 1f;
            //interPoints.Add(-e + h + 1f);
            //interPoints.Add(-e - h + 1f);
        }
    }
}
