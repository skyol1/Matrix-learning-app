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
    /// Interaction logic for UCRiadkoveOperacie.xaml
    /// </summary>
    public partial class UCRiadkoveOperacie : UserControl
    {
        public UCRiadkoveOperacie()
        {
            InitializeComponent();
            SetMatrices();
        }

        private void SetMatrices()
        {
            string[,] matrixData =
            {
                { "1", "2", "3" },
                { "4", "5", "6" },
                { "7", "8", "9" },
            };
            matrixD1_0.SetMatrix(matrixData);
            matrixD1_0.RowSwap(0, 1);

            matrixD2_0.SetMatrix(matrixData);
            matrixD2_0.RowMultiply(0, "2");

            matrixD3_0.SetMatrix(matrixData);
            matrixD3_0.RowAddMultiplied(0, 2, "2");

            matrixData = new string[,]
            {
                { "4", "5", "6" },
                { "1", "2", "3" },
                { "7", "8", "9" },
            };
            matrixD1_1.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "2", "4", "6" },
                { "4", "5", "6" },
                { "7", "8", "9" },
            };
            matrixD2_1.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "1", "2", "3" },
                { "4", "5", "6" },
                { "9", "12", "15" },
            };
            matrixD3_1.SetMatrix(matrixData);

            matrixData = new string[,]
            {
                { "2", "4", "-2", "2"  },
                { "4", "9", "-3", "8"  },
                { "-2", "-3", "7", "10"  }
            };
            matrix1.SetMatrix(matrixData);
            matrix1.MakeSLR();

            matrix1_0.SetMatrix(matrixData);
            matrix1_0.MakeSLR();
            matrix1_0.RowAddMultiplied(0, 1, "-2");

            matrixData = new string[,]
            {
                { "2", "4", "-2", "2"  },
                { "0", "1", "1", "4"  },
                { "-2", "-3", "7", "10"  }
            };
            matrix1_1.SetMatrix(matrixData);
            matrix1_1.MakeSLR();
            matrix1_1.RowAddMultiplied(0, 2, "1");

            matrixData = new string[,]
            {
                { "2", "4", "-2", "2"  },
                { "0", "1", "1", "4"  },
                { "0", "1", "5", "12"  }
            };
            matrix1_2.SetMatrix(matrixData);
            matrix1_2.MakeSLR();
            matrix1_2.RowAddMultiplied(1, 2, "-1");

            matrixData = new string[,]
            {
                { "2", "4", "-2", "2"  },
                { "0", "1", "1", "4"  },
                { "0", "0", "4", "8"  }
            };
            matrix1_3.SetMatrix(matrixData);
            matrix1_3.MakeSLR();

            matrix1_4.SetMatrix(matrixData);
            matrix1_4.MakeSLR();

            matrix2_0.SetMatrix(matrixData);
            matrix2_0.MakeSLR();
            matrix2_0.RowMultiply(2, "¼");

            matrixData = new string[,]
            {
                { "2", "4", "-2", "2"  },
                { "0", "1", "1", "4"  },
                { "0", "0", "1", "2"  }
            };
            matrix2_1.SetMatrix(matrixData);
            matrix2_1.MakeSLR();
            matrix2_1.RowAddMultiplied(2, 1, "-1");

            matrixData = new string[,]
            {
                { "2", "4", "-2", "2"  },
                { "0", "1", "0", "2"  },
                { "0", "0", "1", "2"  }
            };
            matrix2_2.SetMatrix(matrixData);
            matrix2_2.MakeSLR();
            matrix2_2.RowAddMultiplied(2, 0, "2");

            matrixData = new string[,]
            {
                { "2", "4", "0", "6"  },
                { "0", "1", "0", "2"  },
                { "0", "0", "1", "2"  }
            };
            matrix2_3.SetMatrix(matrixData);
            matrix2_3.MakeSLR();
            matrix2_3.RowAddMultiplied(1, 0, "-4");

            matrixData = new string[,]
            {
                { "2", "0", "0", "-2"  },
                { "0", "1", "0", "2"  },
                { "0", "0", "1", "2"  }
            };
            matrix2_4.SetMatrix(matrixData);
            matrix2_4.MakeSLR();
            matrix2_4.RowMultiply(0, "½");

            matrixData = new string[,]
            {
                { "1", "0", "0", "-1"  },
                { "0", "1", "0", "2"  },
                { "0", "0", "1", "2"  }
            };
            matrix2_5.SetMatrix(matrixData);
            matrix2_5.MakeSLR();

            matrix2_6.SetMatrix(matrixData);
            matrix2_6.MakeSLR();
        }
    }
}
