using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TetrisGame.Systems
{
    public class KeyboardExtended : ISystem, IUpdatable
    {
        /// <summary>
        /// This event handler handles a key being pressed as well as introduce a frame delay if the key is held down
        /// </summary>
        public event EventHandler<Keys> KeyDownSlow;

        public event EventHandler<Keys> KeyUp;

        private int frameDelay = 5;

        private Dictionary<Keys, int> keyFrames = new Dictionary<Keys, int>();

        private KeyboardStateExtended state;
        public KeyboardExtended()
        {
            KeyboardState kbs = new KeyboardState();
            state = new KeyboardStateExtended(kbs, kbs);
        }
        public void Init(Game game)
        {
        }

        public void Update(GameTime gameTime)
        {
            state.PreviousKeyboardState = state.CurrentKeyboardState;
            state.CurrentKeyboardState = Keyboard.GetState();

            foreach (var key in state.GetDownKeys())
            {
                if (keyFrames.ContainsKey(key))
                {
                    keyFrames[key]++;
                    if (keyFrames[key] % frameDelay == 0)
                    {
                        KeyDownSlow?.Invoke(this, key);
                    }
                }
                else
                {
                    keyFrames.Add(key, 0);
                    KeyDownSlow?.Invoke(this, key);
                }
            }
            foreach (var key in state.GetUpKeys())
            {
                keyFrames.Remove(key);
                KeyUp?.Invoke(this, key);
            }
        }

        public KeyboardStateExtended GetState()
        {
            return state;
        }
    }
}
