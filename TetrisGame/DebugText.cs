using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TetrisGame.Systems;

namespace TetrisGame
{
    /// <summary>
    /// Used to draw debug text to the screen
    /// </summary>
    internal class DebugText : IRenderable
    {
        readonly MouseStateExtended mouseState;
        readonly SpriteFont font;
        readonly GameWindow window;
        readonly FpsUps fpsups;

        public DebugText(SpriteFont font, GameWindow window, MouseStateExtended mouseState, FpsUps fpsups = null)
        {
            this.mouseState = mouseState;
            this.font = font;
            this.window = window;
            this.fpsups = fpsups;
        }

        /// <summary>
        /// Draws the mouse coordinates in the top right corner of the screen
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            var size = font.MeasureString(XY);
            var position = new Vector2(window.ClientBounds.Width - size.X, 10);
            sb.DrawString(font, XY, position, Color.White);
            if (fpsups != null)
            {
                var str = string.Format("FPS/UPS: {0} / {1}", fpsups.Fps, fpsups.Ups);
                size = font.MeasureString(str);
                position = new Vector2(window.ClientBounds.Width - size.X, 10 + size.Y + 10);
                sb.DrawString(font, str, position, Color.White);
            }
        }

        private string XY => $"X: {mouseState.X} Y: {mouseState.Y}";

    }
}
