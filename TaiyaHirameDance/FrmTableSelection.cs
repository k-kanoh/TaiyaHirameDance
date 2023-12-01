using TaiyaHirameDance.Domain;
using TaiyaHirameDance.Domain.TableSelection;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class FrmTableSelection : BaseForm
    {
        public string No { get; set; }

        public TableSelectionMode Mode { get; set; }

        public Evidence Evidence => (Evidence)DataContext;

        public FrmTableSelection()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs eventArgs)
        {
            SetGridData();

            dgvTables.CellValueChanged += (s, e) =>
            {
                SyncGridDataAndDataContext();
                SetGridCellStyle();
            };

            dgvTables.CellBeginEdit += (s, e) =>
            {
                dgvTables[e.ColumnIndex, e.RowIndex].Style.Reset();
                dgvTables[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Empty;
            };
        }

        private void SetGridData()
        {
            using (Executing())
            {
                dgvTables.DataSource = null;

                var tableSelections = TableInfoQuery.GetTableSelections();

                tableSelections.ExcludeIgnoreSchemaAndTable();

                switch (Mode)
                {
                    case TableSelectionMode.SnapshotTables:
                        {
                            var merged = tableSelections.MergeTableEvidence(Evidence.SnapshotTables);
                            merged.UpdateRowCountFromQuery();
                            dgvTables.DataSource = merged;
                        }
                        break;

                    case TableSelectionMode.CompareTables:
                        {
                            var merged = tableSelections.MergeTableEvidence(Evidence.CompareTables);
                            merged.UpdateRowCountFromQuery();
                            dgvTables.DataSource = merged;
                        }
                        break;
                }

                SetGridCellStyle();
            }
        }

        protected override void Save()
        {
            YamlUtil.YamlSave(Common.WorkDir, No, Evidence);
            base.Save();
        }

        protected override void Reload()
        {
            var evidence = YamlUtil.YamlLoadOrNew<Evidence>(Common.WorkDir, No);
            Util.ShallowCopy(evidence, Evidence);
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
            SetGridCellStyle();
        }

        private void SyncGridDataAndDataContext()
        {
            var tableSelections = (TableSelections)dgvTables.DataSource;

            switch (Mode)
            {
                case TableSelectionMode.SnapshotTables:
                    Evidence.SnapshotTables = tableSelections.CovertToTableEvidence();
                    break;

                case TableSelectionMode.CompareTables:
                    Evidence.CompareTables = tableSelections.CovertToTableEvidence();
                    break;
            }
        }

        private void SetGridCellStyle()
        {
            var font = dgvTables.RowTemplate.DefaultCellStyle.Font;
            font = new Font(font, font.Style | FontStyle.Bold);

            dgvTables.SetCellStyleAllRow<TableSelection>("RealRowCount", (item, style) =>
            {
                style.Reset();
                if (item.QueryRowCount.HasValue)
                {
                    style.Font = font;
                    style.ForeColor = Color.Red;
                    style.SelectionForeColor = Color.Red;
                }
            });

            dgvTables.SetCellStyleAllRow<TableSelection>("WhereClause", (item, style) =>
            {
                style.Reset();
                if (item.IsQueryError)
                {
                    style.Font = font;
                    style.ForeColor = Color.Red;
                    style.SelectionForeColor = Color.Red;
                }
            });

            dgvTables.SetCellStyleAllRow<TableSelection>((item, style) =>
            {
                style.BackColor = item.IsChecked ? Color.Yellow : Color.Empty;
            });

            dgvTables.SetCellStyleAllRow<TableSelection>("Color", (item, style) =>
            {
                style.Reset();
                foreach (var (pattern, color) in Setting.TableColor)
                    if (item.TableName.IsMatch(pattern))
                        style.BackColor = style.SelectionBackColor = color;
            });
        }
    }

    public enum TableSelectionMode
    {
        SnapshotTables,
        CompareTables
    }
}
