using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

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

        public static void Copy2DstringArrayToClipboard(string[,] array, int format)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            StringBuilder sb = new StringBuilder();

            if (format == 1)
            {
                sb.Append("{");
                for (int i = 0; i < rows; i++)
                {
                    sb.Append("{");
                    for (int j = 0; j < columns; j++)
                    {
                        sb.Append(array[i, j] + ", ");
                    }
                    sb.Length -= 2;
                    sb.Append("},\n");
                }
                sb.Length -= 2;
                sb.Append("}");
            } else if (format == 2)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        sb.Append(array[i, j] + " ");
                    }
                    sb.Length -= 1;
                    sb.Append("\n");
                }
                sb.Length -= 1;
            }

            // Copy to clipboard
            Clipboard.SetText(sb.ToString());
        }

        public static bool TryConvertStringTo2DArray(string input, out string[,] outputArray, out string info)
        {
            input = input.Trim();
            outputArray = null;
            info = null;

            string[] inputRows;

            // Check if the input contains braces to identify format
            if (input.StartsWith("{") && input.EndsWith("}"))
            {
                // Assumed format: "{{...}, {...}}"
                inputRows = input.Trim('{', '}').Split(new[] { "},\n{", "},\r\n{" }, StringSplitOptions.None);
            }
            else
            {
                // Format: "...\n...\n..."
                // Split by new lines to get rows
                inputRows = input.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }

            int rows = inputRows.Length;
            if (rows < 1 || rows > 9)
            {
                info = $"Too many rows ({rows})";
                return false;
            }
            int columns = inputRows[0].Split(new[] { ", ", " " }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (columns < 1 || columns > 9)
            {
                info = $"Too many columns in the first row ({columns})";
                return false;
            }

            outputArray = new string[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                // Split each row by commas to get values
                var values = inputRows[i].Split(new[] { ", ", " " }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != columns)
                {
                    info = $"Number of elements in the row {i+1} is not equal to the number of elements in the first row";
                    return false;
                }

                for (int j = 0; j < columns; j++)
                {
                    // Try parsing each value as a double
                    if (ParseDouble(values[j].Trim(), out double number))
                    {
                        if (number == 0) number = 0; // Get rid of -0
                        outputArray[i, j] = number.ToString();
                    }
                    else
                    {
                        outputArray = null;
                        info = $"Failed to parse in row {i+1}, column {j+1}. Text:\n{values[j].Trim().Replace(',', '.')}";
                        return false;
                    }
                }
            }

            return true;
        }

        public static double RoundToNearestPrecision(double value)
        {
            if (value == 0 || !double.IsFinite(value)) return 0;

            double roundedValue;
            // Check progressively finer decimal places, starting from 0
            for (int decimals = 0; decimals <= 15; decimals++)
            {
                roundedValue = Math.Round(value, decimals);
                if (Math.Abs(value - roundedValue) < Math.Pow(10, -6 - decimals))
                    return (roundedValue == 0) ? 0 : roundedValue;
            }

            // If no close rounded value is found, return the original value
            return value;
        }

        public static double[,] RoundToNearestPrecision(double[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    array[i, j] = RoundToNearestPrecision(array[i, j]);
            return array;
        }

        public static bool ParseDouble(string text, out double value)
        {
            if (double.TryParse(text.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out value)) { return true; }
            else { return false; }
        }


        public static (double dpiX, double dpiY) GetDpi()
        {
            var hwndSource = PresentationSource.FromVisual(Application.Current.MainWindow) as HwndSource;
            if (hwndSource != null)
            {
                var dpi = hwndSource.CompositionTarget.TransformToDevice;
                return (dpi.M11 * 96, dpi.M22 * 96); // 96 is the default DPI
            }
            return (96, 96);
        }

    }
}
