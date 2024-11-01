using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaticeApp.Highlighters
{
    public class SingleElementHighlighter : MatrixHighlihgter
    {
        public SingleElementHighlighter(IMatrix dstMatrix, Color highlightColor) : base(dstMatrix, highlightColor) { }
        public override void Highlight(int row, int column)
        {
            AddHighlight(row, column, row, column, highlightColor);
            base.Highlight(row, column);
        }
    }
}
