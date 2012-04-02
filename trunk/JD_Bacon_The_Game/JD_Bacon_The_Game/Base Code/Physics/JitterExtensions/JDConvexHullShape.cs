using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using Jitter.Collision;
using Microsoft.Xna.Framework.Graphics;
using Physics;

namespace JD_Bacon_The_Game
{
    public static class JDConvexHullShape
    {
        public static ConvexHullShape GenerateShape(Model model)
        {
            List<JVector> jvecs = new List<JVector>();
            List<TriangleVertexIndices> indices = new List<TriangleVertexIndices>();

            ModelDataExtraction.ExtractData(ref jvecs, ref indices, model);

            int[] convexHullIndices = JConvexHull.Build(jvecs, JConvexHull.Approximation.Level6);

            List<JVector> hullPoints = new List<JVector>();

            for (int i = 0; i < convexHullIndices.Length; i++)
            {
                hullPoints.Add(jvecs[convexHullIndices[i]]);
            }

            return new ConvexHullShape(hullPoints);
        }
    }
}
