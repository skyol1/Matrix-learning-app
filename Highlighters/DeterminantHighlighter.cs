using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaticeApp.Highlighters
{
    public class DeterminantHighlighter:MatrixHighlihgter
    {
        private List<Rectangle> _highlightRectangles = new List<Rectangle>();
        private Color highlightColor2;
        public DeterminantHighlighter(Matrix dstMatrix, Color highlightColor1, Color highlightColor2) : base(dstMatrix, highlightColor1) { this.highlightColor2 = highlightColor2; }
        public override void Highlight(int row, int column)
        {
            AddHighlightMultiple(0, 0, row - 1, column - 1, highlightColor);// left up
            AddHighlightMultiple(row+1, 0, dstMatrix.RowsCount-1, column - 1, highlightColor);// left down

            AddHighlightMultiple(0, column+1, row - 1,dstMatrix.ColumnsCount-1, highlightColor);// right up
            AddHighlightMultiple(row + 1, column+1, dstMatrix.RowsCount - 1, dstMatrix.ColumnsCount - 1, highlightColor);// right down

            AddHighlightMultiple(row, column, row, column, highlightColor2); //center

            base.Highlight(row, column);
        }

        public override void ClearHighlight()
        {
            RemoveHighlights();
        }

        private void AddHighlightMultiple(int rowA, int colA, int rowB, int colB, Color color)
        {
            if (rowA > rowB || colA > colB)
            {
                var tempRow = rowA;
                var tempCol = colA;
                rowA = rowB;
                colA = colB;
                rowB = tempRow;
                colB = tempCol;
            }

            // Calculate the position and size of the rectangle
            double top = rowA * Matrix.RowHeight;
            double left = colA * dstMatrix.CellWidth;
            double height = (rowB - rowA + 1) * Matrix.RowHeight;
            double width = (colB - colA + 1) * dstMatrix.CellWidth;

            // Create the rectangle for highlighting
            Rectangle newHighlightRectangle = new Rectangle
            {
                Stroke = Brushes.Red, // Color of the highlight border
                StrokeThickness = 2,
                Fill = new SolidColorBrush(color), // Semi-transparent fill
                Width = width,
                Height = height,
                IsHitTestVisible = false // Make sure the highlight doesn't block interaction
            };
            _highlightRectangles.Add(newHighlightRectangle);
            // Position the rectangle over the diagonal
            Canvas.SetLeft(newHighlightRectangle, left);
            Canvas.SetTop(newHighlightRectangle, top);

            // Add the rectangle to the highlight canvas
            dstMatrix.GetHighlightCanvas().Children.Add(newHighlightRectangle);
            isHighlighted = true;
        }

        protected void RemoveHighlights()
        {
            foreach (var highlight in _highlightRectangles)
            {
                dstMatrix.GetHighlightCanvas().Children.Remove(highlight);
            }
            _highlightRectangles.Clear();
            isHighlighted = false;
        }

    }
}
