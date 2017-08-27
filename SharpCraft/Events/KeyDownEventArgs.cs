using System;
using Microsoft.Xna.Framework.Input;

namespace SharpCraft.Events
{
    public class KeyDownEventArgs : EventArgs
    {
        public Keys Key { get; private set; }

        public KeyDownEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
