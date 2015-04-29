using System;
using System.Text;
using System.Windows.Forms;

namespace MatrixGameProject
{
    public partial class Form2 : Form
    {
        private double[][] _matrixX, _matrixY;
        public double[] _optX, _optY, _P, _Q;
        private double _extr, _V;
        private int _index = 0;
        private bool _auto = false;

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        public Form2(Form1 f)
        {
            InitializeComponent();

            for (int i = 0; i < f.dataGridView1.ColumnCount; i++) dataGridView1.Columns.Add("Column" + i, "B" + i);
            dataGridView1.Rows.Add(f.dataGridView1.RowCount);
            
            for (int i = 0; i < f.dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < f.dataGridView1.ColumnCount; j++)
                {
                   
                    dataGridView1.Rows[i].Cells[j].Value = f.dataGridView1.Rows[i].Cells[j].Value;
                }
            }
            
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value
                    = "A" + i.ToString();
            }

            Timer timer1 = new Timer();
        }

        private double[][] DataGridToArray()
        {
            double[][] temp = new double[dataGridView1.RowCount][];

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                temp[i] = new double[dataGridView1.ColumnCount];
                
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    
                    temp[i][j] = double.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString());
                }
            }
           
            return temp;
        }

        private double[][] CreateMatrixX(double[][] matrix)
        {
            double[][] temp = new double[matrix.Length][];

            for (int i = 0; i < matrix.Length; i++)
            {   
                temp[i] = new double[matrix[i].Length];
                
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    temp[i][j] = matrix[i][j];
                }
            }

            return temp;
        }

        private string ConvertMatrixToSLAE(double[][] A, char variable, string symbol, double[] B)
        {
            StringBuilder temp = new StringBuilder();

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A[i].Length; j++)
                {
                    if (A[i][j] >= 0 && j!=0)
                        temp.Append("+");

                    temp.Append(A[i][j].ToString());
                    temp.Append(variable);
                    
                }

                temp.Append(symbol + B[i] + "\n");
            }

            return temp.ToString();
        }
        private void ShowStep(int index)
        {
            switch (index)
            {
                case 1: { label1.Show(); break; }
                case 2: { ShowSLAE(); break; }
                case 3: { label4.Show(); break; }
                case 4: { ShowOpt(); break; }
                case 5: { label5.Show(); break; }
                case 6: { ShowExtr(); break; }
                case 7: { label6.Show(); break; }
                case 8: { ShowV(); break; }
                case 9: {ShowPQ(); break; }

                default: break;
            }
        }

        private void ShowSLAE()
        {
            StringBuilder tempY = new StringBuilder("Максимизировать \n \t"),
                         tempX = new StringBuilder("Минимизировать \n \t");
            double[] free = new double[_matrixY.Length];

            for (int i = 0; i < _matrixX.Length; i++)
            {
                free[i] = 1;

                tempY.Append("Y" + (i + 1));
                tempX.Append("X" + (i + 1));

                if (i != _matrixX.Length - 1)
                {
                    tempY.Append("+");
                    tempX.Append("+");
                }
            }

            tempY.Append("\nпри");
            tempX.Append("\nпри");

            label2.Text = tempY.ToString();
            labelMax.Text = ConvertMatrixToSLAE(_matrixY, 'Y', "<=", free);
            

            label3.Text = tempX.ToString();
            labelMin.Text = ConvertMatrixToSLAE(_matrixX, 'X', ">=", free);

            panel1.Visible = true;
            panel2.Visible = true;
        }

        private void ShowOpt()
        {
            for (int i = 0; i < _optX.Length; i++) dataGridView2.Columns.Add("Column" + i, "");
            dataGridView2.Rows.Add(2);

            for (int i = 0; i < 2; i++)
            {
                double[] values;

                if (i == 0)
                    values = _optX;
                else
                {
                    values = _optY;
                }

                for (int j = 0; j < _optX.Length; j++)
                {


                    dataGridView2.Rows[i].Cells[j].Value = Math.Round(values[j], 2);
                }
            }

            dataGridView2.Rows[0].HeaderCell.Value = "X";
            dataGridView2.Rows[1].HeaderCell.Value = "Y";

            dataGridView2.Visible = true;
        }

        private void ShowExtr()
        {
            extrBox.Text = Math.Round(_extr,2).ToString();
            extrBox.Visible = true;
        }

        private void ShowV()
        {
            label7.Visible = true;
            vBox.Text = Math.Round(_V, 2).ToString();
            vBox.Visible = true;
        }

        public void ShowPQ()
        {
            for (int i = 0; i < _optX.Length; i++) dataGridView3.Columns.Add("Column" + i, "");
            dataGridView3.Rows.Add(2);

            for (int i = 0; i < 2; i++)
            {
                double[] values;

                if (i == 0)
                    values = _P;
                else
                {
                    values = _Q;
                }

                for (int j = 0; j < _P.Length; j++)
                {
                    dataGridView3.Rows[i].Cells[j].Value = Math.Round(values[j], 2);
                }
            }

            dataGridView3.Rows[0].HeaderCell.Value = "P";
            dataGridView3.Rows[1].HeaderCell.Value = "Q";

            dataGridView3.Visible = true;
            label8.Visible = true;
        }

        private double FindExt(double[] opt)
        {
            double sum = 0;

            for (int i = 0; i < opt.Length; i++)
            {
                sum += opt[i];
            }

            return sum;
        }

        private double[] FindPQ(double[] opt, double v)
        {
            double[] temp = new double[opt.Length];

            for (int i = 0; i < opt.Length; i++)
            {
                temp[i] = opt[i]*v;
            }

            return temp;
        }

        private void Solve(double[][] matrixX, double[][] matrixY)
        {
            Matrix temp = new Matrix(matrixX);
            _optX = temp.Solve();

            temp = new Matrix(matrixY);
            _optY = temp.Solve();

            _extr = FindExt(_optX);
            _V = 1/_extr;

            _P = FindPQ(_optX, _V);
            _Q = FindPQ(_optY, _V);
        }

        public void SolveMatrix()
        {
            _matrixY = DataGridToArray();
            _matrixX = CreateMatrixX(_matrixY);

            Solve(_matrixX, _matrixY);

            button1.Visible = true;
            button3.Visible = true;

            timer1.Start();
        }

        private void solveBtn_Click(object sender, EventArgs e)
        {
            SolveMatrix();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _index++;
            ShowStep(_index);
            
            if (!_auto)
            {
                timer1.Stop();
            }
            else
            {
                button1.Enabled = false;
                button3.Enabled = false;

                if (_index > 10)
                {
                    timer1.Stop();
                    button1.Enabled = true;
                    button3.Enabled = true;
                }
            }
            
        }

        public void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            _auto = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
