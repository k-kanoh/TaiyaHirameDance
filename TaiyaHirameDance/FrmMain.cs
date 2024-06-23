using TaiyaHirameDance.Domain;
using TaiyaHirameDance.Domain.ExcelExporter;
using TaiyaHirameDance.Domain.FileComparer;
using TaiyaHirameDance.Domain.MainModels;
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
                lblScenarioNo.Text = __no;
                ImagePreviewOff();
                Reload();
            }
        }

        public Scenario Scenario => (Scenario)DataContext;

        private FileInfo _logFile;
        private long _logSizeOnStart;
        private DapperQueryContainers _containers;

        public FrmMain()
        {
            InitializeComponent();
            RegistEvents();
        }

        private void Form_Load(object sender, EventArgs eventArgs)
        {
            No = ScenarioUtil.GetScenarioNames(Common.WorkDir).FirstOrDefault() ?? "1";

            Task.Run(async () =>
            {
                while (true)
                {
                    if (!chkLogMonitoring.Checked)
                        continue;

                    if (MonitoringStart.Checked)
                        Scenario.LogIncrease = _logFile.LengthOrZero() - _logSizeOnStart;

                    ActionInvoke(() => LogSizeDisp.Text = $"{_logFile.LengthOrZero():#,##0} ({Scenario.LogIncrease:+#,##0;-#,##0})");

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
            menuTemplate[0].Add("テスト結果を出力", SaveEvidences, unMonitoring);
            menuTemplate[1].Add("シナリオ名の変更", Rename, unMonitoring, Keys.F2);
            menuTemplate[1].Add("エビデンスを全削除", ClearAllEvidence, unMonitoring);
            menuTemplate[2].Add("新規シナリオ", Insert, unMonitoring, Keys.Insert);
            menuTemplate[3].Add("テーブル定義書からCREATE文逆作成(&R)", ShowModeless<FrmReverseCreation>);
            menuTemplate[3].Add("カラム名確認(&V)", ShowModeless<FrmColumnsViewer>);
            menuTemplate[3].Add("BCPユーティリティ", ShowModeless<FrmBcpUtil>);
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
                using (WithWaitCursor())
                {
                    Setting.ChangeMainDirectory(dir);
                    No = ScenarioUtil.GetScenarioNames(Common.WorkDir).FirstOrDefault() ?? "1";
                    Reload();
                }
            }
        }

        protected override void Save()
        {
            var save = GetMergedNewDataContext<Scenario>();
            save.YamlSave(No);

            _ = DispMessageInTitleBarAsync("現在の状態を保存しました");
        }

        protected override void Reload()
        {
            DataContext = Scenario.YamlLoadOrNew(No);

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
                using (WithWaitCursor())
                {
                    if (MonitoringStart.Checked)
                    {
                        RefreshMainPanelAtMonitoringStatusChanging();

                        _logSizeOnStart = _logFile.LengthOrZero();

                        if (chkTableCompare.Checked)
                        {
                            _containers = DapperQueryContainers.CreateQueryContainers(Scenario.CompareTables.FilteredChecked());
                            _containers.FetchByQuery();
                        }
                    }
                    else
                    {
                        if (_logFile != null)
                            Scenario.AddEvidenceTextIncrease(_logFile, _logSizeOnStart);

                        if (chkTableCompare.Checked && _containers.Any())
                        {
                            _containers.FetchByQuery2();
                            using (var exporter = new CompareExport())
                            {
                                var rowCount = _containers.Sum(x => x.CompareRows.Count);
                                ExecuteAbortableAction((progress, cancel) => exporter.Export(_containers, progress, cancel), rowCount);

                                var bytes = exporter.GetOutputBinary();
                                Scenario.AddEvidenceCompareXlsx(bytes);
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
            catch (Exception ex)
            {
                MonitoringStart.Checked = false;
                RefreshMainPanelAtMonitoringStatusChanging();
                ErrorMessageBox(ex.Message);
            }
            finally
            {
                MonitoringStart.CheckedChanged += MonitoringStart_CheckedChanged;
            }
        }

        private void DbSnapshot_Click(object sender, EventArgs e)
        {
            using (var dlg = new FrmTableSelection())
            {
                dlg.DataContext = Scenario;
                dlg.No = No;
                dlg.Mode = TableSelectionMode.SnapshotTables;

                if (dlg.ShowDialog() != DialogResult.Yes)
                    return;
            }

            if (!Scenario.SnapshotTables.FilteredChecked().Any())
                return;

            using (WithWaitCursor())
            {
                try
                {
                    var containers = DapperQueryContainers.CreateQueryContainers(Scenario.SnapshotTables.FilteredChecked());
                    containers.FetchByQuery();

                    using (var exporter = new SnapshotExport())
                    {
                        var rowCount = containers.Sum(x => x.Values.Count);
                        ExecuteAbortableAction((progress, cancel) => exporter.Export(containers, progress, cancel), rowCount);

                        var bytes = exporter.GetOutputBinary();
                        Scenario.AddEvidenceSnapshotXlsx(bytes);
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
                catch (Exception ex)
                {
                    MonitoringStart.Checked = false;
                    RefreshMainPanelAtMonitoringStatusChanging();
                    ErrorMessageBox(ex.Message);
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

            Scenario.AddEvidenceScreenShotImage(image);
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
                Scenario.AddEvidenceFileCopy(finfo);

            SaveAndReload();
        }

        /// <summary>
        /// エビデンスのリンクラベルを生成して画面左下に並べる
        /// </summary>
        private void RefreshImageLink()
        {
            foreach (var con in Controls.OfType<ToolBox.LinkLabel>().ToList())
                Controls.Remove(con);

            var files = Scenario.Evidences.Where(x => !x.Deleted);

            int i = files.Count();
            foreach (var file in files.Reverse())
            {
                var link = new ToolBox.LinkLabel()
                {
                    AutoSize = true,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                    Font = new Font("Segoe Script", 12F),
                    Location = new Point(9 + 18 * (i - 1), 340),
                    Text = $"{i--}",
                    TabStop = false
                };

                link.MouseHover += (s, e) => ImagePreview(EvidenceUtil.LoadImage(file.Hash));
                link.LinkClicked += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        EvidenceUtil.OpenOnApplication(file.Hash);
                };

                var menuTemplate = new MenuStripTemplate();

                menuTemplate.Add("キャプションを編集", () =>
                {
                    void closingHandler(object sender, FormClosingEventArgs e)
                    {
                        var dlg = (SimpleTextEditDialog)sender;

                        if (dlg.DialogResult == DialogResult.OK && dlg.Text.IsMatch("[\"<>|:*?/\\\\]"))
                        {
                            e.Cancel = true;
                            dlg.SetTextBoxError("ファイル名に使えない文字が含まれています。");
                        }
                    }

                    if (ShowSimpleTextEditDialog("キャプションを編集", closingHandler, file.Description, out var editedText))
                    {
                        file.Description = editedText;
                        SaveAndReload();
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
                    SaveAndReload();
                });

                var menu = new ContextMenuStrip();
                menu.Items.AddRange(menuTemplate.ConvertToStripItem());
                menu.Opened += (s, e) => menuTemplate.OnOpenedMethod(menu.Items);
                menu.Closed += (s, e) => menuTemplate.OnClosedMethod(menu.Items);
                link.ContextMenuStrip = menu;

                ToolTip.SetToolTip(link, file.TooltipDispName);
                Controls.Add(link);
            }
        }

        private void ClearAllEvidence()
        {
            if (MonitoringStart.Checked)
                return;

            if (!InfoMessageBoxOKCancel("エビデンスを全削除します。よろしいですか?"))
                return;

            foreach (var file in Scenario.Evidences)
                EvidenceUtil.DeleteFile(file.Hash);

            Scenario.Evidences.Clear();
            SaveAndReload();
        }

        private void RefreshMainPanelAtMonitoringStatusChanging()
        {
            MonitoringStart.AutoCheck = !MonitoringStart.Checked;

            var noDisableControls = new Control[] { lblScenarioNo, LogSizeDisp, MonitoringStart, MonitoringStop, DbSnapshot };
            foreach (var control in MainPanel.ManyControls(containChild: false).Where(x => !noDisableControls.Contains(x)))
                control.Enabled = !MonitoringStart.Checked;
        }

        /// <summary>
        /// DB監視対象テーブル表示窓の更新
        /// </summary>
        private void RefreshMonitTablesListBox()
        {
            var items = Scenario.CompareTables.FilteredChecked().Select(x => x.TableName).ToArray();

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
                Scenario.LogIncrease = 0;
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
                ShowModeless<FrmTableSelection>(Scenario, form => { form.No = No; form.Mode = TableSelectionMode.CompareTables; }, (s, e) => RefreshMonitTablesListBox());
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

            var res = ScenarioUtil.GetScenarioNoByKeyInput(No, e.KeyData);

            if (res != null)
                No = res;
        }

        private void Insert()
        {
            if (MonitoringStart.Checked)
                return;

            No = ScenarioUtil.GetNewScenarioNo();
        }

        private void Rename()
        {
            if (MonitoringStart.Checked)
                return;

            void closingHandler(object sender, FormClosingEventArgs e)
            {
                var dlg = (SimpleTextEditDialog)sender;

                if (No != dlg.Text && dlg.DialogResult == DialogResult.OK)
                {
                    if (!YamlUtil.YamlRename(Common.WorkDir, No, dlg.Text))
                    {
                        e.Cancel = true;
                        dlg.SetTextBoxError("その名前にはリネームできません。");
                    }
                }
                else
                {
                    dlg.DialogResult = DialogResult.Cancel;
                }
            }

            if (ShowSimpleTextEditDialog("シナリオ名の変更", closingHandler, No, out var editedText))
                No = editedText;
        }

        private void SaveEvidences()
        {
            if (!InfoMessageBoxOKCancel($"「{Common.MainDir}」に全シナリオのテスト結果を出力します。\r\nフォルダ内の全てのデータは削除されます。よろしいですか?"))
                return;

            var prepareDir = ScenarioUtil.SaveEvidencesPrepare();

            try
            {
                var actions = FileCompareUtil.DirectorySyncInteractive(prepareDir, Common.MainDir, ".nai");

                foreach (var action in actions)
                    ExecuteRetriableAction(action);

                InformationMessageBox("テスト結果を出力しました。");
            }
            catch (OperationCanceledException)
            {
                // 処理なし
            }
            finally
            {
                prepareDir.DeleteQuiet();
            }
        }
    }
}
