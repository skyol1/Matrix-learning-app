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
    /// Interaction logic for UCTransponovanaMatica.xaml
    /// </summary>
    public partial class UCTransponovanaMatica : UserControl
    {
        public UCTransponovanaMatica()
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
            matrix1.SetMatrix(matrixData, true);

            matrixData = new string[,]
            {
                { "a_11_", "a_21_", "...", "a_m1_" },
                { "a_12_", "a_22_", "...", "a_m2_" },
                { "⋮", "⋮", "⋱", "⋮" },
                { "a_1n_", "a_2n_", "...", "a_mn_" }
            };
            matrix2.SetMatrix(matrixData, true);

            matrixData = new string[,]
            {
                { "1", "2", "3" },
                { "4", "5", "6" }
            };
            matrix3.SetMatrix(matrixData, true);

            matrixData = new string[,]
            {
                { "1", "4" },
                { "2", "5" },
                { "3", "6" }
            };
            matrix4.SetMatrix(matrixData, true);
            matrix3.highlighters.Add(new TransposeHighlightConnector(new SingleElementHighlighter(matrix4, Color.FromArgb(40, 255, 0, 0))));
            matrix4.highlighters.Add(new TransposeHighlightConnector(new SingleElementHighlighter(matrix3, Color.FromArgb(40, 255, 0, 0))));
        }
    }
}
