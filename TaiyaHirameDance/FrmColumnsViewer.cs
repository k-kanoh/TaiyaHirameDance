using System.Text.RegularExpressions;
using TaiyaHirameDance.Domain.A5MK2Csv;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class FrmColumnsViewer : BaseForm
    {
        private bool _reloadRequestFlg = true;

        public FrmColumnsViewer()
        {
            InitializeComponent();

            SearchWord.TextChanged += (s, e) => _reloadRequestFlg = true;
            IsRegexMatch.CheckedChanged += (s, e) => _reloadRequestFlg = true;
        }

        private void Form_Load(object sender, EventArgs e)
        {
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
            using (Executing())
            {
                SearchWord.SetError(null);

                var columns = A5MK2Util.ExcludeIgnoreSchemaAndTable(A5MK2.Columns).AsEnumerable();

                if (IsRegexMatch.Checked)
                {
                    try
                    {
                        columns = columns.Where(x => x.RegexMatching(SearchWord.Text));
                    }
                    catch (Exception ex)
                    {
                        SearchWord.SetError(ex.Message);
                    }
                }
                else
                {
                    foreach (var word in SearchWord.Text.SplitByWhiteSpace())
                        columns = columns.Where(x => x.WordMatching(word));
                }

                dgvColumns.DataSource = columns.ToList();
            }
        }

        private void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var font = dgvColumns.RowTemplate.DefaultCellStyle.Font;
            font = new Font(font, font.Style | FontStyle.Bold);

            dgvColumns.SetCellStyleAllRow<A5MK2ColumnInfo>("DataTypeUpper", (item, style) =>
            {
                if (item.DataTypeUpper.Contains("IDENTITY"))
                    style.Font = font;
            });

            dgvColumns.SetCellStyleAllRow<A5MK2ColumnInfo>("ColumnName", (item, style) =>
            {
                if (item.PKeyNo.HasValue)
                    style.Font = font;
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
