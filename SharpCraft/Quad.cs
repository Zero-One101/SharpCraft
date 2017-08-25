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
        private GraphicsDevice graphicsDevice;
        private float radius = 50f;
        private Vector3 position;
        private Vector3 rotation;
        private Matrix translationMatrix;
        private Matrix rotationMatrix;
        private Matrix worldMatrix;
        private Texture2D texture;
        VertexPositionTexture[] tVerts;

        public void Initialise(Vector3 position, Vector3 rotation, Texture2D texture, GraphicsDevice graphicsDevice)
        {
            this.texture = texture;
            this.position = position;
            translationMatrix = Matrix.CreateTranslation(position);
            this.rotation = rotation;

            rotationMatrix = Matrix.CreateFromYawPitchRoll(
                MathHelper.ToRadians(rotation.X * 90),
                MathHelper.ToRadians(rotation.Y * 90),
                MathHelper.ToRadians(rotation.Z * 90));

            worldMatrix = rotationMatrix * translationMatrix;

            tVerts = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(radius, radius, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(-radius, radius, 0), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(-radius, -radius, 0), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(radius, radius, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(-radius, -radius, 0), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(radius, -radius, 0), new Vector2(0, 1))
            };

            this.graphicsDevice = graphicsDevice;
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix, BasicEffect effect)
        {
            effect.Texture = texture;
            effect.Projection = projectionMatrix;
            effect.View = viewMatrix;
            effect.World = worldMatrix;

            foreach(var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, tVerts, 0, 2);
            }
        }
    }
}
