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
        private uint matrixSize = 3;
        public MatrixCalculator()
        {
            InitializeComponent();
        }
        public void SetupCalculator(Operation selectedOperation)
        {

            this.selectedOperation=selectedOperation;

            matrix1.SetMatrix(matrixSize, matrixSize, true);
            matrix2.SetMatrix(matrixSize, matrixSize, true);

            string[,] matrixData = new string[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
                for (int j = 0; j < matrixSize; j++)
                    matrixData[i, j] = "0";
            matrix3.SetMatrix(matrixSize, matrixSize,matrixData, true);
            matrix2.onInputChanged += onInputChanged;
            matrix1.onInputChanged += onInputChanged;

            switch (selectedOperation)
            {
                case Operation.Add:
                    OperationSymbol.Text = "+";
                    matrix1.highlighters.Add(new SingleElementHighlighter(matrix2,Color.FromArgb(40, 255, 0, 0)));
                    matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(40, 0, 0, 255)));
                    matrix3.highlighters.Add(new SingleElementHighlighter(matrix1, Color.FromArgb(40, 255, 0, 0)));
                    break;
                case Operation.Subtract:
                    OperationSymbol.Text = "-";
                    matrix1.highlighters.Add(new SingleElementHighlighter(matrix2, Color.FromArgb(40, 255, 0, 0)));
                    matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(40, 0, 0, 255)));
                    matrix3.highlighters.Add(new SingleElementHighlighter(matrix1, Color.FromArgb(40, 255, 0, 0)));
                    break;
                case Operation.Multiply:
                    OperationSymbol.Text = "*";
                    matrix1.highlighters.Add(new RowHighlighter(matrix2, Color.FromArgb(50, 0, 0, 255)));
                    matrix2.highlighters.Add(new SingleElementHighlighter(matrix3, Color.FromArgb(50, 255, 0, 0)));
                    matrix3.highlighters.Add(new ColumnHighlighter(matrix1, Color.FromArgb(50, 0, 255, 0)));
                    break;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OperationSymbol == null) return;
            try
            {
                matrixSize = uint.Parse((sender as TextBox).Text);
            }
            catch { return; };
            SetupCalculator(selectedOperation);
        }
        private void onInputChanged()
        {
            double[,] input1 = new double[matrixSize, matrixSize];
            double[,] input2 = new double[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
                for (int j = 0; j < matrixSize; j++)
                {
                    input1[i, j] = double.Parse(((matrix1.MatrixGrid.Children[i * (int)matrixSize + j] as Border).Child as TextBox).Text);
                    input2[i, j] = double.Parse(((matrix2.MatrixGrid.Children[i * (int)matrixSize + j] as Border).Child as TextBox).Text);
                }
            double[,] result= null;
            switch (selectedOperation) {
                case Operation.Add:
                    result = AddMatrix(input1, input2); break;
                case Operation.Subtract:
                    result = SubtractMatrix(input1, input2); break;
                case Operation.Multiply:
                    result = MultiplyMatrix(input1, input2); break;
            }
            matrix3.SetMatrix(matrixSize, matrixSize,ToStringData(result),true);
        }

        private double[,] AddMatrix(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixSize, matrixSize];
            for (int i = 0; i<matrixSize; i++)
                for(int j = 0;j < matrixSize; j++)
                    result[i,j] = matrixA[i,j] + matrixB[i,j];
            return result;
        }
        private double[,] SubtractMatrix(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
                for (int j = 0; j < matrixSize; j++)
                    result[i, j] = matrixA[i, j] - matrixB[i, j];
            return result;
        }

        private double[,] MultiplyMatrix(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
                for (int j = 0; j < matrixSize; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < matrixSize; k++)
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            return result;
        }

        private string[,] ToStringData(double[,] doubles)
        {
            string[,] result = new string[matrixSize, matrixSize];
            for (int i = 0;i < matrixSize; i++)
                for( int j = 0; j<matrixSize; j++)
                    result[i,j]=doubles[i,j].ToString();
            return result;
        }

        private void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            matrix1.Randomize();
            matrix2.Randomize();
            onInputChanged();
        }
    }
}
