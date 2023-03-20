using TaiyaHirameDance.Domain.MainModels;
using TaiyaHirameDance.Domain.TableCriteria;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public enum TableSelectionMode
    {
        SnapshotTables,
        CompareTables
    }

    public partial class FrmTableSelection : BaseForm
    {
        public string No { get; set; }

        public TableSelectionMode Mode { get; set; }

        public Scenario Scenario => (Scenario)DataContext;

        public FrmTableSelection()
        {
            InitializeComponent();
            TableInfoUtil.PutOriginalQuerySample();
        }

        private void Form_Load(object sender, EventArgs eventArgs)
        {
            SetGridData();

            dgvTables.CellValueChanged += (s, e) =>
            {
                SyncGridDataAndDataContext();
                SetGridCellStyleAndErrorMsg();
            };

            dgvTables.CellBeginEdit += (s, e) =>
            {
                dgvTables[e.ColumnIndex, e.RowIndex].Style.Reset();
                dgvTables[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Empty;
            };
        }

        private void SetGridData()
        {
            using (WithWaitCursor())
            {
                dgvTables.DataSource = null;

                var tableSelections = TableInfoUtil.GetTableSelections();

                tableSelections.ExcludeIgnoreSchemaAndTable();
                tableSelections.Expansion();

                switch (Mode)
                {
                    case TableSelectionMode.SnapshotTables:
                        {
                            var merged = tableSelections.MergeTableSettings(Scenario.SnapshotTables);
                            merged.UpdateRowCountFromQuery();

                            dgvTables.DataSource = merged;
                            Column7.Width = 400;
                            Column9.Visible = false;
                        }
                        break;

                    case TableSelectionMode.CompareTables:
                        {
                            var merged = tableSelections.MergeTableSettings(Scenario.CompareTables);
                            merged.UpdateRowCountFromQuery();

                            dgvTables.DataSource = merged;
                        }
                        break;
                }

                SetGridCellStyleAndErrorMsg();
            }
        }

        protected override void Save()
        {
            Scenario.YamlSave(No);
            base.Save();
        }

        protected override void Reload()
        {
            var scenario = Scenario.YamlLoadOrNew(No);

            Util.ShallowCopy(scenario, Scenario);
            SetGridData();
            base.Reload();
        }

        private void Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTables.Columns[e.ColumnIndex].DataPropertyName == "WhereClause")
            {
                var tableSelections = (TableSelections)dgvTables.DataSource;
                tableSelections.UpdateRowCountFromQuery();
            }

            SyncGridDataAndDataContext();
            SetGridCellStyleAndErrorMsg();
        }

        private void SyncGridDataAndDataContext()
        {
            var tableSelections = (TableSelections)dgvTables.DataSource;

            switch (Mode)
            {
                case TableSelectionMode.SnapshotTables:
                    Scenario.SnapshotTables = tableSelections.CovertToTableSettings();
                    break;

                case TableSelectionMode.CompareTables:
                    Scenario.CompareTables = tableSelections.CovertToTableSettings();
                    break;
            }
        }

        private void SetGridCellStyleAndErrorMsg()
        {
            dgvTables.SetCellStyleRows<TableSelection>("RealRowCount", (item, style) =>
            {
                style.Reset();
                if (item.QueryRowCount.HasValue)
                {
                    style.Font = dgvTables.DefaultFont.WithBold();
                    style.ForeColor = Color.Red;
                    style.SelectionForeColor = Color.Red;
                }
            });

            dgvTables.SetCellStyleRows<TableSelection>("WhereClause", (item, style) =>
            {
                style.Reset();
                if (item.QueryErrorMsg.Val())
                {
                    style.Font = dgvTables.DefaultFont.WithBold();
                    style.ForeColor = Color.Red;
                    style.SelectionForeColor = Color.Red;
                }
            });

            dgvTables.SetCellStyleAllRow<TableSelection>((item, style) =>
            {
                style.BackColor = item.IsChecked ? Color.Yellow : Color.Empty;
            });

            dgvTables.SetCellStyleRows<TableSelection>("Color", (item, style) =>
            {
                foreach (var (pattern, color) in Setting.TableColor)
                    if (item.TableName.IsMatch(pattern))
                        style.BackColor = style.SelectionBackColor = color;
            });

            foreach (var row in dgvTables.Rows.Cast<DataGridViewRow>())
            {
                var item = row.GetBoundItem<TableSelection>();

                if (item.QueryErrorMsg.Val())
                {
                    row.Cells[6].ErrorText = item.QueryErrorMsg;
                }
                else
                {
                    row.Cells[6].ErrorText = null;
                }
            }
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Mode == TableSelectionMode.SnapshotTables)
            {
                if (InfoMessageBoxYesNoCancel("現在のDBスナップショットを取得、保存しますか?", out var isYes))
                {
                    if (isYes)
                        DialogResult = DialogResult.Yes;

                    return;
                }

                e.Cancel = true;
            }
        }
    }
}
