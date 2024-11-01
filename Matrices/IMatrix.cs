using MaticeApp.Highlighters;
using System.Windows.Controls;

namespace MaticeApp
{
    public interface IMatrix
    {
        List<IHighlightable> highlighters { get; }
        double RowHeight { get; }
        double CellWidth { get; }
        int RowsCount { get; }
        int ColumnsCount { get; }
        Canvas GetHighlightCanvas();
    }
}