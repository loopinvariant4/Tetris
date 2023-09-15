using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TetrisGame
{
    public class FpsUps : IRenderable, IUpdatable
    {
        private float fps = 0f, ups = 0f;
        public float Fps => fps;
        public float Ups => ups;
        private TimeSpan elapsedFps = TimeSpan.Zero, elapsedUps = TimeSpan.Zero;
        private int fpsCounter = 0, upsCounter = 0;
        TimeSpan ONESECOND = TimeSpan.FromSeconds(1);

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            fpsCounter += 1;
            elapsedFps += gameTime.ElapsedGameTime;
            if (elapsedFps > ONESECOND)

            {
                elapsedFps -= ONESECOND;
                fps = fpsCounter;
                fpsCounter = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            upsCounter += 1;
            elapsedUps += gameTime.ElapsedGameTime;
            if (elapsedUps > ONESECOND)
            {
                elapsedUps -= ONESECOND;
                ups = upsCounter;
                upsCounter = 0;
            }
        }
    }
}
