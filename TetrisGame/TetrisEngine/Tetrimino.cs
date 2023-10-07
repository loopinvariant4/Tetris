using Microsoft.Xna.Framework;
using System;

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

        public Tetrimino(Shape shape, Point boardPosition, int rotation = 0)
        {
            this.Shape = shape;
            this.Rotation = rotation;
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
        public abstract Point[] GetNextRotation(bool clockwise);

        /// <summary>
        /// Get the next North coordinate based off of the current rotation and NorthPosition
        /// </summary>
        /// <returns></returns>
        public abstract Point GetNextNorthRotation(bool clockwise);

        /// <summary>
        /// Get the coordinates of all the blocks for the next movement based off of the NorthPosition
        /// </summary>
        /// <returns></returns>
        public Point[] GetNextMovement(Movement move, int steps = 1)
        {
            var coords = new Point[Coords.Length];
            switch (move)
            {
                case Movement.Left:
                    for (int i = 0; i < coords.Length; i++)
                    {
                        coords[i] = new Point(Coords[i].X - steps, Coords[i].Y);
                    }
                    break;
                case Movement.Right:
                    for (int i = 0; i < coords.Length; i++)
                    {
                        coords[i] = new Point(Coords[i].X + steps, Coords[i].Y);
                    }
                    break;
                case Movement.Down:
                    for (int i = 0; i < coords.Length; i++)
                    {
                        coords[i] = new Point(Coords[i].X, Coords[i].Y - steps);
                    }
                    break;
                case Movement.Drop:
                    throw new NotImplementedException();
                default:
                    break;
            }
            return coords;
        }

        /// <summary>
        /// Get the next North coordinate based off of the current NorthPosition and the movement
        /// </summary>
        /// <returns></returns>
        public Point GetNextNorthMovement(Movement move, int steps = 1)
        {
            switch (move)
            {
                case Movement.Left:
                    return new Point(NorthPosition.X - steps, NorthPosition.Y);
                case Movement.Right:
                    return new Point(NorthPosition.X + steps, NorthPosition.Y);
                case Movement.Down:
                    return new Point(NorthPosition.X, NorthPosition.Y - steps);
                case Movement.Drop:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public abstract Tetrimino Clone();

        /// <summary>
        /// Set the new Rotation statet of the tetrimino and set the new NorthPosition
        /// </summary>
        public void Rotate(bool clockwise)
        {
            NorthPosition = GetNextNorthRotation(clockwise);
            Rotation = getNextRotationIndex(clockwise);
        }

        /// <summary>
        /// Set the new NorthPosition of the tetrimino based off of the movement
        /// </summary>
        /// <param name="move"></param>
        public void Move(Movement move, int steps = 1)
        {
            NorthPosition = GetNextNorthMovement(move, steps);
        }

        protected int getNextRotationIndex(bool clockwise) => clockwise ? (Rotation + 1) % 4 : (Rotation == 0 ? 3 : Rotation - 1);
    }
}
