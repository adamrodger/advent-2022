using System;
using System.Collections.Generic;

namespace AdventOfCode.Utilities
{
    public readonly record struct Point2D(int X, int Y)
    {
        public static implicit operator (int x, int y)(Point2D point)
        {
            return (point.X, point.Y);
        }

        public static implicit operator Point2D((int x, int y) coordinates)
        {
            return new Point2D(coordinates.x, coordinates.y);
        }

        public static implicit operator Point2D(Point3D point)
        {
            return new Point2D(point.X, point.Y);
        }

        public static Point2D operator +(Point2D a, Point2D b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        public IEnumerable<Point2D> Adjacent4()
        {
            yield return new Point2D(this.X, this.Y - 1);
            yield return new Point2D(this.X - 1, this.Y);
            yield return new Point2D(this.X + 1, this.Y);
            yield return new Point2D(this.X, this.Y + 1);
        }

        public IEnumerable<Point2D> Adjacent8()
        {
            yield return new Point2D(this.X - 1, this.Y - 1);
            yield return new Point2D(this.X, this.Y - 1);
            yield return new Point2D(this.X + 1, this.Y - 1);
            yield return new Point2D(this.X - 1, this.Y);
            yield return new Point2D(this.X + 1, this.Y);
            yield return new Point2D(this.X - 1, this.Y + 1);
            yield return new Point2D(this.X, this.Y + 1);
            yield return new Point2D(this.X + 1, this.Y + 1);
        }

        /// <summary>
        /// Move from this position to another in the given direction and number of steps
        /// </summary>
        /// <param name="bearing">Move direction</param>
        /// <param name="steps">Move steps</param>
        /// <returns>New position</returns>
        public Point2D Move(Bearing bearing, int steps = 1) => bearing switch
        {
            Bearing.North => (this.X, this.Y + steps),
            Bearing.South => (this.X, this.Y - steps),
            Bearing.East => (this.X + steps, this.Y),
            Bearing.West => (this.X - steps, this.Y),
            _ => throw new ArgumentOutOfRangeException()
        };

        public int ManhattanDistance(Point2D other) => Math.Abs(this.X - other.X) + Math.Abs(this.Y -other.Y);
    }

    public readonly record struct Point3D(int X, int Y, int Z)
    {
        public static implicit operator (int x, int y, int z)(Point3D point)
        {
            return (point.X, point.Y, point.Z);
        }

        public static implicit operator Point3D((int x, int y, int z) coordinates)
        {
            return new Point3D(coordinates.x, coordinates.y, coordinates.z);
        }

        public static implicit operator Point3D(Point2D point)
        {
            return new Point3D(point.X, point.Y, 0);
        }

        public static Point3D operator +(Point3D a, Point3D b)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public IEnumerable<Point3D> Adjacent4()
        {
            yield return new Point3D(this.X, this.Y - 1, this.Z);
            yield return new Point3D(this.X - 1, this.Y, this.Z);
            yield return new Point3D(this.X + 1, this.Y, this.Z);
            yield return new Point3D(this.X, this.Y + 1, this.Z);
        }

        public int ManhattanDistance(Point3D other)
        {
            return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y) + Math.Abs(this.Z - other.Z);
        }
    }
}
