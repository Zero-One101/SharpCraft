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

        public static SpriteFont spriteFont;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        EntityManager entityManager;
        InputManager inputManager;
        ResourceManager resourceManager;

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

            resourceManager = new ResourceManager(Content);

            entityManager = new EntityManager(inputManager, resourceManager, GraphicsDevice);

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

            if (downKeys.Contains(Keys.F3) && !heldKeys.Contains(Keys.F3))
            {
                showDebug = !showDebug;
            }

            entityManager.Update(gameTime);

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

            entityManager.Draw(gameTime);

            var framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds);

            if (showDebug)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, framerate.ToString(), Vector2.Zero, Color.White);
                spriteBatch.DrawString(spriteFont, GraphicsAdapter.DefaultAdapter.Description, new Vector2(0, 20), Color.White);
                spriteBatch.DrawString(spriteFont, string.Format("Block count: {0}", entityManager.BlockCount), new Vector2(0, 40), Color.White);
                spriteBatch.DrawString(spriteFont, string.Format("Total quads: {0}", entityManager.BlockCount * 6), new Vector2(0, 60), Color.White);
                spriteBatch.DrawString(spriteFont, string.Format("Quads drawn: {0}", entityManager.QuadCount), new Vector2(0, 80), Color.White);
                spriteBatch.End();

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }

            base.Draw(gameTime);
        }
    }
}
