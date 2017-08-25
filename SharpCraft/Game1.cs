using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharpCraft
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Texture2D grassTop;
        public static Texture2D grassSide;
        public static Texture2D dirt;
        static Vector3 worldSize = new Vector3(16, 256, 16);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Cube[, ,] cubes = new Cube[(int)worldSize.X, (int)worldSize.Y, (int)worldSize.Z];

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

            // Set up camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -500);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 10000000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f)); // Y up

            for (var x = 0; x < worldSize.X; x++)
            {
                for (var y = 0; y < worldSize.Y; y++)
                {
                    for (var z = 0; z < worldSize.Z; z++)
                    {
                        cubes[x, y, z] = new Cube(new Vector3(x * 100, y * 100, z * 100), GraphicsDevice);
                    }
                }
            }
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

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                camPosition.X += 1f;
                camTarget.X += 1f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                camPosition.X -= 1f;
                camTarget.X -= 1f;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                camPosition.Y += 1f;
                camTarget.Y += 1f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                camPosition.Y -= 1f;
                camTarget.Y -= 1f;
            }
            if (keyboardState.IsKeyDown(Keys.OemPlus) || keyboardState.IsKeyDown(Keys.Add))
            {
                camPosition.Z += 1f;
            }
            if (keyboardState.IsKeyDown(Keys.OemMinus) || keyboardState.IsKeyDown(Keys.Subtract))
            {
                camPosition.Z -= 1f;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }

            if (orbit)
            {
                var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

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

            foreach (var cube in cubes)
            {
                cube.Draw(gameTime, viewMatrix, projectionMatrix);
            }
            base.Draw(gameTime);
        }
    }
}
