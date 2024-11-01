using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MaticeApp.Highlighters;
using static MaticeApp.Static;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Providers.LinearAlgebra;

namespace MaticeApp
{
    public partial class MatrixCalculator : UserControl
    {
        
        private int matrixARows = 3;
        private int matrixAColumns = 3;
        private int matrixBRows = 3;
        private int matrixBColumns = 3;

        private bool AddSubstractPossible = true;
        private bool MultiplyPossible = true;
        private bool InvertDeterminantAPossible = true;
        private bool InvertDeterminantBPossible = true;

        private Brush Button2MatrixOperationsBackgroundNormal = new SolidColorBrush(Color.FromArgb(255, 135, 240, 255));
        private Brush ButtonMatrixFillBackgroundNormal = new SolidColorBrush(Color.FromArgb(255, 135, 160, 255));
        private Brush Button1MatrixOperationBackgroundNormal = new SolidColorBrush(Color.FromArgb(255, 165, 135, 255));
        private Brush ButtonBackgroundError = new SolidColorBrush(Color.FromArgb(90, 255, 0, 0));

        private TextBlock _lastOutputTextBlock;

        public MatrixCalculator()
        {
            InitializeComponent();

            inputMatrix1.Resize(matrixARows, matrixAColumns);
            inputMatrix1.RedrawBrackets(matrixARows);
            inputMatrix2.Resize(matrixBRows, matrixBColumns);
            inputMatrix2.RedrawBrackets(matrixBRows);

            inputMatrix1.InputMatrixSizeChanged += CheckOperationButtons1;
            inputMatrix2.InputMatrixSizeChanged += CheckOperationButtons2;

            _lastOutputTextBlock = outputTextAdd;

            ButtonAdd.Background = Button2MatrixOperationsBackgroundNormal;
            ButtonSubstract.Background = Button2MatrixOperationsBackgroundNormal;
            ButtonMultiply.Background = Button2MatrixOperationsBackgroundNormal;

            ButtonClearA.Background = ButtonMatrixFillBackgroundNormal;
            ButtonClearB.Background = ButtonMatrixFillBackgroundNormal;
            ButtonRandomA.Background = ButtonMatrixFillBackgroundNormal;
            ButtonRandomB.Background = ButtonMatrixFillBackgroundNormal;
            ButtonIdentityA.Background = ButtonMatrixFillBackgroundNormal;
            ButtonIdentityB.Background = ButtonMatrixFillBackgroundNormal;
            ButtonCopyBtoA.Background = ButtonMatrixFillBackgroundNormal;
            ButtonCopyAtoB.Background = ButtonMatrixFillBackgroundNormal;

            ButtonTransposeA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonTransposeB.Background = Button1MatrixOperationBackgroundNormal;
            ButtonInvertA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonInvertB.Background = Button1MatrixOperationBackgroundNormal;
            ButtonDeterminantA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonDeterminantB.Background = Button1MatrixOperationBackgroundNormal;
            ButtonMultiplyA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonMultiplyB.Background = Button1MatrixOperationBackgroundNormal;
        }

        private bool IsNewSizeValueValid(TextBoxInputMatrixSize textBox, out int newSize)
        {
            if (int.TryParse(textBox.Text, out newSize) && newSize > 0 && newSize < 10)
            {
                textBox.Tag = "1";
                textBox.Background= Brushes.White;
                return true;
            }
            else
            {
                textBox.Tag = "0";
                textBox.Background = new SolidColorBrush(Color.FromArgb(60, 255, 0, 0));
                return false;
            }
        }

        private void CheckAddSubstractMultiply()
        {
            if (matrixARows == matrixBRows && matrixAColumns == matrixBColumns)
            {
                AddSubstractPossible = true;
                ButtonAdd.ToolTip = null;
                ButtonSubstract.ToolTip = null;
                ButtonAdd.Background = Button2MatrixOperationsBackgroundNormal;
                ButtonSubstract.Background = Button2MatrixOperationsBackgroundNormal;
            }
            else
            {
                AddSubstractPossible = false;
                ButtonAdd.ToolTip = "Matice musia byť rovnakého typu";
                ButtonSubstract.ToolTip = "Matice musia byť rovnakého typu";
                ButtonAdd.Background = ButtonBackgroundError;
                ButtonSubstract.Background = ButtonBackgroundError;
            }

            if (matrixAColumns == matrixBRows)
            {
                MultiplyPossible = true;
                ButtonMultiply.ToolTip = null;
                ButtonMultiply.Background = Button2MatrixOperationsBackgroundNormal;
            }
            else
            {
                MultiplyPossible = false;
                ButtonMultiply.Background = ButtonBackgroundError;
                ButtonMultiply.ToolTip = "Počet stĺpcov matice A a\npočet riadkov matice B\nmusia byť rovnaké";
            }
        }

