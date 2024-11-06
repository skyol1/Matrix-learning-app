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
    /// Interaction logic for UCNasobokMatice.xaml
    /// </summary>
    public partial class UCNasobokMatice : UserControl
    {
        public UCNasobokMatice()
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
            matrix1.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "3⋅1", "3⋅2" },
                { "3⋅3", "3⋅4" }
            };
            matrix2.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "3", "6" },
                { "9", "12" }
            };
            matrix3.SetMatrix(matrixData);

            
            matrix1.highlighters.Add(new SingleElementHighlighter(matrix2, Color.FromArgb(50, 255, 0, 0)));
            matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(50, 255, 0, 0)));
            matrix3.highlighters.Add(new SingleElementHighlighter(matrix1, Color.FromArgb(50, 0, 255, 0)));
            matrix3.highlighters.Add(new ScalarHighlighter(Scalar, Color.FromArgb(50, 0, 0, 255)));
             
        }
    }
}
