﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharpCraft
{
    class Cube
    {
        private GraphicsDevice graphicsDevice;
        private Vector3 position;
        private float radius = 50;
        private Quad[] faces;
        private BasicEffect effect;

        public Cube(Vector3 position, GraphicsDevice graphicsDevice)
        {
            this.position = position;
            this.graphicsDevice = graphicsDevice;
            faces = new Quad[6]
                {
                    new Quad(),
                    new Quad(),
                    new Quad(),
                    new Quad(),
                    new Quad(),
                    new Quad()
                };

            var topPos = new Vector3(position.X, position.Y + radius, position.Z);
            var bottomPos = new Vector3(position.X, position.Y - radius, position.Z);
            var leftPos = new Vector3(position.X + radius, position.Y, position.Z);
            var rightPos = new Vector3(position.X - radius, position.Y, position.Z);
            var forwardPos = new Vector3(position.X, position.Y, position.Z + radius);
            var backwardPos = new Vector3(position.X, position.Y, position.Z - radius);

            faces[0].Initialise(topPos, Vector3.Up, Game1.grassTop, graphicsDevice);
            faces[1].Initialise(bottomPos, Vector3.Down, Game1.dirt, graphicsDevice);
            faces[2].Initialise(leftPos, Vector3.Left, Game1.grassSide, graphicsDevice);
            faces[3].Initialise(rightPos, Vector3.Right, Game1.grassSide, graphicsDevice);
            faces[5].Initialise(forwardPos, Vector3.Left * 2f, Game1.grassSide, graphicsDevice);
            faces[4].Initialise(backwardPos, Vector3.Zero, Game1.grassSide, graphicsDevice);

            effect = new BasicEffect(graphicsDevice)
            {
                Alpha = 1f,
                TextureEnabled = true
            };
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (var face in faces)
            {
                face.Draw(gameTime, viewMatrix, projectionMatrix, effect);
            }
        }
    }
}