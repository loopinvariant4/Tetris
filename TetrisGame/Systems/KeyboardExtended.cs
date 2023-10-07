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

        /// <summary>
        /// This is the number of frames to wait before triggering the KeyDownSlow event
        /// </summary>
        private int frameDelay = 2;  

        /// <summary>
        /// This is the number of frames to wait when the key is first pressed before triggering the KeyDownSlow event. 
        /// This ensures that we dont skip frames too fast if the user actually meant to tap the keyboard but the frameDelay 
        /// has a lower value to make sure continuous keydown feels fast
        /// </summary>
        private int initialFrameDelay = 10; 

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
                    keyFrames[key]--;
                    if (keyFrames[key] == 0)
                    {
                        KeyDownSlow?.Invoke(this, key);
                        keyFrames[key] = frameDelay;
                    }
                }
                else
                {
                    keyFrames.Add(key, initialFrameDelay);
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
