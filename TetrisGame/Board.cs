using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TetrisGame.Systems;
using TetrisGame.TetrisEngine;

namespace TetrisGame
{
    internal class Board : IUpdatable, IRenderable
    {
        const int COLS = 10;
        const int ROWS = 20;
        private readonly List<List<Block>> mainBlocks = new(ROWS);
        readonly GameWindow window;
        readonly Vector2 blockSize = new(40, 40);
        readonly Vector2 boardSize;
        readonly Vector2 boardPosition;
        private Texture2D line;
        const float BOARDTHICKNESS = 8f;
        private BoardMatrix boardMatrix;
        private KeyboardExtended keyboard;

        public Board(GameWindow window, GraphicsDevice device, KeyboardExtended keyboard)
        {
            this.window = window;
            this.keyboard = keyboard;
            keyboard.KeyDownSlow += handleKeyDownSlow;
            keyboard.KeyUp += handleKeyUp;
            int heightMargin = (int)(window.ClientBounds.Height * 0.10);
            int widthMargin = (int)(window.ClientBounds.Width * 0.40);
            boardSize = new Vector2(blockSize.X * 10, blockSize.Y * 20);
            boardPosition = new Vector2(widthMargin, heightMargin);
            init(device);
            resetBoard();
        }

        private void init(GraphicsDevice device)
        {
            line = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.White });

            var yellow = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            yellow.SetData(new[] { Color.Yellow });

            var lightBlue = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            lightBlue.SetData(new[] { Color.LightBlue });

            var final = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            final.SetData(new[] { Color.DarkGray });

            boardMatrix = new BoardMatrix(new Dictionary<Shape, Texture2D>{
                { Shape.O, yellow },
                { Shape.I, lightBlue },
                { Shape.Final, final}
            });
        }

        private void resetBoard()
        {
            mainBlocks.Clear();
            for (int i = 0; i < COLS; i++)
            {
                mainBlocks.Add(new List<Block>(ROWS));
                for (int j = 0; j < ROWS; j++)
                {
                    mainBlocks[i].Add(new Block(new Vector2(
                        (boardPosition.X + (40 * i)),
                        (boardPosition.Y + 40 * ROWS - (j + 1) * 40)), null));
                }
            }
        }

        private void DrawLine(SpriteBatch sb, Vector2 point, float width, float height, float angle, Color color, float layerDepth = 0)
        {
            var scale = new Vector2(width, height);
            sb.Draw(line, point, null, color, angle, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            drawBoardBorder(sb);
            drawBoardGrid(sb);
            drawBoard(sb, gameTime);
        }

        private void drawBoard(SpriteBatch sb, GameTime gameTime)
        {
            for (var i = 0; i < mainBlocks.Count; i++)
            {
                for (var j = 0; j < mainBlocks[i].Count; j++)
                {
                    mainBlocks[i][j].Draw(sb, gameTime);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            boardMatrix.UpdateBoard();
            updateBoardBlocks();
        }

        private void handleKeyDownSlow(object sender, Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    boardMatrix.HandleCommand(Command.LEFT);
                    break;
                case Keys.Right:
                    boardMatrix.HandleCommand(Command.RIGHT);
                    break;
                case Keys.Down:
                    boardMatrix.HandleCommand(Command.SOFTDROP);
                    break;
            }
        }

        private void handleKeyUp(object sender, Keys key)
        {
            switch (key)
            {
                case Keys.Space:
                    boardMatrix.HandleCommand(Command.HARDDROP);
                    break;
                case Keys.Up:
                    boardMatrix.HandleCommand(Command.ROTATECW);
                    break;
            }

        }

        private void updateBoardBlocks()
        {
            for (int i = 0; i < COLS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    mainBlocks[i][j].Square = boardMatrix.Matrix[i][j].Square;
                }
            }
        }

        private void drawBoardBorder(SpriteBatch sb)
        {
            //draw the board border
            DrawLine(sb, new Vector2(boardPosition.X - BOARDTHICKNESS, boardPosition.Y - BOARDTHICKNESS), boardSize.X + (2 * BOARDTHICKNESS), BOARDTHICKNESS, 0, Color.White);
            DrawLine(sb, new Vector2(boardPosition.X - BOARDTHICKNESS, boardPosition.Y - BOARDTHICKNESS), BOARDTHICKNESS, boardSize.Y + (2 * BOARDTHICKNESS), 0, Color.White);
            DrawLine(sb, new Vector2(boardPosition.X - BOARDTHICKNESS, boardPosition.Y + boardSize.Y), boardSize.X + (2 * BOARDTHICKNESS), BOARDTHICKNESS, 0, Color.White);
            DrawLine(sb, new Vector2(boardPosition.X + boardSize.X, boardPosition.Y - BOARDTHICKNESS), BOARDTHICKNESS, boardSize.Y + (2 * BOARDTHICKNESS), 0, Color.White);
        }

        private void drawBoardGrid(SpriteBatch sb)
        {
            // draw vertical lines
            for (int i = 0; i <= COLS; i++)
            {
                DrawLine(sb, new Vector2(boardPosition.X + (i * blockSize.X), boardPosition.Y), 1f, boardSize.Y, 0, Color.Red);
            }
            // draw horizontal lines
            for (int i = 0; i <= ROWS; i++)
            {
                DrawLine(sb, new Vector2(boardPosition.X, boardPosition.Y + (i * blockSize.Y)), boardSize.X, 1f, 0, Color.Red);
            }

        }
    }
}
