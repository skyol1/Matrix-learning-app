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

namespace MaticeApp
{
    public partial class MatrixCalculator : UserControl
    {
        public enum Operation
        {
            Add,
            Subtract,
            Multiply
        }
        private Operation selectedOperation;
        private uint matrixARows = 3;
        private uint matrixAColumns = 3;
        private uint matrixBRows = 3;
        private uint matrixBColumns = 3;
        public MatrixCalculator()
        {
            InitializeComponent();
        }
        public void SetupCalculator(Operation selectedOperation)
        {
            matrix1.highlighters.Clear();
            matrix2.highlighters.Clear();
            matrix3.highlighters.Clear();
            if (matrix1.onInputChanged==null)
                matrix1.onInputChanged += onInputChanged;
            if (matrix2.onInputChanged == null)
                matrix2.onInputChanged += onInputChanged;
            this.selectedOperation = selectedOperation;
            CheckMatrixSizes();
            switch (selectedOperation)
            {
                case Operation.Add:
                case Operation.Subtract:
                    SetupAddSubtract();
                    break;
                case Operation.Multiply:
                    SetupMultiply();
                    break;
            }
        }
        private void SetupAddSubtract()
        {
            matrix1.SetMatrix(matrixAColumns, matrixARows, true);
            matrix2.SetMatrix(matrixAColumns, matrixARows, true);

            matrix1.highlighters.Add(new SingleElementHighlighter(matrix2, Color.FromArgb(40, 255, 0, 0)));
            matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(40, 0, 0, 255)));
            matrix3.highlighters.Add(new SingleElementHighlighter(matrix1, Color.FromArgb(40, 255, 0, 0)));

            RightRows.IsReadOnly = true;
            RightColumns.IsReadOnly = true;

            string[,] matrixData = new string[matrixAColumns, matrixARows];
            for (int i = 0; i < matrixAColumns; i++)
                for (int j = 0; j < matrixARows; j++)
                    matrixData[i, j] = "0";
            matrix3.SetMatrix(matrixData, true);

            switch (selectedOperation)
            {
                case Operation.Add:
                    OperationSymbol.Text = "+";
                    break;
                case Operation.Subtract:
                    OperationSymbol.Text = "-";
                    break;
            }
        }
        private void SetupMultiply()
        {
            matrix1.SetMatrix(matrixAColumns, matrixARows, true);
            matrix2.SetMatrix(matrixBColumns, matrixBRows, true);

            RightRows.IsReadOnly = true;
            RightColumns.IsReadOnly = false;

            string[,] matrixData = new string[matrixAColumns, matrixBRows];
            for (int i = 0; i < matrixAColumns; i++)
                for (int j = 0; j < matrixBRows; j++)
                    matrixData[i, j] = "0";
            matrix3.SetMatrix(matrixData, true);

            OperationSymbol.Text = "*";
            matrix1.highlighters.Add(new RowHighlighter(matrix2, Color.FromArgb(50, 0, 0, 255)));
            matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(50, 255, 0, 0)));
            matrix3.highlighters.Add(new ColumnHighlighter(matrix1, Color.FromArgb(50, 0, 255, 0)));
        }
        private void onInputChanged()
        {
            double[,] input1 = new double[matrixAColumns, matrixARows];
            double[,] input2 = new double[matrixBColumns, matrixBRows];
            for (int i = 0; i < matrixAColumns; i++)
                for (int j = 0; j < matrixARows; j++)
                    input1[i, j] = double.Parse(((matrix1.MatrixGrid.Children[i * (int)matrixARows + j] as Border).Child as TextBox).Text);
            for (int i = 0; i < matrixBColumns; i++)
                for (int j = 0; j < matrixBRows; j++)
                    input2[i, j] = double.Parse(((matrix2.MatrixGrid.Children[i * (int)matrixBRows + j] as Border).Child as TextBox).Text);
            double[,] result= null;
            switch (selectedOperation) {
                case Operation.Add:
                    result = AddMatrix(input1, input2); break;
                case Operation.Subtract:
                    result = SubtractMatrix(input1, input2); break;
                case Operation.Multiply:
                    result = MultiplyMatrix(input1, input2); break;
            }
            matrix3.SetMatrix(ToStringData(result),true);
        }

        private double[,] AddMatrix(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixAColumns, matrixARows];
            for (int i = 0; i<matrixAColumns; i++)
                for(int j = 0;j < matrixARows; j++)
                    result[i,j] = matrixA[i,j] + matrixB[i,j];
            return result;
        }
        private double[,] SubtractMatrix(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixAColumns, matrixARows];
            for (int i = 0; i < matrixAColumns; i++)
                for (int j = 0; j < matrixARows; j++)
                    result[i, j] = matrixA[i, j] - matrixB[i, j];
            return result;
        }
        private double[,] MultiplyMatrix(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixAColumns, matrixBRows];
            for (int i = 0; i < matrixAColumns; i++)
                for (int j = 0; j < matrixBRows; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < matrixARows; k++)
                        result[i, j] += matrixA[i, k] * matrixB[k,j];
                }
            return result;
        }

        private string[,] ToStringData(double[,] doubles)
        {
            string[,] result = new string[doubles.GetUpperBound(0)+1, doubles.GetUpperBound(1) + 1];
            for (int i = 0;i < doubles.GetUpperBound(0)+1; i++)
                for( int j = 0; j < doubles.GetUpperBound(1)+1; j++)
                    result[i,j]=doubles[i,j].ToString();
            return result;
        }
        private void RandomizeLeftButton_Click(object sender, RoutedEventArgs e)
        {
            matrix1.Randomize();
            onInputChanged();
        }

        private void RandomizeRightButton_Click(object sender, RoutedEventArgs e)
        {
            matrix2.Randomize();
            onInputChanged();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            matrix1.Clear();
            matrix2.Clear();
            onInputChanged();
        }

        private void LeftColumns_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OperationSymbol == null) return;
            if (!uint.TryParse((sender as TextBox).Text, out matrixARows))return;
            CheckMatrixSizes();
            SetupCalculator(selectedOperation);
        }
        private void LeftRows_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OperationSymbol == null) return;
            if (!uint.TryParse((sender as TextBox).Text, out matrixAColumns)) return;
            CheckMatrixSizes();
            SetupCalculator(selectedOperation);
        }
        private void RightColumns_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OperationSymbol == null) return;
            if (!uint.TryParse((sender as TextBox).Text, out matrixBRows)) return;
            CheckMatrixSizes();
            SetupCalculator(selectedOperation);
        }
        private void RightRows_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OperationSymbol == null) return;
            if (!uint.TryParse((sender as TextBox).Text, out matrixBColumns)) return;
            CheckMatrixSizes();
            SetupCalculator(selectedOperation);
        }

        private void CheckMatrixSizes()
        {
            switch (selectedOperation){
                case Operation.Subtract:
                case Operation.Add:
                    matrixBRows = matrixARows;
                    RightColumns.Text=matrixBRows.ToString();
                    matrixBColumns = matrixAColumns;
                    RightRows.Text=matrixBColumns.ToString();
                    break;
                case Operation.Multiply:
                    matrixBColumns = matrixARows;
                    RightRows.Text = matrixBColumns.ToString();
                    break;
            }
            
        }
    }
}
