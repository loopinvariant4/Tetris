using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisGame
{
    public interface IRenderable
    {
        void Draw(SpriteBatch sb, GameTime gameTime);
    }
}
