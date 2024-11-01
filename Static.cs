using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaticeApp
{
    public static class Static
    {
        public static async Task ShowMessage(string message)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageBox.Show(message);
            });
        }
    }
}
