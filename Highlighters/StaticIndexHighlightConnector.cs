namespace MaticeApp.Highlighters
{
    public class StaticIndexHighlightConnector : IHighlightable
    {
        private IHighlightable connectedHighlighter;
        private int staticRow;
        private int staticColumn;
        public bool isHighlighted { get; protected set; }
        public StaticIndexHighlightConnector(IHighlightable connectedHighlighter,int staticRow,int staticColumn) { this.connectedHighlighter = connectedHighlighter; }
        public void ClearHighlight()
        {
            if (!isHighlighted || !connectedHighlighter.isHighlighted) return;
            connectedHighlighter.ClearHighlight();
            isHighlighted = false;
        }
        public void Highlight(int row, int column)
        {
            if (isHighlighted || connectedHighlighter.isHighlighted) return;
            connectedHighlighter.Highlight(staticRow, staticColumn);
            isHighlighted = true;
        }
    }
}