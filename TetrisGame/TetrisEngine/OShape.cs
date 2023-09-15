using Microsoft.Xna.Framework;
using System;

namespace TetrisGame.TetrisEngine
{
    internal class OShape : Tetrimino
    {
        public OShape(Point NorthPosition) : base(Shape.O, NorthPosition)
        {
        }

        public override Point[] Coords => new Point[]
                {
                    new Point(NorthPosition.X, NorthPosition.Y),
                    new Point(NorthPosition.X + 1, NorthPosition.Y),
                    new Point(NorthPosition.X, NorthPosition.Y - 1),
                    new Point(NorthPosition.X + 1, NorthPosition.Y - 1)
                };

        public override Point[] GetMovement(Movement move, int steps = 1)
        {
            var coords = Coords;
            switch (move)
            {
                case Movement.Left:
                    for (int i = 0; i < coords.Length; i++)
                    {
                        coords[i] = new Point(coords[i].X - steps, coords[i].Y);
                    }
                    break;
                case Movement.Right:
                    for (int i = 0; i < coords.Length; i++)
                    {
                        coords[i] = new Point(coords[i].X + steps, coords[i].Y);
                    }
                    break;
                case Movement.Down:
                    for (int i = 0; i < coords.Length; i++)
                    {
                        coords[i] = new Point(coords[i].X, coords[i].Y - steps);
                    }
                    break;
                case Movement.Drop:
                    throw new NotImplementedException();
                default:
                    break;
            }
            return coords;
        }

        public override Point GetNextNorthMovement(Movement move, int steps = 1)
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

        public override Point GetNextNorthRotation(bool clockwise) => NorthPosition;

        public override Point[] GetRotation(bool clockwise) => Coords;
    }
}
