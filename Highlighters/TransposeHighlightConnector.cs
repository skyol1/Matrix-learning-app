using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MaticeApp.Highlighters
{
    public class TransposeHighlightConnector : IHighlightable
    {
        private BaseHighlighter connectedHighlighter;
        public bool isHighlighted { get; protected set; }
        public TransposeHighlightConnector(BaseHighlighter connectedHighlighter){ this.connectedHighlighter = connectedHighlighter; }
        public void ClearHighlight()
        {
            if (!isHighlighted || !connectedHighlighter.isHighlighted) return;
            connectedHighlighter.ClearHighlight();
            isHighlighted = false;
        }
        public void Highlight(int row, int column)
        {
            if (isHighlighted || connectedHighlighter.isHighlighted) return;
            connectedHighlighter.Highlight(column,row);
            isHighlighted = true;
        }
    }
}
