using System;
using System.Collections.Generic;
using System.Linq;

namespace DraughtboardPuzzleConsole
{
    public class Piece
    {
        private readonly IEnumerable<Square> _squares;

        public Piece(IEnumerable<Square> squares, char name = '?')
        {
            _squares = squares;
            Name = name;
        }

        public Piece(string[] initStrings, char name = '?')
        {
            if (initStrings.Length == 0)
                throw new ArgumentException("At least one initialisation string must be provided.", "initStrings");

            int width = initStrings[0].Length;
            int height = initStrings.Length;

            if (!initStrings.All(s => s.Length == width))
                throw new ArgumentException("Initialisation strings must all have the same length.", "initStrings");

            var squares = new List<Square>();

            for (var y = 0; y < height; y++)
            {
                var s = initStrings[y];

                for (var x = 0; x < width; x++)
                {
                    switch (s[x])
                    {
                        case 'W':
                            squares.Add(new Square(x, height - y - 1, Colour.White));
                            break;
                        case 'B':
                            squares.Add(new Square(x, height - y - 1, Colour.Black));
                            break;
                        case ' ':
                            break;
                        default:
                            throw new ArgumentException("Initialisation strings must not contain characters other than SPACE or 'W' or 'B'.", "initStrings");
                    }
                }
            }

            if (squares.Count == 0)
                throw new ArgumentException("Zero squares found in the initialisation strings.", "initStrings");

            _squares = squares;
            Name = name;
        }

        public readonly char Name;

        public int Width
        {
            get
            {
                return _squares.Max(s => s.X) + 1;
            }
        }

        public int Height
        {
            get
            {
                return _squares.Max(s => s.Y) + 1;
            }
        }

        public Square SquareAt(int x, int y)
        {
            if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");

            return _squares.FirstOrDefault(s => s.X == x && s.Y == y);
        }

        public bool IsValid()
        {
            if (!_squares.Any())
                return false;

            var result = true;

            var firstSquare = _squares.First();
            var colourOfSquareZeroZero = firstSquare.Colour.RelativeColour(firstSquare.X, firstSquare.Y);

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var square = SquareAt(x, y);
                    if (square != null)
                    {
                        var expectedColour = colourOfSquareZeroZero.RelativeColour(x, y);
                        if (square.Colour != expectedColour)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
