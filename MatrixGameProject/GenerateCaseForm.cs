using System;
using System.Globalization;
using System.Windows.Forms;

namespace MatrixGameProject
{
    public partial class GenerateCaseForm : Form
    {
        public static MatrixCase Case;
        public Form1 F1;
        public Form2 F2;

        public const string RESULT_DATA_GRID_VIEW_NAME = "ResultDataGridView";
        public const string INITIAL_DATA_GRID_VIEW_NAME = "InitialDataGridView";

        public GenerateCaseForm()
        {
            InitializeComponent();
        }

        private void Generate()
        {
            F1 = new Form1();

            Case = new MatrixCase(
                rowCount: (int)numericUpDown1.Value,
                columnCount: (int)numericUpDown1.Value);
            //columnCount: (int) numericUpDown2.Value);

            Case.Matrix = Case.GenerateMatrix((int)numericUpDown3.Value, (int)numericUpDown4.Value, checkBox1.Checked);

            F1.textBox1.Text = Case.RowCount.ToString(CultureInfo.InvariantCulture);
            F1.textBox2.Text = Case.ColumnCount.ToString(CultureInfo.InvariantCulture);

            F1.ClearForm();
            F1.ShowMatrix(Case.Matrix);

            F1.Raschet(false);

            //F1.ShowDialog();

            if (F1.ClearStrategy)
            {
                Case.SolutionType = SolutionType.Clear;
                Case.ClearStrategySolution = F1.MinMaxGlobal;
            }
            else
            {
                Case.SolutionType = SolutionType.Mixed;
                F2 = new Form2(F1);

                F2.SolveMatrix();

                var p = F2._P;
                var q = F2._Q;

                for (var i = 0; i < p.Length; i++)
                {
                    p[i] = Math.Round(p[i], 2);
                    q[i] = Math.Round(q[i], 2);
                }

                Case.MixedStrategySolution = new[] { p, q };
            }

            DrawSolutions();

            exportLaTeX.Enabled = Case.HasValue;
            exportExcel.Enabled = Case.HasValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Generate();
        }

        private void DrawSolutions()
        {
            ClearDataGridView();

            var dataGridView1 = new DataGridView() { Name = INITIAL_DATA_GRID_VIEW_NAME };
            initialDataGridViewPanel.Controls.Add(dataGridView1);
            ShowMatrix(dataGridView1, Case.Matrix);

            switch (Case.SolutionType)
            {
                case SolutionType.Mixed:
                resultDataGridViewPanel.Controls.Add(new Label
                                                     {
                                                         Text = "Решение в смешанных стратегиях.",
                                                         Width = 200,
                                                         Dock = DockStyle.Top,
                                                     });
                var dataGridView2 = new DataGridView() { Name = RESULT_DATA_GRID_VIEW_NAME };
                resultDataGridViewPanel.Controls.Add(dataGridView2);
                if (Case.MixedStrategySolution != null)
                    ShowMatrix(dataGridView2, Case.MixedStrategySolution);
                dataGridView2.Rows[0].HeaderCell.Value = "P";
                dataGridView2.Rows[1].HeaderCell.Value = "Q";
                break;
                case SolutionType.Clear:
                resultDataGridViewPanel.Controls.Add(new Label
                                                     {
                                                         Text =
                                                             "Решение в чистых стратегиях. Цена игры = " +
                                                             Case.ClearStrategySolution,
                                                         Width = 200,
                                                         Dock = DockStyle.Top,
                                                     });
                break;
            }
        }


        public void ShowMatrix(DataGridView dataGridView, double[][] matrix)
        {
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToOrderColumns = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            int n = matrix.Length,
                m = matrix[1].Length;

            for (var i = 0; i < m; i++)
                dataGridView.Columns.Add("Column" + i, "B" + i);

            dataGridView.ColumnHeadersHeight = 20;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.Rows.Add(n);
            dataGridView.RowHeadersWidth = 70;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;


            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    dataGridView.Rows[i].Cells[j].Value = matrix[i][j].ToString(CultureInfo.InvariantCulture);
                }
            }
            for (var i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView.Rows[i].HeaderCell.Value = "A" + i;
            }
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void ClearDataGridView()
        {
            initialDataGridViewPanel.Controls.Clear();
            resultDataGridViewPanel.Controls.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = @"Excel Documents (*.csv)|*.csv",
                FileName = string.Format("export_{0}.csv",
                                         DateTime.Now.ToString(CultureInfo.InvariantCulture)
                                                 .Replace("/", "_")
                                                 .Replace(":", "_")),
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            if (Case.HasValue)
                MatrixCaseExporter.ToCsv(Case, sfd.FileName); // Here dataGridview1 is your grid view name 
        }

        private void exportLaTeX_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = @"TeX Files(*.tex)|*.tex",
                FileName = string.Format("export_{0}.tex",
                                         DateTime.Now.ToString(CultureInfo.InvariantCulture)
                                                 .Replace("/", "_")
                                                 .Replace(":", "_")),
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            if (Case.HasValue)
                MatrixCaseExporter.ToLaTeX(Case, sfd.FileName);

        }

    }
}