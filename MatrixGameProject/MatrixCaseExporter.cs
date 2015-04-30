using System.IO;
using System.Text;

namespace MatrixGameProject
{
    public class MatrixCaseExporter
    {
        public static void ToCsv(MatrixCase matrixCase, string filename = @"C:\export.csv")
        {
            var sb = new StringBuilder();
            
            for (var i = 0; i < matrixCase.RowCount; i++)
            {
                for (var j = 0; j < matrixCase.ColumnCount; j++)
                    sb.AppendFormat("{0},", matrixCase.Matrix[i][j]);

                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine();

            switch (matrixCase.SolutionType)
            {
                case SolutionType.Clear:
                    sb.AppendLine("Решение в чистых стратегиях. Цена игры = \r\n" + matrixCase.ClearStrategySolution);
                    break;
                case SolutionType.Mixed:
                    sb.AppendLine("Решение в смешанных стратегиях. ");
                    foreach (var d in matrixCase.MixedStrategySolution)
                    {
                        foreach (var d1 in d)
                        {
                            sb.AppendFormat("{0},", d1);
                        }
                        sb.AppendLine();
                    }
                    break;
            }

            File.WriteAllText(filename, sb.ToString(), Encoding.GetEncoding(1251));

            //var utf16 = Encoding.GetEncoding(1251);
            //var output = utf16.GetBytes(sb.ToString());

            //var fs = new FileStream(filename, FileMode.Create);
            //var bw = new BinaryWriter(fs);
            //bw.Write(output, 0, output.Length); //write the encoded file
            //bw.Flush();
            //bw.Close();
            //fs.Close();
        }

        public static void ToLaTeX(MatrixCase matrixCase, string filename = @"C:\export.csv")
        {
            var converter = new MatrixCaseLatexConverter();
            var converted = converter.Convert(matrixCase);
            File.WriteAllText(filename, converted, Encoding.GetEncoding(1251));
        }
    }
}