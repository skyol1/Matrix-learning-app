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
                { "2", "4", "-2", "2"  },
                { "4", "9", "-3", "8"  },
                { "-2", "-3", "7", "10"  }
            };
            matrix1.SetMatrix(matrixData);
        }
    }
}
