using System;
using Microsoft.Xna.Framework.Input;

namespace SharpCraft.Events
{
    public class KeyUpEventArgs : EventArgs
    {
        public Keys Key { get; private set; }

        public KeyUpEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
