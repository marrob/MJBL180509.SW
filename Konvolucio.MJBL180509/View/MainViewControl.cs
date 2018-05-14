
namespace Konvolucio.MJBL180509
{
    using System;
    using System.Data;
    using System.Windows.Forms;
    using Konvolucio.MJBL180509.Controls;

    public interface IMainViewControl
    {
        void Update(string[,] table, int row, int column);
    }

    public partial class MainViewControl : UserControl, IMainViewControl
    {
        string[,] _table;

        public void Update(string[,] table, int row, int column)
        {
            knvDataGridView1.SuspendLayout();
            knvDataGridView1.Hide();

            knvDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            knvDataGridView1.ColumnHeadersVisible = false;

            _table = table;
            knvDataGridView1.ColumnCount = column;
            knvDataGridView1.RowCount = row;
            knvDataGridView1.RowHeadersWidth = 40;

            for (int i = 0; i < knvDataGridView1.ColumnCount; i++)
                knvDataGridView1.Columns[i].HeaderCell.Value = i.ToString();

            knvDataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            foreach (DataGridViewColumn dgvcolumn in knvDataGridView1.Columns)
                dgvcolumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            knvDataGridView1.ColumnHeadersVisible = true;

            knvDataGridView1.Show();
            knvDataGridView1.ResumeLayout();
        }

        public MainViewControl()
        {
            InitializeComponent();
            knvDataGridView1.KnvDoubleBuffered(true);
        }

        private void knvDataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = _table[e.RowIndex, e.ColumnIndex];
        }
    }
}
