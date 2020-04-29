using System;
using System.Collections.Generic;
using System.Linq;

// TODO:
// * have Solve() return a list of PiecePlacement
// * add a DrawSolution() function
//  * list piece names/orientations/locations
//  - draw grid of letters
//  - add row/col separators between all grid cells
//  - skip separators within pieces
//  - use box characters

namespace DraughtboardPuzzleConsole
{
  class Program
  {
    private static void DrawSolution(IEnumerable<PiecePlacement> solution)
    {
      foreach (var piecePlacement in solution)
      {
        Console.WriteLine($"name: {piecePlacement.RotatedPiece.Piece.Name}; coords: ({piecePlacement.Coords.X}, {piecePlacement.Coords.Y}); orientation: {piecePlacement.RotatedPiece.Orientation}");
      }
    }

    static void Main(string[] args)
    {
      var solver = new Solver();
      var solution = solver.Solve();
      DrawSolution(solution);
    }
  }
}
