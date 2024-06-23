namespace TaiyaHirameDance
{
    partial class FrmTableSelection
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dgvTables = new ToolBox.DataGridView();
            Column1 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvTables).BeginInit();
            SuspendLayout();
            // 
            // dgvTables
            // 
            dgvTables.AllowUserToAddRows = false;
            dgvTables.AllowUserToDeleteRows = false;
            dgvTables.AllowUserToResizeRows = false;
            dgvTables.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTables.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9 });
            dgvTables.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvTables.Location = new Point(12, 12);
            dgvTables.Name = "dgvTables";
            dgvTables.RowHeadersVisible = false;
            dgvTables.RowTemplate.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dgvTables.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 192, 255);
            dgvTables.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvTables.RowTemplate.Height = 20;
            dgvTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTables.ShowCellToolTips = false;
            dgvTables.Size = new Size(1160, 737);
            dgvTables.TabIndex = 0;
            dgvTables.TabStop = false;
            dgvTables.CellEndEdit += Grid_CellEndEdit;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "IsChecked";
            Column1.HeaderText = "";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.True;
            Column1.SortMode = DataGridViewColumnSortMode.Automatic;
            Column1.Width = 30;
            // 
            // Column2
            // 
            Column2.DataPropertyName = "SchemaName";
            Column2.HeaderText = "スキーマ";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.DataPropertyName = "Color";
            Column3.HeaderText = "";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 8;
            // 
            // Column4
            // 
            Column4.DataPropertyName = "TableName";
            Column4.HeaderText = "物理テーブル";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Width = 250;
            // 
            // Column5
            // 
            Column5.DataPropertyName = "TableComment";
            Column5.HeaderText = "論理テーブル";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Width = 250;
            // 
            // Column6
            // 
            Column6.DataPropertyName = "RealRowCount";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "#,##0";
            Column6.DefaultCellStyle = dataGridViewCellStyle1;
            Column6.HeaderText = "件数";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Width = 70;
            // 
            // Column7
            // 
            Column7.DataPropertyName = "WhereClause";
            Column7.HeaderText = "フィルタ (Where句)";
            Column7.Name = "Column7";
            Column7.Width = 320;
            // 
            // Column8
            // 
            Column8.DataPropertyName = "Seq";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column8.DefaultCellStyle = dataGridViewCellStyle2;
            Column8.HeaderText = "順";
            Column8.MaxInputLength = 3;
            Column8.Name = "Column8";
            Column8.Width = 30;
            // 
            // Column9
            // 
            Column9.DataPropertyName = "OmitConformity";
            Column9.HeaderText = "同一省略";
            Column9.Name = "Column9";
            Column9.Resizable = DataGridViewTriState.True;
            Column9.SortMode = DataGridViewColumnSortMode.Automatic;
            Column9.Width = 80;
            // 
            // FrmTableSelection
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1184, 761);
            Controls.Add(dgvTables);
            Margin = new Padding(4);
            Name = "FrmTableSelection";
            Text = "Main";
            Title = "Main";
            FormClosing += Form_FormClosing;
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTables).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private ToolBox.DataGridView dgvTables;
        private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewCheckBoxColumn Column9;
    }
}