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
    public class ScalarHighlighter : BaseHighlighter
    {
        private Run dstRun;
        public ScalarHighlighter(Run dstRun, Color color) : base(color) { this.dstRun = dstRun; }
        public override void ClearHighlight()
        {
            RemoveHighlight();
        }
        public override void Highlight(int row, int column)
        {
            AddHighlight(highlightColor);
        }
        protected void AddHighlight(Color color)
        {
            if (isHighlighted) {return; }
            dstRun.Background = new SolidColorBrush(highlightColor);
            isHighlighted = true;
        }
        protected void RemoveHighlight()
        {
            if (!isHighlighted) { return; }
            dstRun.Background = new SolidColorBrush(Color.FromArgb(0,0,0,0));
            isHighlighted = false;
        }
    }
}
