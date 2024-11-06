using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MaticeApp.Highlighters
{
    class ColumnHighlighter: MatrixHighlihgter
    {
        public ColumnHighlighter(Matrix dstMatrix, Color highlightColor) : base(dstMatrix, highlightColor) { }
        public override void Highlight(int row, int column)
        {
            AddHighlight(row,0, row, dstMatrix.ColumnsCount-1, highlightColor);
            base.Highlight(row, column);
        }
    }
}
