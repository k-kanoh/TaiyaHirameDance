namespace TaiyaHirameDance
{
    partial class SimpleTextEditDialog
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
            TextBox = new ToolBox.TextBox();
            OK = new Button();
            Cancel = new Button();
            SuspendLayout();
            // 
            // TextBox
            // 
            TextBox.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            TextBox.Location = new Point(12, 12);
            TextBox.Name = "TextBox";
            TextBox.Size = new Size(383, 29);
            TextBox.TabIndex = 0;
            // 
            // OK
            // 
            OK.DialogResult = DialogResult.OK;
            OK.Location = new Point(500, 50);
            OK.Name = "OK";
            OK.Size = new Size(75, 23);
            OK.TabIndex = 1;
            OK.TabStop = false;
            OK.Text = "OK";
            OK.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            Cancel.DialogResult = DialogResult.Cancel;
            Cancel.Location = new Point(581, 50);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(75, 23);
            Cancel.TabIndex = 2;
            Cancel.TabStop = false;
            Cancel.Text = "Cancel";
            Cancel.UseVisualStyleBackColor = true;
            // 
            // TextInputDialog
            // 
            AcceptButton = OK;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = Cancel;
            ClientSize = new Size(407, 53);
            Controls.Add(Cancel);
            Controls.Add(OK);
            Controls.Add(TextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TextInputDialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button OK;
        private Button Cancel;
        private ToolBox.TextBox TextBox;
    }
}