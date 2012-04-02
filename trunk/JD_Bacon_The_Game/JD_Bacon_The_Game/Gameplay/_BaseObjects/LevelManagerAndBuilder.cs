using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LevelContentStructure;
using Microsoft.Xna.Framework.Graphics;

namespace JD_Bacon_The_Game
{
    /// <summary>
    /// Contains references to all the Factories used to generate the objects for levels.
    /// will track all entities in the current level, 
    /// </summary>
    public class LevelManagerAndBuilder : DrawableGameComponent
    {
        string ManagerContentName;
        List<string> LevelNames;
        int CurrentLevelIndex = -1;
        JDBaconTheGame GameReference { get { return (JDBaconTheGame)this.Game; } }
        public bool CompletedAllLevels { get; protected set; }

        JDLevel CurrentLevelReference = null;

        public LevelManagerAndBuilder(JDBaconTheGame game, string managerContentName)
            : base((Game)game)
        {
            this.ManagerContentName = managerContentName;
            this.LevelNames = new List<string>();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            JDLevelManagerObject managerContent = this.Game.Content.Load<JDLevelManagerObject>(this.ManagerContentName);
            LevelNames = new List<string>(managerContent.Levels);
        }

        public void LoadNextLevel()
        {
            // The Current Level Index has passed the last element, we have no more levels.
            if (++CurrentLevelIndex >= this.LevelNames.Count())
            {
                this.CompletedAllLevels = true;
                return;
            }

            if (CurrentLevelReference != null)
            {
                CurrentLevelReference.Dispose();
                CurrentLevelReference = null;
            }

            JDLevelObject LevelContent = this.Game.Content.Load<JDLevelObject>(this.LevelNames[CurrentLevelIndex]);

            CurrentLevelReference = new JDLevel(this.Game, LevelContent);
            CurrentLevelReference.Build();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            CurrentLevelReference.Draw(gameTime);
        }
    }
}
