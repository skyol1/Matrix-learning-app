using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for Matrix.xaml
    /// </summary>
    public partial class Matrix : UserControl
    {
        private const double RowHeight = 30; // Constant row height
        private const double CellWidth = 55; // Constant column width

        public Matrix()
        {
            InitializeComponent();
        }

        public void SetMatrix(int rows, int columns, string[,] matrixValues, bool autoWidth)
        {
            MatrixGrid.Children.Clear();
            MatrixGrid.RowDefinitions.Clear();
            MatrixGrid.ColumnDefinitions.Clear();

            // Define rows and columns for the matrix grid
            for (int i = 0; i < rows; i++)
            {
                MatrixGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(RowHeight) });
            }

            for (int j = 0; j < columns; j++)
            {
                MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition());
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
                        Margin = new Thickness(1)
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



        private Rectangle? _highlightRectangle;
        public void Highlight(int rowA, int colA, int rowB, int colB, Color color = Color.FromArgb(50, 255, 0, 0))
        {
            // Remove the previous highlight if it exists
            if (_highlightRectangle != null)
            {
                MatrixGrid.Children.Remove(_highlightRectangle);
            }

            // Ensure rowA <= rowB and colA <= colB
            if (rowA > rowB || colA > colB)
            {
                var tempRow = rowA;
                var tempCol = colA;
                rowA = rowB;
                colA = colB;
                rowB = tempRow;
                colB = tempCol;
            }

            // Calculate the position and size of the rectangle
            double top = rowA * RowHeight;
            double left = colA * CellWidth;
            double height = (rowB - rowA + 1) * RowHeight;
            double width = (colB - colA + 1) * CellWidth;

            // Create the rectangle for highlighting
            _highlightRectangle = new Rectangle
            {
                Stroke = Brushes.Red, // Color of the highlight border
                StrokeThickness = 2,
                Fill = new SolidColorBrush(color), // Semi-transparent fill
                Width = width,
                Height = height,
                IsHitTestVisible = false // Make sure the highlight doesn't block interaction
            };

            // Position the rectangle over the diagonal
            Canvas.SetLeft(_highlightRectangle, left);
            Canvas.SetTop(_highlightRectangle, top);

            // Add the rectangle to the highlight canvas
            HighlightCanvas.Children.Add(_highlightRectangle);
        }

        private void LeftBracket_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            translateTransformLeftBracket.X = LeftBracket.ActualWidth;
        }
    }
}
