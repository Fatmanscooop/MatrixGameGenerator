using System;

namespace MatrixGameProject
{
    public enum SolutionType
    {
        Clear,
        Mixed,
    }

    public class MatrixCase
    {
        private static readonly Random Rand = new Random();

        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public double[][] Matrix { get; set; }

        public SolutionType SolutionType { get; set; }
        public double ClearStrategySolution { get; set; }
        public double[][] MixedStrategySolution { get; set; }

        public MatrixCase(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        public double[][] GenerateMatrix(int min = -10, int max = 10, bool isDouble = false)
        {
            var matrix = new double[RowCount][];
            for (var i = 0; i < RowCount; i++) matrix[i] = new double[ColumnCount];

            for (var i = 0; i < RowCount; i++)
            {
                for (var j = 0; j < ColumnCount; j++)
                {
                    matrix[i][j] = Rand.Next(min, max);
                    if (isDouble) matrix[i][j] += Math.Round(Rand.NextDouble(), 2);
                }
            }

            return matrix;
        }

        public bool HasValue
        {
            get
            {
                return (RowCount > 0 && ColumnCount > 0 && Matrix != null)
                       &&
                       (
                           (SolutionType == SolutionType.Clear) ||
                           (SolutionType == SolutionType.Mixed && MixedStrategySolution != null)
                        );
            }
        }
    }
}
