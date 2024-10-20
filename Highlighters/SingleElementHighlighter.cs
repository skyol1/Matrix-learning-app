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
        public override void Highlight(uint row, uint column)
        {
            AddHighlight(row, column, row, column, highlightColor);
            base.Highlight(row, column);
        }
    }
}
