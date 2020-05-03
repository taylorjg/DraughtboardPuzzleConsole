using System;

namespace DraughtboardPuzzleConsole
{
    public class Board
    {
        public readonly int BoardSize;
        private readonly Colour ColourOfSquareZeroZero;

        public Board(int boardSize)
        {
            BoardSize = boardSize;
            ColourOfSquareZeroZero = Colour.White;
        }

        public bool CanPlacePieceAt(RotatedPiece rotatedPiece, int x, int y)
        {
            if (x < 0 || x >= BoardSize) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y >= BoardSize) throw new ArgumentOutOfRangeException("y");

            if (x + rotatedPiece.Width > BoardSize) return false;
            if (y + rotatedPiece.Height > BoardSize) return false;

            // Check that each square of the piece to be placed has
            // the appropriate colour for its intended board position.
            for (var pieceX = 0; pieceX < rotatedPiece.Width; pieceX++)
            {
                for (var pieceY = 0; pieceY < rotatedPiece.Height; pieceY++)
                {
                    var square = rotatedPiece.SquareAt(pieceX, pieceY);
                    if (square != null)
                    {
                        var boardX = x + pieceX;
                        var boardY = y + pieceY;
                        var expectedColour = ColourOfSquareZeroZero.RelativeColour(boardX, boardY);
                        if (square.Colour != expectedColour)
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
