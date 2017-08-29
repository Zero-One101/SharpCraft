using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SharpCraft.Events
{
    public class MouseMoveEventArgs : EventArgs
    {
        public Vector2 MouseVector { get; private set; }

        public MouseMoveEventArgs(Vector2 mouseVector)
        {
            MouseVector = mouseVector;
        }
    }
}
