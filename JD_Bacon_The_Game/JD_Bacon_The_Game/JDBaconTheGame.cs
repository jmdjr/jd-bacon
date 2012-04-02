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
using Jitter.LinearMath;

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

        private JDCamera cameraReference;
        public JDCamera CameraReference
        {
            get
            {
                return cameraReference;
            }
            set
            {
                if (this.Components.Contains(cameraReference))
                {
                    this.Components.Remove(cameraReference);
                }

                cameraReference = value;

                if (!this.Components.Contains(cameraReference))
                {
                    this.Components.Add(this.cameraReference);
                }
            }
        }

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


            // Establishing JDBTG object instances.
            JDBTG.MusicManager = new EasyXnaAudioComponent(this, "Assets/Audio");
            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

            Collision = new CollisionSystemPersistentSAP();

            World = new World(Collision);
            World.AllowDeactivation = true;
            World.Gravity = new JVector(0, -10, 0);

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
            base.LoadContent();
        }

        protected override void Initialize()
        {
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
