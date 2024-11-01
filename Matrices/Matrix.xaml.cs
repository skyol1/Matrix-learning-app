using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using MaticeApp.Highlighters;
using System.Windows.Media.Media3D;
using System.Net;
using System.Numerics;
using System.Security.Cryptography.Xml;

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for Matrix.xaml
    /// </summary>
    public partial class Matrix : UserControl
    {
        public enum Bracket
        {
            Round,
            Straight
        }
        public Bracket BracketType { get; set; } = Bracket.Round;
        public bool CreateLeftBracket { get; set; } = true;
        public bool CreateRightBracket { get; set; } = true;
        public List<IHighlightable> highlighters { get; set; }

        public const int RowHeight = 30;
        public double CellWidth { get; set; } = 45 ;
        public bool AutoWidth { get; set; } = true;
        public int RowsCount {  get; private set; }
        public int ColumnsCount { get; private set; }
        public bool IsSet { get; private set; } = false;
        

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
            this.RowsCount = matrixValues.GetLength(0);
            this.ColumnsCount = matrixValues.GetLength(1);

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
            
            if(AutoWidth)
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
                        Background = new SolidColorBrush(Color.FromArgb(5, 0, 0, 0)),
                        MinWidth = 20
                    };

                    TextBlock textBlock = CreateRichText(matrixValues[i, j]);

                    border.Child = textBlock;

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    MatrixGrid.Children.Add(border);
                }
            }

            if(CreateLeftBracket)
                CreateBracket(LeftBracket, true);
            if(CreateRightBracket)
                CreateBracket(RightBracket, false);

            Width = Double.NaN;
            Background = Brushes.Transparent;
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

        private void CreateBracket(Viewbox viewbox, bool isLeft)
        {
            // Clear existing content
            viewbox.Child = null; // Clear the existing child

            if(BracketType == Bracket.Round)
            {
                Path path = new Path
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure { StartPoint = new Point(0, 0) };
                ArcSegment arcSegment = new ArcSegment
                {
                    Size = new Size(15 + Height * 0.3, Height * 0.75), // Example size, adjust as needed
                    Point = new Point(0, Height / 2),
                    SweepDirection = SweepDirection.Clockwise
                };
                pathFigure.Segments.Add(arcSegment);
                pathGeometry.Figures.Add(pathFigure);
                path.Data = pathGeometry;
                viewbox.Child = path;

                viewbox.SizeChanged += (s, e) =>
                {
                    if (isLeft)
                    {
                        ScaleTransform scaleTransform = new ScaleTransform
                        {
                            ScaleX = -1,
                        };
                        TranslateTransform translateTransform = new TranslateTransform
                        {
                            X = viewbox.ActualWidth
                        };
                        TransformGroup transformGroup = new TransformGroup();
                        transformGroup.Children.Add(scaleTransform);
                        transformGroup.Children.Add(translateTransform);
                        viewbox.RenderTransform = transformGroup;
                    }
                };
                
            }
            else if(BracketType == Bracket.Straight)
            {
                Rectangle rectangle = new Rectangle
                {
                    Width = 2.5,
                    Height = this.Height,
                    Fill = Brushes.Black
                };
                viewbox.Child = rectangle;
            }
            IsSet = true;
        }

        public void MakeSLR()
        {
            if (IsSet && ColumnsCount > 1 && RowsCount > 0)
            {
                // This section creates a straight line before the last column
                MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition());
                int lastColumn = ColumnsCount - 1;
                for (int i = 0; i < RowsCount; i++)
                {
                    if (MatrixGrid.Children[i * ColumnsCount + lastColumn] is Border border)
                    {
                        Grid.SetColumn(border, ColumnsCount);
                    }
                }
                MatrixGrid.ColumnDefinitions[ColumnsCount - 1].Width = new GridLength(2);
                Rectangle rectangle = new Rectangle
                {
                    Width = 2,
                    Height = this.Height,
                    Fill = Brushes.Black
                };
                Grid.SetRow(rectangle, 0);
                Grid.SetRowSpan(rectangle, RowsCount);
                Grid.SetColumn(rectangle, ColumnsCount - 1);
                MatrixGrid.Children.Add(rectangle);
            }
        }

        public void RowSwap(int row1, int row2)
        {
            if (row1 != row2)
            {
                if (row1 > row2)
                {
                    row2 += row1;
                    row1 = row2 - row1;
                    row2 -= row1;
                }
                int halfRowHeight = RowHeight / 2;

                CanvasRowOperations.Children.Add(CanvasCreateArrowPath(row1 * RowHeight));
                CanvasRowOperations.Children.Add(CanvasCreateArrowPath(row2 * RowHeight));

                CanvasRowOperations.Children.Add(CanvasCreateLine(10, (row1 * RowHeight + halfRowHeight), 20, (row1 * RowHeight + halfRowHeight)));
                CanvasRowOperations.Children.Add(CanvasCreateLine(10, (row2 * RowHeight + halfRowHeight), 20, (row2 * RowHeight + halfRowHeight)));
                CanvasRowOperations.Children.Add(CanvasCreateLine(20, (row1 * RowHeight + halfRowHeight), 20, (row2 * RowHeight + halfRowHeight)));

                CanvasRowOperations.Width = 20;
            }
        }

        public void RowMultiply(int row, string value)
        {
            TextBlock textBlockMultiplySymbol = new TextBlock
            {
                FontSize = 30,
                Text = "⋅"
            };
            TextBlock textBlockValue = new TextBlock
            {
                FontSize = 18,
                Text = $"({value})"
            };
            Canvas.SetLeft(textBlockMultiplySymbol, 3);
            Canvas.SetTop(textBlockMultiplySymbol, row * RowHeight - 9);
            Canvas.SetLeft(textBlockValue, 11);
            Canvas.SetTop(textBlockValue, row * RowHeight + 0.5);

            CanvasRowOperations.Children.Add(textBlockMultiplySymbol);
            CanvasRowOperations.Children.Add(textBlockValue);

            textBlockValue.Loaded += (s, e) =>
            {
                CanvasRowOperations.Width = 11 + textBlockValue.ActualWidth;
            };
        }

        public void RowAddMultiplied(int rowStart, int rowEnd, string value)
        {
            TextBlock textBlockMultiplySymbol = new TextBlock
            {
                FontSize = 30,
                Text = "⋅"
            };
            TextBlock textBlockValue = new TextBlock
            {
                FontSize = 18,
                Text = $"({value})"
            };
            Canvas.SetLeft(textBlockMultiplySymbol, 3);
            Canvas.SetTop(textBlockMultiplySymbol, rowStart * RowHeight - 9);
            Canvas.SetLeft(textBlockValue, 11);
            Canvas.SetTop(textBlockValue, rowStart * RowHeight + 0.5);

            CanvasRowOperations.Children.Add(textBlockMultiplySymbol);
            CanvasRowOperations.Children.Add(textBlockValue);

            textBlockValue.SizeChanged += (s, e) =>
            {
                int textBlockValueCenterX = (int)textBlockValue.ActualWidth / 2 + 11;

                int halfRowHeight = RowHeight / 2;
                int rowEndCenterY = rowEnd * RowHeight + halfRowHeight;

                int verticalLineHeight = Math.Abs(rowEnd - rowStart) * RowHeight - 10;
                int verticalLineY2 = rowEndCenterY + ((rowStart < rowEnd) ? -verticalLineHeight : verticalLineHeight);

                CanvasRowOperations.Children.Add(CanvasCreateArrowPath(rowEnd * RowHeight));
                CanvasRowOperations.Children.Add(CanvasCreateLine(10, rowEndCenterY, textBlockValueCenterX, rowEndCenterY));
                CanvasRowOperations.Children.Add(CanvasCreateLine(textBlockValueCenterX, rowEndCenterY, textBlockValueCenterX, verticalLineY2));

                CanvasRowOperations.Width = 11 + textBlockValue.ActualWidth;
            };
        }

        private Path CanvasCreateArrowPath(int y)
        {
            Path arrowPath = new Path
            {
                Fill = Brushes.Black,
                Width = 10,
                Height = RowHeight,

            };
            StreamGeometry triangleGeometry = new StreamGeometry();
            using (StreamGeometryContext ctx = triangleGeometry.Open())
            {
                int centerY = RowHeight / 2;
                // Start at the top point of the triangle
                ctx.BeginFigure(new Point(0, centerY), true, true);
                // Draw lines to form the triangle
                ctx.LineTo(new Point(10, centerY - 5), true, false);
                ctx.LineTo(new Point(10, centerY + 5), true, false);
            }
            arrowPath.Data = triangleGeometry;
            Canvas.SetLeft(arrowPath, 0);
            Canvas.SetTop(arrowPath, y);
            return arrowPath;
        }

        private Line CanvasCreateLine(int x1, int y1, int x2, int y2)
        {
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            return line;
        }

        private void MatrixGrid_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    if (MatrixGrid.Children[(i * ColumnsCount) + j].IsMouseOver)
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

        private void MatrixGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach (var highlighter in highlighters)
            {
                highlighter.ClearHighlight();
            }
        }
    }
}
