using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Jitter.Dynamics;
using Microsoft.Xna.Framework;
using LevelContentStructure;
using Jitter.LinearMath;
using Jitter.Collision;
using Physics;
using Jitter.Collision.Shapes;

namespace JD_Bacon_The_Game
{
    public class AnyGround : BaseEntityModel
    {
        Model GroundModel { get { return this.model; } set { this.model = value; } }
        RigidBody GroundBody { get { return this.body; } set { this.body = value; } }
        Texture2D GroundTexture { get { return this.texture; } set { this.texture = value; } }

        string MeshFileName;
        string TextureFileName;

        public AnyGround(Game game, JDAppearance groundDetails)
            :base(game)
        {
            MeshFileName = groundDetails.MeshSource;
            TextureFileName = groundDetails.TextureSource;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            if (MeshFileName != "" && TextureFileName != "")
            {
                GroundModel = this.myGame.Content.Load<Model>(MeshFileName);
                GroundTexture = this.myGame.Content.Load<Texture2D>(TextureFileName);

                ConvexHullShape generalshape = JDConvexHullShape.GenerateShape(GroundModel);
                GroundBody = new RigidBody(generalshape);
                GroundBody.Tag = BodyTag.DrawMe;
                GroundBody.IsStatic = true;
            }
        }

        protected void GenericTransform(Model model, BasicEffect effect, ModelMesh mesh)
        {
            Matrix matrix = Conversion.ToXNAMatrix(GroundBody.Orientation);
            matrix.Translation = Conversion.ToXNAVector(GroundBody.Position) -
                Vector3.Transform(new Vector3(0, 1.0f, 0), matrix);
            effect.World = matrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (GroundModel != null)
            {
                this.DrawModelsMeshEffects(GroundModel, GenericTransform);
            }
            base.Draw(gameTime);
        }
    }
}
