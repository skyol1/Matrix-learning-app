using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using MaticeApp.Highlighters;

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for Matrix.xaml
    /// </summary>
    public partial class Matrix : IMatrix
    {
        public List<IHighlightable> highlighters { get; set; }
        public int RowHeight{ get; set; } = 30;
        public int CellWidth { get; set; } = 55 ;
        public uint RowsCount {  get; private set; }
        public uint ColumnsCount { get; private set; }

        public Matrix()
        {
            InitializeComponent();
            highlighters=new List<IHighlightable>();
        }
        public Canvas GetHighlightCanvas()
        {
            return HighlightCanvas;
        }

        public void SetMatrix(string[,] matrixValues)
        {
            MatrixGrid.Children.Clear();
            MatrixGrid.RowDefinitions.Clear();
            MatrixGrid.ColumnDefinitions.Clear();
            this.RowsCount = (uint)matrixValues.GetUpperBound(0)+1;
            this.ColumnsCount = (uint)matrixValues.GetUpperBound(1)+1;
            // Define rows and columns for the matrix grid
            for (int i = 0; i < RowsCount; i++)
            {
                MatrixGrid.RowDefinitions.Add(new RowDefinition { });
            }

            for (int j = 0; j < ColumnsCount; j++)
            {
                MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition { });
            }

            Height = RowsCount * RowHeight;
            MatrixGrid.Width = ColumnsCount * CellWidth;


            // Set the values in the grid
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    Border border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(5),
                        Margin = new Thickness(0.5),
                        Background= new SolidColorBrush(Color.FromArgb(5, 0, 0, 0))
                    };

                    TextBlock textBlock = CreateRichText(matrixValues[i, j]);

                    border.Child = textBlock;
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    MatrixGrid.Children.Add(border);
                }
            }

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

        private TextBlock CreateRichText(string text)
        {
            TextBlock textBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            int i = 0;
            while (i < text.Length)
            {
                char c = text[i];

                if (c == '_') // Check for subscript markers
                {
                    i++; // Move past the underscore
                    string subscript = "";

                    // Gather characters for subscript until the next underscore
                    while (i < text.Length && text[i] != '_')
                    {
                        subscript += text[i];
                        i++;
                    }

                    // If another underscore is reached, set the subscript
                    if (i < text.Length && text[i] == '_')
                    {
                        // Create a subscript run with smaller font size
                        Run subscriptRun = new Run(subscript)
                        {
                            BaselineAlignment = BaselineAlignment.Subscript,
                            FontSize = 12 // Font size for subscript
                        };
                        textBlock.Inlines.Add(subscriptRun);
                        i++; // Move past the closing underscore
                    }
                    else
                    {
                        // If there's no closing underscore, add the text normally
                        textBlock.Inlines.Add(new Run($"_{subscript}"));
                    }
                }
                else // Handle normal text
                {
                    string normalText = "";
                    while (i < text.Length && text[i] != '_')
                    {
                        normalText += text[i];
                        i++;
                    }
                    Run normalRun = new Run(normalText);
                    textBlock.Inlines.Add(normalRun);
                }
            }

            return textBlock;
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
    }
}
