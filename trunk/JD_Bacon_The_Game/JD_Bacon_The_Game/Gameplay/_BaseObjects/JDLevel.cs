using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LevelContentStructure;

namespace JD_Bacon_The_Game
{
    public class JDLevel : BaseEntityModel
    {
        public string OverworldSong { get; protected set; }
        public List<BaseEntityModel> LevelContentCollection = new List<BaseEntityModel>();

        public JDLevel(Game game, JDLevelObject LevelContent)
            :base(game)
        {
            BuildLevelContent(LevelContent);
        }

        private void BuildLevelContent(JDLevelObject LevelContent)
        {

            // Music
            if (LevelContent.WorldMusic.FilePath != "" && LevelContent.WorldMusic.FileName != "")
            {
                JDBTG.MusicManager.LoadSong(LevelContent.WorldMusic.FileName, LevelContent.WorldMusic.FilePath);
                OverworldSong = LevelContent.WorldMusic.FileName;
            }

            // Ground
            AnyGround ground = new AnyGround(this.Game, LevelContent.GroundAppearance);
            
            this.LevelContentCollection.Add(ground);

            // Camera
            JDDebugCamera camera = new JDDebugCamera(this.Game, LevelContent.CameraObject);
            this.myGame.CameraReference = camera;

            // Static Objects
            foreach (JDStaticObject entry in LevelContent.StaticObjectSet)
            {
                this.LevelContentCollection.Add(StaticObjectFactory.Spawn(entry, this.Game));
            }

            // Collectable Objects
            foreach (JDCollectableObject entry in LevelContent.CollectableObjectSet)
            {
                this.LevelContentCollection.Add(CollectablesFactory.Spawn(entry, this.Game));
            }

            // Physical Object
            foreach (JDPhysicalObject entry in LevelContent.PhysicalObjectSet)
            {
                this.LevelContentCollection.Add(PhysicalObjectFactory.Spawn(entry, this.Game));
            }

            // CharacterObjects
            foreach (JDCharacterObject entry in LevelContent.CharacterObjectSet)
            {
                this.LevelContentCollection.Add(CharacterFactory.Spawn(entry, this.Game));
            }

            // Trigger Objects
            foreach (JDTriggerObject entry in LevelContent.TriggerObjectSet)
            {
            }
        }

        public override void Build()
        {
            base.Build();

            foreach (BaseEntityModel component in this.LevelContentCollection)
            {
                component.Build();
            }
        }
        protected override void Dispose(bool disposing)
        {
            DestroyLevel();
            base.Dispose(disposing);
        }

        private void DestroyLevel()
        {
            // No longer needing to use this level's camera.
            this.myGame.CameraReference = null;

            // While our reference list of level content is still referencing stuff, remove it from the game.
            while (this.LevelContentCollection.Count() > 0)
            {
                BaseEntityModel component = this.LevelContentCollection.FirstOrDefault();

                if (this.myGame.Components.Contains(component))
                {
                    this.myGame.Components.Remove(component);

                    if (this.myGame.World.RigidBodies.Contains(component.BaseBody))
                    {
                        this.myGame.World.RemoveBody(component.BaseBody);
                    }

                    this.LevelContentCollection.Remove(component);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (BaseEntityModel entity in this.LevelContentCollection)
            {
                entity.Draw(gameTime);
            }
        }
    }
}
