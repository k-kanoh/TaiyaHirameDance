namespace TaiyaHirameDance
{
    partial class FrmMain
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
            MonitoringStart = new CheckBox();
            chkTableCompare = new CheckBox();
            SaveImage = new ToolBox.Button();
            LogMonitorGroupBox = new ToolBox.GroupBox();
            txtLogPath = new ToolBox.TextBox();
            lnkLogFileDetach = new LinkLabel();
            chkLogMonitoring = new CheckBox();
            TableCompareGroupBox = new ToolBox.GroupBox();
            lnkTableConfig = new LinkLabel();
            MonitTablesListBox = new ListBox();
            SaveFile = new ToolBox.Button();
            MonitoringStop = new ToolBox.Button();
            MainPanel = new Panel();
            lblScenarioNo = new Label();
            DbSnapshot = new ToolBox.Button();
            LogSizeDisp = new Label();
            PreviewImage = new ToolBox.PictureBox();
            MenuStrip = new MenuStrip();
            LogMonitorGroupBox.SuspendLayout();
            TableCompareGroupBox.SuspendLayout();
            MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PreviewImage).BeginInit();
            SuspendLayout();
            // 
            // MonitoringStart
            // 
            MonitoringStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MonitoringStart.Appearance = Appearance.Button;
            MonitoringStart.Font = new Font("Meiryo UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            MonitoringStart.Location = new Point(296, 177);
            MonitoringStart.Name = "MonitoringStart";
            MonitoringStart.Size = new Size(110, 75);
            MonitoringStart.TabIndex = 5;
            MonitoringStart.Text = "開 始";
            MonitoringStart.TextAlign = ContentAlignment.MiddleCenter;
            MonitoringStart.UseVisualStyleBackColor = true;
            // 
            // chkTableCompare
            // 
            chkTableCompare.AutoSize = true;
            chkTableCompare.Checked = true;
            chkTableCompare.CheckState = CheckState.Checked;
            chkTableCompare.Location = new Point(10, 0);
            chkTableCompare.Name = "chkTableCompare";
            chkTableCompare.Size = new Size(75, 19);
            chkTableCompare.TabIndex = 0;
            chkTableCompare.Text = "DBの監視";
            chkTableCompare.UseVisualStyleBackColor = true;
            // 
            // SaveImage
            // 
            SaveImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveImage.Location = new Point(334, 345);
            SaveImage.Name = "SaveImage";
            SaveImage.Size = new Size(88, 24);
            SaveImage.TabIndex = 2;
            SaveImage.Text = "画像を保存";
            SaveImage.UseVisualStyleBackColor = true;
            SaveImage.Click += SaveImage_Click;
            // 
            // LogMonitorGroupBox
            // 
            LogMonitorGroupBox.AllowDrop = true;
            LogMonitorGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LogMonitorGroupBox.Controls.Add(txtLogPath);
            LogMonitorGroupBox.Controls.Add(lnkLogFileDetach);
            LogMonitorGroupBox.Controls.Add(chkLogMonitoring);
            LogMonitorGroupBox.Location = new Point(12, 41);
            LogMonitorGroupBox.Name = "LogMonitorGroupBox";
            LogMonitorGroupBox.Padding = new Padding(10, 3, 10, 10);
            LogMonitorGroupBox.Size = new Size(530, 51);
            LogMonitorGroupBox.TabIndex = 1;
            LogMonitorGroupBox.TabStop = false;
            // 
            // txtLogPath
            // 
            txtLogPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtLogPath.BorderStyle = BorderStyle.None;
            txtLogPath.Location = new Point(23, 22);
            txtLogPath.Name = "txtLogPath";
            txtLogPath.ReadOnly = true;
            txtLogPath.Size = new Size(457, 16);
            txtLogPath.TabIndex = 1;
            txtLogPath.Text = "XXXXXXXXXXXXXXXXXXX";
            // 
            // lnkLogFileDetach
            // 
            lnkLogFileDetach.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkLogFileDetach.AutoSize = true;
            lnkLogFileDetach.Location = new Point(486, 22);
            lnkLogFileDetach.Name = "lnkLogFileDetach";
            lnkLogFileDetach.Size = new Size(31, 15);
            lnkLogFileDetach.TabIndex = 2;
            lnkLogFileDetach.TabStop = true;
            lnkLogFileDetach.Text = "解除";
            // 
            // chkLogMonitoring
            // 
            chkLogMonitoring.AutoSize = true;
            chkLogMonitoring.Checked = true;
            chkLogMonitoring.CheckState = CheckState.Checked;
            chkLogMonitoring.Location = new Point(10, 0);
            chkLogMonitoring.Name = "chkLogMonitoring";
            chkLogMonitoring.Size = new Size(129, 19);
            chkLogMonitoring.TabIndex = 0;
            chkLogMonitoring.Text = "テキストファイルの監視";
            chkLogMonitoring.UseVisualStyleBackColor = true;
            // 
            // TableCompareGroupBox
            // 
            TableCompareGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TableCompareGroupBox.Controls.Add(lnkTableConfig);
            TableCompareGroupBox.Controls.Add(MonitTablesListBox);
            TableCompareGroupBox.Controls.Add(chkTableCompare);
            TableCompareGroupBox.Location = new Point(12, 98);
            TableCompareGroupBox.Name = "TableCompareGroupBox";
            TableCompareGroupBox.Padding = new Padding(10, 3, 10, 10);
            TableCompareGroupBox.Size = new Size(246, 192);
            TableCompareGroupBox.TabIndex = 2;
            TableCompareGroupBox.TabStop = false;
            // 
            // lnkTableConfig
            // 
            lnkTableConfig.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lnkTableConfig.AutoSize = true;
            lnkTableConfig.Location = new Point(202, 1);
            lnkTableConfig.Name = "lnkTableConfig";
            lnkTableConfig.Size = new Size(31, 15);
            lnkTableConfig.TabIndex = 1;
            lnkTableConfig.TabStop = true;
            lnkTableConfig.Text = "設定";
            // 
            // MonitTablesListBox
            // 
            MonitTablesListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MonitTablesListBox.Font = new Font("メイリオ", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            MonitTablesListBox.FormattingEnabled = true;
            MonitTablesListBox.HorizontalScrollbar = true;
            MonitTablesListBox.ItemHeight = 17;
            MonitTablesListBox.Location = new Point(13, 22);
            MonitTablesListBox.Name = "MonitTablesListBox";
            MonitTablesListBox.SelectionMode = SelectionMode.None;
            MonitTablesListBox.Size = new Size(220, 157);
            MonitTablesListBox.TabIndex = 2;
            // 
            // SaveFile
            // 
            SaveFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveFile.Location = new Point(428, 345);
            SaveFile.Name = "SaveFile";
            SaveFile.Size = new Size(114, 24);
            SaveFile.TabIndex = 3;
            SaveFile.Text = "ファイルを追加";
            SaveFile.UseVisualStyleBackColor = true;
            SaveFile.Click += SaveFile_Click;
            // 
            // MonitoringStop
            // 
            MonitoringStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MonitoringStop.Font = new Font("Meiryo UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            MonitoringStop.Location = new Point(407, 177);
            MonitoringStop.Name = "MonitoringStop";
            MonitoringStop.Size = new Size(110, 75);
            MonitoringStop.TabIndex = 6;
            MonitoringStop.Text = "終 了";
            MonitoringStop.UseVisualStyleBackColor = true;
            // 
            // MainPanel
            // 
            MainPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainPanel.Controls.Add(lblScenarioNo);
            MainPanel.Controls.Add(DbSnapshot);
            MainPanel.Controls.Add(LogSizeDisp);
            MainPanel.Controls.Add(MonitoringStop);
            MainPanel.Controls.Add(MonitoringStart);
            MainPanel.Controls.Add(LogMonitorGroupBox);
            MainPanel.Controls.Add(TableCompareGroupBox);
            MainPanel.Location = new Point(0, 27);
            MainPanel.Name = "MainPanel";
            MainPanel.Padding = new Padding(8, 2, 0, 0);
            MainPanel.Size = new Size(554, 312);
            MainPanel.TabIndex = 1;
            // 
            // lblScenarioNo
            // 
            lblScenarioNo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblScenarioNo.Font = new Font("Arial", 24F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblScenarioNo.Location = new Point(12, 2);
            lblScenarioNo.Name = "lblScenarioNo";
            lblScenarioNo.Size = new Size(530, 36);
            lblScenarioNo.TabIndex = 0;
            lblScenarioNo.Text = "ScenarioNo";
            lblScenarioNo.UseMnemonic = false;
            // 
            // DbSnapshot
            // 
            DbSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DbSnapshot.Location = new Point(296, 146);
            DbSnapshot.Name = "DbSnapshot";
            DbSnapshot.Size = new Size(221, 30);
            DbSnapshot.TabIndex = 4;
            DbSnapshot.Text = "DBスナップショット";
            DbSnapshot.UseVisualStyleBackColor = true;
            DbSnapshot.Click += DbSnapshot_Click;
            // 
            // LogSizeDisp
            // 
            LogSizeDisp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LogSizeDisp.Location = new Point(342, 95);
            LogSizeDisp.Name = "LogSizeDisp";
            LogSizeDisp.Size = new Size(200, 15);
            LogSizeDisp.TabIndex = 3;
            LogSizeDisp.TextAlign = ContentAlignment.MiddleRight;
            // 
            // PreviewImage
            // 
            PreviewImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PreviewImage.Location = new Point(0, 0);
            PreviewImage.Name = "PreviewImage";
            PreviewImage.Size = new Size(554, 339);
            PreviewImage.SizeMode = PictureBoxSizeMode.StretchImage;
            PreviewImage.TabIndex = 13;
            PreviewImage.TabStop = false;
            PreviewImage.Visible = false;
            // 
            // MenuStrip
            // 
            MenuStrip.BackColor = Color.Transparent;
            MenuStrip.Location = new Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Size = new Size(554, 24);
            MenuStrip.TabIndex = 0;
            MenuStrip.Text = "Menu";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(554, 381);
            Controls.Add(MainPanel);
            Controls.Add(SaveImage);
            Controls.Add(SaveFile);
            Controls.Add(PreviewImage);
            Controls.Add(MenuStrip);
            KeyPreview = true;
            MainMenuStrip = MenuStrip;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmMain";
            Title = "";
            Load += Form_Load;
            LogMonitorGroupBox.ResumeLayout(false);
            LogMonitorGroupBox.PerformLayout();
            TableCompareGroupBox.ResumeLayout(false);
            TableCompareGroupBox.PerformLayout();
            MainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PreviewImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.CheckBox MonitoringStart;
        private System.Windows.Forms.CheckBox chkTableCompare;
        private TaiyaHirameDance.ToolBox.Button SaveImage;
        private ToolBox.GroupBox LogMonitorGroupBox;
        private ToolBox.GroupBox TableCompareGroupBox;
        private System.Windows.Forms.ListBox MonitTablesListBox;
        private TaiyaHirameDance.ToolBox.Button SaveFile;
        private TaiyaHirameDance.ToolBox.Button MonitoringStop;
        private System.Windows.Forms.LinkLabel lnkTableConfig;
        private System.Windows.Forms.LinkLabel lnkLogFileDetach;
        private ToolBox.TextBox txtLogPath;
        private System.Windows.Forms.Panel MainPanel;
        private ToolBox.PictureBox PreviewImage;
        private System.Windows.Forms.Label LogSizeDisp;
        private ToolBox.Button DbSnapshot;
        private System.Windows.Forms.CheckBox chkLogMonitoring;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private Label lblScenarioNo;
    }
}