using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TetrisGame.Systems
{
    public class MouseStateExtended : IUpdatable
    {
        private MouseState currentState;
        public MouseState PreviousState { get; set; }
        public MouseState CurrentState
        {
            get => currentState;
            set
            {
                PreviousState = CurrentState;
                currentState = value;
            }
        }

        public MouseStateExtended(MouseState prev, MouseState curr)
        {
            PreviousState = prev;
            CurrentState = curr;
        }

        #region MouseState methods
        public int X => CurrentState.X;
        public int Y => CurrentState.Y;
        public Point Position => CurrentState.Position;
        public ButtonState LeftButton => CurrentState.LeftButton;
        public ButtonState MiddleButton => CurrentState.MiddleButton;
        public ButtonState RightButton => CurrentState.RightButton;
        public int ScrollWheelValue => CurrentState.ScrollWheelValue;
        public int HorizontalScrollWheelValue => CurrentState.HorizontalScrollWheelValue;
        public ButtonState XButton1 => CurrentState.XButton1;
        public ButtonState XButton2 => CurrentState.XButton2;
        #endregion

        #region MouseState Extensions
        public bool IsLeftButtonDown => CurrentState.LeftButton == ButtonState.Pressed;
        public bool IsRightButtonDown => CurrentState.RightButton == ButtonState.Pressed;
        public bool IsLeftButtonUp => CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed;
        public bool IsRightButtonUp => CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed;

        public void Update(GameTime gameTime)
        {

        }

        #endregion
    }
}
