using System.Collections.Generic;
using System.Linq;
using DlxLib;

namespace DraughtboardPuzzleConsole
{
    internal class Solver
    {
        private class InternalRow
        {
            public InternalRow(bool[] matrixRow, PiecePlacement piecePlacement)
            {
                MatrixRow = matrixRow;
                PiecePlacement = piecePlacement;
            }

            public bool[] MatrixRow { get; private set; }
            public PiecePlacement PiecePlacement { get; private set; }
        }

        private readonly Piece[] _pieces;
        private readonly Board _board;
        private readonly IList<InternalRow> _data = new List<InternalRow>();

        public Solver()
        {
            _pieces = Pieces.ThePieces.ToArray();
            _board = new Board(8);
            _board.ForceColourOfSquareZeroZeroToBeWhite();
        }

        public IEnumerable<PiecePlacement> Solve()
        {
            BuildMatrixAndDictionary();
            var dlx = new Dlx();
            var solution = dlx.Solve(_data, d => d, r => r.MatrixRow).First();
            return solution.RowIndexes.Select(rowIndex => _data[rowIndex].PiecePlacement);
        }

        private void BuildMatrixAndDictionary()
        {
            for (var pieceIndex = 0; pieceIndex < _pieces.Length; pieceIndex++)
            {
                var piece = _pieces[pieceIndex];
                AddDataItemsForPieceWithSpecificOrientation(pieceIndex, piece, Orientation.North);
                var isFirstPiece = (pieceIndex == 0);
                if (!isFirstPiece)
                {
                    AddDataItemsForPieceWithSpecificOrientation(pieceIndex, piece, Orientation.South);
                    AddDataItemsForPieceWithSpecificOrientation(pieceIndex, piece, Orientation.East);
                    AddDataItemsForPieceWithSpecificOrientation(pieceIndex, piece, Orientation.West);
                }
            }
        }

        private void AddDataItemsForPieceWithSpecificOrientation(int pieceIndex, Piece piece, Orientation orientation)
        {
            var rotatedPiece = new RotatedPiece(piece, orientation);

            for (var x = 0; x < _board.BoardSize; x++)
            {
                for (var y = 0; y < _board.BoardSize; y++)
                {
                    _board.Reset();
                    _board.ForceColourOfSquareZeroZeroToBeWhite();
                    if (!_board.PlacePieceAt(rotatedPiece, x, y)) continue;
                    var dataItem = BuildDataItem(pieceIndex, rotatedPiece, new Coords(x, y));
                    _data.Add(dataItem);
                }
            }
        }

        private InternalRow BuildDataItem(int pieceIndex, RotatedPiece rotatedPiece, Coords coords)
        {
            var numColumns = _pieces.Length + _board.BoardSize * _board.BoardSize;
            var matrixRow = new bool[numColumns];

            matrixRow[pieceIndex] = true;

            var w = rotatedPiece.Width;
            var h = rotatedPiece.Height;

            for (var pieceX = 0; pieceX < w; pieceX++)
            {
                for (var pieceY = 0; pieceY < h; pieceY++)
                {
                    if (rotatedPiece.SquareAt(pieceX, pieceY) == null) continue;
                    var boardX = coords.X + pieceX;
                    var boardY = coords.Y + pieceY;
                    var boardLocationColumnIndex = _pieces.Length + (_board.BoardSize * boardX) + boardY;
                    matrixRow[boardLocationColumnIndex] = true;
                }
            }

            return new InternalRow(matrixRow, new PiecePlacement(rotatedPiece, coords));
        }
    }
}
