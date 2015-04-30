using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MatrixGameProject;

namespace Tests
{
    public class MatrixCaseConverterTest
    {
        [Test]
        public void SimpleTest()
        {
            var matrixCase = new MatrixCase(3, 3);
            matrixCase.Matrix = new double[3][];
            for (var i = 0; i < 3; i++) matrixCase.Matrix[i] = new double[3];
            matrixCase.Matrix[0][0] = 1;
            matrixCase.Matrix[0][1] = 2;
            matrixCase.Matrix[0][2] = 3;

            matrixCase.Matrix[1][0] = 4;
            matrixCase.Matrix[1][1] = 5;
            matrixCase.Matrix[1][2] = 6;

            matrixCase.Matrix[2][0] = 7;
            matrixCase.Matrix[2][1] = 8;
            matrixCase.Matrix[2][2] = 9;

            var converter = new MatrixCaseLatexConverter();

            var actualConvertResult = converter.Convert(matrixCase);
            var expectedConvertResult = "\\documentclass{article}\r\n" +
                                        "\\usepackage{amsmath}\r\n" + 
                                        "\\begin{document}\r\n" +
                                        "$\\begin{matrix}\r\n" + 
                                        "1&2&3\\\\\r\n4&5&6\\\\\r\n7&8&9\r\n" +
                                        "\\end{matrix}$\r\n" +
                                        "\\\\"+
                                        "Solution in pure strategies. Game cost= \r\n" +
                                        "0\r\n"+
                                        "\\end{document}\r\n";
            Console.Write(expectedConvertResult);
            Console.Write(actualConvertResult);
            Assert.AreEqual(expectedConvertResult, actualConvertResult);
        }
    }
}
