using MaticeApp.Highlighters;
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
    /// Interaction logic for UCSucetMatic.xaml
    /// </summary>
    public partial class UCSucetMatic : UserControl
    {
        public UCSucetMatic()
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
                { "1+5", "2+6" },
                { "3+7", "4+8" }
            };
            matrix3.SetMatrix(matrixData, true);

            matrixData = new string[,]
            {
                { "6", "8" },
                { "10", "12" }
            };
            matrix4.SetMatrix(matrixData, true);

            matrix1.highlighters.Add(new SingleElementHighlighter(matrix2, Color.FromArgb(50, 0, 0, 255)));
            matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(50, 255, 0, 0)));
            matrix3.highlighters.Add(new SingleElementHighlighter(matrix4, Color.FromArgb(50, 255, 0, 0)));
            matrix4.highlighters.Add(new SingleElementHighlighter(matrix1, Color.FromArgb(50, 0, 255, 0)));
            matrixCalculator.SetupCalculator(MatrixCalculator.Operation.Add);
        }
    }
}
