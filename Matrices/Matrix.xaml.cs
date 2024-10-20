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
        public double RowHeight{get;set; } = 30;
        public double CellWidth { get; set; } = 55 ;
        public uint RowsCount {  get;private set; }
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

        public void SetMatrix(uint rows, uint columns, string[,] matrixValues, bool autoWidth)
        {
            MatrixGrid.Children.Clear();
            MatrixGrid.RowDefinitions.Clear();
            MatrixGrid.ColumnDefinitions.Clear();
            this.RowsCount = rows;
            this.ColumnsCount = columns;
            // Define rows and columns for the matrix grid
            for (int i = 0; i < rows; i++)
            {
                MatrixGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(RowHeight) });
            }

            for (int j = 0; j < columns; j++)
            {
                MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition {Width= new GridLength(CellWidth) });
            }

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

                    TextBlock textBlock = CreateRichText(matrixValues[i, j]);

                    border.Child = textBlock;
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

            // Scale the UserControl's width based on the number of columns if needed
            if(autoWidth)
            {
                Width = (columns * 55) + 8;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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
