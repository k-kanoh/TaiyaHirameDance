namespace TaiyaHirameDance
{
    partial class FrmColumnsViewer
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            SearchWord = new ToolBox.TextBox();
            dgvColumns = new ToolBox.DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column13 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column12 = new DataGridViewTextBoxColumn();
            ResultCountLabel = new ToolBox.ResultCountLabel();
            IsRegexMatch = new CheckBox();
            txtPasteFormat = new ToolBox.TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvColumns).BeginInit();
            SuspendLayout();
            // 
            // SearchWord
            // 
            SearchWord.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SearchWord.Location = new Point(13, 13);
            SearchWord.Name = "SearchWord";
            SearchWord.Size = new Size(1387, 23);
            SearchWord.TabIndex = 0;
            // 
            // dgvColumns
            // 
            dgvColumns.AllowUserToAddRows = false;
            dgvColumns.AllowUserToDeleteRows = false;
            dgvColumns.AllowUserToResizeRows = false;
            dgvColumns.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvColumns.ColorBunrui = true;
            dgvColumns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvColumns.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column13, Column3, Column4, Column7, Column5, Column6, Column8, Column9, Column10, Column11, Column12 });
            dgvColumns.Location = new Point(13, 42);
            dgvColumns.Name = "dgvColumns";
            dgvColumns.ReadOnly = true;
            dgvColumns.ResultCountLabel = ResultCountLabel;
            dgvColumns.RowHeadersVisible = false;
            dgvColumns.RowTemplate.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dgvColumns.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 192, 255);
            dgvColumns.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvColumns.RowTemplate.Height = 20;
            dgvColumns.ShowCellToolTips = false;
            dgvColumns.Size = new Size(1458, 667);
            dgvColumns.TabIndex = 2;
            dgvColumns.TabStop = false;
            dgvColumns.CellMouseClick += Grid_CellMouseClick;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "TableCatalog";
            Column1.HeaderText = "TableCatalog";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.DataPropertyName = "TableSchema";
            Column2.HeaderText = "スキーマ";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column13
            // 
            Column13.DataPropertyName = "Color";
            Column13.HeaderText = "Column13";
            Column13.Name = "Column13";
            Column13.ReadOnly = true;
            Column13.Resizable = DataGridViewTriState.False;
            Column13.Width = 8;
            // 
            // Column3
            // 
            Column3.DataPropertyName = "TableName";
            Column3.HeaderText = "物理テーブル";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Width = 200;
            // 
            // Column4
            // 
            Column4.DataPropertyName = "TableLogicalName";
            Column4.HeaderText = "論理テーブル";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Width = 200;
            // 
            // Column7
            // 
            Column7.DataPropertyName = "OrdinalPosition";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            Column7.DefaultCellStyle = dataGridViewCellStyle1;
            Column7.HeaderText = "No";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Width = 30;
            // 
            // Column5
            // 
            Column5.DataPropertyName = "ColumnName";
            Column5.HeaderText = "物理カラム";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Width = 200;
            // 
            // Column6
            // 
            Column6.DataPropertyName = "LogicalName";
            Column6.HeaderText = "論理カラム";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Width = 200;
            // 
            // Column8
            // 
            Column8.DataPropertyName = "ColumnDefault";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column8.DefaultCellStyle = dataGridViewCellStyle2;
            Column8.HeaderText = "デフォルト";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            // 
            // Column9
            // 
            Column9.DataPropertyName = "IsNullable";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column9.DefaultCellStyle = dataGridViewCellStyle3;
            Column9.HeaderText = "Null";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            Column9.Width = 50;
            // 
            // Column10
            // 
            Column10.DataPropertyName = "DataTypeUpper";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column10.DefaultCellStyle = dataGridViewCellStyle4;
            Column10.HeaderText = "型情報";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            // 
            // Column11
            // 
            Column11.DataPropertyName = "KeyPosition";
            Column11.HeaderText = "KeyPosition";
            Column11.Name = "Column11";
            Column11.ReadOnly = true;
            Column11.Visible = false;
            // 
            // Column12
            // 
            Column12.DataPropertyName = "Description";
            Column12.HeaderText = "備考(A5MK2独自)";
            Column12.Name = "Column12";
            Column12.ReadOnly = true;
            Column12.Width = 200;
            // 
            // ResultCountLabel
            // 
            ResultCountLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ResultCountLabel.Location = new Point(1321, 712);
            ResultCountLabel.Name = "ResultCountLabel";
            ResultCountLabel.Size = new Size(150, 15);
            ResultCountLabel.TabIndex = 5;
            // 
            // IsRegexMatch
            // 
            IsRegexMatch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            IsRegexMatch.Appearance = Appearance.Button;
            IsRegexMatch.Location = new Point(1401, 13);
            IsRegexMatch.Name = "IsRegexMatch";
            IsRegexMatch.Size = new Size(70, 23);
            IsRegexMatch.TabIndex = 1;
            IsRegexMatch.Text = "正規表現";
            IsRegexMatch.TextAlign = ContentAlignment.MiddleCenter;
            IsRegexMatch.UseVisualStyleBackColor = true;
            // 
            // txtPasteFormat
            // 
            txtPasteFormat.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtPasteFormat.Location = new Point(196, 715);
            txtPasteFormat.Name = "txtPasteFormat";
            txtPasteFormat.Size = new Size(450, 23);
            txtPasteFormat.TabIndex = 4;
            txtPasteFormat.Text = "[%4%.%7%] (%3%.%6%)";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(13, 718);
            label1.Name = "label1";
            label1.Size = new Size(182, 15);
            label1.TabIndex = 3;
            label1.Text = "マウスホイール押下時の貼り付け書式";
            // 
            // FrmColumnsViewer
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1484, 761);
            Controls.Add(label1);
            Controls.Add(txtPasteFormat);
            Controls.Add(IsRegexMatch);
            Controls.Add(ResultCountLabel);
            Controls.Add(dgvColumns);
            Controls.Add(SearchWord);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmColumnsViewer";
            Padding = new Padding(10, 10, 10, 20);
            Text = "ColumnsViewer";
            Title = "ColumnsViewer";
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)dgvColumns).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolBox.TextBox SearchWord;
        private ToolBox.DataGridView dgvColumns;
        private ToolBox.ResultCountLabel ResultCountLabel;
        private System.Windows.Forms.CheckBox IsRegexMatch;
        private ToolBox.TextBox txtPasteFormat;
        private Label label1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column13;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column12;
    }
}