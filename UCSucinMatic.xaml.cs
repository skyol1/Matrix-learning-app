using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaticeApp.Highlighters;

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for UCSucinMatic.xaml
    /// </summary>
    public partial class UCSucinMatic : UserControl
    {
        public UCSucinMatic()
        {
            InitializeComponent();
            SetMatrices();
        }

        private void SetMatrices()
        {
            string[,] matrixData =
            {
                { "1", "2" },
                { "3", "4" }
            };
            matrix1.SetMatrix(matrixData, true);

            matrixData = new string[,]
            {
                { "5", "6" },
                { "7", "8" }
            };
            matrix2.SetMatrix(matrixData, true);

            matrixData = new string[,]
            {
                { "(1⋅5+2⋅7)", "(1⋅6+2⋅8)" },
                { "(3⋅5+4⋅7)", "(3⋅6+4⋅8)" }
            };
            matrix3.CellWidth = 90;
            matrix3.SetMatrix(matrixData, false);

            matrixData = new string[,]
            {
                { "19", "22" },
                { "43", "50" }
            };
            matrix4.SetMatrix(matrixData, true);

            matrix1.highlighters.Add(new RowHighlighter(matrix2, Color.FromArgb(50, 0, 0, 255)));
            matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(50, 255, 0, 0)));
            matrix3.highlighters.Add(new SingleElementHighlighter(matrix4, Color.FromArgb(50, 255, 0, 0)));
            matrix4.highlighters.Add(new ColumnHighlighter(matrix1, Color.FromArgb(50, 0, 255, 0)));
            matrixCalculator.SetupCalculator(MatrixCalculator.Operation.Multiply);
        }
    }
}
