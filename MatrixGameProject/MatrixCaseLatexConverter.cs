using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixGameProject
{
    public class MatrixCaseLatexConverter
    {
        public string Convert(MatrixCase matrixCase)
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"\documentclass{article}");
            builder.AppendLine(@"\usepackage{amsmath}");
            builder.AppendLine(@"\begin{document}");
            
            builder.AppendLine(@"$\begin{matrix}");

            for (var i = 0; i < matrixCase.RowCount; i++)
            {
                for (var j = 0; j < matrixCase.ColumnCount; j++)
                {
                    builder.Append(matrixCase.Matrix[i][j]);
                    if (j != matrixCase.ColumnCount - 1)
                    {
                        builder.Append("&");
                    }
                }
                if (i != matrixCase.RowCount - 1)
                {
                    builder.Append(@"\\");
                }
                builder.AppendLine();
            }

            builder.AppendLine(@"\end{matrix}$");
            builder.Append(@"\\");

            switch (matrixCase.SolutionType)
            {
                case SolutionType.Clear:
                    builder.AppendLine("Solution in pure strategies. Game cost= \r\n" + matrixCase.ClearStrategySolution);
                    break;
                case SolutionType.Mixed:
                    builder.AppendLine("Solution in mixed strategies. \\\\");
                    foreach (var d in matrixCase.MixedStrategySolution)
                    {
                        foreach (var d1 in d)
                        {
                            builder.AppendFormat("{0} ", d1);
                        }
                        builder.Append("\\\\");
                    }
                    break;
            }
            
            builder.AppendLine(@"\end{document}");

            return builder.ToString();
        }
    }
}
