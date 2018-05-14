namespace Konvolucio.MJBL180509
{
    partial class MainViewControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.knvDataGridView1 = new Konvolucio.MJBL180509.Controls.KnvDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.knvDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // knvDataGridView1
            // 
            this.knvDataGridView1.AllowUserToAddRows = false;
            this.knvDataGridView1.AllowUserToDeleteRows = false;
            this.knvDataGridView1.AllowUserToResizeRows = false;
            this.knvDataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.knvDataGridView1.BackgroundText = "Húzz ide  egy *mes, *typ  vagy  *csv fájlt  a megnyitásohoz.";
            this.knvDataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.knvDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.knvDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.knvDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.knvDataGridView1.Name = "knvDataGridView1";
            this.knvDataGridView1.ReadOnly = true;
            this.knvDataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.knvDataGridView1.ShowCellToolTips = false;
            this.knvDataGridView1.ShowEditingIcon = false;
            this.knvDataGridView1.ShowRowErrors = false;
            this.knvDataGridView1.Size = new System.Drawing.Size(674, 303);
            this.knvDataGridView1.TabIndex = 0;
            this.knvDataGridView1.VirtualMode = true;
            this.knvDataGridView1.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.knvDataGridView1_CellValueNeeded);
            this.knvDataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.knvDataGridView1_DataBindingComplete);
            // 
            // MainViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.knvDataGridView1);
            this.Name = "MainViewControl";
            this.Size = new System.Drawing.Size(674, 303);
            ((System.ComponentModel.ISupportInitialize)(this.knvDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.KnvDataGridView knvDataGridView1;
    }
}
