using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SharpCraft.Managers
{
    /// <summary>
    /// Handles the loading, caching and unloading of game resources
    /// </summary>
    public class ResourceManager
    {
        private ContentManager content;
        private readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public ResourceManager(ContentManager content)
        {
            this.content = content;
        }

        /// <summary>
        /// Checks to see if the specified texture has already been loaded.
        /// If not, loads it, stores it in memory, then passes it to the caller.
        /// </summary>
        /// <param name="fileName">The name of the texture to load</param>
        /// <returns>The loaded texture</returns>
        public Texture2D LoadTexture(string fileName)
        {
            if (textures.ContainsKey(fileName))
            {
                return textures[fileName];
            }

            var texture = content.Load<Texture2D>(string.Format("Textures/{0}", fileName));
            textures.Add(fileName, texture);
            return texture;
        }

        /// <summary>
        /// Unloads all resources from memory.
        /// </summary>
        public void Clear()
        {
            textures.Clear();
        }
    }
}
