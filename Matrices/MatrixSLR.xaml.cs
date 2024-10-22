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

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for MatrixSLR.xaml
    /// </summary>
    public partial class MatrixSLR : UserControl
    {
        public int RowHeight { get; set; } = 30;
        public int CellWidth { get; set; } = 55;

        public MatrixSLR()
        {
            InitializeComponent();
            matrix1.RightBracket = null;
            matrix2.LeftBracket = null;
        }

        public void SetMatrix(string[,] matrixValues)
        {
            int RowsCount = matrixValues.GetUpperBound(0) + 1;
            int ColumnsCount = matrixValues.GetUpperBound(1) + 1;

            for (int i = 0; i < RowsCount; i++)
            {
                matrix1.MatrixGrid.RowDefinitions.Add(new RowDefinition { });
                matrix2.MatrixGrid.RowDefinitions.Add(new RowDefinition { });
            }
            for (int j = 0; j < (ColumnsCount - 1); j++)
            {
                matrix1.MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition { });
            }
            matrix2.MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition { });

            int height = RowsCount * RowHeight;
            matrix1.Height = height;
            matrix2.Height = height;
            matrix1.MatrixGrid.Width = (ColumnsCount - 1) * CellWidth;
            matrix2.MatrixGrid.Width = CellWidth;

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
                        Background = new SolidColorBrush(Color.FromArgb(5, 0, 0, 0))
                    };

                    TextBlock textBlock = CreateRichText(matrixValues[i, j]);

                    border.Child = textBlock;
                    Grid.SetRow(border, i);
                    if (j < (ColumnsCount - 1))
                    {
                        Grid.SetColumn(border, j);
                        matrix1.MatrixGrid.Children.Add(border);
                    } else
                    {
                        matrix2.MatrixGrid.Children.Add(border);
                    }
                }
            }

            // Update the arc sizes for the brackets
            double arcSize = (height * 0.45);
            double arcHeight = (height * 0.75);

            // Set the properties for the arcs
            matrix1.LeftArc.Size = new Size(arcSize, arcHeight);
            matrix1.LeftArc.Point = new Point(0, height / 2);
            matrix2.RightArc.Size = new Size(arcSize, arcHeight);
            matrix2.RightArc.Point = new Point(0, height / 2);

            matrix1.Width = matrix1.LeftBracket.Width + matrix1.MatrixGrid.Width;
            matrix2.Width = matrix2.RightBracket.Width + matrix2.MatrixGrid.Width;
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
    }
}
