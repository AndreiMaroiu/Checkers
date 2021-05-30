using System.Windows;

namespace joc_dame.Models
{
    public struct Position
    {
        const int SIZE = 8;

        public int x;
        public int y;

        public Position(int x = 0, int y = 0)
            => (this.x, this.y) = (x, y);

        public Position(Position pos)
            => (x, y) = (pos.x, pos.y);

        public Position(Point point)
            => (x, y) = ((int)point.X, (int)point.Y);

        public override string ToString()
            => $"{x} {y}";

        public void Deconstruct(out int x, out int y)
            => (x, y) = (this.x, this.y);

        public static Position operator +(Position left, Position right)
            => new Position(left.x + right.x, left.y + right.y);

        public static Position operator -(Position left, Position right)
            => new Position(left.x - right.x, left.y - right.y);

        public static Position operator *(Position point, int scalar)
            => new Position(point.x * scalar, point.y * scalar);

        public static Position operator /(Position point, int scalar)
            => new Position(point.x / scalar, point.y / scalar);

        public static implicit operator Point(Position point)
            => new Point(point.x, point.y);

        public static explicit operator Position(Point point)
            => new Position((int)point.X, (int)point.Y);

        public override int GetHashCode()
            => x.GetHashCode() * 17 + y.GetHashCode();
    }
}
