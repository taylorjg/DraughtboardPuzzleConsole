using System;
using System.Collections.Generic;
using System.Linq;

namespace DraughtboardPuzzleConsole
{
    static class Draw
    {
        private static Dictionary<string, char> CORNERS_TABLE = new Dictionary<string, char>() {
            {"-b-b", '\u250F'},
            {"b--b", '\u2513'},
            {"b-b-", '\u251B'},
            {"-bb-", '\u2517'},

            {"-bbb", '\u2523'},
            {"b-bb", '\u252B'},
            {"bb-b", '\u2533'},
            {"bbb-", '\u253B'},

            {"-nbb", '\u2503'},
            {"n-bb", '\u2503'},
            {"bb-n", '\u2501'},
            {"bbn-", '\u2501'},

            {"nbbb", '\u2523'},
            {"bnbb", '\u252B'},
            {"bbnb", '\u2533'},
            {"bbbn", '\u253B'},

            {"bbnn", '\u2501'},
            {"nnbb", '\u2503'},
            {"bnbn", '\u251B'},
            {"nbnb", '\u250F'},
            {"bnnb", '\u2513'},
            {"nbbn", '\u2517'},

            {"bbbb", '\u254B'},
            {"nnnn", ' '}
        };

        private static Dictionary<string, char> SIDES_TABLE = new Dictionary<string, char>() {
            {"hn", ' '},
            {"vn", ' '},
            {"hb", '\u2501'},
            {"vb", '\u2503'}
        };

        private enum Corner { Nw, Ne, Sw, Se };
        private enum Side { Left, Right, Top, Bottom };

        private static char FindPieceNameAt(List<PiecePlacement> solution, int x, int y)
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

        private static (char, char, char, char) AdjacentCornerPieceNames(List<PiecePlacement> solution, int x, int y, Corner corner)
        {
            var thisPieceName = FindPieceNameAt(solution, x, y);
            var nPieceName = FindPieceNameAt(solution, x, y - 1);
            var sPieceName = FindPieceNameAt(solution, x, y + 1);
            var ePieceName = FindPieceNameAt(solution, x + 1, y);
            var wPieceName = FindPieceNameAt(solution, x - 1, y);
            var nwPieceName = FindPieceNameAt(solution, x - 1, y - 1);
            var nePieceName = FindPieceNameAt(solution, x + 1, y - 1);
            var swPieceName = FindPieceNameAt(solution, x - 1, y + 1);
            var sePieceName = FindPieceNameAt(solution, x + 1, y + 1);
            switch (corner)
            {
                case Corner.Nw: return (nwPieceName, nPieceName, wPieceName, thisPieceName);
                case Corner.Ne: return (nPieceName, nePieceName, thisPieceName, ePieceName);
                case Corner.Sw: return (wPieceName, thisPieceName, swPieceName, sPieceName);
                case Corner.Se: return (thisPieceName, ePieceName, sPieceName, sePieceName);
                default: throw new ArgumentOutOfRangeException("corner");
            }
        }

        private static char LookupCorner(List<PiecePlacement> solution, int x, int y, Corner corner)
        {
            var (nw, ne, sw, se) = AdjacentCornerPieceNames(solution, x, y, corner);
            string WallType(char pieceName1, char pieceName2) =>
                pieceName1 == '?' && pieceName2 == '?'
                    ? "-"
                    : pieceName1 == pieceName2 ? "n" : "b";
            var l = WallType(nw, sw);
            var r = WallType(ne, se);
            var u = WallType(nw, ne);
            var d = WallType(sw, se);
            var key = string.Concat(l, r, u, d);
            return CORNERS_TABLE[key];
        }

        private static (int, int) AdjacentSideCoords(int x, int y, Side side)
        {
            switch (side)
            {
                case Side.Left: return (x - 1, y);
                case Side.Right: return (x + 1, y);
                case Side.Top: return (x, y - 1);
                case Side.Bottom: return (x, y + 1);
                default: throw new ArgumentOutOfRangeException("side");
            }
        }

        private static (char, char) AdjacentSidePieceNames(List<PiecePlacement> solution, int x, int y, Side side)
        {
            var thisPieceName = FindPieceNameAt(solution, x, y);
            var (otherX, otherY) = AdjacentSideCoords(x, y, side);
            var otherPieceName = FindPieceNameAt(solution, otherX, otherY);
            return (thisPieceName, otherPieceName);
        }

        private static char LookupSide(List<PiecePlacement> solution, int x, int y, Side side)
        {
            var (thisPieceName, otherPieceName) = AdjacentSidePieceNames(solution, x, y, side);
            switch (side)
            {
                case Side.Left:
                case Side.Right:
                    {
                        var key = thisPieceName == otherPieceName ? "vn" : "vb";
                        return SIDES_TABLE[key];
                    }
                case Side.Top:
                case Side.Bottom:
                    {
                        var key = thisPieceName == otherPieceName ? "hn" : "hb";
                        return SIDES_TABLE[key];
                    }
                default:
                    throw new ArgumentOutOfRangeException("side");
            }
        }

        public static void DrawSolution(List<PiecePlacement> solution)
        {
            var xs = Enumerable.Range(0, 8).ToList();
            var ys = Enumerable.Range(0, 8).ToList();

            var topLine = "" + LookupCorner(solution, 0, 0, Corner.Nw);
            foreach (var x in xs)
            {
                topLine += new string(LookupSide(solution, x, 0, Side.Top), 3);
                topLine += LookupCorner(solution, x, 0, Corner.Ne);
            }
            Console.WriteLine(topLine);

            foreach (var y in ys)
            {
                var valuesLine = "" + LookupSide(solution, 0, y, Side.Left);
                var separatorLine = "" + LookupCorner(solution, 0, y, Corner.Sw);
                foreach (var x in xs)
                {
                    var pieceName = FindPieceNameAt(solution, x, y);
                    valuesLine += $" {pieceName} ";
                    valuesLine += LookupSide(solution, x, y, Side.Right);
                    separatorLine += new string(LookupSide(solution, x, y, Side.Bottom), 3);
                    separatorLine += LookupCorner(solution, x, y, Corner.Se);
                }
                Console.WriteLine(valuesLine);
                Console.WriteLine(separatorLine);
            }
        }
    }
}
