using MaticeApp.Highlighters;
using System.Windows.Controls;

namespace MaticeApp
{
    public interface IMatrix
    {
        List<IHighlightable> highlighters { get; }
        int RowHeight { get; }
        int CellWidth {  get; }
        uint RowsCount { get; }
        uint ColumnsCount { get; }
        Canvas GetHighlightCanvas();
    }
}