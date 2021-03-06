﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Segment : IShadowCastingObject
    {
        public readonly Vector2 startPos, dir, orthDir;
        public readonly float rotation, length, radius;
        
        private readonly Disk startDisk, endDisk;
        private readonly Image image;

        public Segment(Vector2 startPos, Vector2 endPos, float radius, Color color)
        {
            this.startPos = startPos;
            dir = endPos - startPos;
            dir.Normalize();
            orthDir = new Vector2(-dir.Y, dir.X);
            rotation = (float)Math.Atan2(dir.Y, dir.X);
            length = Vector2.Distance(startPos, endPos);
            this.radius = radius;

            startDisk = new Disk(startPos, radius, 0, "disk", color);
            endDisk = new Disk(endPos, radius, 0, "disk", color);

            image = new Image("pixel", length, 2 * radius, color);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            startDisk.Draw(spriteBatch);
            endDisk.Draw(spriteBatch);
            image.Draw(spriteBatch, startPos + dir * length / 2, rotation);
        }

        public void RelAngles(Vector2 lightPos, List<float> relAngles)
        {
            startDisk.RelAngles(lightPos, relAngles);
            endDisk.RelAngles(lightPos, relAngles);
        }

        public void InterPoint(Vector2 lightPos, Vector2 lightDir, List<float> interPoints)
        {
            startDisk.InterPoint(lightPos, lightDir, interPoints);
            endDisk.InterPoint(lightPos, lightDir, interPoints);
            float a = dir.X,
                  b = -lightDir.X,
                  c = lightPos.X - startPos.X,
                  d = dir.Y,
                  e = -lightDir.Y,
                  f = lightPos.Y - startPos.Y;

            // now we have to solve
            // t1 * a + t2 * b = c
            // t1 * d + t2 * e = f
            // where t2 is what we want and t1 tell us if they intersect at all

            float det = a * e - b * d;
            if (det == 0)
                return;

            float t1 = (c * e - b * f) / det;
                  //t2 = (a * f - c * d) / det;

            if (t1 < 0 || t1 > length)
                return;

            // this is not exactly correct but should be close enough
            Disk temporary = new Disk(startPos + dir * t1, radius, 0, "disk", image.color);
            temporary.InterPoint(lightPos, lightDir, interPoints);
        }
    }
}
