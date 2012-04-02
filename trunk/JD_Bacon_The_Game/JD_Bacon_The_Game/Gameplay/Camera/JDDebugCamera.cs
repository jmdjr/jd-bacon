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
using LevelContentStructure;


namespace JD_Bacon_The_Game
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class JDDebugCamera : JDCamera
    {
        private MouseState prevMouseState = new MouseState();


        public JDDebugCamera(Game game, JDCameraObject cameraObject)
            : base(game, cameraObject)
        {
            Mouse.SetPosition(widthOver2, heightOver2);
        }


        public JDDebugCamera(Game game)
            : base(game)
        {
            Mouse.SetPosition(widthOver2, heightOver2);
        }

        /// <summary>
        /// Updates camera with input and updates view matrix.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                double elapsedTime = (double)gameTime.ElapsedGameTime.Ticks / (double)TimeSpan.TicksPerSecond;
                ProcessInput((float)elapsedTime * 50.0f);
                UpdateView();

                base.Update(gameTime);
            }
        }

        private void ProcessInput(float amountOfMovement)
        {
            Vector3 moveVector = new Vector3();

            KeyboardState keys = Keyboard.GetState();
            GamePadState buttons = GamePad.GetState(PlayerIndex.One);

            if (keys.IsKeyDown(Keys.D))
                moveVector.X += amountOfMovement;
            if (keys.IsKeyDown(Keys.A))
                moveVector.X -= amountOfMovement;
            if (keys.IsKeyDown(Keys.S))
                moveVector.Z += amountOfMovement;
            if (keys.IsKeyDown(Keys.W))
                moveVector.Z -= amountOfMovement;

            moveVector.Z += (buttons.DPad.Down == ButtonState.Pressed) ? amountOfMovement : 0.0f;
            moveVector.X -= (buttons.DPad.Left == ButtonState.Pressed) ? amountOfMovement : 0.0f;
            moveVector.Z -= (buttons.DPad.Up == ButtonState.Pressed) ? amountOfMovement : 0.0f;
            moveVector.X += (buttons.DPad.Right == ButtonState.Pressed) ? amountOfMovement : 0.0f;

            angles.Y -= buttons.ThumbSticks.Right.X * amountOfMovement * 0.05f;
            angles.X += buttons.ThumbSticks.Right.Y * amountOfMovement * 0.05f;

            Matrix cameraRotation = Matrix.CreateRotationX(angles.X) * Matrix.CreateRotationY(angles.Y);
            position += Vector3.Transform(moveVector, cameraRotation);

            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                Mouse.SetPosition(widthOver2, heightOver2);
            }
            else if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                if (currentMouseState.X != widthOver2)
                    angles.Y -= amountOfMovement / 80.0f * (currentMouseState.X - widthOver2);
                if (currentMouseState.Y != heightOver2)
                    angles.X -= amountOfMovement / 80.0f * (currentMouseState.Y - heightOver2);

                Mouse.SetPosition(widthOver2, heightOver2);
            }

            prevMouseState = currentMouseState;

            if (angles.X > 1.4) angles.X = 1.4f;
            if (angles.X < -1.4) angles.X = -1.4f;
            if (angles.Y > Math.PI) angles.Y -= 2 * (float)Math.PI;
            if (angles.Y < -Math.PI) angles.Y += 2 * (float)Math.PI;
        }
    }
}
