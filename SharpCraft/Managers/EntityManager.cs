using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SharpCraft.Events;

namespace SharpCraft.Managers
{
    /// <summary>
    /// Handles the management of all objects relevant to the game
    /// Includes GameObjects, cameras and other managers
    /// Allows GameObjects access to the rest of the game
    /// </summary>
    public class EntityManager
    {
        /// <summary>
        /// Fired when a key is pressed this frame
        /// </summary>
        public event KeyDownHandler KeyDown;
        /// <summary>
        /// Fired when a key has been pressed for more than one frame
        /// </summary>
        public event KeyHeldHandler KeyHeld;
        /// <summary>
        /// Fired when a key has been released
        /// </summary>
        public event KeyUpHandler KeyUp;
        /// <summary>
        /// Fired when the mouse has been moved
        /// </summary>
        public event MouseMoveHandler MouseMove;
        /// <summary>
        /// The graphics device used to draw the scene
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }
        /// <summary>
        /// The resource manager used to handle resources
        /// </summary>
        public ResourceManager ResourceManager { get; private set; }
        /// <summary>
        /// The game camera
        /// </summary>
        public Camera Camera { get; private set; }
        /// <summary>
        /// How many blocks are in the current scene
        /// </summary>
        public int BlockCount { get; private set; }
        /// <summary>
        /// The number of quads being drawn in the current scene
        /// </summary>
        public int QuadCount { get; private set; }

        private readonly List<GameObject> entities = new List<GameObject>();
        private readonly List<Keys> downKeys = new List<Keys>();
        private readonly List<Keys> heldKeys = new List<Keys>();
        private readonly List<Keys> upKeys = new List<Keys>();
        private Vector2 mouseVector = Vector2.Zero;
        private static Point worldSize = new Point(2, 2);
        private Chunk[,] chunks;

        /// <summary>
        /// Instantiates the EntityManager
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="resourceManager"></param>
        /// <param name="graphicsDevice"></param>
        public EntityManager(InputManager inputManager, ResourceManager resourceManager, GraphicsDevice graphicsDevice)
        {
            inputManager.KeyDown += InputManager_KeyDown;
            inputManager.KeyHeld += InputManager_KeyHeld;
            inputManager.KeyUp += InputManager_KeyUp;
            inputManager.MouseMove += InputManager_MouseMove;

            ResourceManager = resourceManager;
            GraphicsDevice = graphicsDevice;

            Camera = new Camera(this, 60f, graphicsDevice.DisplayMode.AspectRatio);

            chunks = new Chunk[worldSize.X, worldSize.Y];
            for (var x = 0; x < worldSize.X; x++)
            {
                for (var y = 0; y < worldSize.X; y++)
                {
                    chunks[x, y] = new Chunk(this, new Vector3(x * Chunk.chunkSize.X * 100, 0, y * Chunk.chunkSize.Z * 100));
                }
            }
        }

        private void InputManager_KeyDown(object sender, Events.KeyDownEventArgs e)
        {
            downKeys.Add(e.Key);
        }

        private void InputManager_KeyHeld(object sender, Events.KeyHeldEventArgs e)
        {
            heldKeys.Add(e.Key);
        }

        private void InputManager_KeyUp(object sender, Events.KeyUpEventArgs e)
        {
            upKeys.Add(e.Key);
        }

        private void InputManager_MouseMove(object sender, MouseMoveEventArgs e)
        {
            mouseVector = e.MouseVector;
        }

        /// <summary>
        /// Loops through every relevant object and calls Update on them
        /// Also handles refiring of Key events
        /// </summary>
        /// <param name="gameTime">The elapsed game time since the last frame</param>
        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            foreach (var downKey in downKeys)
            {
                FireKeyDown(downKey);
            }

            foreach (var heldKey in heldKeys)
            {
                FireKeyHeld(heldKey);
            }

            foreach (var upKey in upKeys)
            {
                FireKeyUp(upKey);
            }

            if (mouseVector != Vector2.Zero)
            {
                FireMouseMove();
                mouseVector = Vector2.Zero;
            }

            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }

            downKeys.Clear();
            heldKeys.Clear();
            upKeys.Clear();
        }

        /// <summary>
        /// Loops through every relevant object and calls Draw on them
        /// </summary>
        /// <param name="gameTime">The elapsed game time since the last frame</param>
        public void Draw(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Draw(gameTime);
            }

            BlockCount = 0;
            QuadCount = 0;

            foreach (var chunk in chunks)
            {
                chunk.Draw(gameTime);
                BlockCount += chunk.BlockCount;
                QuadCount += chunk.GetQuadCount();
            }
        }

        private void FireKeyDown(Keys key)
        {
            KeyDown?.Invoke(this, new KeyDownEventArgs(key));
        }

        private void FireKeyHeld(Keys key)
        {
            KeyHeld?.Invoke(this, new KeyHeldEventArgs(key));
        }

        private void FireKeyUp(Keys key)
        {
            KeyUp?.Invoke(this, new KeyUpEventArgs(key));
        }

        private void FireMouseMove()
        {
            MouseMove?.Invoke(this, new MouseMoveEventArgs(mouseVector));
        }

        internal void DebugDraw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Camera.DebugDraw(spriteBatch, spriteFont);
        }
    }
}
