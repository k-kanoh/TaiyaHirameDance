using TaiyaHirameDance.Domain.BcpUtility;
using TaiyaHirameDance.Domain.TableCriteria;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class FrmBcpUtil : BaseForm
    {
        private readonly BcpUtil _bcpUtil;
        private BcpTableSelections _tableSelections;

        public FrmBcpUtil()
        {
            InitializeComponent();

            _bcpUtil = new BcpUtil();
            _bcpUtil.ConsoleReported += (s, msg) =>
            {
                if (InvokeRequired)
                {
                    ActionInvoke(() => ConsoleMessage.AppendLine(msg));
                }
                else
                {
                    ConsoleMessage.AppendLine(msg);
                }
            };
        }

        private async void Form_Load(object sender, EventArgs e)
        {
            SetupMenuStrip();
            AddConsoleMessageMenuStripItems();

            dgvTables.CellValueChanged += (s, e) => SetGridCellStyle();
            dgvTables.DataSourceChanged += (s, e) => SetGridCellStyle();

            Reload();

            if (!await _bcpUtil.CheckBcpExists())
            {
                InformationMessageBox("bcp が見つかりません。");
                Close();
                return;
            }
        }

        private void SetupMenuStrip()
        {
            bool hasOwnedStash() => !cmbBcpContainer.IsEmpty && _tableSelections.FilteredOwnedStash().Any();

            var menuTemplate = new MenuStripTemplate { "編集(&E)", "バックアップ(&B)", "リストア(&R)" };
            menuTemplate[0].Add("バックアップ説明文を修正", EditDescription, null, Keys.F2);
            menuTemplate[1].Add("バックアップを実行", BackupTables);
            menuTemplate[2].Add("リストアを実行", RestoreTables);
            menuTemplate[2].Add("直近のスタッシュをポップ", PopStashTables, hasOwnedStash);
            menuTemplate[2].Add("スタッシュテーブルのクリーン", CleanStashTables, hasOwnedStash);
            MenuStrip.Items.AddRange(menuTemplate.ConvertToStripItem());
        }

        private void AddConsoleMessageMenuStripItems()
        {
            var additional = new MenuStripTemplate()
            {
                new MenuStripTemplate("-"),
                new MenuStripTemplate("すべてクリア", ConsoleMessage.Clear)
            };

            ConsoleMessage.ContextMenuStrip.Items.AddRange(additional.ConvertToStripItem());
        }

        protected override void Reload()
        {
            using (WithWaitCursor())
            {
                dgvTables.DataSource = null;

                _tableSelections = BcpTableSelections.CreateBcpTableSelections(TableInfoUtil.GetTableSelections());

                var containers = BcpContainers.LoadBcpContainers(_bcpUtil.DbName);
                cmbBcpContainer.ComboBoxItems = containers.GetComboBoxItems();
            }
        }

        private void Grid_ButtonClick(object sender, DataGridViewCellButtonClickEventArgs e)
        {
            var table = e.Row.GetBoundItem<BcpTableSelection>();
            var container = cmbBcpContainer.GetBoundItem<BcpContainer>();

            if (e.Value == "リストア")
            {
                if (!InfoMessageBoxOKCancel($"{table.TableName}をリストアします。"))
                    return;

                DateTime? now = DateTime.Now;

                if (!InfoMessageBoxYesNoCancel($"念のため現在のデータを「{table.TableName}.{now:yyyyMMddHHmmss}.{container.HashPrefix}」にスタッシュしておきますか?", out var withBackup))
                    return;

                if (!withBackup)
                    now = null;

                try
                {
                    ExecuteAbortableTask((progress, cancel) => _bcpUtil.BcpRestore(container, table, now, progress, cancel));
                    InformationMessageBox("リストアを完了しました。");
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
                finally
                {
                    Reload();
                    cmbBcpContainer.SelectedKey = container.Hash;
                }
            }
            else if (e.Value == "ポップ")
            {
                if (!InfoMessageBoxOKCancel($"「{table.TableName}」をポップします。"))
                    return;

                try
                {
                    ExecuteAbortableTask((progress, cancel) => _bcpUtil.PopStashTable(table, progress, cancel));
                    InformationMessageBox($"ポップを完了しました。\r\n「{table.TableName}」は削除されました。");
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
                finally
                {
                    Reload();
                    cmbBcpContainer.SelectedKey = container.Hash;
                }
            }
        }

        private void BackupTables()
        {
            var tables = (BcpTableSelections)dgvTables.DataSource;

            tables = tables.FilteredChecked();

            if (tables.All(x => x.TableRowCount == 0))
            {
                InformationMessageBox("バックアップ対象のデータがありません。");
                return;
            }

            if (!InfoMessageBoxOKCancel($"バックアップを開始します。"))
                return;

            try
            {
                var container = ExecuteAbortableTask((progress, cancel) => _bcpUtil.BcpBackup(tables, progress, cancel));
                InformationMessageBox("バックアップを完了しました。");

                Reload();
                cmbBcpContainer.SelectedKey = container.Hash;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RestoreTables()
        {
            var container = cmbBcpContainer.GetBoundItem<BcpContainer>();

            if (!InfoMessageBoxOKCancel($"「{container.ComboBoxDispName}」をリストアします。"))
                return;

            DateTime? now = DateTime.Now;

            if (!InfoMessageBoxYesNoCancel($"念のため現在のデータを「(テーブル名).{now:yyyyMMddHHmmss}.{container.HashPrefix}」にスタッシュしておきますか?", out var withBackup))
                return;

            if (!withBackup)
                now = null;

            try
            {
                ExecuteAbortableTask((progress, cancel) => _bcpUtil.BcpRestore(container, _tableSelections, now, progress, cancel));
                InformationMessageBox("リストアを完了しました。");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                Reload();
                cmbBcpContainer.SelectedKey = container.Hash;
            }
        }

        private void PopStashTables()
        {
            var container = cmbBcpContainer.GetBoundItem<BcpContainer>();

            var latestStash = (from t in _tableSelections.FilteredOwnedStash()
                               group t by t.StashTableNameParts.datetime into g
                               orderby g.Key descending
                               select g).First();

            var tables = latestStash.Select(x => x).ToBcpTableSelections();

            if (!InfoMessageBoxOKCancel($"「(全てのテーブル).{latestStash.Key}.{container.HashPrefix}」をポップします。"))
                return;

            try
            {
                ExecuteAbortableTask((progress, cancel) => _bcpUtil.PopStashTable(tables, progress, cancel));
                InformationMessageBox($"ポップを完了しました。\r\n「(全てのテーブル).{latestStash.Key}.{container.HashPrefix}」は削除されました。");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                Reload();
                cmbBcpContainer.SelectedKey = container.Hash;
            }
        }

        private async void CleanStashTables()
        {
            var container = cmbBcpContainer.GetBoundItem<BcpContainer>();

            var drops = _tableSelections.FilteredOwnedStash();

            if (!InfoMessageBoxOKCancel($"{container.HashPrefix}のスタッシュテーブル{drops.Count}件を削除します。"))
                return;

            try
            {
                await _bcpUtil.CleanStashTables(drops);
                InformationMessageBox("スタッシュテーブルのクリーンを完了しました。");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                Reload();
                cmbBcpContainer.SelectedKey = container.Hash;
            }
        }

        private void HandleException(Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                ConsoleMessage.AppendLine("処理を中断しました。");
            }
            else
            {
                ConsoleMessage.AppendLine(ex.Message);
                ErrorMessageBox("エラーが発生しました。");
            }
        }

        private void ComboBox_EmptySelected(object sender, EventArgs e)
        {
            using (WithWaitCursor())
            {
                var container = cmbBcpContainer.GetBoundItem<BcpContainer>();
                _tableSelections.MergeBcpContainer(container);
                _tableSelections.ExcludeStash();

                dgvTables.DataSource = _tableSelections.ToList().ToBcpTableSelections();

                Column1.ReadOnly = false;
                Column7.Visible = false;
                Column8.Visible = false;

                MenuStrip.Items[0].Visible = false;
                MenuStrip.Items[1].Visible = true;
                MenuStrip.Items[2].Visible = false;
            }
        }

        private void ComboBox_ItemSelected(object sender, EventArgs e)
        {
            using (WithWaitCursor())
            {
                var container = cmbBcpContainer.GetBoundItem<BcpContainer>();
                _tableSelections.MergeBcpContainer(container);
                _tableSelections.ExcludeStash(true);

                dgvTables.DataSource = _tableSelections.ToList().ToBcpTableSelections();

                Column1.ReadOnly = true;
                Column7.Visible = true;
                Column8.Visible = true;

                foreach (var row in dgvTables.Rows.Cast<DataGridViewRow>())
                {
                    var item = row.GetBoundItem<BcpTableSelection>();

                    if (item.IsChecked && item.BackupedCount > 0)
                    {
                        row.Cells[7] = new DataGridViewButtonCell()
                        {
                            Value = "リストア",
                            Style = { Font = new Font("Yu Gothic UI", 8.25F) }
                        };
                    }
                    else if (item.IsOwnedStash)
                    {
                        row.Cells[7] = new DataGridViewButtonCell()
                        {
                            Value = "ポップ",
                            Style = { Font = new Font("Yu Gothic UI", 8.25F) }
                        };
                    }
                }

                MenuStrip.Items[0].Visible = true;
                MenuStrip.Items[1].Visible = false;
                MenuStrip.Items[2].Visible = true;
            }
        }

        private void SetGridCellStyle()
        {
            dgvTables.SetCellStyleRows<BcpTableSelection>("RealRowCount", (item, style) =>
            {
                if (item.BackupedCount.HasValue && item.RealRowCount != item.BackupedCount)
                {
                    style.Font = dgvTables.DefaultFont.WithBold();
                    style.ForeColor = Color.Red;
                    style.SelectionForeColor = Color.Red;
                }
            });

            dgvTables.SetCellStyleAllRow<BcpTableSelection>((item, style) =>
            {
                style.BackColor = item.IsStash ? Color.Silver : Color.Empty;
            });

            dgvTables.SetCellStyleRows<BcpTableSelection>("Color", (item, style) =>
            {
                foreach (var (pattern, color) in Setting.TableColor)
                    if (item.TableName.IsMatch(pattern))
                        style.BackColor = style.SelectionBackColor = color;
            });
        }

        private void EditDescription()
        {
            if (cmbBcpContainer.IsEmpty) return;

            var container = cmbBcpContainer.GetBoundItem<BcpContainer>();

            void closingHandler(object sender, FormClosingEventArgs e)
            {
                var dlg = (SimpleTextEditDialog)sender;

                if (container.Description == dlg.Text)
                    dlg.DialogResult = DialogResult.Cancel;
            }

            if (ShowSimpleTextEditDialog("バックアップ説明文を修正", closingHandler, container.Description, out var editedText))
            {
                container.Description = editedText;
                container.Save(_bcpUtil.DbName);
                cmbBcpContainer.SelectedItem.DispName = container.ComboBoxDispName;
                cmbBcpContainer.Refresh();
            }
        }
    }
}
