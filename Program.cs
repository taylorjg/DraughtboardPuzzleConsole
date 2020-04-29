using System;
using System.Collections.Generic;
using System.Linq;

namespace DraughtboardPuzzleConsole
{
  class Program
  {
    private static void DrawSolution(List<PiecePlacement> solution)
    {
      char FindPieceNameAt(int x, int y)
      {
        foreach (var piecePlacement in solution)
        {
          var xWithinPiece = x - piecePlacement.Coords.X;
          var yWithinPiece = y - piecePlacement.Coords.Y;
          if (xWithinPiece >= 0 && xWithinPiece < piecePlacement.RotatedPiece.Width &&
              yWithinPiece >= 0 && yWithinPiece < piecePlacement.RotatedPiece.Height)
          {
            var square = piecePlacement.RotatedPiece.SquareAt(xWithinPiece, yWithinPiece);
            if (square != null)
            {
              return piecePlacement.RotatedPiece.Piece.Name;
            }
          }
        }
        return '?';
      }

      foreach (var y in Enumerable.Range(0, 8))
      {
        var names = Enumerable.Range(0, 8).Select(x => FindPieceNameAt(x, y));
        Console.WriteLine(string.Concat(names));
      }
    }

    static void Main(string[] args)
    {
      var solver = new Solver();
      var solution = solver.Solve().ToList();
      DrawSolution(solution);
    }
  }
}
