﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpCraft.Managers;

namespace SharpCraft
{
    /// <summary>
    /// One world "unit" holding a collection of blocks
    /// </summary>
    class Chunk : GameObject
    {
        /// <summary>
        /// The dimensions of each chunk
        /// </summary>
        public static Vector3 chunkSize = new Vector3(16, 16, 16);

        private Cube[,,] cubes;
        private Vector3 position;

        /// <summary>
        /// The number of blocks in this chunk
        /// </summary>
        public int BlockCount { get; private set; }
        
        /// <summary>
        /// Creates a Chunk at the specified postion
        /// </summary>
        /// <param name="entityManager">The EntityManager instance</param>
        /// <param name="position">The position of the chunk in 3D space</param>
        public Chunk(EntityManager entityManager, Vector3 position) : base(entityManager)
        {
            this.position = position;
            cubes = new Cube[(int)chunkSize.X, (int)chunkSize.Y, (int)chunkSize.Z];

            for (var x = 0; x < chunkSize.X; x++)
            {
                for (var y = 0; y < chunkSize.Y; y++)
                {
                    for (var z = 0; z < chunkSize.Z; z++)
                    {
                        cubes[x, y, z] = new Cube(entityManager, this, new Vector3(position.X + x * 100, position.Y + y * 100, position.Z + z * 100));
                        BlockCount++;
                    }
                }
            }

            foreach (var cube in cubes)
            {
                cube.Update(new GameTime());
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var cube in cubes)
            {
                cube.Draw(gameTime);
            }
        }

        /// <summary>
        /// Returns 0-6 blocks surrounding the given position
        /// </summary>
        /// <param name="position">The position of the centre block</param>
        /// <returns></returns>
        public Cube[] GetSurroundingCubes(Vector3 position)
        {
            // Use position as array index
            position /= 100;
            position -= (this.position / 100);

            var surroundingCubes = new Cube[6];
            surroundingCubes[0] = position.Y == chunkSize.Y - 1 ? null : cubes[(int)position.X, (int)position.Y + 1, (int)position.Z]; // Above
            surroundingCubes[1] = position.Y == 0 ? null : cubes[(int)position.X, (int)position.Y - 1, (int)position.Z]; // Below
            surroundingCubes[2] = position.X == chunkSize.X - 1 ? null : cubes[(int)position.X + 1, (int)position.Y, (int)position.Z]; // Left
            surroundingCubes[3] = position.X == 0 ? null : cubes[(int)position.X - 1, (int)position.Y, (int)position.Z]; // Right
            surroundingCubes[4] = position.Z == chunkSize.Z - 1 ? null : cubes[(int)position.X, (int)position.Y, (int)position.Z + 1]; // Forwards
            surroundingCubes[5] = position.Z == 0 ? null : cubes[(int)position.X, (int)position.Y, (int)position.Z - 1]; // Backwards

            return surroundingCubes;
        }

        /// <summary>
        /// Returns the number of quads drawn in this chunk
        /// </summary>
        /// <returns></returns>
        public int GetQuadCount()
        {
            var quadCount = 0;

            foreach (var cube in cubes)
            {
                quadCount += cube.GetDrawCount();
            }

            return quadCount;
        }
    }
}
