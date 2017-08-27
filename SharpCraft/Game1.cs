using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpCraft.Events;
using SharpCraft.Managers;

namespace SharpCraft
{
    public delegate void KeyDownHandler(object sender, KeyDownEventArgs e);
    public delegate void KeyUpHandler(object sender, KeyUpEventArgs e);
    public delegate void KeyHeldHandler(object sender, KeyHeldEventArgs e);

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private bool showDebug = false;
        private readonly List<Keys> downKeys = new List<Keys>();
        private readonly List<Keys> upKeys = new List<Keys>();
        private readonly List<Keys> heldKeys = new List<Keys>();

        public static Texture2D grassTop;
        public static Texture2D grassSide;
        public static Texture2D dirt;
        public static SpriteFont spriteFont;
        static Point worldSize = new Point(2, 2);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputManager inputManager;

        // Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Chunk[,] chunks;

        bool orbit = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            inputManager = new InputManager();
            inputManager.KeyDown += InputManager_KeyDown;
            inputManager.KeyUp += InputManager_KeyUp;
            inputManager.KeyHeld += InputManager_KeyHeld;
            // Set up camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -500);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 10000000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f)); // Y up

            chunks = new Chunk[worldSize.X, worldSize.Y];
            for (var x = 0; x < worldSize.X; x++)
            {
                for (var y = 0; y < worldSize.Y; y++)
                {
                    chunks[x, y] = new Chunk(new Vector3(x * Chunk.chunkSize.X * 100, 0, y * Chunk.chunkSize.Z * 100), GraphicsDevice);
                }
            }

            graphics.SynchronizeWithVerticalRetrace = false;
            base.IsFixedTimeStep = false;
        }

        private void InputManager_KeyDown(object sender, KeyDownEventArgs e)
        {
            downKeys.Add(e.Key);
        }

        private void InputManager_KeyUp(object sender, KeyUpEventArgs e)
        {
            upKeys.Add(e.Key);
        }

        private void InputManager_KeyHeld(object sender, KeyHeldEventArgs e)
        {
            heldKeys.Add(e.Key);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            grassTop = Content.Load<Texture2D>("Textures/grass_top_green");
            grassSide = Content.Load<Texture2D>("Textures/grass_side");
            dirt = Content.Load<Texture2D>("Textures/dirt");
            spriteFont = Content.Load<SpriteFont>("Fonts/spritefont1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            inputManager.Update();

            if (downKeys.Contains(Keys.Left))
            {
                camPosition.X += 1f;
                camTarget.X += 1f;
            }
            if (downKeys.Contains(Keys.Right))
            {
                camPosition.X -= 1f;
                camTarget.X -= 1f;
            }
            if (downKeys.Contains(Keys.Up))
            {
                camPosition.Y += 1f;
                camTarget.Y += 1f;
            }
            if (downKeys.Contains(Keys.Down))
            {
                camPosition.Y -= 1f;
                camTarget.Y -= 1f;
            }
            if (downKeys.Contains(Keys.OemPlus) || downKeys.Contains(Keys.Add))
            {
                camPosition.Z += 1f;
            }
            if (downKeys.Contains(Keys.OemMinus) || downKeys.Contains(Keys.Subtract))
            {
                camPosition.Z -= 1f;
            }
            if (downKeys.Contains(Keys.Space) && !heldKeys.Contains(Keys.Space))
            {
                orbit = !orbit;
            }
            if (downKeys.Contains(Keys.F3) && !heldKeys.Contains(Keys.F3))
            {
                showDebug = !showDebug;
            }

            if (orbit)
            {
                var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            downKeys.Clear();
            upKeys.Clear();
            heldKeys.Clear();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.CullCounterClockwiseFace
            };
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            int blockCount = 0;
            int quadCount = 0;

            foreach (var chunk in chunks)
            {
                chunk.Draw(gameTime, viewMatrix, projectionMatrix);
                blockCount += chunk.BlockCount;
                quadCount += chunk.GetQuadCount();
            }

            var framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds);

            if (showDebug)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, framerate.ToString(), Vector2.Zero, Color.White);
                spriteBatch.DrawString(spriteFont, GraphicsAdapter.DefaultAdapter.Description, new Vector2(0, 20), Color.White);
                spriteBatch.DrawString(spriteFont, string.Format("Block count: {0}", blockCount), new Vector2(0, 40), Color.White);
                spriteBatch.DrawString(spriteFont, string.Format("Total quads: {0}", blockCount * 6), new Vector2(0, 60), Color.White);
                spriteBatch.DrawString(spriteFont, string.Format("Quads drawn: {0}", quadCount), new Vector2(0, 80), Color.White);
                spriteBatch.End();

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }

            base.Draw(gameTime);
        }
    }
}
