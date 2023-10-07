using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame.TetrisEngine
{
    internal class IShape : Tetrimino
    {
        public IShape(Point NorthPosition, int rotation = 0) : base(Shape.I, NorthPosition, rotation)
        {
        }

        public override Point[] Coords => getAllCoords(Rotation, NorthPosition);

        public override Tetrimino Clone() => new IShape(NorthPosition, Rotation);

        public override Point GetNextNorthRotation(bool clockwise)
        {
            switch (Rotation)
            {
                case 0:
                    return new Point(NorthPosition.X + 2, NorthPosition.Y + 1);
                case 1:
                    return new Point(NorthPosition.X + 1, NorthPosition.Y - 2);
                case 2:
                    return new Point(NorthPosition.X - 2, NorthPosition.Y - 1);
                case 3:
                    return new Point(NorthPosition.X - 1, NorthPosition.Y + 2);
            }
            throw new Exception("Rotation is out of range");
        }

        public override Point[] GetNextRotation(bool clockwise)
        {
            var rotation = getNextRotationIndex(clockwise);
            return getAllCoords(rotation, GetNextNorthRotation(clockwise));
        }

        private Point[] getAllCoords(int rotation, Point north)
        {
            switch (rotation)
            {
                case 0:
                    return new Point[]
                    {
                            new Point(north.X, north.Y),
                            new Point(north.X+1, north.Y),
                            new Point(north.X+2, north.Y),
                            new Point(north.X+3, north.Y)
                    };
                case 1:
                    return new Point[]
                    {
                            new Point(north.X, north.Y),
                            new Point(north.X, north.Y - 1),
                            new Point(north.X, north.Y - 2),
                            new Point(north.X, north.Y - 3)
                    };
                case 2:
                    return new Point[]
                    {
                            new Point(north.X, north.Y),
                            new Point(north.X-1, north.Y),
                            new Point(north.X-2, north.Y),
                            new Point(north.X-3, north.Y)
                    };
                case 3:
                    return new Point[]
                    {
                            new Point(north.X, north.Y),
                            new Point(north.X, north.Y + 1),
                            new Point(north.X, north.Y + 2),
                            new Point(north.X, north.Y + 3)
                    };

            }
            throw new Exception("Rotation is out of range");
        }

    }
}
