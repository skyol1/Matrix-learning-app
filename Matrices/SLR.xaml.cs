using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaticeApp.Static;

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for SLR.xaml
    /// </summary>
    public partial class SLR : UserControl
    {
        private const double canvasMarginUp = 3;
        public bool isSet { get; private set; } = false;
        public bool isExtended { get; private set; } = false;
        public Border[,] BordersCoefficients { get; private set; }
        public Border[] BordersFollows { get; private set; }
        
        public SLR()
        {
            InitializeComponent();

            BracketCanvas.Margin = new Thickness(0, canvasMarginUp, 0, 0);
        }

        public void SetValues(string[,] values)
        {
            if (isSet) return;

            int rows = values.GetLength(0);
            int columns = values.GetLength(1);

            if (rows < 2 || rows > 20 || columns < 3 || columns > 20) return;

            isSet = true;
            this.Width = double.NaN;
            this.Height = double.NaN;
            this.Background = Brushes.Transparent;

            for (int i = 0; i < rows; i++)
            {
                ContentGrid.RowDefinitions.Add(new RowDefinition { });
            }
            for (int j = 0; j < columns; j++)
            {
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { });
            }

            BordersCoefficients = new Border[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns - 1; j++)
                {
                    TextBlock coefficient = new TextBlock
                    {
                        Margin = new Thickness(5,0,5,0),
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Text = values[i, j]
                    };
                    TextBlock variable = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(0, 0, 0, 0),
                        Text = "x"
                    };
                    Run variableIndex = new Run
                    {
                        Text = (j + 1).ToString(),
                        BaselineAlignment = BaselineAlignment.Subscript,
                        FontSize = 14
                    };

                    variable.Inlines.Add(variableIndex);

                    Grid.SetRow(variable, i);
                    Grid.SetColumn(variable, j * 2 + 1);

                    Border borderCoefficient = BorderHighlighting(i, j * 2, 5, 0);
                    borderCoefficient.HorizontalAlignment = HorizontalAlignment.Right;
                    borderCoefficient.Child = coefficient;
                    BordersCoefficients[i, j] = borderCoefficient;

                    ContentGrid.Children.Add(borderCoefficient);
                    ContentGrid.Children.Add(variable);
                }

                TextBlock equals = new TextBlock
                {
                    Text = "=",
                    Margin = new Thickness(7, 0, 7, 0)
                };
                Grid.SetRow(equals, i);
                Grid.SetColumn(equals, (columns - 1) * 2);

                TextBlock constant = new TextBlock
                {
                    Margin = new Thickness(5, 0, 5, 0),
                    Text = values[i, columns - 1]
                };
                Border borderConstant = BorderHighlighting(i, (columns - 1) * 2 + 1, -5, -5);
                borderConstant.HorizontalAlignment = HorizontalAlignment.Left;
                borderConstant.Child = constant;
                BordersCoefficients[i, columns - 1] = borderConstant;

                ContentGrid.Children.Add(equals);
                ContentGrid.Children.Add(borderConstant);
            }
        }

        public void Extend(string[] indexes, string[] values)
        {
            if (!isSet || isExtended) return;

            int rows = ContentGrid.RowDefinitions.Count;
            int columns = ContentGrid.ColumnDefinitions.Count;

            if (indexes.Length != rows || values.Length != rows) return;

            isExtended = true;

            BordersFollows = new Border[rows];

            for (int j = 0; j < 4; j++)
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { });

            for (int i = 0; i < rows; i++)
            {
                TextBlock follows = new TextBlock
                {
                    Margin = new Thickness(15, 0, 15, 0),
                    Text = "⇒",
                };
                Grid.SetRow(follows, i);
                Grid.SetColumn(follows, columns);

                TextBlock variable = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Text = "x"
                };
                Run variableIndex = new Run
                {
                    Text = indexes[i],
                    BaselineAlignment = BaselineAlignment.Subscript,
                    FontSize = 14
                };
                variable.Inlines.Add(variableIndex);
                Grid.SetRow(variable, i);
                Grid.SetColumn(variable, columns + 1);

                TextBlock equals = new TextBlock
                {
                    Text = "=",
                    Margin = new Thickness(7, 0, 7, 0)
                };
                Grid.SetRow(equals, i);
                Grid.SetColumn(equals, columns + 2);

                TextBlock constant = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Text = values[i]
                };
                Grid.SetRow(constant, i);
                Grid.SetColumn(constant, columns + 3);

                Border border = BorderHighlighting(i, columns + 1, -7, -7);
                Grid.SetColumnSpan(border, 3);
                BordersFollows[i] = border;

                ContentGrid.Children.Add(border);
                ContentGrid.Children.Add(follows);
                ContentGrid.Children.Add(variable);
                ContentGrid.Children.Add(equals);
                ContentGrid.Children.Add(constant);
            }
        }

        private Border BorderHighlighting(int gridRow, int gridColumn, double marginLeft, double marginRight)
        {
            Border border = new Border
            {
                CornerRadius = new CornerRadius(5),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(marginLeft, -1, marginRight, -1)
            };
            Grid.SetRow(border, gridRow);
            Grid.SetColumn(border, gridColumn);
            return border;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = this.ActualHeight - canvasMarginUp;
            double curvesHeight = 10;

            if (height < curvesHeight * 4) return;

            double centerY = height / 2;

            BracketCanvas.Children.Add(CanvasCreateArc(2, 5.7, 0));
            BracketCanvas.Children.Add(CanvasCreateLine(5.7, 9.5, 5.7, centerY - 9.5));
            BracketCanvas.Children.Add(CanvasCreateArc(4, 0, centerY - 10));
            BracketCanvas.Children.Add(CanvasCreateArc(1, 0, centerY));
            BracketCanvas.Children.Add(CanvasCreateLine(5.7, centerY + 9.5, 5.7, height - 9.5));
            BracketCanvas.Children.Add(CanvasCreateArc(3, 5.7, height - 10));
        }

        private Path CanvasCreateArc(int quadrant, double canvasX, double canvasY)
        {
            Path path = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure 
            { 
                StartPoint = (quadrant == 1 || quadrant == 3) ? new Point(0, 0) : new Point(0, 10)
            };
            ArcSegment arcSegment = new ArcSegment
            {
                Size = new Size(6, 10),
                Point = (quadrant == 1 || quadrant == 3) ? new Point(6, 10) : new Point(6, 0),
                SweepDirection = (quadrant == 3 || quadrant == 4) ? 
                    SweepDirection.Counterclockwise : SweepDirection.Clockwise,
            };
            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);
            path.Data = pathGeometry;
            Canvas.SetLeft(path, canvasX);
            Canvas.SetTop(path, canvasY);
            return path;
        }
        private Line CanvasCreateLine(double x1, double y1, double x2, double y2)
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
    }
}
