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
        public RowHighlighter(Matrix dstMatrix, Color highlightColor) : base(dstMatrix, highlightColor) { }
        public override void Highlight(int row, int column)
        {
            AddHighlight(0, column, dstMatrix.RowsCount - 1, column, highlightColor);
            base.Highlight(row, column);
        }
    }
}
