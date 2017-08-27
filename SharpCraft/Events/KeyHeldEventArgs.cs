using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Events
{
    public class KeyHeldEventArgs : EventArgs
    {
        public Keys Key { get; private set; }

        public KeyHeldEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
