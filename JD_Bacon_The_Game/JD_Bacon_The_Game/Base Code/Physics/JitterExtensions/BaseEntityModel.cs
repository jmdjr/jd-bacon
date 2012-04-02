using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jitter.Dynamics;

namespace JD_Bacon_The_Game
{
    /// <summary>
    /// Used to generalize functionality used in any model object we import from our content.
    /// </summary>
    public abstract class BaseEntityModel : DrawableGameComponent
    {
        protected delegate void CustomEffectTransforms(Model model, BasicEffect effect, ModelMesh mesh);

        protected Model model;
        protected Texture2D texture;
        protected RigidBody body;

        public Model BaseModel { get { return this.model; } }
        public Texture2D BaseTexture { get { return this.texture; } }
        public RigidBody BaseBody { get { return this.body; } }

        protected JDBaconTheGame myGame { get { return (JDBaconTheGame)this.Game; } }

        public BaseEntityModel(Game game)
            : base(game)
        {
        }

        public virtual void Build()
        {
            this.myGame.Components.Add(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (this.BaseBody != null)
            {
                this.myGame.World.AddBody(this.BaseBody);
            }
        }
       
        /// <summary>
        /// Calls the transforms on each effect of each mesh of the defined model, using a CustomEffectTransforms delegate
        /// </summary>
        /// <param name="model">The model to draw.</param>
        /// <param name="transforms"> A custom delegate used to customize how the effect is changed on the mesh's of the model </param>
        protected void DrawModelsMeshEffects(Model model, CustomEffectTransforms transforms)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    transforms(model, effect, mesh);
                    
                    effect.View = myGame.CameraReference.View;
                    effect.Projection = myGame.CameraReference.Projection;
                }

                mesh.Draw();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
