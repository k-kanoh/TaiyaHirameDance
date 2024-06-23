using System.Text.RegularExpressions;
using TaiyaHirameDance.Domain.A5MK2Csv;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class FrmColumnsViewer : BaseForm
    {
        private bool _reloadRequestFlg = false;

        public FrmColumnsViewer()
        {
            InitializeComponent();

            SearchWord.TextChanged += (s, e) => _reloadRequestFlg = true;
            IsRegexMatch.CheckedChanged += (s, e) => _reloadRequestFlg = true;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            dgvColumns.DataSourceChanged += (s, e) => SetGridCellStyle();

            Reload();

            Task.Run(async () =>
            {
                while (true)
                {
                    if (_reloadRequestFlg)
                    {
                        ActionInvoke(() => Reload());
                        _reloadRequestFlg = false;
                    }

                    await Task.Delay(1000);
                }
            });
        }

        protected override void Reload()
        {
            using (WithWaitCursor())
            {
                SearchWord.SetError(null);

                var columns = ColumnInfoUtil.GetColumnInfos();

                columns.ExcludeIgnoreSchemaAndTable();

                if (IsRegexMatch.Checked)
                {
                    try
                    {
                        columns.FilterByRegex(SearchWord.Text);
                    }
                    catch (Exception ex)
                    {
                        SearchWord.SetError(ex.Message);
                    }
                }
                else
                {
                    columns.FilterByPhraseWord(SearchWord.Text);
                }

                dgvColumns.DataSource = columns;
                dgvColumns.Refresh();
            }
        }

        private void SetGridCellStyle()
        {
            dgvColumns.SetCellStyleRows<ColumnInfo>("DataTypeUpper", (item, style) =>
            {
                if (item.DataTypeUpper.Contains("IDENTITY"))
                    style.Font = dgvColumns.DefaultFont.WithBold();
            });

            dgvColumns.SetCellStyleRows<ColumnInfo>("ColumnName", (item, style) =>
            {
                if (item.PKeyNo.HasValue)
                    style.Font = dgvColumns.DefaultFont.WithBold();
            });

            dgvColumns.SetCellStyleRows<ColumnInfo>("Color", (item, style) =>
            {
                foreach (var (pattern, color) in Setting.TableColor)
                    if (item.TableName.IsMatch(pattern))
                        style.BackColor = style.SelectionBackColor = color;
            });
        }

        private void Grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                var sb = Util.CreateStringBuilder();
                var rows = (from x in dgvColumns.SelectedCells.Cast<DataGridViewCell>() orderby x.RowIndex select x.OwningRow).Distinct();

                foreach (var row in rows)
                {
                    var text = txtPasteFormat.Text;
                    var matches = Regex.Matches(text, "%([0-9]+)%").Cast<Match>();

                    foreach (var m in matches)
                    {
                        var num = m.Groups[1].Value.ToInt().Value;
                        var values = row.Cells.Cast<DataGridViewCell>().Where(x => x.Visible).Select(x => (string)x.Value ?? "").ToList();

                        if (values.Count >= num)
                            text = text.RegexReplace(m.Value, values[num - 1]);
                    }

                    sb.AppendLine(text);
                }

                Clipboard.SetText(sb.ToString());

                dgvColumns.SelectionBlink(Color.DeepSkyBlue);
            }
        }
    }
}
