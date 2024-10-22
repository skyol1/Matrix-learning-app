using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using MaticeApp.Highlighters;

namespace MaticeApp
{
    public partial class InputMatrix : IMatrix
    {
        public List<IHighlightable> highlighters { get; set; }
        public int RowHeight { get; set; } = 30;
        public int CellWidth { get; set; } = 70;
        public uint RowsCount {  get;private set; }
        public uint ColumnsCount { get; private set; }
        public Action onInputChanged;
        private const int MaxInput= 10;

        public InputMatrix()
        {
            InitializeComponent();
            highlighters = new List<IHighlightable>();
        }
        public Canvas GetHighlightCanvas()
        {
            return HighlightCanvas;
        }
        public void SetMatrix(uint rows, uint columns)
        {
            MatrixGrid.Children.Clear();
            MatrixGrid.RowDefinitions.Clear();
            MatrixGrid.ColumnDefinitions.Clear();
            this.RowsCount = rows;
            this.ColumnsCount = columns;
            // Define rows and columns for the matrix grid
            for (int i = 0; i < rows; i++)
            {
                MatrixGrid.RowDefinitions.Add(new RowDefinition { });
            }

            for (int j = 0; j < columns; j++)
            {
                MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition { });
            }

            Height = rows * RowHeight;
            MatrixGrid.Width = columns * CellWidth;

            // Set the values in the grid
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Border border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(5),
                        Margin = new Thickness(1),
                        Background= new SolidColorBrush(Color.FromArgb(10, 0, 0, 0))
                    };

                    TextBox textBox = new TextBox
                    {
                        Text = 0.ToString(),
                        FontSize = 18,
                        TextAlignment= TextAlignment.Center,
                        MaxLength = 6
                    };
                    textBox.TextChanged += OnInputTextChanged;
                    border.Child = textBox;
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    MatrixGrid.Children.Add(border);
                }
            }

            // Calculate and set the UserControl's height based on the number of rows
            Height = rows * RowHeight;
            Width = columns * CellWidth+10;

            // Update the arc sizes for the brackets
            double arcSize = (Height * 0.45);
            double arcHeight = (Height * 0.75);

            // Set the properties for the arcs
            LeftArc.Size = new Size(arcSize, arcHeight);
            LeftArc.Point = new Point(0, Height / 2);
            RightArc.Size = new Size(arcSize, arcHeight);
            RightArc.Point = new Point(0, Height / 2);

            Width = 2 * LeftBracket.Width + MatrixGrid.Width;
        }

        private void LeftBracket_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            translateTransformLeftBracket.X = LeftBracket.ActualWidth;
        }

        private void MatrixGrid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            for (uint i = 0; i < RowsCount; i++)
            {
                for (uint j = 0; j < ColumnsCount; j++)
                {
                    if (MatrixGrid.Children[(int)((i * ColumnsCount) + j)].IsMouseOver)
                    {
                        foreach (var highlighter in highlighters)
                        {
                            highlighter.ClearHighlight();
                            highlighter.Highlight(i, j);
                        }
                    }
                }
            }
        }

        private void MatrixGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            foreach (var highlighter in highlighters)
            {
                highlighter.ClearHighlight();
            }
        }
        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            double tmp;
            foreach (var textbox in MatrixGrid.Children)
            {
                if (!double.TryParse(((textbox as Border).Child as TextBox).Text, out tmp)) return;
            }
            if (onInputChanged != null)
            {
                onInputChanged();
            }
        }
        public void Randomize()
        {
            var rnd = new Random();
            foreach (var textbox in MatrixGrid.Children)
                ((textbox as Border).Child as TextBox).Text = (rnd.Next()%MaxInput).ToString();
        }
        public void Clear()
        {
            foreach (var textbox in MatrixGrid.Children)
                ((textbox as Border).Child as TextBox).Text = "0";
        }
    }
}
