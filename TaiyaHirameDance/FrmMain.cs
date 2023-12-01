using TaiyaHirameDance.Domain;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class FrmMain : BaseForm
    {
        private string __no;

        public string No
        {
            get => __no;
            set
            {
                if (__no == value)
                    return;

                __no = value;
                lblRecipeNo.Text = __no;
                ImagePreviewOff();
                Reload();
            }
        }

        public Evidence Evidence => (Evidence)DataContext;

        private FileInfo _logFile;
        private long _logSizeOnStart;
        private List<DapperQueryContainer> _containers = [];

        public FrmMain()
        {
            InitializeComponent();
            RegistEvents();
        }

        private void Form_Load(object sender, EventArgs eventArgs)
        {
            No = RecipeUtil.GetRecipeList(Common.WorkDir).FirstOrDefault() ?? "1";

            Task.Run(async () =>
            {
                while (true)
                {
                    if (!chkLogMonitoring.Checked)
                        continue;

                    if (MonitoringStart.Checked)
                        Evidence.LogIncrease = _logFile.LengthSafety() - _logSizeOnStart;

                    ActionInvoke(() => LogSizeDisp.Text = $"{_logFile.LengthSafety():#,##0} ({Evidence.LogIncrease:+#,##0;-#,##0})");

                    await Task.Delay(100);
                }
            });

            SetupMenuStrip();
        }

        /// <summary>
        /// メニューの設定
        /// </summary>
        private void SetupMenuStrip()
        {
            bool unMonitoring() => !MonitoringStart.Checked;

            var menuTemplate = new MenuStripTemplate { "ファイル(&F)", "編集(&E)", "シナリオ(&S)", "ツール(&T)" };
            menuTemplate[0].Add("作業フォルダを選択", ChangeMainDirectory, unMonitoring);
            menuTemplate[0].Add("ファイルを出力", null, unMonitoring);
            menuTemplate[1].Add("シナリオ名の変更", CallRecipeRenameDialog, unMonitoring, Keys.F2);
            menuTemplate[1].Add("エビデンスを全削除", ClearAllEvidence, unMonitoring);
            menuTemplate[2].Add("新規シナリオ", Insert, unMonitoring, Keys.Insert);
            menuTemplate[3].Add("テーブル定義書からCREATE文逆作成(&R)", ShowModeless<FrmReverseCreation>);
            menuTemplate[3].Add("カラム名確認(&V)", ShowModeless<FrmColumnsViewer>);
            menuTemplate[3].Add("-");
            menuTemplate[3].Add("設定(&C)", ShowModeless<FrmConfig>);
            menuTemplate[3].Add("[x]常に手前に表示(&T)", () => TopMost = !TopMost, null, () => TopMost);
            MenuStrip.Items.AddRange(menuTemplate.ConvertToStripItem());
        }

        private void ChangeMainDirectory()
        {
            var dir = Common.MainDir.FullName;
            if (PickFolderDialog(ref dir))
            {
                using (Executing())
                {
                    Setting.ChangeMainDirectory(dir);
                    No = RecipeUtil.GetRecipeList(Common.WorkDir).FirstOrDefault() ?? "1";
                }
            }
        }

        protected override void Save()
        {
            var save = GetMergedNewDataContext<Evidence>();
            YamlUtil.YamlSave(Common.WorkDir, No, save);

            _ = DispMessageInTitleBarAsync("現在の状態を保存しました");
        }

        protected override void Reload()
        {
            DataContext = YamlUtil.YamlLoad<Evidence>(Common.WorkDir, No) ?? new Evidence();
            Title = Common.MainDir.FullName;

            SetControlValueFromDataContext();
            RefreshMonitTablesListBox();
            RefreshImageLink();
        }

        private void SaveAndReload()
        {
            Save();
            Reload();
        }

        private void MonitoringStart_CheckedChanged(object sender, EventArgs e)
        {
            MonitoringStart.CheckedChanged -= MonitoringStart_CheckedChanged;

            try
            {
                using (Executing())
                {
                    if (MonitoringStart.Checked)
                    {
                        RefreshMainPanelAtMonitoringStatusChanging();

                        _logSizeOnStart = _logFile.LengthSafety();

                        if (chkTableCompare.Checked)
                        {
                            _containers = DbQueryUtil.CreateQueryContainer(Evidence.CompareTables.Where(x => x.Checked).ToList());
                            DbQueryUtil.FetchByQuery(_containers);
                        }
                    }
                    else
                    {
                        var hash = EvidenceUtil.SaveText(Common.WorkDir, _logFile, _logSizeOnStart);
                        Evidence.FileAdd(hash, _logFile?.Name);

                        if (chkTableCompare.Checked && _containers.Any())
                        {
                            DbQueryUtil.FetchByQuery2(_containers);
                            using (var exporter = CompareExport.Create())
                            {
                                var rowCount = _containers.Sum(x => x.CompareRows.Count);
                                ShowAbortableDialog((ts, prg) => exporter.Export(_containers, ts.Token, prg), rowCount);

                                var bytes = exporter.GetOutputBinary();
                                hash = EvidenceUtil.SaveBinary(Common.WorkDir, bytes, ".xlsx");
                                Evidence.FileAdd(hash, "DB比較.xlsx");
                            }
                        }

                        RefreshMainPanelAtMonitoringStatusChanging();
                    }
                }

                SaveAndReload();
            }
            catch (OperationCanceledException)
            {
                MonitoringStart.Checked = true;
                MonitoringStart.Focus();
            }
            catch (ArgumentOutOfRangeException)
            {
                ErrorMessageBox("10万件を超えるデータはエクスポートできません。");
            }
            finally
            {
                MonitoringStart.CheckedChanged += MonitoringStart_CheckedChanged;
            }
        }

        private void DbSnapshot_Click(object sender, EventArgs e)
        {
            ShowDialog<FrmTableSelection>(Evidence, form => { form.No = No; form.Mode = TableSelectionMode.SnapshotTables; }, null);

            if (!Evidence.SnapshotTables.Where(x => x.Checked).Any())
                return;

            using (Executing())
            {
                try
                {
                    var containers = DbQueryUtil.CreateQueryContainer(Evidence.SnapshotTables.Where(x => x.Checked).ToList());
                    DbQueryUtil.FetchByQuery(containers);

                    using (var exporter = SnapshotExport.Create())
                    {
                        var rowCount = containers.Sum(x => x.Values.Count);
                        ShowAbortableDialog((ts, prg) => exporter.Export(containers, ts.Token, prg), rowCount);

                        var bytes = exporter.GetOutputBinary();
                        var hash = EvidenceUtil.SaveBinary(Common.WorkDir, bytes, ".xlsx");
                        Evidence.FileAdd(hash, "DBスナップショット.xlsx");
                        SaveAndReload();
                    }
                }
                catch (OperationCanceledException)
                {
                    // 処理なし
                }
                catch (ArgumentOutOfRangeException)
                {
                    ErrorMessageBox("10万件を超えるデータはエクスポートできません。");
                }
            }
        }

        /// <summary>
        /// 画像プレビュー表示
        /// </summary>
        private void ImagePreview(Image image)
        {
            if (image == null)
            {
                ImagePreviewOff();
                return;
            }

            MainPanel.Visible = false;
            PreviewImage.Visible = true;
            PreviewImage.SetImage(image);
        }

        /// <summary>
        /// 画像プレビュー表示をキャンセル
        /// </summary>
        private void ImagePreviewOff()
        {
            MainPanel.Visible = true;
            PreviewImage.Visible = false;
            PreviewImage.SetImage(null);
        }

        /// <summary>
        /// 「画像を保存」をクリック
        /// </summary>
        private void SaveImage_Click(object sender, EventArgs e)
        {
            var image = Clipboard.GetImage();

            if (image == null)
                return;

            var hash = EvidenceUtil.SaveImage(Common.WorkDir, image);
            Evidence.FileAdd(hash, "スクリーンショット.png");
            SaveAndReload();
        }

        /// <summary>
        /// 「ファイルを追加」をクリック
        /// </summary>
        private void SaveFile_Click(object sender, EventArgs e)
        {
            if (!PickMultiFileDialog(out var files))
                return;

            foreach (var finfo in files.Select(x => new FileInfo(x)))
            {
                var hash = EvidenceUtil.SaveFile(Common.WorkDir, finfo);
                Evidence.FileAdd(hash, desc: finfo.Name);
            }

            SaveAndReload();
        }

        /// <summary>
        /// エビデンスのリンクラベルを生成して画面左下に並べる
        /// </summary>
        private void RefreshImageLink()
        {
            foreach (var con in Controls.OfType<ToolBox.LinkLabel>().ToList())
                Controls.Remove(con);

            var files = Evidence.Files.Where(x => !x.Deleted);

            int i = files.Count();
            foreach (var file in files.Reverse())
            {
                var link = new ToolBox.LinkLabel()
                {
                    AutoSize = true,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                    Font = new Font("Segoe Script", 12F),
                    Location = new Point(9 + 18 * (i - 1), 340),
                    Text = $"{i--}"
                };

                link.MouseHover += (s, e) => ImagePreview(EvidenceUtil.LoadImage(Common.WorkDir, file.Hash));
                link.LinkClicked += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        EvidenceUtil.OpenOnApplication(Common.WorkDir, file.Hash);
                };

                var menuTemplate = new MenuStripTemplate();

                menuTemplate.Add("キャプションを編集", () =>
                {
                    using (var dlg = new TextInputDialog())
                    {
                        dlg.Text = file.Description;

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            file.Description = dlg.Text;
                            SaveAndReload();
                        }
                    }
                });

                menuTemplate.Add("-");
                menuTemplate.Add("削除", () =>
                {
                    if (!InfoMessageBoxOKCancel("削除します。よろしいですか?"))
                        return;

                    file.Deleted = true;
                    ImagePreviewOff();
                    RefreshImageLink();
                });

                var menu = new ContextMenuStrip();
                menu.Items.AddRange(menuTemplate.ConvertToStripItem());
                menu.Opened += (s, e) => menuTemplate.OnOpenedMethod(menu.Items);
                menu.Closed += (s, e) => menuTemplate.OnClosedMethod(menu.Items);
                link.ContextMenuStrip = menu;

                ToolTip.SetToolTip(link, $"{file.Description} ({file.Hash.Substring(0, 6)}) {file.RegistAt:M/d HH:mm:ss}");
                Controls.Add(link);
            }
        }

        private void ClearAllEvidence()
        {
            if (MonitoringStart.Checked)
                return;

            if (!InfoMessageBoxOKCancel("エビデンスを全削除します。よろしいですか?"))
                return;

            foreach (var file in Evidence.Files)
                EvidenceUtil.DeleteFile(Common.WorkDir, file.Hash);

            Evidence.Files.Clear();
            SaveAndReload();
        }

        private void RefreshMainPanelAtMonitoringStatusChanging()
        {
            MonitoringStart.AutoCheck = !MonitoringStart.Checked;

            var noDisableControls = new Control[] { lblRecipeNo, LogSizeDisp, MonitoringStart, MonitoringStop, DbSnapshot };
            foreach (var control in MainPanel.ManyControls(containChild: false).Where(x => !noDisableControls.Contains(x)))
                control.Enabled = !MonitoringStart.Checked;
        }

        /// <summary>
        /// DB監視対象テーブル表示窓の更新
        /// </summary>
        private void RefreshMonitTablesListBox()
        {
            var items = (from x in Evidence.CompareTables where x.Checked select x.TableName).ToArray();

            MonitTablesListBox.Items.Clear();
            MonitTablesListBox.Items.AddRange(items);
        }

        /// <summary>
        /// コントロールイベントに処理をセット
        /// </summary>
        private void RegistEvents()
        {
            // 「☑テキストファイルの監視」ON/OFF切替
            chkLogMonitoring.CheckStateChanged += (s, e) =>
            {
                LogMonitorGroupBox.AllowDrop = chkLogMonitoring.Checked;
                txtLogPath.Enabled = chkLogMonitoring.Checked;
                lnkLogFileDetach.Visible = chkLogMonitoring.Checked;
                LogSizeDisp.Visible = chkLogMonitoring.Checked;
            };

            // 監視対象ファイルをドロップ
            LogMonitorGroupBox.DragDrop += (s, e) =>
            {
                Evidence.LogIncrease = 0;
                txtLogPath.Text = LogMonitorGroupBox.DroppedPath[0];
                lnkLogFileDetach.Visible = true;
            };

            // 監視対象ファイルのパスが変更された
            txtLogPath.TextChanged += (s, e) =>
            {
                LogSizeDisp.Text = "";
                _logFile = txtLogPath.Text.Val() ? new FileInfo(txtLogPath.Text) : null;
            };

            // 監視対象ファイルの「解除」クリック
            lnkLogFileDetach.LinkClicked += (s, e) => txtLogPath.Clear();

            // 「☑DBの監視」ON/OFF切替
            chkTableCompare.CheckStateChanged += (s, e) => MonitTablesListBox.Enabled = chkTableCompare.Checked;

            // DB監視の「設定」クリック
            lnkTableConfig.Click += (s, e) =>
            {
                ShowModeless<FrmTableSelection>(Evidence, form => { form.No = No; form.Mode = TableSelectionMode.CompareTables; }, () => RefreshMonitTablesListBox());
            };

            // クリップボードの画像をプレビュー
            SaveImage.MouseHover += (s, e) => ImagePreview(Clipboard.GetImage());
            SaveImage.MouseLeave += (s, e) => ImagePreviewOff();

            // 画像プレビューをキャンセル
            PreviewImage.Click += (s, e) => ImagePreviewOff();
            PreviewImage.MouseLeave += (s, e) => ImagePreviewOff();

            // 「(モニタリングの)終了」をクリック
            MonitoringStop.Click += (s, e) => MonitoringStart.Checked = false;

            // 「(モニタリングの)開始」ON/OFF切替
            MonitoringStart.CheckedChanged += MonitoringStart_CheckedChanged;

            // キー入力
            KeyUp += Form_KeyUp;
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (MonitoringStart.Checked)
                return;

            var res = RecipeUtil.GetRecipeNoByKeyInput(No, e.KeyData);

            if (res != null)
                No = res;
        }

        private void Insert()
        {
            if (MonitoringStart.Checked)
                return;

            No = RecipeUtil.GetNewRecipeNo();
        }

        private void CallRecipeRenameDialog()
        {
            if (MonitoringStart.Checked)
                return;

            using (var dlg = new TextInputDialog())
            {
                dlg.Text = No;

                dlg.FormClosing += (s, e) =>
                {
                    if (No != dlg.Text && dlg.DialogResult == DialogResult.OK)
                    {
                        if (!YamlUtil.YamlRename(Common.WorkDir, No, dlg.Text))
                        {
                            e.Cancel = true;
                            dlg.SetTextBoxError("その名前にはリネームできません");
                        }
                    }
                    else
                    {
                        dlg.DialogResult = DialogResult.Cancel;
                    }
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                    No = dlg.Text;
            }
        }
    }
}
