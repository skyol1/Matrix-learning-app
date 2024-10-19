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
    /// Interaction logic for UCMatice.xaml
    /// </summary>
    public partial class UCMatice : UserControl
    {
        public UCMatice()
        {
            InitializeComponent();
            SetMatrices();
        }

        private void SetMatrices()
        {
            string[,] matrixData =
            {
                { "a_11_", "a_12_", "...", "a_1n_"},
                { "a_21_", "a_22_", "...", "a_2n_"},
                { "⋮", "⋮", "⋱", "⋮"},
                { "a_m1_", "a_m2_", "...", "a_mn_"}
            };
            matrix1.SetMatrix(4, 4, matrixData, true);

            matrixData = new string[,]
            {
                { "a_11_", "a_12_", "a_13_", "a_14_" },
                { "a_21_", "a_22_", "a_23_", "a_24_" },
                { "a_31_", "a_32_", "a_33_", "a_34_" },
                { "a_41_", "a_42_", "a_43_", "a_44_" }
            };
            matrix2.SetMatrix(3, 3, matrixData, true);
            matrix3.SetMatrix(3, 4, matrixData, true);
            matrix4.SetMatrix(4, 3, matrixData, true);
            matrix5.SetMatrix(3, 3, matrixData, true);
            matrix6.SetMatrix(3, 4, matrixData, true);
            matrix7.SetMatrix(4, 3, matrixData, true);
            //matrix2.HighlightDiagonal(0, 0, 2, 2);

            matrixData = new string[,]
            {
                { "r_1_", "r_2_", "...", "r_n_" }
            };
            matrix8.SetMatrix(1, 4, matrixData, true);

            matrixData = new string[,]
            {
                { "c_1_" },
                { "c_2_" },
                { "⋮" },
                { "c_n_" },
            };
            matrix9.SetMatrix(4, 1, matrixData, true);

            matrixData = new string[,]
            {
                { "0", "0", "...", "0" },
                { "0", "0", "...", "0" },
                { "⋮", "⋮", "⋱", "⋮"},
                { "0", "0", "...", "0" }
            };
            matrix10.SetMatrix(4, 4, matrixData, true);

            matrixData = new string[,]
            {
                { "a_11_", "a_12_", "a_13_", "...", "a_1n_"},
                { "0", "a_22_", "a_23_", "...", "a_2n_"},
                { "0", "0", "a_33_", "...", "a_3n_"},
                { "⋮", "⋮", "⋮", "⋱", "⋮"},
                { "0", "0", "0", "...", "a_nn_"}
            };
            matrix11.SetMatrix(5, 5, matrixData, true);

            matrixData = new string[,]
            {
                { "b_11_", "0", "0", "...", "0"},
                { "b_21_", "b_22_", "0", "...", "0"},
                { "b_31_", "B_32_", "b_33_", "...", "0"},
                { "⋮", "⋮", "⋮", "⋱", "⋮"},
                { "b_n1_", "b_n2_", "b_n3_", "...", "b_nn_"}
            };
            matrix12.SetMatrix(5, 5, matrixData, true);

            matrixData = new string[,]
            {
                { "1", "0", "0", "...", "0"},
                { "0", "1", "0", "...", "0"},
                { "0", "0", "1", "...", "0"},
                { "⋮", "⋮", "⋮", "⋱", "⋮"},
                { "0", "0", "0", "...", "1"}
            };
            matrix13.SetMatrix(5, 5, matrixData, true);
        }
    }
}
