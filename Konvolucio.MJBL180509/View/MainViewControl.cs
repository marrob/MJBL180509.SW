
namespace Konvolucio.MJBL180509
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using Konvolucio.MJBL180509.Controls;

    public interface IMainViewControl
    {
        void FillContent(string[,] table, int row, int column);
        void UpdateContent(string[,] table, int row, int column);
        bool AlwaysShowLastRecord { get; set; }
    }

    public partial class MainViewControl : UserControl, IMainViewControl
    {

        public  bool AlwaysShowLastRecord { get; set; }

        string[,] _table;

        int _saveFirstCoulmnDisplayCoulmn = 0;
        int _saveCoulmnIndex = 0;
        int _saveFirstDisplayRow = 0;
        int _saveRowIndex = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewControl()
        {
            InitializeComponent();
            knvDataGridView1.KnvDoubleBuffered(true);
            knvDataGridView1.RowPrePaint += KnvDataGridView1_RowPrePaint;
        }

        /// <summary>
        /// Fill Content
        /// </summary>
        public void FillContent(string[,] table, int row, int column)
        {
            knvDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            ResetDgvStatew();

            TableUpdate(table, row, column);

            knvDataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        /// <summary>
        /// Update Content
        /// </summary>
        public void UpdateContent(string[,] table, int row, int column)
        {

            knvDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            SaveDgvState();

            TableUpdate(table, row, column);

            RestoreDgvStatew();

            /*Új elem hozzáadásakor az utolsó sorba lép a Data Gridben.. oda teszi az új elemet...*/
            if (AlwaysShowLastRecord && knvDataGridView1.RowCount > 0)
                knvDataGridView1.FirstDisplayedScrollingRowIndex = knvDataGridView1.RowCount - 1;

            knvDataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

        }

        /// <summary>
        /// SaveDgvState
        /// </summary>
        void SaveDgvState()
        {
            if (knvDataGridView1.CurrentCell != null)
            {
                _saveRowIndex = knvDataGridView1.CurrentCell.RowIndex;
                _saveCoulmnIndex = knvDataGridView1.CurrentCell.ColumnIndex;
            }

            if (knvDataGridView1.Rows.Count > 0 && knvDataGridView1.FirstDisplayedCell != null)
            {
                _saveFirstDisplayRow = knvDataGridView1.FirstDisplayedCell.RowIndex;
            }

            if (knvDataGridView1.Columns.Count > 0 && knvDataGridView1.FirstDisplayedCell != null)
            {
                _saveFirstCoulmnDisplayCoulmn = knvDataGridView1.FirstDisplayedCell.ColumnIndex;
            }
        }

        /// <summary>
        /// Table Update
        /// </summary>
        void TableUpdate(string[,] table, int row, int column)
        {
            knvDataGridView1.SuspendLayout();
            knvDataGridView1.Hide();

            knvDataGridView1.ColumnHeadersVisible = false;


            _table = table;
            knvDataGridView1.ColumnCount = column;
            knvDataGridView1.RowCount = row;
            knvDataGridView1.RowHeadersWidth = 40;

            if (row > 4)
            {
                for (int i = 0; i < knvDataGridView1.ColumnCount; i++)
                {
                    var header = (i + 1).ToString();
                    header += "\r\n";
                    header += (table[0, i] != null) ? (table[0, i].ToString()) : ("");
                    header += "\r\n";
                    header += (table[1, i] != null) ? (table[1, i].ToString()) : ("");
                    header += "\r\n";
                    header += (table[2, i] != null) ? (table[2, i].ToString()) : ("");
                    header += "\r\n";
                    header += (table[3, i] != null) ? (table[3, i].ToString()) : ("");
                    knvDataGridView1.Columns[i].HeaderCell.Value = header;
                }
            }
            else
            {
                for (int i = 0; i < knvDataGridView1.ColumnCount; i++)
                    knvDataGridView1.Columns[i].HeaderCell.Value = (i + 1).ToString();
            }

            foreach (DataGridViewColumn col in knvDataGridView1.Columns)
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn dgvcolumn in knvDataGridView1.Columns)
                dgvcolumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            knvDataGridView1.ColumnHeadersVisible = true;

            knvDataGridView1.Show();
            knvDataGridView1.ResumeLayout();
        }

        /// <summary>
        /// RestoreDgvStatew
        /// </summary>
        void RestoreDgvStatew()
        {
            if (_saveRowIndex < knvDataGridView1.Rows.Count && _saveCoulmnIndex < knvDataGridView1.Columns.Count)
            {
                knvDataGridView1.CurrentCell = knvDataGridView1.Rows[_saveRowIndex].Cells[_saveCoulmnIndex];

                if(knvDataGridView1.Rows.Count > _saveFirstDisplayRow)
                     knvDataGridView1.FirstDisplayedScrollingRowIndex = _saveFirstDisplayRow;

                knvDataGridView1.FirstDisplayedScrollingColumnIndex = _saveFirstCoulmnDisplayCoulmn;
            }
        }

        /// <summary>
        /// ResetDgvStatew
        /// </summary>
        void ResetDgvStatew()
        {
             _saveFirstCoulmnDisplayCoulmn = 0;
             _saveCoulmnIndex = 0;
             _saveFirstDisplayRow = 0;
             _saveRowIndex = 0;

            RestoreDgvStatew();
        }

        /// <summary>
        /// RowPrePaint
        /// </summary>
        private void KnvDataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            if (knvDataGridView1.Rows[e.RowIndex].Cells[0].Value == null) return;

            var result = knvDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().ToUpper().Trim();

            if (result == "KO")
                knvDataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
            else
            if (result == "OK")
                knvDataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
            else
                knvDataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

        }

        /// <summary>
        /// CellValueNeeded
        /// </summary>
        private void knvDataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = _table[e.RowIndex, e.ColumnIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        private void knvDataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
    }
}
