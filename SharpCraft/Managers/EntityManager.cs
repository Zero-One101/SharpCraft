using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SharpCraft.Events;

namespace SharpCraft.Managers
{
    public class EntityManager
    {
        public event KeyDownHandler KeyDown;
        public event KeyHeldHandler KeyHeld;
        public event KeyUpHandler KeyUp;
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Camera Camera { get; private set; }
        public int BlockCount { get; private set; }
        public int QuadCount { get; private set; }

        private readonly List<GameObject> entities = new List<GameObject>();
        private readonly List<Keys> downKeys = new List<Keys>();
        private readonly List<Keys> heldKeys = new List<Keys>();
        private readonly List<Keys> upKeys = new List<Keys>();
        private static Point worldSize = new Point(2, 2);
        private Chunk[,] chunks;

        public EntityManager(InputManager inputManager, GraphicsDevice graphicsDevice)
        {
            inputManager.KeyDown += InputManager_KeyDown;
            inputManager.KeyHeld += InputManager_KeyHeld;
            inputManager.KeyUp += InputManager_KeyUp;

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

            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }

            downKeys.Clear();
            heldKeys.Clear();
            upKeys.Clear();
        }

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
    }
}
