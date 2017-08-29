using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private float yaw;
        private float pitch;
        private float roll;
        private Matrix rotationMatrix;
        private Matrix translationMatrix;

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

            camPosition = new Vector3(0f, 0f, -500);
            translationMatrix = Matrix.CreateTranslation(camPosition);
            rotationMatrix = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), aspectRatio, 1f, 10000000f);
            ViewMatrix = Matrix.CreateLookAt(camPosition, rotationMatrix.Forward, Vector3.Up);
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
            if (downKeys.Contains(Keys.W))
            {
                camPosition.Z += 1;
            }
            if (downKeys.Contains(Keys.S))
            {
                camPosition.Z -= 1;
            }
            if (downKeys.Contains(Keys.A))
            {
                camPosition.X += 1;
            }
            if (downKeys.Contains(Keys.D))
            {
                camPosition.X -= 1;
            }

            rotationMatrix = Matrix.CreateRotationY(0);
            translationMatrix = Matrix.CreateTranslation(camPosition) * rotationMatrix;
            ViewMatrix = Matrix.CreateLookAt(camPosition, rotationMatrix.Forward, Vector3.Up);

            downKeys.Clear();
            upKeys.Clear();
        }
    }
}
