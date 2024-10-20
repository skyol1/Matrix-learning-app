using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaticeApp.Highlighters
{
    public abstract class BaseHighlighter
    {
        
        public Color highlightColor;
        
        protected Rectangle? _highlightRectangle;
        public bool isHighlighted {  get;protected set; }
        
        public BaseHighlighter(Color highlightColor)
        {
            this.highlightColor = highlightColor;
        }
        public abstract void Highlight(int row, int column);
        public abstract void ClearHighlight();
    }
}