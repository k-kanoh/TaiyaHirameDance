namespace TaiyaHirameDance
{
    partial class FrmBcpUtil
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dgvTables = new ToolBox.DataGridView();
            Column1 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            ConsoleMessage = new ToolBox.TextBox();
            cmbBcpContainer = new ToolBox.ComboBox();
            MenuStrip = new MenuStrip();
            Splitter = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)dgvTables).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Splitter).BeginInit();
            Splitter.Panel1.SuspendLayout();
            Splitter.Panel2.SuspendLayout();
            Splitter.SuspendLayout();
            SuspendLayout();
            // 
            // dgvTables
            // 
            dgvTables.AllowUserToAddRows = false;
            dgvTables.AllowUserToDeleteRows = false;
            dgvTables.AllowUserToResizeRows = false;
            dgvTables.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTables.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8 });
            dgvTables.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvTables.Location = new Point(12, 32);
            dgvTables.Name = "dgvTables";
            dgvTables.RowHeadersVisible = false;
            dgvTables.RowTemplate.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dgvTables.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 192, 255);
            dgvTables.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvTables.RowTemplate.Height = 20;
            dgvTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTables.ShowCellToolTips = false;
            dgvTables.Size = new Size(960, 325);
            dgvTables.TabIndex = 1;
            dgvTables.TabStop = false;
            dgvTables.ButtonClick += Grid_ButtonClick;
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
            Column7.DataPropertyName = "BackupedCount";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "#,##0";
            dataGridViewCellStyle2.NullValue = "-";
            Column7.DefaultCellStyle = dataGridViewCellStyle2;
            Column7.HeaderText = "バックアップ件数";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Width = 120;
            // 
            // Column8
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column8.DefaultCellStyle = dataGridViewCellStyle3;
            Column8.HeaderText = "";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Width = 60;
            // 
            // ConsoleMessage
            // 
            ConsoleMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConsoleMessage.BackColor = Color.White;
            ConsoleMessage.Location = new Point(12, 3);
            ConsoleMessage.Multiline = true;
            ConsoleMessage.Name = "ConsoleMessage";
            ConsoleMessage.ReadOnly = true;
            ConsoleMessage.ScrollBars = ScrollBars.Vertical;
            ConsoleMessage.Size = new Size(960, 156);
            ConsoleMessage.TabIndex = 0;
            // 
            // cmbBcpContainer
            // 
            cmbBcpContainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbBcpContainer.DisplayMember = "DispName";
            cmbBcpContainer.DrawMode = DrawMode.OwnerDrawFixed;
            cmbBcpContainer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBcpContainer.Font = new Font("ＭＳ ゴシック", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbBcpContainer.FormattingEnabled = true;
            cmbBcpContainer.IntegralHeight = false;
            cmbBcpContainer.Location = new Point(12, 3);
            cmbBcpContainer.MaxDropDownItems = 13;
            cmbBcpContainer.Name = "cmbBcpContainer";
            cmbBcpContainer.Size = new Size(960, 23);
            cmbBcpContainer.TabIndex = 0;
            cmbBcpContainer.ValueMember = "Key";
            cmbBcpContainer.EmptySelected += ComboBox_EmptySelected;
            cmbBcpContainer.ItemSelected += ComboBox_ItemSelected;
            // 
            // MenuStrip
            // 
            MenuStrip.BackColor = Color.Transparent;
            MenuStrip.Location = new Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Size = new Size(984, 24);
            MenuStrip.TabIndex = 0;
            MenuStrip.Text = "Menu";
            // 
            // Splitter
            // 
            Splitter.Dock = DockStyle.Fill;
            Splitter.Location = new Point(0, 24);
            Splitter.Name = "Splitter";
            Splitter.Orientation = Orientation.Horizontal;
            // 
            // Splitter.Panel1
            // 
            Splitter.Panel1.Controls.Add(dgvTables);
            Splitter.Panel1.Controls.Add(cmbBcpContainer);
            // 
            // Splitter.Panel2
            // 
            Splitter.Panel2.Controls.Add(ConsoleMessage);
            Splitter.Size = new Size(984, 537);
            Splitter.SplitterDistance = 360;
            Splitter.TabIndex = 0;
            // 
            // FrmBcpUtil
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(984, 561);
            Controls.Add(Splitter);
            Controls.Add(MenuStrip);
            Name = "FrmBcpUtil";
            Text = "BcpUtil";
            Title = "BcpUtil";
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTables).EndInit();
            Splitter.Panel1.ResumeLayout(false);
            Splitter.Panel2.ResumeLayout(false);
            Splitter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Splitter).EndInit();
            Splitter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolBox.DataGridView dgvTables;
        private ToolBox.TextBox ConsoleMessage;
        private ToolBox.ComboBox cmbBcpContainer;
        private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private MenuStrip MenuStrip;
        private SplitContainer Splitter;
    }
}