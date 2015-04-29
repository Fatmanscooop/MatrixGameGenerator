using System;
using System.Windows.Forms;

namespace MatrixGameProject
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GenerateCaseForm());
            //Application.Run(new Form1());
        }
    }
}
