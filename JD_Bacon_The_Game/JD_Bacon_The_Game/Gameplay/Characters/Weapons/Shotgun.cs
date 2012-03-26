using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using Jitter.Collision;
using Physics;

namespace JD_Bacon_The_Game
{
    public class Shotgun : BaseEntityModel
    {
        Model ShotgunModel = null;
        public RigidBody ShotgunBody = null;

        public Shotgun(Game game)
            : base(game)
        {
            this.Build();
        }

        public override void Build()
        {
            base.Build();
        }
        

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ShotgunModel = this.myGame.Content.Load<Model>(@"Assets\3d Assets\Done\Level 1\1by1");

            List<JVector> jvecs = new List<JVector>();
            List<TriangleVertexIndices> indices = new List<TriangleVertexIndices>();

            ModelDataExtraction.ExtractData(jvecs, indices, ShotgunModel);

            int[] convexHullIndices = JConvexHull.Build(jvecs, JConvexHull.Approximation.Level6);

            List<JVector> hullPoints = new List<JVector>();

            for (int i = 0; i < convexHullIndices.Length; i++)
            {
                hullPoints.Add(jvecs[convexHullIndices[i]]);
            }

            ConvexHullShape generalshape = new ConvexHullShape(hullPoints);
            ShotgunBody = new RigidBody(generalshape);
            ShotgunBody.Tag = BodyTag.DrawMe;
            this.myGame.World.AddBody(this.ShotgunBody);
        }

        protected void GenericTransform(Model model, BasicEffect effect, ModelMesh mesh)
        {
            Matrix matrix = Conversion.ToXNAMatrix(ShotgunBody.Orientation);
            matrix.Translation = Conversion.ToXNAVector(ShotgunBody.Position) -
                Vector3.Transform(new Vector3(0, 1.0f, 0), matrix);
            effect.World = matrix;
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawModelsMeshEffects(ShotgunModel, GenericTransform);
            base.Draw(gameTime);
        }
    }
}