        private void CheckOperationButtons1()
        {
            CheckAddSubstractMultiply();

            if (matrixARows == matrixAColumns)
            {
                InvertDeterminantAPossible = true;
                ButtonInvertA.ToolTip = null;
                ButtonDeterminantA.ToolTip = null;
                ButtonInvertA.Background = Button1MatrixOperationBackgroundNormal;
                ButtonDeterminantA.Background = Button1MatrixOperationBackgroundNormal;
            }
            else
            {
                InvertDeterminantAPossible = false;
                ButtonInvertA.Background = ButtonBackgroundError;
                ButtonDeterminantA.Background = ButtonBackgroundError;
                ButtonInvertA.ToolTip = "Matica nie je štvorcová";
                ButtonDeterminantA.ToolTip = "Matica nie je štvorcová";
            }
        }

        private void CheckOperationButtons2()
        {
            CheckAddSubstractMultiply();

            if (matrixBRows == matrixBColumns)
            {
                InvertDeterminantBPossible = true;
                ButtonInvertB.ToolTip = null;
                ButtonDeterminantB.ToolTip = null;
                ButtonInvertB.Background = Button1MatrixOperationBackgroundNormal;
                ButtonDeterminantB.Background = Button1MatrixOperationBackgroundNormal;
            }
            else
            {
                InvertDeterminantBPossible = false;
                ButtonInvertB.Background = ButtonBackgroundError;
                ButtonDeterminantB.Background = ButtonBackgroundError;
                ButtonInvertB.ToolTip = "Matica nie je štvorcová";
                ButtonDeterminantB.ToolTip = "Matica nie je štvorcová";
            }
        }

