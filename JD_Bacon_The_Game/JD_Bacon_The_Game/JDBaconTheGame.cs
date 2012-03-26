using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using JD_Bacon_The_Game.GameStateManagement;
using Jitter.Collision;
using Jitter;

namespace JD_Bacon_The_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class JDBaconTheGame : Game
    {
        #region Fields

        public GraphicsDeviceManager graphics;
        public ScreenManager screenManager;

        public CollisionSystem Collision;
        public World World;
        public JD_Game_Camera Camera;

        private Color backgroundColor = new Color(63, 66, 73);

        RasterizerState  cullMode, normal;

        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        static readonly string[] preloadAssets =
        {
            "gradient",
        };

        
        #endregion

        #region Initialization


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public JDBaconTheGame()
        {
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            this.Window.AllowUserResizing = true;

            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 840;
            graphics.PreferredBackBufferHeight = 480;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

            Collision = new CollisionSystemPersistentSAP();

            World = new World(Collision);
            World.AllowDeactivation = true;

            cullMode = new RasterizerState();
            cullMode.CullMode = CullMode.None;

            normal = new RasterizerState();

            Components.Add(screenManager);
        }
         
        /// <summary>
        /// Loads graphics content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        protected override void Initialize()
        {
            this.Camera = new JD_Game_Camera(this);
            Camera.Position = new Vector3(150, 150, 300);
            Camera.Target = Camera.Position + Vector3.Normalize(new Vector3(10, 5, 20));
            this.Components.Add(this.Camera);
            
            base.Initialize();
        }

        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            GraphicsDevice.RasterizerState = cullMode;

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
            GraphicsDevice.RasterizerState = normal;
        }

        #endregion
    }

    #region Entry Point

    #if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (JDBaconTheGame game = new JDBaconTheGame())
            {
                game.Run();
            }
        }
    }
    #endif  

    #endregion
}
