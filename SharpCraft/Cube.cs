using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpCraft.Managers;

namespace SharpCraft
{
    /// <summary>
    /// The smallest unit of the world
    /// </summary>
    class Cube : GameObject
    {
        private Chunk parent;
        private Vector3 position;
        private float radius = 50;
        private Quad[] faces;
        private List<Quad> drawList = new List<Quad>();
        private BasicEffect effect;
        private Texture2D grassTop;
        private Texture2D grassSide;
        private Texture2D dirt;

        /// <summary>
        /// Creates an instance of a block
        /// </summary>
        /// <param name="entityManager">The EntityManager instance</param>
        /// <param name="parent">The chunk this block belongs to</param>
        /// <param name="position">The position of this block in 3D space</param>
        public Cube(EntityManager entityManager, Chunk parent, Vector3 position) : base(entityManager)
        {
            grassTop = entityManager.ResourceManager.LoadTexture("grass_top_green");
            grassSide = entityManager.ResourceManager.LoadTexture("grass_side");
            dirt = entityManager.ResourceManager.LoadTexture("dirt");

            this.position = position;
            this.parent = parent;
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

            faces[0].Initialise(topPos, Vector3.Up, grassTop, entityManager.GraphicsDevice);
            faces[1].Initialise(bottomPos, Vector3.Down, dirt, entityManager.GraphicsDevice);
            faces[2].Initialise(leftPos, Vector3.Left, grassSide, entityManager.GraphicsDevice);
            faces[3].Initialise(rightPos, Vector3.Right, grassSide, entityManager.GraphicsDevice);
            faces[4].Initialise(forwardPos, Vector3.Left * 2f, grassSide, entityManager.GraphicsDevice);
            faces[5].Initialise(backwardPos, Vector3.Zero, grassSide, entityManager.GraphicsDevice);

            effect = new BasicEffect(entityManager.GraphicsDevice)
            {
                Alpha = 1f,
                TextureEnabled = true
            };
        }

        public override void Update(GameTime gameTime)
        {
            var cubes = parent.GetSurroundingCubes(position);
            drawList.Clear();

            for (var i = 0; i < 6; i++)
            {
                if (cubes[i] == null)
                {
                    drawList.Add(faces[i]);
                }
            }
            
        }

        /// <summary>
        /// Returns the number of quads this block will draw
        /// </summary>
        /// <returns></returns>
        public int GetDrawCount()
        {
            return drawList.Count;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var face in drawList)
            {
                face.Draw(gameTime, entityManager.Camera.ViewMatrix, entityManager.Camera.ProjectionMatrix, effect);
            }
        }
    }
}
