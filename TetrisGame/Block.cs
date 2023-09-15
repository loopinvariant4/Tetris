using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisGame
{
    internal class Block : IRenderable
    {
        readonly Vector2 position;
        private Texture2D square;
        public bool IsEmpty { get; set; } = true;

        const int BLOCKSIZE = 40;
        readonly int size = BLOCKSIZE - 2;
        Rectangle rect = new(0, 0, 1, 1);

        public Vector2 Position => position;
        public Texture2D Square
        {
            get => square;
            set
            {
                square = value;
                IsEmpty = (value == null);
                if (!IsEmpty)
                {
                    rect = new Rectangle((int)position.X + 1, (int)position.Y + 1, size, size);
                }
            }
        }

        public Block(Vector2 position, Texture2D square)
        {
            this.position = position;
            this.Square = square;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            if (IsEmpty) return;
            sb.Draw(square, rect, Color.White);
        }
    }
}
