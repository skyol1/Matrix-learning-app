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
    /// Interaction logic for OutputMatrix.xaml
    /// </summary>
    public partial class OutputMatrix : UserControl
    {
        private TextBlock[,] TextBlocks;

        private Border[,] Cells;

        public const int Rows = 9;

        public const int Columns = 9;

        public const int RowHeight = 30;
        public double MinCellWidth { get; set; } = 40;
        public double CellWidth { get; set; } = 45;
        public int RowsVisible { get; private set; } = 0;
        public int ColumnsVisible { get; private set; } = 0;

        public OutputMatrix()
        {
            InitializeComponent();

            Cells = new Border[Rows, Columns];
            TextBlocks = new TextBlock[Rows, Columns];

            SetMatrix();
        }

        private void SetMatrix()
        {
            // Define rows and columns for the matrix grid
            for (int i = 0; i < Rows; i++)
                MatrixGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            for (int j = 0; j < Columns; j++)
                MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition { });

            // Create TextBlocks
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Border border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(5),
                        Margin = new Thickness(1),
                        Background = new SolidColorBrush(Color.FromArgb(5, 0, 0, 0)),
                        Visibility = Visibility.Collapsed,
                        Height = RowHeight - 2,
                        MinWidth = MinCellWidth
                    };
                    Cells[i, j] = border;

                    TextBlock textBlock = new TextBlock
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        
                    };

                    TextBlocks[i, j] = textBlock;
                    border.Child = textBlock;
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    MatrixGrid.Children.Add(border);
                }
            }

            Width = Double.NaN;
        }


        private void RedrawBrackets(int rows)
        {
            double height = rows * RowHeight;
            Size newSize = new Size(15 + height * 0.1, height * 0.75);
            Point end = new Point(0, height);
            LeftArc.Point = end;
            LeftArc.Size = newSize;
            RightArc.Point = end;
            RightArc.Size = newSize;
        }

        private void Resize(int rows, int columns)
        {
            try
            {
                if (rows < 1 || rows > Rows || columns < 1 || columns > Columns)
                    throw new ArgumentOutOfRangeException("Visualization range out of bounds");

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        Cells[i, j].Visibility = (i < rows && j < columns) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }

                if (rows != RowsVisible)
                    this.RedrawBrackets(rows);

                RowsVisible = rows;
                ColumnsVisible = columns;

            }
            catch (Exception ex)
            {
                ShowMessage("Error in Resize(): " + ex.Message);
            }
        }

        public void Set(int rows, int columns, string[,] strings)
        {
            try
            {
                this.Resize(rows, columns);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        TextBlocks[i, j].Text = strings[i, j];
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error in Set(): " + ex.Message);
            }
        }

        public void Set(int rows, int columns, double[,] values)
        {
            try
            {
                this.Resize(rows, columns);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        TextBlocks[i, j].Text = values[i, j].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error in Set(): " + ex.Message);
            }
        }

        public string[,] GetStrings()
        {
            try
            {
                string[,] strings = new string[RowsVisible, ColumnsVisible];
                for (int i = 0; i < RowsVisible; i++)
                {
                    for (int j = 0; j < ColumnsVisible; j++)
                    {
                        strings[i, j] = TextBlocks[i, j].Text;
                    }
                }
                return strings;
            }
            catch (Exception ex)
            {
                ShowMessage("Error in GetStrings(): " + ex.Message);
                return new string[0, 0];
            }
        }

        private void LeftBracket_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            translateTransformLeftBracket.X = LeftBracket.ActualWidth;
        }
    }
}
