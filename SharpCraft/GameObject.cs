using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SharpCraft.Managers;

namespace SharpCraft
{
    public abstract class GameObject
    {
        protected EntityManager entityManager;

        public GameObject(EntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
