using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using MaticeApp.Highlighters;
using static MaticeApp.Static;
using System.Globalization;

namespace MaticeApp
{
    public partial class InputMatrix : UserControl
    {
        public List<IHighlightable> highlighters { get; set; }

        private TextBoxInputMatrix[,] TextBoxes;

        private Border[,] MatrixCells;
        public double RowHeight { get; set; } = 30;
        public double MinCellWidth { get; set; } = 50;

        public const int Rows = 9;

        public const int Columns = 9;
        public int RowsVisible { get; private set; } = 0;
        public int ColumnsVisible { get; private set; } = 0;
        public bool IsInputValid {  get; private set; } = true; // zero matrix is created

        private const int MaxInputLength = 15;

        public event Action ?InputMatrixSizeChanged;

        public InputMatrix()
        {
            InitializeComponent();
            highlighters = new List<IHighlightable>();
            MatrixCells = new Border[Rows, Columns];
            TextBoxes = new TextBoxInputMatrix[Rows, Columns];
            SetMatrix();
        }
        public Canvas GetHighlightCanvas()
        {
            return HighlightCanvas;
        }
        private void SetMatrix()
        {
            // Define rows and columns for the matrix grid
            for (int i = 0; i < Rows; i++)
                MatrixGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            for (int j = 0; j < Columns; j++)
                MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition { });

            // Create TextBoxes
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
                        Background= new SolidColorBrush(Color.FromArgb(10, 0, 0, 0)),
                        Visibility = Visibility.Collapsed,
                        Height = RowHeight - 2
                    };
                    MatrixCells[i, j] = border;

                    TextBoxInputMatrix textBox = new TextBoxInputMatrix
                    {
                        MaxLength = MaxInputLength,
                        MinWidth = MinCellWidth,
                        Valid = true
                    };

                    TextBoxes[i, j] = textBox;
                    textBox.TextChanged += OnInputTextChanged;
                    border.Child = textBox;
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    MatrixGrid.Children.Add(border);
                }
            }

            Height = Double.NaN;
            Width = Double.NaN;
        }


        private void MatrixGrid_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (MatrixGrid.Children[(i * Columns) + j].IsMouseOver)
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


        private void CheckValidity()
        {
            for (int i = 0; i < RowsVisible; i++)
                for (int j = 0; j < ColumnsVisible; j++)
                    if (TextBoxes[i, j].Valid == false) 
                    { IsInputValid = false; return; }
            IsInputValid = true;
        }
        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textBox = (sender as TextBoxInputMatrix);
                if (textBox == null) return;
                if (ParseDouble(textBox.Text, out double tmp) ||
                    string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Valid = true;
                    textBox.Background = Brushes.White;
                    if (!IsInputValid) CheckValidity();
                } 
                else
                {
                    textBox.Valid = false;
                    textBox.Background = new SolidColorBrush(Color.FromArgb(60, 255, 0, 0));
                    IsInputValid = false;
                }
            } 
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        public void Randomize()
        {
            var rnd = new Random();
            for (int i = 0; i < RowsVisible; i++)
            {
                for (int j = 0; j < ColumnsVisible; j++)
                {
                    TextBoxes[i, j].Text = rnd.Next(-10, 11).ToString();
                    TextBoxes[i, j].MakeNormal();
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < RowsVisible; i++)
            {
                for (int j = 0; j < ColumnsVisible; j++)
                {
                    TextBoxes[i, j].MakeDefault0();
                }
            }
        }

        public void MakeIdentity()
        {
            for (int i = 0; i < RowsVisible; i++)
            {
                for (int j = 0; j < ColumnsVisible; j++)
                {
                    TextBoxes[i, j].Text = (i == j) ? "1" : "0";
                    TextBoxes[i, j].MakeNormal();
                }
            }
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
        public void Resize(int rows, int columns)
        {
            try
            {
                if (rows < 1 || rows > Rows || columns < 1 || columns > Columns)
                    throw new ArgumentOutOfRangeException("Visualization range out of bounds");

                for (int i = 0; i < Rows; i++)
                {
                    for(int j = 0; j < Columns; j++)
                    {
                        MatrixCells[i, j].Visibility = (i < rows && j < columns) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }

                if (rows != RowsVisible)
                    this.RedrawBrackets(rows);

                RowsVisible = rows;
                ColumnsVisible = columns;

                InputMatrixSizeChanged?.Invoke();
            } 
            catch (Exception ex) { ShowMessage("Error in Resize(): " + ex.Message); }
        }
        public void ResizeSet(string[,] strings, int rows, int columns)
        {
            try 
            {
                this.Resize(rows, columns);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        TextBoxes[i, j].Text = strings[i, j];
                        TextBoxes[i, j].MakeNormal();
                    }
                }
            }
            catch (Exception ex) { ShowMessage("Error in ResizeSet(): " + ex.Message); }
        }

        public string[,] GetStrings()
        {
            try 
            {
                string[,] strings = new string[RowsVisible, ColumnsVisible];

                for (int i = 0; i < RowsVisible; i++)
                    for (int j = 0; j < ColumnsVisible; j++)
                        strings[i, j] = TextBoxes[i, j].Text;

                return strings;
            } 
            catch (Exception ex)
            {
                ShowMessage("Error in GetStrings(): " + ex.Message);
                return new string[0, 0];
            }
        }

        public double[,] GetNumbers()
        {
            if (IsInputValid)
            {
                double[,] numbers = new double[RowsVisible, ColumnsVisible];
                for (int i = 0; i < RowsVisible; i++)
                {
                    for (int j = 0; j < ColumnsVisible; j++)
                    {
                        numbers[i, j] = double.Parse(TextBoxes[i, j].Text);
                    }
                }
                return numbers;
            } else
            {
                return new double[0, 0];
            }
        }

        private void LeftBracket_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            translateTransformLeftBracket.X = LeftBracket.ActualWidth;
        }
    }
}
