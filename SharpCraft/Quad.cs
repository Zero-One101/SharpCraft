﻿using System;
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
        private Texture2D texture;
        VertexPositionTexture[] tVerts;

        public void Initialise(Vector3 position, Vector3 rotation, Texture2D texture)
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
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            var basicEffect = new BasicEffect(graphicsDevice)
            {
                Alpha = 1f,
                TextureEnabled = true,
                Texture = texture,
                //LightingEnabled = true,
                Projection = projectionMatrix,
                View = viewMatrix,
                World = worldMatrix
            };

            foreach(var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, tVerts, 0, 2);
            }
        }
    }
}
