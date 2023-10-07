using Microsoft.Xna.Framework;
using System;

namespace TetrisGame.TetrisEngine
{
    internal class OShape : Tetrimino
    {
        public OShape(Point NorthPosition) : base(Shape.O, NorthPosition)
        {
        }

        /// <summary>
        /// Always return a new array of points. This is to prevent the user from modifying the original array.
        /// </summary>
        public override Point[] Coords => new Point[]
                        {
                    new Point(NorthPosition.X, NorthPosition.Y),
                    new Point(NorthPosition.X + 1, NorthPosition.Y),
                    new Point(NorthPosition.X, NorthPosition.Y - 1),
                    new Point(NorthPosition.X + 1, NorthPosition.Y - 1)
                        };

        public override Tetrimino Clone() => new OShape(NorthPosition);


        public override Point GetNextNorthRotation(bool clockwise) => NorthPosition;

        public override Point[] GetNextRotation(bool clockwise) => Coords;
    }
}
