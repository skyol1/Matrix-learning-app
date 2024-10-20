using MaticeApp.Highlighters;
using System.Windows.Controls;

namespace MaticeApp
{
    public interface IMatrix
    {
        List<IHighlightable> highlighters { get; }
        double RowHeight { get; }
        double CellWidth {  get; }
        uint RowsCount { get; }
        uint ColumnsCount { get; }
        Canvas GetHighlightCanvas();
    }
}