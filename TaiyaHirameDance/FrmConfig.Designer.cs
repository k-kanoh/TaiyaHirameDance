namespace TaiyaHirameDance
{
    partial class FrmConfig
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
            groupBox1 = new ToolBox.GroupBox();
            txtConnectionString = new ToolBox.TextBox();
            A5MK2CsvGroupBox = new ToolBox.GroupBox();
            label1 = new Label();
            groupBox2 = new ToolBox.GroupBox();
            txtIgnoreTable = new ToolBox.TextBox();
            label3 = new Label();
            txtIgnoreSchema = new ToolBox.TextBox();
            label2 = new Label();
            groupBox3 = new ToolBox.GroupBox();
            txtTableColor = new ToolBox.TextBox();
            label5 = new Label();
            groupBox1.SuspendLayout();
            A5MK2CsvGroupBox.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(txtConnectionString);
            groupBox1.Location = new Point(13, 13);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(10, 3, 10, 10);
            groupBox1.Size = new Size(558, 90);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "接続文字列";
            // 
            // txtConnectionString
            // 
            txtConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtConnectionString.Location = new Point(13, 22);
            txtConnectionString.Multiline = true;
            txtConnectionString.Name = "txtConnectionString";
            txtConnectionString.Size = new Size(532, 55);
            txtConnectionString.TabIndex = 0;
            txtConnectionString.Text = "User=*****;Password=*****;Database=*****;Server=*****;TrustServerCertificate=True";
            // 
            // A5MK2CsvGroupBox
            // 
            A5MK2CsvGroupBox.AllowDrop = true;
            A5MK2CsvGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            A5MK2CsvGroupBox.Controls.Add(label1);
            A5MK2CsvGroupBox.Location = new Point(13, 109);
            A5MK2CsvGroupBox.Name = "A5MK2CsvGroupBox";
            A5MK2CsvGroupBox.Size = new Size(558, 75);
            A5MK2CsvGroupBox.TabIndex = 1;
            A5MK2CsvGroupBox.TabStop = false;
            A5MK2CsvGroupBox.Text = "A5MK2のテーブル定義CSV";
            A5MK2CsvGroupBox.DragDrop += A5MK2CsvGroupBox_DragDrop;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(176, 35);
            label1.Name = "label1";
            label1.Size = new Size(206, 15);
            label1.TabIndex = 0;
            label1.Text = "ファイルまたはフォルダをドロップしてください。";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(txtIgnoreTable);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtIgnoreSchema);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(13, 190);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(10, 3, 10, 10);
            groupBox2.Size = new Size(558, 117);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "無視";
            // 
            // txtIgnoreTable
            // 
            txtIgnoreTable.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtIgnoreTable.Location = new Point(13, 81);
            txtIgnoreTable.Name = "txtIgnoreTable";
            txtIgnoreTable.Size = new Size(532, 23);
            txtIgnoreTable.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 63);
            label3.Name = "label3";
            label3.Size = new Size(186, 15);
            label3.TabIndex = 2;
            label3.Text = "無視するテーブル (正規表現・複数可)";
            // 
            // txtIgnoreSchema
            // 
            txtIgnoreSchema.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtIgnoreSchema.Location = new Point(13, 37);
            txtIgnoreSchema.Name = "txtIgnoreSchema";
            txtIgnoreSchema.Size = new Size(532, 23);
            txtIgnoreSchema.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 19);
            label2.Name = "label2";
            label2.Size = new Size(186, 15);
            label2.TabIndex = 0;
            label2.Text = "無視するスキーマ (正規表現・複数可)";
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(txtTableColor);
            groupBox3.Controls.Add(label5);
            groupBox3.Location = new Point(13, 313);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(10, 3, 10, 10);
            groupBox3.Size = new Size(558, 73);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "テーブル色分け";
            // 
            // txtTableColor
            // 
            txtTableColor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTableColor.Location = new Point(13, 37);
            txtTableColor.Name = "txtTableColor";
            txtTableColor.Size = new Size(532, 23);
            txtTableColor.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 19);
            label5.Name = "label5";
            label5.Size = new Size(208, 15);
            label5.TabIndex = 0;
            label5.Text = "(設定例) ^Table[1-9]+:Blue　※複数可";
            // 
            // FrmConfig
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(584, 409);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(A5MK2CsvGroupBox);
            Controls.Add(groupBox1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmConfig";
            Padding = new Padding(10, 10, 10, 20);
            Text = "Config";
            Title = "Config";
            Load += Form_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            A5MK2CsvGroupBox.ResumeLayout(false);
            A5MK2CsvGroupBox.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ToolBox.GroupBox groupBox1;
        private ToolBox.TextBox txtConnectionString;
        private ToolBox.GroupBox A5MK2CsvGroupBox;
        private System.Windows.Forms.Label label1;
        private ToolBox.GroupBox groupBox2;
        private ToolBox.TextBox txtIgnoreTable;
        private System.Windows.Forms.Label label3;
        private ToolBox.TextBox txtIgnoreSchema;
        private System.Windows.Forms.Label label2;
        private ToolBox.GroupBox groupBox3;
        private ToolBox.TextBox txtTableColor;
        private System.Windows.Forms.Label label5;
    }
}