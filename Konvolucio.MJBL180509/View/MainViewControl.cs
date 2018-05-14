
namespace Konvolucio.MJBL180509
{

    using System;
    using System.Data;
    using System.Windows.Forms;
    using Konvolucio.MJBL180509.Controls;

    public interface IMainViewControl
    {

        int RowCount { get; set; }
        int CoulumnCount { get; set; }
        string[,] Table { get; set; }
    }


    public partial class MainViewControl : UserControl, IMainViewControl
    {

        public int RowCount
        {
            get
            {
                return knvDataGridView1.RowCount;
            }

            set
            {
                knvDataGridView1.RowCount = value;
            }
        }

        public int CoulumnCount
        {
            get => knvDataGridView1.ColumnCount;
            set => knvDataGridView1.ColumnCount = value;
        }

        public string[,] Table { get; set; }

        /// <summary>
        /// Konstructor
        /// </summary>
        public MainViewControl()
        {
            InitializeComponent();
            //knvDataGridView1.KnvDoubleBuffered(true);
            //knvDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void knvDataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //knvDataGridView1.RowHeadersWidth = 80;

            //for (int row = 0; row < knvDataGridView1.RowCount; row++)
            //    knvDataGridView1.Rows[row].HeaderCell.Value = row.ToString();

            //foreach (DataGridViewColumn column in knvDataGridView1.Columns)
            //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void knvDataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = Table[e.RowIndex, e.ColumnIndex];
        }
    }
}
