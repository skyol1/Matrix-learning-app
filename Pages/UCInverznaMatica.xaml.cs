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
    /// Interaction logic for UCInverznaMatica.xaml
    /// </summary>
    public partial class UCInverznaMatica : UserControl
    {
        public UCInverznaMatica()
        {
            InitializeComponent();
            SetMatrices();
        }

        private void SetMatrices()
        {
            string[,] matrixData =
            {
                { "D_11_", "D_21_", "...", "D_n1_",  },
                { "D_12_", "D_22_", "...", "D_n2_",  },
                { "⋮", "⋮", "⋱", "⋮"},
                { "D_1n_", "D_2n_", "...", "D_nn_",  },
            };
            matrix1.SetMatrix(matrixData);
        }
    }
}
