using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharpCraft
{
    class Quad
    {
        private float radius = 50f;
        private Vector3 position;
        private Vector3 rotation;
        private Matrix translationMatrix;
        private Matrix rotationMatrix;
        private Matrix worldMatrix;
        VertexPositionColor[] vertices;

        public void Initialise(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            translationMatrix = Matrix.CreateTranslation(position);
            this.rotation = rotation;

            rotationMatrix = Matrix.CreateFromYawPitchRoll(
                MathHelper.ToRadians(rotation.X * 90),
                MathHelper.ToRadians(rotation.Y * 90),
                MathHelper.ToRadians(rotation.Z * 90));

            worldMatrix = rotationMatrix * translationMatrix;

            vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(radius, radius, 0), Color.Red),
                new VertexPositionColor(new Vector3(-radius, radius, 0), Color.Green),
                new VertexPositionColor(new Vector3(-radius, -radius, 0), Color.Blue),
                new VertexPositionColor(new Vector3(radius, radius, 0), Color.Red),
                new VertexPositionColor(new Vector3(-radius, -radius, 0), Color.Blue),
                new VertexPositionColor(new Vector3(radius, -radius, 0), Color.Green)
            };
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            var basicEffect = new BasicEffect(graphicsDevice)
            {
                Alpha = 1f,
                VertexColorEnabled = true,
                Projection = projectionMatrix,
                View = viewMatrix,
                World = worldMatrix
            };

            foreach(var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
            }
        }
    }
}
