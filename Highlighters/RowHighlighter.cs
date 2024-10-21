using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MaticeApp.Highlighters
{
    public class RowHighlighter: MatrixHighlihgter
    {
        public RowHighlighter(IMatrix dstMatrix, Color highlightColor) : base(dstMatrix, highlightColor) { }
        public override void Highlight(uint row, uint column)
        {
            AddHighlight(0, column, dstMatrix.RowsCount - 1, column, highlightColor);
            base.Highlight(row, column);
        }
    }
}
