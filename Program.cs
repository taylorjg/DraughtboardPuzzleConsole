using System;
using System.Linq;

namespace DraughtboardPuzzleConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      var solver = new Solver();
      var solution = solver.Solve();
      var rowIndices = solution.RowIndexes.Select(rowIndex => rowIndex.ToString());
      Console.WriteLine($"rowIndices: [{String.Join(", ", rowIndices)}]");
    }
  }
}
