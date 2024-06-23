namespace TaiyaHirameDance.ToolBox
{
    partial class AbortableDialog
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
            Abort = new Button();
            ProgressBar = new ProgressBar();
            SuspendLayout();
            // 
            // Abort
            // 
            Abort.Location = new Point(136, 45);
            Abort.Name = "Abort";
            Abort.Size = new Size(112, 24);
            Abort.TabIndex = 1;
            Abort.Text = "キャンセル";
            Abort.UseVisualStyleBackColor = true;
            Abort.Click += Abort_Click;
            // 
            // ProgressBar
            // 
            ProgressBar.Location = new Point(15, 15);
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(354, 21);
            ProgressBar.TabIndex = 0;
            // 
            // AbortableDialog
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(384, 77);
            Controls.Add(ProgressBar);
            Controls.Add(Abort);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AbortableDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "処理中...";
            FormClosing += Dialog_FormClosing;
            Load += Dialog_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button Abort;
        private System.Windows.Forms.ProgressBar ProgressBar;
    }
}