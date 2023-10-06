using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TetrisGame.Systems;

namespace TetrisGame.Debugger
{
    /// <summary>
    /// Used to draw debug text to the screen
    /// </summary>
    internal class DebugTextRenderer : IRenderable
    {
        readonly SpriteFont font;
        readonly GameWindow window;
        public List<IDebugText> DebugTexts { get; set; } = new List<IDebugText>();

        public DebugTextRenderer(SpriteFont font, GameWindow window)
        {
            this.font = font;
            this.window = window;
        }

        /// <summary>
        /// Draws the mouse coordinates in the top right corner of the screen
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            var startHeight = 10;
            DebugTexts.ForEach((txt) =>
            {
                var size = font.MeasureString(txt.DebugText);
                var position = new Vector2(window.ClientBounds.Width - size.X, startHeight);
                sb.DrawString(font, txt.DebugText, position, Color.White);
                startHeight += (int)size.Y + 10;
            });
        }

        public void Register(IDebugText debugText)
        {
            DebugTexts.Add(debugText);
        }

        public void Unregister(IDebugText debugText)
        {
            DebugTexts.Remove(debugText);
        }
    }
}
