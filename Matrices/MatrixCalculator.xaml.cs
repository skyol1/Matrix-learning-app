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
using MathNet.Numerics;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaticeApp
{
    public partial class MatrixCalculator : UserControl
    {

        private int RowsA = 3;
        private int ColumnsA = 3;
        private int RowsB = 3;
        private int ColumnsB = 3;

        private bool AddSubstractPossible = true;
        private bool MultiplyPossible = true;
        private bool InvertDeterminantAPossible = true;
        private bool InvertDeterminantBPossible = true;

        private Brush Button2MatrixOperationsBackgroundNormal = new SolidColorBrush(Color.FromArgb(255, 135, 240, 255));
        private Brush ButtonMatrixFillBackgroundNormal = new SolidColorBrush(Color.FromArgb(255, 135, 160, 255));
        private Brush Button1MatrixOperationBackgroundNormal = new SolidColorBrush(Color.FromArgb(255, 200, 135, 255));
        private Brush ButtonBackgroundError = new SolidColorBrush(Color.FromArgb(90, 255, 0, 0));

        private TextBlock ?_lastOutputTextBlock;

        private enum OutputPanelType
        {
            Matrix,
            Determinant,
            Error
        }

        public MatrixCalculator()
        {
            try
            {
                InitializeComponent();

                inputMatrix1.Resize(RowsA, ColumnsA);
                inputMatrix2.Resize(RowsB, ColumnsB);

                // Whenever input matrix changes it's size using Resize(), the event is invoked
                inputMatrix1.InputMatrixSizeChanged += SetRowsColumnsA;
                inputMatrix2.InputMatrixSizeChanged += SetRowsColumnsB;

                _lastOutputTextBlock = outputTextAdd;

                SetButtonColors();
            }
            catch (Exception ex) { ShowMessage("MatrixCalculator(): " + ex.Message); }
        }

        private void SetButtonColors()
        {
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
            ButtonCopyOutputToA.Background = ButtonMatrixFillBackgroundNormal;
            ButtonCopyOutputToB.Background = ButtonMatrixFillBackgroundNormal;
            ButtonPasteToA.Background = ButtonMatrixFillBackgroundNormal;
            ButtonPasteToB.Background = ButtonMatrixFillBackgroundNormal;

            ButtonTransposeA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonTransposeB.Background = Button1MatrixOperationBackgroundNormal;
            ButtonInvertA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonInvertB.Background = Button1MatrixOperationBackgroundNormal;
            ButtonDeterminantA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonDeterminantB.Background = Button1MatrixOperationBackgroundNormal;
            ButtonMultiplyA.Background = Button1MatrixOperationBackgroundNormal;
            ButtonMultiplyB.Background = Button1MatrixOperationBackgroundNormal;

            ButtonCopyOutputToClipboard1.ToolTip = "{{1, 2},\n{3, 4}}";
            ButtonCopyOutputToClipboard2.ToolTip = "1, 2\n3, 4";
        }

        #region Check if size sensitive operations are possible depending on the size of input matrices
        private void CheckAddSubstractMultiply()
        {
            if (RowsA == RowsB && ColumnsA == ColumnsB)
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

            if (ColumnsA == RowsB)
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
        private void CheckOperationButtonsA()
        {
            CheckAddSubstractMultiply();

            if (RowsA == ColumnsA)
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
        private void CheckOperationButtonsB()
        {
            CheckAddSubstractMultiply();

            if (RowsB == ColumnsB)
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
        #endregion

        #region Resizing matrix on text changed in TextBoxInputMatrixSize if input is valid
        private bool IsNewSizeValueValid(TextBoxInputMatrixSize textBox, out int newSize)
        {
            if (int.TryParse(textBox.Text, out newSize) && newSize > 0 && newSize < 10)
            {
                textBox.Valid = true;
                textBox.Background = Brushes.White;
                return true;
            }
            else
            {
                textBox.Valid = false;
                textBox.Background = new SolidColorBrush(Color.FromArgb(60, 255, 0, 0));
                return false;
            }
        }
        private void TextBoxInputMatrixARow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsNewSizeValueValid(TextBoxInputMatrixARow, out int newSize))
            {
                if (newSize != RowsA && TextBoxInputMatrixAColumn.Valid)
                {
                    RowsA = newSize;
                    inputMatrix1.Resize(newSize, ColumnsA);
                }
            }
        }
        private void TextBoxInputMatrixAColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNewSizeValueValid(TextBoxInputMatrixAColumn, out int newSize))
            {
                if (newSize != ColumnsA && TextBoxInputMatrixARow.Valid)
                {
                    ColumnsA = newSize;
                    inputMatrix1.Resize(RowsA, newSize);
                }
            }
        }
        private void TextBoxInputMatrixBRow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNewSizeValueValid(TextBoxInputMatrixBRow, out int newSize))
            {
                if (newSize != RowsB && TextBoxInputMatrixBColumn.Valid)
                {
                    RowsB = newSize;
                    inputMatrix2.Resize(newSize, ColumnsB);
                }
            }
        }
        private void TextBoxInputMatrixBColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNewSizeValueValid(TextBoxInputMatrixBColumn, out int newSize))
            {
                if (newSize != ColumnsB && TextBoxInputMatrixBRow.Valid)
                {
                    ColumnsB = newSize;
                    inputMatrix2.Resize(RowsB, newSize);
                }
            }
        }
        #endregion


        private void SetRowsColumnsA()
        {
            try
            {
                RowsA = inputMatrix1.RowsVisible;
                ColumnsA = inputMatrix1.ColumnsVisible;
                TextBoxInputMatrixARow.Text = RowsA.ToString();
                TextBoxInputMatrixAColumn.Text = ColumnsA.ToString();
                CheckOperationButtonsA();
            }
            catch (Exception ex) { ShowMessage("SetRowsColumnsA(): " + ex.Message); }
        }
        private void SetRowsColumnsB()
        {
            try
            {
                RowsB = inputMatrix2.RowsVisible;
                ColumnsB = inputMatrix2.ColumnsVisible;
                TextBoxInputMatrixBRow.Text = RowsB.ToString();
                TextBoxInputMatrixBColumn.Text = ColumnsB.ToString();
                CheckOperationButtonsB();
            }
            catch (Exception ex) { ShowMessage("SetRowsColumnsB(): " + ex.Message); }
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


        private void ButtonPasteToA_Click(object sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                SetOutputPanelsVisibility(OutputPanelType.Error);
                errorTextBlock.Text = "Clipboard does not contain text";
                return;
            }
            
            if (TryConvertStringTo2DArray(Clipboard.GetText(), out string[,] arr, out string info))
            {
                errorTextBlock.Visibility = Visibility.Collapsed;
                inputMatrix1.ResizeSet(arr, arr.GetLength(0), arr.GetLength(1));
            } else
            {
                SetOutputPanelsVisibility(OutputPanelType.Error);
                errorTextBlock.Text = info;
            }
        }

        private void ButtonPasteToB_Click(object sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                SetOutputPanelsVisibility(OutputPanelType.Error);
                errorTextBlock.Text = "Clipboard does not contain text";
                return;
            }

            if (TryConvertStringTo2DArray(Clipboard.GetText(), out string[,] arr, out string info))
            {
                errorTextBlock.Visibility = Visibility.Collapsed;
                inputMatrix2.ResizeSet(arr, arr.GetLength(0), arr.GetLength(1));
            }
            else
            {
                SetOutputPanelsVisibility(OutputPanelType.Error);
                errorTextBlock.Text = info;
            }
        }

        private void ButtonCopyBtoA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputMatrix1.ResizeSet(inputMatrix2.GetStrings(), RowsB, ColumnsB);
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonCopyAtoB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputMatrix2.ResizeSet(inputMatrix1.GetStrings(), RowsA, ColumnsA);
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void SetNewOutputTextBlock(TextBlock textBlock)
        {
            try
            {
                _lastOutputTextBlock.Visibility = Visibility.Collapsed;
                textBlock.Visibility = Visibility.Visible;
                _lastOutputTextBlock = textBlock;

                SetOutputPanelsVisibility(OutputPanelType.Matrix);
            } 
            catch (Exception ex) { ShowMessage(ex.Message); }
        }
        private void SetOutputPanelsVisibility(OutputPanelType type)
        {
            switch (type)
            {
                case OutputPanelType.Matrix:
                    outputPanel1.Visibility = Visibility.Visible;
                    outputPanel2.Visibility = Visibility.Collapsed;
                    errorTextBlock.Visibility = Visibility.Collapsed;
                    break;
                case OutputPanelType.Determinant:
                    outputPanel1.Visibility = Visibility.Collapsed;
                    outputPanel2.Visibility = Visibility.Visible;
                    errorTextBlock.Visibility = Visibility.Collapsed;
                    break;
                case OutputPanelType.Error:
                    errorTextBlock.Visibility = Visibility.Visible;
                    break;
            }
        }
        
        private void ButtonAddSubstract_Click(object sender, RoutedEventArgs e)
        {
            if (!AddSubstractPossible || !inputMatrix1.IsInputValid || !inputMatrix2.IsInputValid) return;

            bool operation = (sender as Button).Name == "ButtonAdd";

            int rows = RowsA;
            int columns = ColumnsA;

            double[,] inputA = inputMatrix1.GetNumbers();
            double[,] inputB = inputMatrix2.GetNumbers();

            double[,] output = new double[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    output[i, j] = operation ? (inputA[i, j] + inputB[i, j]) : (inputA[i, j] - inputB[i, j]);

            outputMatrix.Set(rows, columns, output);
            SetNewOutputTextBlock(operation ? outputTextAdd : outputTextSubstract);
        }

        private void ButtonMultiply_Click(object sender, RoutedEventArgs e)
        {
            if (!MultiplyPossible || !inputMatrix1.IsInputValid || !inputMatrix2.IsInputValid) return;

            double[,] inputA = inputMatrix1.GetNumbers();
            double[,] inputB = inputMatrix2.GetNumbers();

            double[,] output = new double[RowsA, ColumnsB];

            for (int i = 0; i < RowsA; i++)
                for (int j = 0; j < ColumnsB; j++)
                {
                    output[i, j] = 0;
                    for (int k = 0; k < ColumnsA; k++)
                        output[i, j] += inputA[i, k] * inputB[k, j];
                }

            outputMatrix.Set(RowsA, ColumnsB, RoundToNearestPrecision(output));
            SetNewOutputTextBlock(outputTextMultiply);
        }

        private void ButtonTransposeA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid) return;
            try
            {
                string[,] input = inputMatrix1.GetStrings();
                string[,] transposed = new string[ColumnsA, RowsA];
            
                for (int i = 0; i < RowsA; i++)
                    for (int j = 0; j < ColumnsA; j++)
                        transposed[j, i] = input[i, j];

                outputMatrix.Set(ColumnsA, RowsA, transposed);
                SetNewOutputTextBlock(outputTextTransposed);
                outputTextTransposedMatrixName.Text = "A";
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonTransposeB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid) return;
            try
            {
                string[,] input = inputMatrix2.GetStrings();
                string[,] transposed = new string[ColumnsB, RowsB];

                for (int i = 0; i < RowsB; i++)
                    for (int j = 0; j < ColumnsB; j++)
                        transposed[j, i] = input[i, j];

                outputMatrix.Set(ColumnsB, RowsB, transposed);
                SetNewOutputTextBlock(outputTextTransposed);
                outputTextTransposedMatrixName.Text = "B";
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonInvertA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid || !InvertDeterminantAPossible) return;
            try
            {
                var matrix = DenseMatrix.OfArray(inputMatrix1.GetNumbers());
                if (matrix.Determinant() != 0)
                {
                    outputMatrix.Set(RowsA, ColumnsA, RoundToNearestPrecision(matrix.Inverse().ToArray()));
                    SetNewOutputTextBlock(outputTextInverted);
                    outputTextInvertedMatrixName.Text = "A";
                } else
                {
                    SetOutputPanelsVisibility(OutputPanelType.Error);
                    errorTextBlock.Text = "Matica A je singulárna a nemá inverznú maticu.";
                }
            } 
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonInvertB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid || !InvertDeterminantBPossible) return;
            try
            {
                var matrix = DenseMatrix.OfArray(inputMatrix2.GetNumbers());
                if (matrix.Determinant() != 0)
                {
                    outputMatrix.Set(RowsB, ColumnsB, RoundToNearestPrecision(matrix.Inverse().ToArray()));
                    SetNewOutputTextBlock(outputTextInverted);
                    outputTextInvertedMatrixName.Text = "B";
                }
                else
                {
                    SetOutputPanelsVisibility(OutputPanelType.Error);
                    errorTextBlock.Text = "Matica B je singulárna a nemá inverznú maticu.";
                }
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonDeterminantA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid || !InvertDeterminantAPossible) return;
            try
            {
                double determinant = RoundToNearestPrecision(DenseMatrix.OfArray(inputMatrix1.GetNumbers()).Determinant());
                SetOutputPanelsVisibility(OutputPanelType.Determinant);
                outputTextDeterminantMatrixName.Text = "A";
                determinatValue.Text = determinant.ToString();
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonDeterminantB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid || !InvertDeterminantBPossible) return;
            try
            {
                double determinant = RoundToNearestPrecision(DenseMatrix.OfArray(inputMatrix2.GetNumbers()).Determinant());
                SetOutputPanelsVisibility(OutputPanelType.Determinant);
                outputTextDeterminantMatrixName.Text = "B";
                determinatValue.Text = determinant.ToString();
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonMultiplyA_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix1.IsInputValid || !multiplyAby.Valid) return;
            try
            {
                double[,] output = inputMatrix1.GetNumbers();
                double value = double.Parse(multiplyAby.Text);

                for (int i = 0; i < RowsA; i++)
                    for (int j = 0; j < ColumnsA; j++)
                        output[i, j] *= value;

                outputMatrix.Set(RowsA, ColumnsA, output);
                SetNewOutputTextBlock(outputTextMultiplied);
                outputTextMultipliedMatrixName.Text = "A";
                outputTextMultipliedBy.Text = value.ToString();
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonMultiplyB_Click(object sender, RoutedEventArgs e)
        {
            if (!inputMatrix2.IsInputValid || !multiplyBby.Valid) return;
            try
            {
                double[,] output = inputMatrix2.GetNumbers();
                double value = double.Parse(multiplyBby.Text);

                for (int i = 0; i < RowsB; i++)
                    for (int j = 0; j < ColumnsB; j++)
                        output[i, j] *= value;

                outputMatrix.Set(RowsB, ColumnsB, output);
                SetNewOutputTextBlock(outputTextMultiplied);
                outputTextMultipliedMatrixName.Text = "B";
                outputTextMultipliedBy.Text = value.ToString();
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonCopyOutputToA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rows = outputMatrix.RowsVisible;
                int columns = outputMatrix.ColumnsVisible;
                inputMatrix1.ResizeSet(outputMatrix.GetStrings(), rows, columns);
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonCopyOutputToB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rows = outputMatrix.RowsVisible;
                int columns = outputMatrix.ColumnsVisible;
                inputMatrix2.ResizeSet(outputMatrix.GetStrings(), rows, columns);
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void ButtonCopyOutputToClipboard1_Click(object sender, RoutedEventArgs e)
        {
            Copy2DstringArrayToClipboard(outputMatrix.GetStrings(), 1);
        }

        private void ButtonCopyOutputToClipboard2_Click(object sender, RoutedEventArgs e)
        {
            Copy2DstringArrayToClipboard(outputMatrix.GetStrings(), 2);
        }

        private void ButtonCopyDeterminantToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(determinatValue.Text);
        }


        private void MultiplyABby_InputTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textBox = (sender as TextBoxInputMatrix);
                if (ParseDouble(textBox.Text, out double value) ||
                    string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Valid = true;
                    textBox.Background = Brushes.White;
                }
                else
                {
                    textBox.Valid = false;
                    textBox.Background = new SolidColorBrush(Color.FromArgb(60, 255, 0, 0));
                }
            }
            catch (Exception ex) { ShowMessage(ex.Message); }
        }

        private void InnerScrollViewers_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            // Since the inner ScrollViewers only scrolls horizontally, vertical scroll events are directly forwarded to the outer ScrollViewer
            e.Handled = true;
            OuterScrollViewer.RaiseEvent(new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent
            });
        }
    }
}
