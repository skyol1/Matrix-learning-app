using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaticeApp.Highlighters
{
    public class ScalarHighlighter:BaseHighlighter
    {
        private Run dstRun;
        public ScalarHighlighter(Run dstRun, Color color): base(color) { this.dstRun = dstRun; }
        public override void ClearHighlight()
        {
            throw new NotImplementedException();
        }
        public override void Highlight(int row, int column)
        {
            throw new NotImplementedException();
        }
    }
}
