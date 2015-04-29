using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;


namespace MatrixGameProject
{
    public partial class Form1 : Form
    {
        private bool ButtonClicked;
        public Form1()
        {
            InitializeComponent();
            button4.Visible = false;
            button5.Visible = false;
            
            dataGridView2.RowHeadersVisible = false;

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        private double[][] DownloadFromExcel(string s)
        {
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + s + ";Extended Properties=\"Excel 12.0 Xml;HDR=NO\";");
            connection.Open();
            OleDbCommand command = new OleDbCommand("SELECT * FROM [Лист1$]", connection);
            OleDbDataReader reader = command.ExecuteReader();
            List<List<double>> list = new List<List<double>>();

            while (reader.Read())
            {
                List<double> buf = new List<double>();
                for (int i = 0; i < reader.FieldCount; i++)
                    buf.Add(int.Parse(reader[i].ToString()));
                list.Add(buf);
            }
            connection.Dispose();

            double[][] temp = new double[list.Count][];

            for (int i = 0; i < list.Count; i++)
            {
                temp[i] = list[i].ToArray<double>();
            }

            textBox1.Text = temp.Length.ToString();
            textBox2.Text = temp[1].Length.ToString();
            
            return temp;
        }

        public void ClearForm()
        {
            textBox5.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Columns.Clear();
            dataGridView3.Rows.Clear();
        }
        public void Pause(bool need)
        {
          //  if(label6.Text == "Игра не решается в чистых стратегиях") 
            if (need)
            {
                while (ButtonClicked == false)
                {
                    Application.DoEvents();
                }
                ButtonClicked = false;
            }
        }

        public double MinMaxGlobal;
        public void ChistStrat(double[][] A0, bool need, int n, int m)
        {
            double MaxMin = 0, MinMax = 0;
            double[] MinRow = new double[100];
            double[] MaxCol = new double[100];
            int i, j;

            //--------------------- расчёт в чистых стратегиях -----------------------------

            for (i = 0; i < n; i++)
            {
                MinRow[i] = A0[i][0];
            }

            for (i = 0; i < m; i++)
            {
                MaxCol[i] = A0[0][i];
            }

            //очистим стрингриды 2 и 3:
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
            dataGridView3.Columns.Add("Column" + i, "");
            dataGridView3.Rows.Add(1);
            dataGridView2.Columns.Add("Column" + i, "Min");
            dataGridView2.Rows.Add(n);

            button4.Focus();
            // расчёт минимумов и максимумов

            for (i = 0; i < n; i++)
            {
                label6.Text = "Найдём минимальный элемент в строке " + i;
                //Pause(need);
                for (j = 0; j < m; j++)
                {

                    //поиск минимального значения в строках :
                    if (A0[i][j] <= MinRow[i])
                    {
                        MinRow[i] = A0[i][j];
                    }
                }
                //вывод минимумов строк в СтрингГрид2 :
                dataGridView2.Rows[i].Cells[0].Value = MinRow[i];
            }
            dataGridView3.Rows[0].HeaderCell.Value = "Max";

            for (i = 0; i < n; i++)
            {
                dataGridView2.Rows[i].HeaderCell.Value = "";
            }
            for (j = 0; j < m; j++)
            {
                label6.Text = "Найдём максимальный элемент в строке " + j;
                //Pause(need);
                for (i = 0; i < n; i++)
                {
                    //поиск максимального значения в столбцах :
                    if (A0[i][j] >= MaxCol[j])
                    {
                        MaxCol[j] = A0[i][j];
                    }
                }
                dataGridView3.Columns.Add("c" + j, "");
                dataGridView3.Rows[0].Cells[j].Value = MaxCol[j];

                //вывод максимумов столбцов в СтрингГрид3 :
            }
            dataGridView3.Columns.RemoveAt(m);

            //найдём максимин
            MaxMin = MinRow[0];
            label6.Text = "Найдем минимакс";
            //Pause(need);
            MinMax = MaxCol[0];
            for (i = 0; i < m; i++)
            {
                if (MaxCol[i] <= MinMax) { MinMax = MaxCol[i]; }
            }
            textBox4.Text = MinMax.ToString();
            label6.Text = "Найдем максимин";
            //Pause(need);
            //найдём минимакс
            for (i = 0; i < n; i++)
            {
                if (MinRow[i] >= MaxMin) { MaxMin = MinRow[i]; }
            }
            textBox3.Text = MaxMin.ToString();
            if (MinMax == MaxMin)
            {
                label6.Text = "Игра решена в чистых стратегиях";
                
                
                ClearStrategy = true;
                MinMaxGlobal = MinMax;


                //MessageBox.Show("Игра решена в чистых стратегиях");
                textBox5.Text = MinMax.ToString();
            }
            else
            {
                label6.Text = "Игра не решается в чистых стратегиях";

                ClearStrategy = false;


                Form2 f = new Form2(this);
                //f.ShowDialog();
                ////MessageBox.Show("Игра не решается в чистых стратегиях, попробуйте решить её итерационным методом"); 
            }
            //------------------------------------------------------------------------------
        }

