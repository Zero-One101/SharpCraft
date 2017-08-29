using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using SharpCraft.Events;

namespace SharpCraft.Managers
{
    /// <summary>
    /// Handles checking for input from various devices and relaying that information through events
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// Fired when a key is pressed this frame
        /// </summary>
        public event KeyDownHandler KeyDown;
        /// <summary>
        /// Fired when a key has been pressed for more than one frame
        /// </summary>
        public event KeyHeldHandler KeyHeld;
        /// <summary>
        /// Fired when a key is released this frame
        /// </summary>
        public event KeyUpHandler KeyUp;

        private KeyboardState keyboardState;
        private readonly List<Keys> downKeys = new List<Keys>();
        private readonly List<Keys> upKeys = new List<Keys>();
        private readonly List<Keys> prevDownKeys = new List<Keys>();
        private readonly List<Keys> heldKeys = new List<Keys>();

        /// <summary>
        /// Polls input devices to return their current state.
        /// Fires events based on received device states.
        /// </summary>
        public void Update()
        {
            keyboardState = Keyboard.GetState();

            heldKeys.Clear();
            prevDownKeys.Clear();
            prevDownKeys.AddRange(downKeys);
            downKeys.Clear();
            upKeys.Clear();

            downKeys.AddRange(keyboardState.GetPressedKeys());

            foreach (var downKey in downKeys)
            {
                FireKeyDown(downKey);
            }

            foreach (var key in prevDownKeys.Where(key => downKeys.Contains(key)))
            {
                heldKeys.Add(key);
            }

            foreach (var heldKey in heldKeys)
            {
                FireKeyHeld(heldKey);
            }

            foreach (var key in prevDownKeys.Where(key => !downKeys.Contains(key)))
            {
                upKeys.Add(key);
            }

            foreach (var upKey in upKeys)
            {
                FireKeyUp(upKey);
            }
        }

        private void FireKeyDown(Keys downKey)
        {
            KeyDown?.Invoke(this, new KeyDownEventArgs(downKey));
        }

        private void FireKeyUp(Keys upKey)
        {
            KeyUp?.Invoke(this, new KeyUpEventArgs(upKey));
        }

        private void FireKeyHeld(Keys heldHey)
        {
            KeyHeld?.Invoke(this, new KeyHeldEventArgs(heldHey));
        }
    }
}
