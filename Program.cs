using System;
using System.Collections.Generic;
using System.Linq;

namespace DraughtboardPuzzleConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      var solver = new Solver();
      var solution = solver.Solve().ToList();
      Draw.DrawSolution(solution);
    }
  }
}
