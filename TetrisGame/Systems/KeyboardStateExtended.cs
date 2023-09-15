using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TetrisGame.Systems
{
    public struct KeyboardStateExtended
    {
        public KeyboardState CurrentKeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }

        public KeyboardStateExtended(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            CurrentKeyboardState = currentKeyboardState;
            PreviousKeyboardState = previousKeyboardState;
        }

        public bool CapsLock => CurrentKeyboardState.CapsLock;
        public bool NumLock => CurrentKeyboardState.NumLock;
        public bool IsShiftDown() => CurrentKeyboardState.IsKeyDown(Keys.LeftShift) || CurrentKeyboardState.IsKeyDown(Keys.RightShift);
        public bool IsControlDown() => CurrentKeyboardState.IsKeyDown(Keys.LeftControl) || CurrentKeyboardState.IsKeyDown(Keys.RightControl);
        public bool IsAltDown() => CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) || CurrentKeyboardState.IsKeyDown(Keys.RightAlt);
        public bool IsKeyDown(Keys key) => CurrentKeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => CurrentKeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);

        /// <summary>
        /// Get list of keys that are being held down on the keyboard right now
        /// </summary>
        /// <returns></returns>
        public Keys[] GetDownKeys() => CurrentKeyboardState.GetPressedKeys();

        public Keys[] GetUpKeys()
        {
            var instance = this;
            return PreviousKeyboardState.GetPressedKeys().Where(key => instance.IsKeyUp(key)).ToArray();
        }
        /*
       public bool WasKeyJustDown(Keys key) => PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key);
       public bool WasKeyJustUp(Keys key) => PreviousKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key);
       public bool WasAnyKeyJustDown() => PreviousKeyboardState.GetPressedKeys().Any();
       */
    }
}
