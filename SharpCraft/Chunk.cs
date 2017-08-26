﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharpCraft
{
    class Chunk
    {
        public static Vector3 chunkSize = new Vector3(16, 16, 16);
        private Cube[,,] cubes;
        private Vector3 position;

        public int BlockCount { get; private set; }
        
        public Chunk(Vector3 position, GraphicsDevice graphicsDevice)
        {
            this.position = position;
            cubes = new Cube[(int)chunkSize.X, (int)chunkSize.Y, (int)chunkSize.Z];

            for (var x = 0; x < chunkSize.X; x++)
            {
                for (var y = 0; y < chunkSize.Y; y++)
                {
                    for (var z = 0; z < chunkSize.Z; z++)
                    {
                        cubes[x, y, z] = new Cube(this, new Vector3(position.X + x * 100, position.Y + y * 100, position.Z + z * 100), graphicsDevice);
                        BlockCount++;
                    }
                }
            }

            foreach (var cube in cubes)
            {
                cube.Update();
            }
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (var cube in cubes)
            {
                cube.Draw(gameTime, viewMatrix, projectionMatrix);
            }
        }

        public Cube[] GetSurroundingCubes(Vector3 position)
        {
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
