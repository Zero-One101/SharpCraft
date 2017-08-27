using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using SharpCraft.Events;

namespace SharpCraft.Managers
{
    public class InputManager
    {
        public event KeyDownHandler KeyDown;
        public event KeyUpHandler KeyUp;
        public event KeyHeldHandler KeyHeld;
        private KeyboardState keyboardState;
        private readonly List<Keys> downKeys = new List<Keys>();
        private readonly List<Keys> upKeys = new List<Keys>();
        private readonly List<Keys> prevDownKeys = new List<Keys>();
        private readonly List<Keys> heldKeys = new List<Keys>();

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