        private void TextBoxInputMatrixARow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsNewSizeValueValid(TextBoxInputMatrixARow, out int newSize))
            {
                if (newSize != matrixARows && TextBoxInputMatrixAColumn.Valid)
                {
                    matrixARows = newSize;
                    inputMatrix1.Resize(newSize, matrixAColumns);
                }
            }
        }
        private void TextBoxInputMatrixAColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNewSizeValueValid(TextBoxInputMatrixAColumn, out int newSize))
            {
                if (newSize != matrixAColumns && TextBoxInputMatrixARow.Valid)
                {
                    matrixAColumns = newSize;
                    inputMatrix1.Resize(matrixARows, newSize);
                }
            }
        }
        private void TextBoxInputMatrixBRow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNewSizeValueValid(TextBoxInputMatrixBRow, out int newSize))
            {
                if (newSize != matrixBRows && TextBoxInputMatrixBColumn.Valid)
                {
                    matrixBRows = newSize;
                    inputMatrix2.Resize(newSize, matrixBColumns);
                }
            }
        }
        private void TextBoxInputMatrixBColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNewSizeValueValid(TextBoxInputMatrixBColumn, out int newSize))
            {
                if (newSize != matrixBColumns && TextBoxInputMatrixBRow.Valid)
                {
                    matrixBColumns = newSize;
                    inputMatrix2.Resize(matrixBRows, newSize);
                }
            }
        }

        private void ButtonClearA_Click(object sender, RoutedEventArgs e)
        {
            inputMatrix1.Clear();
        }

        private void ButtonClearB_Click(object sender, RoutedEventArgs e)
        {
            inputMatrix2.Clear();
        }

        private void ButtonRandomA_Click(object sender, RoutedEventArgs e)
        {
            inputMatrix1.Randomize();
        }

        private void ButtonRandomB_Click(object sender, RoutedEventArgs e)
        {
            inputMatrix2.Randomize();
        }

        private void ButtonIdentityA_Click(object sender, RoutedEventArgs e)
        {
            inputMatrix1.MakeIdentity();
        }

        private void ButtonIdentityB_Click(object sender, RoutedEventArgs e)
        {
            inputMatrix2.MakeIdentity();
        }
        
        private void ButtonCopyBtoA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputMatrix1.ResizeSet(matrixBRows, matrixBColumns, inputMatrix2.GetStrings());
                matrixARows = matrixBRows;
                matrixAColumns = matrixBColumns;
            } catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void ButtonCopyAtoB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputMatrix2.ResizeSet(matrixARows, matrixAColumns, inputMatrix1.GetStrings());
                matrixBRows = matrixARows;
                matrixBColumns = matrixAColumns;
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void SetNewOutputText(TextBlock textBlock)
        {
            try
            {
                _lastOutputTextBlock.Visibility = Visibility.Collapsed;
                textBlock.Visibility = Visibility.Visible;
                _lastOutputTextBlock = textBlock;

                outputPanel1.Visibility = Visibility.Visible;
                outputPanel2.Visibility = Visibility.Collapsed;
                errorTextBlock.Visibility = Visibility.Collapsed;
            } catch (Exception ex) { ShowMessage(ex.Message); }
        }
        
        private void ButtonAddSubstract_Click(object sender, RoutedEventArgs e)
        {
            if (!AddSubstractPossible || !inputMatrix1.IsInputValid || !inputMatrix2.IsInputValid) return;

            bool operation = (sender as Button).Name == "ButtonAdd";

            int rows = matrixARows;
            int columns = matrixAColumns;

            double[,] inputA = inputMatrix1.GetNumbers();
            double[,] inputB = inputMatrix2.GetNumbers();

            double[,] output = new double[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    output[i, j] = operation ? (inputA[i, j] + inputB[i, j]) : (inputA[i, j] - inputB[i, j]);

            outputMatrix.Set(rows, columns, ConvertDoublesStrings(output, rows, columns));
            SetNewOutputText(operation ? outputTextAdd : outputTextSubstract);
        }

        private void ButtonMultiply_Click(object sender, RoutedEventArgs e)
        {
            if (!MultiplyPossible || !inputMatrix1.IsInputValid || !inputMatrix2.IsInputValid) return;

            int rowsA = matrixARows;
            int columnsA = matrixAColumns;
            int columnsB = matrixBColumns;

            double[,] inputA = inputMatrix1.GetNumbers();
            double[,] inputB = inputMatrix2.GetNumbers();

            double[,] output = new double[rowsA, columnsB];

            for (int i = 0; i < matrixARows; i++)
                for (int j = 0; j < matrixBColumns; j++)
                {
                    output[i, j] = 0;
                    for (int k = 0; k < matrixAColumns; k++)
                        output[i, j] += inputA[i, k] * inputB[k, j];
                }

            outputMatrix.Set(matrixARows, matrixBColumns, ConvertDoublesStrings(output, matrixARows, matrixBColumns));
            SetNewOutputText(outputTextMultiply);
        }

        private void ButtonTransposeA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid) return;

            string[,] input = inputMatrix1.GetStrings();
            string[,] transposed = new string[matrixAColumns, matrixARows];
            
            for (int i = 0; i < matrixARows; i++)
                for (int j = 0; j < matrixAColumns; j++)
                    transposed[j, i] = input[i, j];

            outputMatrix.Set(matrixAColumns, matrixARows, transposed);
            SetNewOutputText(outputTextTransposed);
            outputTextTransposedMatrixName.Text = "A";
        }

        private void ButtonTransposeB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid) return;

            string[,] input = inputMatrix2.GetStrings();
            string[,] transposed = new string[matrixBColumns, matrixBRows];

            for (int i = 0; i < matrixBRows; i++)
                for (int j = 0; j < matrixBColumns; j++)
                    transposed[j, i] = input[i, j];

            outputMatrix.Set(matrixBColumns, matrixBRows, transposed);
            SetNewOutputText(outputTextTransposed);
            outputTextTransposedMatrixName.Text = "B";
        }

        private void ButtonInvertA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid || !InvertDeterminantAPossible) return;
            try
            {
                var matrix = DenseMatrix.OfArray(inputMatrix1.GetNumbers()).Inverse();
                if (double.IsFinite(matrix.Determinant()))
                {
                    outputMatrix.Set(matrixARows, matrixAColumns, ConvertDoublesStrings(matrix.ToArray(), matrixARows, matrixAColumns));
                    SetNewOutputText(outputTextInverted);
                    outputTextInvertedMatrixName.Text = "A";
                } else
                {
                    errorTextBlock.Visibility = Visibility.Visible;
                    errorTextBlock.Text = "Matica A je singulárna a nemá inverznú maticu.";
                }
            } catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonInvertB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid || !InvertDeterminantBPossible) return;
            try
            {
                var matrix = DenseMatrix.OfArray(inputMatrix2.GetNumbers()).Inverse();
                if (double.IsFinite(matrix.Determinant()))
                {
                    outputMatrix.Set(matrixBRows, matrixBColumns, ConvertDoublesStrings(matrix.ToArray(), matrixBRows, matrixBColumns));
                    SetNewOutputText(outputTextInverted);
                    outputTextInvertedMatrixName.Text = "B";
                }
                else
                {
                    errorTextBlock.Visibility = Visibility.Visible;
                    errorTextBlock.Text = "Matica B je singulárna a nemá inverznú maticu.";
                }
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonDeterminantA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid) return;

        }

        private void ButtonDeterminantB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid) return;

        }

        private void ButtonMultiplyA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid) return;

        }

        private void ButtonMultiplyB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid) return;

        }


        public static string[,] ConvertDoublesStrings(double[,] doubles, int rows, int columns)
        {
            string[,] strings = new string[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    strings[i, j] = doubles[i, j].ToString();
                }
            }
            return strings;
        }
    }
}
