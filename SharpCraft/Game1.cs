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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        BasicEffect basicEffect;

        // Geometry info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

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
            camPosition = new Vector3(0f, 0f, -100f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f)); // Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            // Geometry - a simple quad about the origin
            triangleVertices = new VertexPositionColor[6];
            triangleVertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-100, 0, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(-100, -100, 0), Color.Blue);
            triangleVertices[3] = triangleVertices[0];
            triangleVertices[4] = triangleVertices[2];
            triangleVertices[5] = new VertexPositionColor(new Vector3(0, -100, 0), Color.Green);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), triangleVertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(triangleVertices);
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
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            // TODO: Add your drawing code here

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;

            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach(var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }

            base.Draw(gameTime);
        }
    }
}
