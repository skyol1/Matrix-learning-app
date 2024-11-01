using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaticeApp.Highlighters
{
    public interface IHighlightable
    {
        bool isHighlighted { get; }
        public void Highlight(int row, int column);
        public void ClearHighlight();
    }
}
