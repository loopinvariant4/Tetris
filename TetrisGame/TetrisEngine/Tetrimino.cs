using Microsoft.Xna.Framework;

namespace TetrisGame.TetrisEngine
{
    internal abstract class Tetrimino
    {
        public Shape Shape { get; protected set; }

        /// <summary>
        /// Determine the current rotation of the tetrimino. There can be up to 4 rotation states for each tetrimino
        /// </summary>
        public int Rotation { get; set; } = 0;

        /// <summary>
        /// The north position of the tetrimino on the board. This is used to calculate the coordinates of the blocks and the next rotation
        /// </summary>
        public Point NorthPosition { get; set; } = new Point(0, 0);

        public Tetrimino(Shape shape, Point boardPosition)
        {
            this.Shape = shape;
            NorthPosition = boardPosition;
        }

        /// <summary>
        /// Get the coordinates of all the blocks in the tetrimino on the board based on the NorthPosition
        /// </summary>
        public abstract Point[] Coords { get; }

        /// <summary>
        /// Get the coordinates of all the blocks for the next rotation based off of the current rotation and NorthPosition
        /// </summary>
        /// <returns></returns>
        public abstract Point[] GetRotation(bool clockwise);

        /// <summary>
        /// Get the next North coordinate based off of the current rotation and NorthPosition
        /// </summary>
        /// <returns></returns>
        public abstract Point GetNextNorthRotation(bool clockwise);

        /// <summary>
        /// Get the coordinates of all the blocks for the next movement based off of the NorthPosition
        /// </summary>
        /// <returns></returns>
        public abstract Point[] GetMovement(Movement move, int steps = 1);

        /// <summary>
        /// Get the next North coordinate based off of the current NorthPosition and the movement
        /// </summary>
        /// <returns></returns>
        public abstract Point GetNextNorthMovement(Movement move, int steps = 1);

        /// <summary>
        /// Set the new Rotation statet of the tetrimino and set the new NorthPosition
        /// </summary>
        public void Rotate(bool clockwise)
        {
            Rotation = clockwise ? (Rotation + 1) % 4 : (Rotation == 0 ? 3 : Rotation - 1);
            NorthPosition = GetNextNorthRotation(clockwise);
        }

        /// <summary>
        /// Set the new NorthPosition of the tetrimino based off of the movement
        /// </summary>
        /// <param name="move"></param>
        public void Move(Movement move, int steps = 1)
        {
            NorthPosition = GetNextNorthMovement(move, steps);
        }



        /*
        public Vector2[] GetRotation(bool clockwise) => Tetriminos(shape, rotateI(clockwise), nextR(clockwise));

        private Vector2 rotateI(bool clockwise)
        {
            if (clockwise)
            {
                switch (Rotation)
                {
                    case 0:
                        return new Vector2(BoardPosition.X + 2, BoardPosition.Y - 1);
                    case 1:
                        return new Vector2(BoardPosition.X + 1, BoardPosition.Y - 2);
                    case 2:
                        return new Vector2(BoardPosition.X - 2, BoardPosition.Y - 1);
                    case 3:
                        return new Vector2(BoardPosition.X - 1, BoardPosition.Y + 2);
                }
            }
            else
            {
                switch (Rotation)
                {
                    case 0:
                        return new Vector2(BoardPosition.X + 1, BoardPosition.Y - 2);
                    case 1:
                        return new Vector2(BoardPosition.X - 2, BoardPosition.Y - 1);
                    case 2:
                        return new Vector2(BoardPosition.X - 2, BoardPosition.Y + 2);
                    case 3:
                        return new Vector2(BoardPosition.X + 2, BoardPosition.Y + 1);
                }
            }
            Debug.Assert(1 == 1, "Code should not reach here for I-rotation");
            return new Vector2(0, 0);
        }

        private int nextR(bool clockwise)
        {
            if (clockwise)
            {
                return (Rotation + 1) % 4;
            }
            else if (Rotation == 0)
            {
                return 3;
            }
            else
            {
                return Rotation - 1;
            }
        }

        private Vector2[] Tetriminos(Shape shape, Vector2 boardPosition, int rotation)
        {
            var coords = new Vector2[4];
            switch (shape)
            {
                case Shape.O:
                    coords[0] = new Vector2(boardPosition.X, boardPosition.Y);
                    coords[1] = new Vector2(boardPosition.X + 1, boardPosition.Y);
                    coords[2] = new Vector2(boardPosition.X + 1, boardPosition.Y - 1);
                    coords[3] = new Vector2(boardPosition.X, boardPosition.Y - 1);
                    break;
                case Shape.I:
                    switch (rotation)
                    {
                        case 0:
                            coords[0] = new Vector2(boardPosition.X, boardPosition.Y);
                            coords[1] = new Vector2(boardPosition.X + 1, boardPosition.Y);
                            coords[2] = new Vector2(boardPosition.X + 2, boardPosition.Y);
                            coords[3] = new Vector2(boardPosition.X + 3, boardPosition.Y);
                            break;
                        case 1:
                            coords[0] = new Vector2(boardPosition.X, boardPosition.Y);
                            coords[1] = new Vector2(boardPosition.X, boardPosition.Y - 1);
                            coords[2] = new Vector2(boardPosition.X, boardPosition.Y - 2);
                            coords[3] = new Vector2(boardPosition.X, boardPosition.Y - 3);
                            break;
                        case 2:
                            coords[0] = new Vector2(boardPosition.X, boardPosition.Y);
                            coords[1] = new Vector2(boardPosition.X - 1, boardPosition.Y);
                            coords[2] = new Vector2(boardPosition.X - 2, boardPosition.Y);
                            coords[3] = new Vector2(boardPosition.X - 3, boardPosition.Y);
                            break;
                        case 3:
                            coords[0] = new Vector2(boardPosition.X, boardPosition.Y);
                            coords[1] = new Vector2(boardPosition.X, boardPosition.Y + 1);
                            coords[2] = new Vector2(boardPosition.X, boardPosition.Y + 2);
                            coords[3] = new Vector2(boardPosition.X, boardPosition.Y + 3);
                            break;
                    }
                    break;
            }
            return coords;
        }
        */
    }
}