        public bool ClearStrategy;

        public void Raschet(bool need)
        {
            try
            {
                button4.Visible = true;
                button5.Visible = true;

                int i, j, n, m;

                n = int.Parse(textBox1.Text);
                m = int.Parse(textBox2.Text);

                double[][] A0 = new double[n][];


                for (i = 0; i < n; i++) A0[i] = new double[m];

                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < m; j++)
                    {
                        //перевод значений из таблицы в массив double матрицы A0:
                        A0[i][j] = double.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString());
                    }
                }

                ChistStrat(A0, need, n, m);
            }
            catch
            {
                MessageBox.Show("Проверьте правильность ввода");
            }

        }

        public void Generate(bool hand)
        {
            try
            {
                int i, j, n, m;

                n = int.Parse(textBox1.Text);
                m = int.Parse(textBox2.Text);

                double[][] A0 = new double[n][];
                for (i = 0; i < n; i++) A0[i] = new double[m];

                Random rand = new Random();
                if (hand)
                {
                    //Создаём пустую матрицу
                    for (i = 0; i < n; i++)
                    {
                        for (j = 0; j < m; j++)
                        {
                            A0[i][j] = 0;
                        }
                    }
                }
                else
                {
                    //Создаём рандомную матрицу
                    for (i = 0; i < n; i++)
                    {
                        for (j = 0; j < m; j++)
                        {
                            A0[i][j] = rand.Next(-20, 20);
                        }
                    }
                }

                ShowMatrix(A0);
            }
            catch
            {
                MessageBox.Show("Проверьте правильность введенных данных");
            }

        }

        public void ShowMatrix(double[][] A0)
        {
            int i, j, n = A0.Length, m = A0[1].Length;

            for (i = 0; i < m; i++) dataGridView1.Columns.Add("Column" + i, "B" + i);
            dataGridView1.Rows.Add(n);

            for (i = 0; i < n; i++)
            {
                for (j = 0; j < m; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = A0[i][j].ToString();
                }
            }
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value
                    = "A" + i.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            button5.Visible = false;


            ClearForm();

            Generate(false);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Raschet(true);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            button5.Visible = false;
            ClearForm();
            Generate(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ButtonClicked = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Raschet(false);
            button4.Visible = false;
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.FileName = "";
            OP.Filter = "Excel File (*.xlsx, *.xls)|*.xlsx;*.xls";
            OP.Title = "Открыть";

            if (OP.ShowDialog() != DialogResult.Cancel)
            {
                double[][] temp = DownloadFromExcel(OP.FileName);

                ClearForm();

                ShowMatrix(temp);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(this);
            f.ShowDialog();
        }
    }
}
