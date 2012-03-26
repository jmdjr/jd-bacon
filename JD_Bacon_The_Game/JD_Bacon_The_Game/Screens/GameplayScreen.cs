#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;
using Jitter.Collision.Shapes;
using System.Diagnostics;
using JD_Bacon_The_Game;
using LevelContentStructure;
using Physics.Primitives;
#endregion
using SingleBodyConstraints = Jitter.Dynamics.Constraints.SingleBody;
namespace JD_Bacon_The_Game.GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields
        ContentManager content;
        SpriteFont gameFont;

        private JitterScene scene;
        private bool multithread = true;

        private GamePadState padState;
        private KeyboardState keyState;
        private MouseState mouseState;
        
        KeyboardState keyboardPreviousState = new KeyboardState();
        GamePadState gamePadPreviousState = new GamePadState();
        MouseState mousePreviousState = new MouseState();

        float pauseAlpha;

        #endregion

        #region Initialization
        private void ControlMapping()
        {
            CommandMapper.AddMapping("Left", new Keys[] { Keys.Left });
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.ControlMapping();
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent(Game game)
        {
            base.LoadContent(game);
            if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");

            InputState state = new InputState();
            JDLevelManagerObject curLevel = content.Load<JDLevelManagerObject>("Levels/Level01");

            scene = new EmptyScene((JDBaconTheGame)game);
            scene.Build();
            gameFont = content.Load<SpriteFont>("gamefont");
            BuildPhysicalEntities();
            
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (this.IsActive)
            {
                UpdatePhysics(gameTime);
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                HandlePlayerInput(input);
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            if (this.IsActive)
            {
                DrawGameState(gameTime);
            }

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion

        #region Handling GameState

        private void BuildPhysicalEntities()
        {
        }

        private void UpdatePhysics(GameTime gameTime)
        {
            padState = GamePad.GetState(PlayerIndex.One);
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            // let the user escape the demo
            if (PressedOnce(Keys.Escape, Buttons.Back)) ((JDBaconTheGame)this.game).Exit();
            
            float step = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (step > 1.0f / 100.0f) step = 1.0f / 100.0f;
            ((JDBaconTheGame)this.game).World.Step(step, multithread);

            gamePadPreviousState = padState;
            keyboardPreviousState = keyState;
            mousePreviousState = mouseState;

            this.scene.Draw();
        }

        private bool PressedOnce(Keys key, Buttons button)
        {
            bool keyboard = keyState.IsKeyDown(key) && !keyboardPreviousState.IsKeyDown(key);

            if (key == Keys.Add) key = Keys.OemPlus;
            keyboard |= keyState.IsKeyDown(key) && !keyboardPreviousState.IsKeyDown(key);

            if (key == Keys.Subtract) key = Keys.OemMinus;
            keyboard |= keyState.IsKeyDown(key) && !keyboardPreviousState.IsKeyDown(key);

            bool gamePad = padState.IsButtonDown(button) && !gamePadPreviousState.IsButtonDown(button);

            return keyboard || gamePad;
        }

        private void HandlePlayerInput(InputState input)
        {

            padState = GamePad.GetState(ControllingPlayer.Value);
            keyState = Keyboard.GetState(ControllingPlayer.Value);
            mouseState = Mouse.GetState();
        }

        private void DrawGameState(GameTime gameTime)
        {
        }

        #endregion
    }
}
