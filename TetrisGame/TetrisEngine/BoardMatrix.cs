using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TetrisGame.Systems;

namespace TetrisGame.TetrisEngine
{
    /// <summary>
    /// Represents a logical board matrix for Tetris. It will hold the current block on the board as well as where the blocks have landed.
    /// </summary>
    internal class BoardMatrix
    {
        const int COLS = 10;
        const int ROWS = 20;
        private List<List<Block>> matrix = new List<List<Block>>(ROWS);
        private List<List<Block>> placed = new List<List<Block>>(ROWS);
        private Tetrimino curr;
        private GameTimer lockTimer = new GameTimer(30, false); // this timer allows the player to move the tetrimino for 30 frames in locking phase. This is 0.5s assuming 60 FPS.
        private Dictionary<Shape, Texture2D> squares;

        public State BoardState { get; set; } = State.Generation;
        public int Level { get; set; } = 1;

        /// <summary>
        /// This determines the drop speed for each level. The key is the level and the value is the number of frames between each drop.
        /// </summary>
        private Dictionary<int, int> levelSpeeds = new Dictionary<int, int> { { 1, 60 }, { 2, 48 }, { 3, 38 }, { 4, 29 }, { 5, 22 }, { 6, 16 }, { 7, 12 }, { 8, 9 }, { 9, 6 }, { 10, 4 } };

        /// <summary>
        /// This field determines the rate at which tetriminos will fall. This rate is set in levelSpeeds. When softdrop is used, this rate is reset to 0.
        /// </summary>
        private int autoFallRate = 0;
        public List<List<Block>> Matrix => matrix;

        public BoardMatrix(Dictionary<Shape, Texture2D> squares)
        {
            this.squares = squares;
            for (int i = 0; i < COLS; i++)
            {
                matrix.Add(new List<Block>(ROWS));
                placed.Add(new List<Block>(ROWS));
                for (int j = 0; j < ROWS; j++)
                {
                    matrix[i].Add(new Block(new Vector2(i, j), null));
                    placed[i].Add(new Block(new Vector2(i, j), null));
                }
            }
            lockTimer.Stop();
            lockTimer.Tick += (sender, e) =>
            {
                BoardState = State.Completion;
            };
        }

        /// <summary>
        /// Performs an action on a tetrimino based on the player's input
        /// </summary>
        /// <param name="c"></param>
        public void HandleCommand(Command c)
        {
            switch (c)
            {
                case Command.LEFT:
                    if (isValidLocation(curr.GetMovement(Movement.Left, 1)))
                    {
                        curr.Move(Movement.Left, 1);
                    }
                    break;
                case Command.RIGHT:
                    if (isValidLocation(curr.GetMovement(Movement.Right, 1)))
                    {
                        curr.Move(Movement.Right, 1);
                    }
                    break;
                case Command.SOFTDROP:
                    if (isValidLocation(curr.GetMovement(Movement.Down, 1)))
                    {
                        curr.Move(Movement.Down, 1);
                        autoFallRate = 0; //reset the autoFallRate since we dont want both user initiated and auto initiated movement at the same time
                    }
                    break;
            }
        }

        private bool isValidLocation(Point[] loc)
        {
            foreach (var p in loc)
            {
                if (p.X < 0 || p.X >= COLS || p.Y < 0 || p.Y >= ROWS)
                {
                    return false;
                }
                if (placed[p.X][p.Y].Square != null)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateBoard()
        {
            updateBoardState();
            updateBoardSquares();
        }

        private void updateBoardState()
        {
            switch (BoardState)
            {
                case State.Generation:
                    curr = new OShape(new Point(5, 19));
                    BoardState = State.Falling;
                    break;
                case State.Falling:
                    if (autoFallRate == levelSpeeds[Level])
                    {
                        if (isValidLocation(curr.GetMovement(Movement.Down, 1)))
                        {
                            curr.Move(Movement.Down, 1);
                        }
                        else
                        {
                            BoardState = State.Locking;
                            lockTimer.Restart();
                        }
                        autoFallRate = 0;
                    }
                    else
                    {
                        autoFallRate++;
                    }
                    break;
                case State.Completion:
                    doCompletion();
                    BoardState = State.Generation;
                    break;
            }
        }

        private void doCompletion()
        {
            if(curr != null)
            {
                foreach (var v in curr.Coords)
                {
                    placed[v.X][v.Y].Square = squares[curr.Shape];
                }
            }
        }

        private void updateBoardSquares()
        {
            for (int i = 0; i < COLS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    matrix[i][j].Square = placed[i][j].Square;
                }
            }
            foreach (var v in curr.Coords)
            {
                matrix[v.X][v.Y].Square = squares[curr.Shape];
            }
        }
    }
}
