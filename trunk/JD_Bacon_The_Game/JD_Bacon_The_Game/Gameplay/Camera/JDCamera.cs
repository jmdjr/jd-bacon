using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LevelContentStructure;

namespace JD_Bacon_The_Game
{
    public class JDCamera : GameComponent
    {
        protected Matrix view;
        protected Matrix projection;
        protected Vector2 angles = Vector2.Zero;
        protected Vector3 position = new Vector3(0, 0, 10);
        protected float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
        protected float aspectRatio;
        protected float nearPlaneDistance = 0.01f;
        protected float farPlaneDistance = 1000.0f;

        protected int widthOver2;
        protected int heightOver2;

        public JDCamera(Game game, JDCameraObject cameraObject)
            : base(game)
        {
            farPlaneDistance = cameraObject.FarPlaneDistance;
            nearPlaneDistance = cameraObject.NearPlaneDistance;
            Position = cameraObject.Position;
            Target = Position + Vector3.Normalize(cameraObject.Target);

            widthOver2 = game.Window.ClientBounds.Width / 2;
            heightOver2 = game.Window.ClientBounds.Height / 2;
            aspectRatio = (float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height;
            UpdateProjection();
        }
        public JDCamera(Game game)
            : base(game)
        {
            widthOver2 = game.Window.ClientBounds.Width / 2;
            heightOver2 = game.Window.ClientBounds.Height / 2;
            aspectRatio = (float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height;
            UpdateProjection();
        }

        /// <summary>
        /// Gets or sets camera projection matrix.
        /// </summary>
        public Matrix Projection { get { return projection; } set { projection = value; } }
        /// <summary>
        /// Gets camera view matrix multiplied by projection matrix.
        /// </summary>
        /// <summary>
        /// Gets or sets camera position.
        /// </summary>
        public Vector3 Position { get { return position; } set { position = value; } }

        /// <summary>
        /// Gets or sets camera field of view.
        /// </summary>
        public float FieldOfView { get { return fieldOfView; } set { fieldOfView = value; UpdateProjection(); } }
        /// <summary>
        /// Gets or sets camera aspect ratio.
        /// </summary>
        public float AspectRatio { get { return aspectRatio; } set { aspectRatio = value; UpdateProjection(); } }
        /// <summary>
        /// Gets or sets camera near plane distance.
        /// </summary>
        public float NearPlaneDistance { get { return nearPlaneDistance; } set { nearPlaneDistance = value; UpdateProjection(); } }
        /// <summary>
        /// Gets or sets camera far plane distance.
        /// </summary>
        public float FarPlaneDistance { get { return farPlaneDistance; } set { farPlaneDistance = value; UpdateProjection(); } }

        /// <summary>
        /// Gets camera view matrix.
        /// </summary>
        public Matrix View { get { return view; } }

        /// <summary>
        /// Gets camera view matrix multiplied by projection matrix.
        /// </summary>
        public Matrix ViewProjection { get { return view * projection; } }


        /// <summary>
        /// Gets or sets camera's target.
        /// </summary>
        public Vector3 Target
        {
            get
            {
                Matrix cameraRotation = Matrix.CreateRotationX(angles.X) * Matrix.CreateRotationY(angles.Y);
                return position + Vector3.Transform(Vector3.Forward, cameraRotation);
            }
            set
            {
                Vector3 forward = Vector3.Normalize(position - value);
                Vector3 right = Vector3.Normalize(Vector3.Cross(forward, Vector3.Up));
                Vector3 up = Vector3.Normalize(Vector3.Cross(right, forward));

                Matrix test = Matrix.Identity;
                test.Forward = forward;
                test.Right = right;
                test.Up = up;
                angles.X = -(float)Math.Asin(test.M32);
                angles.Y = -(float)Math.Asin(test.M13);
            }
        }

        protected void UpdateProjection()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
        }

        protected void UpdateView()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(angles.X) * Matrix.CreateRotationY(angles.Y);
            Vector3 targetPos = position + Vector3.Transform(Vector3.Forward, cameraRotation);

            Vector3 upVector = Vector3.Transform(Vector3.Up, cameraRotation);

            view = Matrix.CreateLookAt(position, targetPos, upVector);
        }   

    }
}
