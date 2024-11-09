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
    /// Interaction logic for UCDeterminanty.xaml
    /// </summary>
    public partial class UCDeterminanty : UserControl
    {
        public UCDeterminanty()
        {
            InitializeComponent();
            SetMatrices();
        }

        private void SetMatrices()
        {
            string[,] matrixData =
            {
                { "-5" }
            };
            matrix1.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "1", "2" },
                { "3", "4" }
            };
            matrix2.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "1", "-1", "5" },
                { "1", "3", "3" },
                { "-2", "5", "-4" }
            };
            matrix3_0.SetMatrix(matrixData);
            matrix3_1.BracketType = Matrix.Bracket.Straight;
            matrix3_1.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "1", "-1", "5" },
                { "1", "3", "3" }
            };
            matrix3_2.CreateBrackets = false;
            matrix3_2.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "-3", "-13", "-7", "-8" },
                { "2", "0", "12", "-10" },
                { "-4", "-18", "-6", "-17" },
            };
            matrix4_0.SetMatrix(matrixData);

            matrix4_1.BracketType = Matrix.Bracket.Straight;
            matrix4_1.SetMatrix(matrixData);

            matrix5_0.SetMatrix(matrixData);

            matrix5_1.BracketType = Matrix.Bracket.Straight;
            matrix5_1.SetMatrix(matrixData);

            matrix6_0.SetMatrix(matrixData);

            matrix6_1.SetMatrix(matrixData);
            matrix6_1.RowAddMultiplied(0, 1, "3");

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "0", "2", "-1", "1" },
                { "2", "0", "12", "-10" },
                { "-4", "-18", "-6", "-17" },
            };
            matrix6_2.SetMatrix(matrixData);
            matrix6_2.RowAddMultiplied(0, 2, "-2");

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "0", "2", "-1", "1" },
                { "0", "-10", "8", "-16" },
                { "-4", "-18", "-6", "-17" },
            };
            matrix6_3.SetMatrix(matrixData);
            matrix6_3.RowAddMultiplied(0, 3, "-4");

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "0", "2", "-1", "1" },
                { "0", "-10", "8", "-16" },
                { "0", "2", "2", "-5" },
            };
            matrix6_4.SetMatrix(matrixData);

            matrix6_5.BracketType = Matrix.Bracket.Straight;
            matrix6_5.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "0", "2", "-1", "1" },
                { "0", "-10", "8", "-16" },
                { "0", "2", "2", "-5" },
            };
            matrix6_6.SetMatrix(matrixData);
            matrix6_6.RowAddMultiplied(1, 2, "5");

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "0", "2", "-1", "1" },
                { "0", "0", "3", "-11" },
                { "0", "2", "2", "-5" },
            };
            matrix6_7.SetMatrix(matrixData);
            matrix6_7.RowAddMultiplied(1, 3, "-1");

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "0", "2", "-1", "1" },
                { "0", "0", "3", "-11" },
                { "0", "0", "3", "-6" },
            };
            matrix6_8.SetMatrix(matrixData);
            matrix6_8.RowAddMultiplied(2, 3, "-1");

            matrixData = new string[,]
            {
                { "1", "5", "2", "3" },
                { "0", "2", "-1", "1" },
                { "0", "0", "3", "-11" },
                { "0", "0", "0", "5" },
            };
            matrix6_9.SetMatrix(matrixData);

            matrix6_10.BracketType = Matrix.Bracket.Straight;
            matrix6_10.SetMatrix(matrixData);
        }

    }
}
