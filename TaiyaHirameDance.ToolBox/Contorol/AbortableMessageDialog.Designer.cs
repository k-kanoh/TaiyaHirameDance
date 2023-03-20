namespace TaiyaHirameDance.ToolBox
{
    partial class AbortableMessageDialog<T>
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
            Message = new Label();
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
            // Message
            // 
            Message.Location = new Point(12, 9);
            Message.Name = "Message";
            Message.Size = new Size(360, 33);
            Message.TabIndex = 2;
            Message.Text = "処理中です...";
            Message.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AbortableMessageDialog
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(384, 77);
            Controls.Add(Message);
            Controls.Add(Abort);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AbortableMessageDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "処理中...";
            FormClosing += Dialog_FormClosing;
            Load += Dialog_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button Abort;
        private Label Message;
    }
}