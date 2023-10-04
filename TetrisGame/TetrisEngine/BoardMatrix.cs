using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TetrisGame.Systems;
using TetrisGame.Debug;

namespace TetrisGame.TetrisEngine
{
    /// <summary>
    /// Represents a logical board matrix for Tetris. It will hold the current block on the board as well as where the blocks have landed.
    /// </summary>
    internal class BoardMatrix : IDebugText
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

        public string DebugText => BoardState.ToString();

        public List<int> rowsToEliminate;

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
            lockTimer.Tick += doFullLock;
            Globals.TextRenderer?.Register(this);
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
                case Command.HARDDROP:
                    curr.Move(Movement.Down, calcHardDropSteps());
                    autoFallRate = 0;
                    doFullLock(null, null);
                    break;
            }
        }

        private int calcHardDropSteps()
        {
            var steps = 0;
            while (true)
            {
                if (isValidLocation(curr.GetMovement(Movement.Down, steps)))
                {
                    steps++;
                }
                else
                {
                    break;
                }
            }
            return steps - 1;
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
                    doGeneration();
                    break;
                case State.Falling:
                    doFalling();
                    break;
                case State.Locking:
                    doLocking();
                    break;
                case State.Completion:
                    doCompletion();
                    break;
                case State.CheckPattern:
                    doCheckPattern();
                    break;
                case State.Animate:
                    doAnimate();
                    break;
                case State.Eliminate:
                    doEliminate();
                    break;
            }
        }

        /// <summary>
        /// Remove any marked rows from the board shifting the rows above, down. Award points based on the number of rows cleared.
        /// </summary>
        private void doEliminate()
        {
            if (rowsToEliminate != null)
            {
                var clearCount = rowsToEliminate.Count;
                for (var i = rowsToEliminate.Count - 1; i >= 0; i--) // elimate rows from bottom to top, one at a time
                {
                    var idx = rowsToEliminate[i];
                    for (var j = 0; j < COLS; j++) // blank out the row to be eliminated
                    {
                        placed[j][idx].Square = null;
                    }
                    for (var k = idx; k < ROWS - 1; k++) // shift the rows above this one by one row down
                    {
                        for (var l = 0; l < COLS; l++)
                        {
                            placed[l][k].Square = placed[l][k + 1].Square;
                        }
                    }
                    for (var m = 0; m < COLS; m++) // blank out the top row
                    {
                        placed[m][ROWS - 1].Square = null;
                    }
                } // do this for every row to be eliminated   
            }
            BoardState = State.Completion;
        }

        private void doAnimate()
        {
            BoardState = State.Eliminate;
        }

        /// <summary>
        /// Patterns are checked (such as line clear) and those rows are marked for deletion. This takes no extra time(or frames) to complete.
        /// </summary>
        private void doCheckPattern()
        {
            List<int> rowsToDelete = new List<int>();
            for (int i = 0; i < ROWS; i++)
            {
                bool rowFull = true;
                for (int j = 0; j < COLS; j++)
                {
                    if (placed[j][i].Square == null)
                    {
                        rowFull = false;
                        break;
                    }
                }
                if (rowFull)
                {
                    rowsToDelete.Add(i);
                }
            }
            rowsToEliminate = rowsToDelete;
            BoardState = State.Animate;
        }

        private void doLocking()
        {
            if (isValidLocation(curr.GetMovement(Movement.Down, 1)))
            {
                lockTimer.Stop();
                BoardState = State.Falling;
            }
        }

        private void doFullLock(object sender, EventArgs e)
        {
            if (curr != null)
            {
                foreach (var v in curr.Coords)
                {
                    placed[v.X][v.Y].Square = squares[curr.Shape];
                }
            }
            BoardState = State.CheckPattern;
        }

        private void doFalling()
        {
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
        }

        private void doGeneration()
        {
            curr = new OShape(new Point(5, 19));
            BoardState = State.Falling;
        }

        private void doCompletion()
        {
            BoardState = State.Generation;
            doGeneration();
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
