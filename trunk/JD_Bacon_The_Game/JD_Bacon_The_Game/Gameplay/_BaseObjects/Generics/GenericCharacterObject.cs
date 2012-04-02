using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelContentStructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;

namespace JD_Bacon_The_Game
{
    class GenericCharacterObject: BaseEntityModel
    {
        Model Model { get { return this.model; } set { this.model = value; } }
        RigidBody Body { get { return this.body; } set { this.body = value; } }
        Texture2D Texture { get { return this.texture; } set { this.texture = value; } }

        string MeshFileName;
        string TextureFileName;
        Vector3 Position;

        public GenericCharacterObject(Game game, JDCharacterObject objContent)
            : base(game)
        {
            MeshFileName = objContent.Appearance.MeshSource;
            TextureFileName = objContent.Appearance.TextureSource;
            Position = objContent.Position;
        }
        protected override void LoadContent()
        {
            if (MeshFileName != "" && TextureFileName != "")
            {
                Model = this.myGame.Content.Load<Model>(MeshFileName);
                Texture = this.myGame.Content.Load<Texture2D>(TextureFileName);

                ConvexHullShape generalshape = JDConvexHullShape.GenerateShape(this.Model);
                Body = new RigidBody(generalshape);
                Body.Tag = BodyTag.DrawMe;
            }

            base.LoadContent();
        }

        protected void GenericTransform(Model model, BasicEffect effect, ModelMesh mesh)
        {
            Matrix matrix = Conversion.ToXNAMatrix(Body.Orientation);
            matrix.Translation = Conversion.ToXNAVector(Body.Position) -
                Vector3.Transform(new Vector3(0, 1.0f, 0), matrix);
            effect.World = matrix;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Model != null)
            {
                this.DrawModelsMeshEffects(Model, GenericTransform);
            }

            base.Draw(gameTime);
        }
    }
}
