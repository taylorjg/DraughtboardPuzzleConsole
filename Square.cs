namespace DraughtboardPuzzleConsole
{
    public class Square
    {
        public Square(int x, int y, Colour colour)
        {
            X = x;
            Y = y;
            Colour = colour;
        }

        public readonly int X;
        public readonly int Y;
        public readonly Colour Colour;
    }
}
