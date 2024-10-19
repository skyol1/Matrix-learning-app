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
    /// Interaction logic for UCRozdielMatic.xaml
    /// </summary>
    public partial class UCRozdielMatic : UserControl
    {
        public UCRozdielMatic()
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
                { "9", "8" },
                { "7", "6" }
            };
            matrix2.SetMatrix(2, 2, matrixData, true);

            matrixData = new string[,]
            {
                { "1-9", "2-8" },
                { "3-7", "4-6" }
            };
            matrix3.SetMatrix(2, 2, matrixData, true);

            matrixData = new string[,]
            {
                { "-8", "-6" },
                { "-4", "-2" }
            };
            matrix4.SetMatrix(2, 2, matrixData, true);
        }
    }
}
