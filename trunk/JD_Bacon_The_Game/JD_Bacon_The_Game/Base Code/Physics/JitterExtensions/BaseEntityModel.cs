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

        protected JDBaconTheGame myGame { get { return (JDBaconTheGame)this.Game; } }

        public BaseEntityModel(Game game)
            : base(game)
        {
        }

        public virtual void Build()
        {
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
                    transforms(model, effect, mesh);

                    effect.EnableDefaultLighting();
                    effect.View = myGame.Camera.View;
                    effect.Projection = myGame.Camera.Projection;
                }

                mesh.Draw();
            }
        }
    }
}
