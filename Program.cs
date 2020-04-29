using System;
using System.Linq;

namespace DraughtboardPuzzleConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      var solver = new Solver();
      solver.Solve();
      var solution = solver.FirstSolution;
      var rowIndices = solution.RowIndexes.Select(rowIndex => rowIndex.ToString());
      Console.WriteLine($"rowIndices: [{String.Join(", ", rowIndices)}]");
    }
  }
}
