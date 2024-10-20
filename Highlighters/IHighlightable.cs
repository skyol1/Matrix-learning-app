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
        public void Highlight(uint row, uint column);
        public void ClearHighlight();
    }
}
