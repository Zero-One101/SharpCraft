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
    public class ResourceManager
    {
        private ContentManager content;
        private readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public ResourceManager(ContentManager content)
        {
            this.content = content;
        }

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
    }
}
