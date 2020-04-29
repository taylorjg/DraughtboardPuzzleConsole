using System;
using System.Linq;
using DlxLib;

namespace DraughtboardPuzzleConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      var matrix = new[,] {
        {1, 0, 0, 0},
        {0, 1, 1, 0},
        {1, 0, 0, 1},
        {0, 0, 1, 1},
        {0, 1, 0, 0},
        {0, 0, 1, 0}
    };
      var dlx = new Dlx();
      var solutions = dlx.Solve(matrix);
      foreach (var solution in solutions)
      {
        var rowIndices = solution.RowIndexes.Select(rowIndex => rowIndex.ToString());
        Console.WriteLine($"rowIndices: [{String.Join(", ", rowIndices)}]");
      }
    }
  }
}
