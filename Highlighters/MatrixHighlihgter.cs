using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace MaticeApp.Highlighters
{
    public class MatrixHighlihgter:BaseHighlighter
    {
        protected Matrix dstMatrix;

        public MatrixHighlihgter(Matrix dstMatrix, Color color): base(color) { this.dstMatrix = dstMatrix; }

        public override void Highlight(int row, int column)
        {
            foreach (var highlighter in dstMatrix.highlighters)
                if (!highlighter.isHighlighted)
                    highlighter.Highlight(row, column);
        }
        public override void ClearHighlight()
        {
            RemoveHighlight();
            foreach (var highlighter in dstMatrix.highlighters)
                if (highlighter.isHighlighted)
                    highlighter.ClearHighlight();
        }

        protected void AddHighlight(int rowA, int colA, int rowB, int colB, Color color)
        {
            if (_highlightRectangle != null) return;

            // Ensure rowA <= rowB and colA <= colB
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
            _highlightRectangle = new Rectangle
            {
                Stroke = Brushes.Red, // Color of the highlight border
                StrokeThickness = 2,
                Fill = new SolidColorBrush(color), // Semi-transparent fill
                Width = width,
                Height = height,
                IsHitTestVisible = false // Make sure the highlight doesn't block interaction
            };

            // Position the rectangle over the diagonal
            Canvas.SetLeft(_highlightRectangle, left);
            Canvas.SetTop(_highlightRectangle, top);

            // Add the rectangle to the highlight canvas
            dstMatrix.GetHighlightCanvas().Children.Add(_highlightRectangle);
            isHighlighted = true;
        }
        protected void RemoveHighlight()
        {
            if (_highlightRectangle == null) return;
            dstMatrix.GetHighlightCanvas().Children.Remove(_highlightRectangle);
            _highlightRectangle = null;
            isHighlighted = false;
        }
    }
}
