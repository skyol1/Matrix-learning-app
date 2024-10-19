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
            matrix1.SetMatrix(2, 2, matrixData, true);

            matrixData = new string[,]
            {
                { "3⋅1", "3⋅2" },
                { "3⋅3", "3⋅4" }
            };
            matrix2.SetMatrix(2, 2, matrixData, true);

            matrixData = new string[,]
            {
                { "3", "6" },
                { "9", "12" }
            };
            matrix3.SetMatrix(2, 2, matrixData, true);
        }
    }
}
