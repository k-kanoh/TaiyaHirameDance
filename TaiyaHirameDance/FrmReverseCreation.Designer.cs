namespace TaiyaHirameDance
{
    partial class FrmReverseCreation
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
            AssignDropSql = new CheckBox();
            OptionGroupBox = new ToolBox.GroupBox();
            label1 = new Label();
            OptionGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // AssignDropSql
            // 
            AssignDropSql.AutoSize = true;
            AssignDropSql.Location = new Point(18, 20);
            AssignDropSql.Name = "AssignDropSql";
            AssignDropSql.Size = new Size(185, 19);
            AssignDropSql.TabIndex = 0;
            AssignDropSql.Text = "CREATE文の前にDROP文を出力";
            AssignDropSql.UseVisualStyleBackColor = true;
            // 
            // OptionGroupBox
            // 
            OptionGroupBox.Controls.Add(AssignDropSql);
            OptionGroupBox.Location = new Point(152, 99);
            OptionGroupBox.Name = "OptionGroupBox";
            OptionGroupBox.Size = new Size(220, 50);
            OptionGroupBox.TabIndex = 1;
            OptionGroupBox.TabStop = false;
            OptionGroupBox.Text = "オプション";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(107, 50);
            label1.Name = "label1";
            label1.Size = new Size(170, 15);
            label1.TabIndex = 0;
            label1.Text = "Excelファイルをドロップしてください。";
            // 
            // FrmReverseCreation
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(384, 161);
            Controls.Add(label1);
            Controls.Add(OptionGroupBox);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmReverseCreation";
            Text = "ReverseCreation";
            Title = "ReverseCreation";
            OptionGroupBox.ResumeLayout(false);
            OptionGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox AssignDropSql;
        private System.Windows.Forms.Label label1;
        private ToolBox.GroupBox OptionGroupBox;
    }
}