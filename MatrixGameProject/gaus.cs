using System;
using System.Linq;
using System.Windows.Forms;

namespace MatrixGameProject
{
    class Matrix
    {

        #region Описание переменных
        int i, j, m, n, nSize, PosColumn, PosRow;
        double[,] nArray = new double[MAXSIZE, MAXSIZE + 1];
        double[,] nMatrix = new double[MAXSIZE, MAXSIZE + 1];
        int[] nOrder = new int[MAXSIZE];
        double[] dTotal = new double[MAXSIZE];
        double[] e = new double[MAXSIZE];
        double dMax1, dMax2, dSum;
        public const double EPS = 0.00000000001;
        public const int MAXSIZE = 25; //максимально возможный размер матрицы 
        #endregion

        // Конструктор 
        public Matrix(double[][] matrix)
        {
            nSize = matrix.Length;

            nArray = new double[nSize, nSize+1];

            try
            {
                for (i = 0; i < matrix.Length; i++)
                    for (j = 0; j <= matrix[i].Length; j++)
                    {
                        if (j < matrix[i].Length)
                        {
                            nArray[i, j] = matrix[i][j];
                        }
                        else
                        {
                            nArray[i, j] = 1;
                        }

                    }
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно решить!");
            }

            VariableOrder();
        }


        public void VariableOrder()
        {
            //заполнение промежуточного массива для хранения порядка переменных 
            for (i = 0; i < nSize; i++)
                nOrder[i] = i;
        }

        //замена рядов в массиве 
        public void ReplaceRows(int i, int n)
        {
            double Temp;
            for (j = 0; j <= nSize; j++)
            {
                Temp = nArray[i, j];
                nArray[i, j] = nArray[n, j];
                nArray[n, j] = Temp;
            }
        }

        //замена столбцов в массиве 
        public void ReplaceColumns(int i, int n)
        {
            double Temp;
            for (j = 0; j < nSize; j++)
            {
                Temp = nArray[j, i];
                nArray[j, i] = nArray[j, n];
                nArray[j, n] = Temp;
            }

            //фиксируем перестановку столбцов 
            int Elem = nOrder[i];
            nOrder[i] = nOrder[n];
            nOrder[n] = Elem;
        }

        //Решение методом Гауса 
        public double[] Solve()
        {
            for (i = 0; i < nSize; i++)
            {
                //поиск максимального по модулю элемента в массиве(подмассиве) 
                dMax1 = nArray[i, i];
                dMax2 = nArray[i, i];
                PosColumn = i;
                PosRow = i;

                for (n = i; n < nSize; n++)
                {
                    for (m = i; m < nSize; m++)

                        if (Math.Abs(nArray[n, m]) > Math.Abs(dMax1))
                        {
                            dMax2 = dMax1;
                            dMax1 = nArray[n, m];
                            PosColumn = m;
                            PosRow = n;
                        }
                }


                //условие, когда система несовместна 
                if (dMax1 == 0 && dMax2 == 0)
                {
                    Console.WriteLine("Система не имеет решения");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                //замена в случае необходимости строк 
                if (PosColumn != i)
                    ReplaceColumns(i, PosColumn);


                //замена в случае необходимости столбцов 
                if (PosRow != i)
                    ReplaceRows(i, PosRow);

                //делим i-ю строку на диагональный элемент 
                for (n = i; n <= nSize; n++)
                    if (nArray[i, n] == 0 && dMax1 < 0)
                        nArray[i, n] = 0;
                    else
                        nArray[i, n] /= dMax1;
                //обнуляем все элементы, стоящие под диагональным 
                for (j = nSize - 1; j > i; j--)
                    if (nArray[j, i] != 0)
                    {
                        double dElem = nArray[j, i];
                        for (n = i; n <= nSize; n++)
                        {
                            if (nArray[j, n] == 0 && dElem < 0)
                                nArray[j, n] = 0;
                            else
                                nArray[j, n] /= dElem;

                            if (Math.Abs(nArray[j, n] - nArray[i, n]) < EPS)
                                nArray[j, n] = 0;
                            else
                                nArray[j, n] -= nArray[i, n];
                        }
                    }
            }


            //получение корня, стоящего в последнем столбце значимой матрицы 
            dTotal[nSize - 1] = nArray[nSize - 1, nSize] / nArray[nSize - 1, nSize - 1];

            //получение всех остальных корней 
            for (i = nSize - 2; i >= 0; i--)
            {
                dSum = 0;
                for (j = nSize - 1; j > i; j--)
                    dSum += nArray[i, j] * dTotal[j];
                if (Math.Abs(nArray[i, nSize] - dSum) < EPS)
                    dTotal[i] = 0;
                else
                    dTotal[i] = (nArray[i, nSize] - dSum) / nArray[i, i];
            }

            //восстановление порядка переменных 
            for (i = 0; i < nSize; i++)
                for (j = 0; j < nSize; j++)
                    if (i == nOrder[j])
                        e[i] = dTotal[j];

            //вывод вектора с решением системы 

            return e.Take(nSize).ToArray<double>();
        }
    } 
}
