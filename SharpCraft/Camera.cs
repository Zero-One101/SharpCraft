using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SharpCraft.Managers;
using SharpCraft.Events;


namespace SharpCraft
{
    /// <summary>
    /// A 3D camera
    /// </summary>
    public class Camera
    {
        private readonly List<Keys> downKeys = new List<Keys>();
        private readonly List<Keys> upKeys = new List<Keys>();
        private EntityManager entityManager;
        private Vector3 camPosition;
        private float rotY = 0f;
        private float rotX = 0f;
        private Vector2 mouseVector;
        private Vector3 finalTarget;
        private const float rotationSpeed = 0.03f;
        private const float moveSpeed = 300.0f;

        /// <summary>
        /// The projection matrix of the camera
        /// </summary>
        public Matrix ProjectionMatrix { get; private set; }
        /// <summary>
        /// The view matrix of the camera
        /// </summary>
        public Matrix ViewMatrix { get; private set; }

        /// <summary>
        /// Instantiates the camera with the specified FOV and aspect ratio
        /// </summary>
        /// <param name="entityManager">The EntityManager instance</param>
        /// <param name="fov">The Field Of Vision of this camera</param>
        /// <param name="aspectRatio">The aspect ratio of the game window</param>
        public Camera(EntityManager entityManager, float fov, float aspectRatio)
        {
            this.entityManager = entityManager;
            entityManager.KeyDown += EntityManager_KeyDown;
            entityManager.KeyUp += EntityManager_KeyUp;
            entityManager.MouseMove += EntityManager_MouseMove;

            camPosition = new Vector3(0f, 0f, -500);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), aspectRatio, 1f, 10000000f);
            UpdateViewMatrix();
        }

        private void EntityManager_MouseMove(object sender, MouseMoveEventArgs e)
        {
            mouseVector = e.MouseVector;
        }

        private void EntityManager_KeyDown(object sender, KeyDownEventArgs e)
        {
            downKeys.Add(e.Key);
        }

        private void EntityManager_KeyUp(object sender, KeyUpEventArgs e)
        {
            upKeys.Add(e.Key);
        }

        /// <summary>
        /// Updates the camera, allowing it to respond to inputs
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var moveVector = new Vector3(0, 0, 0);

            if (downKeys.Contains(Keys.W))
            {
                moveVector.Z += 1;
            }
            if (downKeys.Contains(Keys.S))
            {
                moveVector.Z -= 1;
            }
            if (downKeys.Contains(Keys.A))
            {
                moveVector.X += 1;
            }
            if (downKeys.Contains(Keys.D))
            {
                moveVector.X -= 1;
            }

            rotY -= rotationSpeed * mouseVector.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotX += rotationSpeed * mouseVector.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            AddToCameraPosition(moveVector * (float)gameTime.ElapsedGameTime.TotalSeconds);
            UpdateViewMatrix();

            downKeys.Clear();
            upKeys.Clear();
            mouseVector = Vector2.Zero;
        }

        private void AddToCameraPosition(Vector3 moveVector)
        {
            var camRotation = Matrix.CreateRotationX(rotX) * Matrix.CreateRotationY(rotY);
            var rotatedVector = Vector3.Transform(moveVector, camRotation);
            camPosition += rotatedVector * moveSpeed;
        }

        public void DebugDraw(SpriteBatch spriteBatch, SpriteFont debugFont)
        {
            spriteBatch.DrawString(debugFont, string.Format("Cam position: {0}", camPosition.ToString()), new Vector2(0, 100), Color.White);
            spriteBatch.DrawString(debugFont, string.Format("Camera facing: {0}", finalTarget.ToString()), new Vector2(0, 120), Color.White);
        }

        private void UpdateViewMatrix()
        {
            var camRotation = Matrix.CreateRotationX(rotX) * Matrix.CreateRotationY(rotY);

            var camOriginalTarget = new Vector3(0, 0, 1);
            var camRotatedTarget = Vector3.Transform(camOriginalTarget, camRotation);
            finalTarget = camPosition + camRotatedTarget;

            var camOriginalUpVector = Vector3.Up;
            var camRotatedUpVector = Vector3.Transform(camOriginalUpVector, camRotation);

            ViewMatrix = Matrix.CreateLookAt(camPosition, finalTarget, camRotatedUpVector);
        }
    }
}
