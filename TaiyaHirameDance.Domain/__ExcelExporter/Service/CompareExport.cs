using OfficeOpenXml;
using OfficeOpenXml.Style;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ExcelExporter
{
    public class CompareExport : ExcelUtil
    {
        private ExcelWorksheet _main;
        private IProgress<int> _progress;
        private CancellationToken _cancel;
        private int _reportCount = 0;

        public CompareExport()
        {
            package = new ExcelPackage();
        }

        public void Export(DapperQueryContainers containers, IProgress<int> progress, CancellationToken cancel)
        {
            _cancel = cancel;
            _progress = progress;

            foreach (var item in containers)
            {
                AddSheet(item.TableLogicalName.RegexReplace(@"\.yml$", ""), item.SheetColor);
                Export(item);
            }
        }

        private void AddSheet(string name, Color? sheetColor)
        {
            _main = Workbook.Worksheets.Add(name);
            _main.Cells.Style.Font.SetFromFont(new Font("ＭＳ ゴシック", 11));
            _main.View.ZoomScale = 90;

            if (sheetColor.HasValue)
                _main.TabColor = sheetColor.Value;
        }

        private void Export(DapperQueryContainer item)
        {
            var top = _main.Cells["A1"];
            var fieldCount = item.ColumnName.Length;

            var curr = top.Right(2).Bottom();
            for (int i = 0; i < fieldCount; i++)
            {
                var next = curr.Right(i);
                next.Value = item.ColumnName[i];
            }
            SetStyleColumnName(fieldCount + 1);

            curr = curr.Bottom();
            for (int i = 0; i < fieldCount; i++)
            {
                var next = curr.Right(i);
                next.Value = item.LogicalName[i];
            }
            SetStyleLogicalName(fieldCount + 1);

            curr = top.Right().Bottom(3);
            if (item.CompareRows.Any())
            {
                int i = 0;
                foreach (var row in item.CompareRows)
                {
                    _cancel.ThrowIfCancellationRequested();

                    var next = curr;
                    if (row.kbn != CompareRowKbn.OK)
                    {
                        next.SetValue(row.kbn.GetEnumString());
                        next.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    next = next.Right();
                    foreach (var val in row.data)
                    {
                        next.SetValue(val);
                        next = next.Right();
                    }

                    var range = curr.MultipleRange(1, fieldCount + 1);

                    switch (row.kbn)
                    {
                        case CompareRowKbn.追加:
                            range.SetBackgroundColor(i, "#CCCCFF", ExcelFillMode.AllRows);
                            break;

                        case CompareRowKbn.削除:
                            range.SetBackgroundColor(i, "#777777", ExcelFillMode.AllRows);
                            break;

                        case CompareRowKbn.更新前:
                            range.SetBackgroundColor(i, "#FFFFCC", ExcelFillMode.AllRows);
                            foreach (var updIdx in item.UpdateIdx[row.key])
                            {
                                curr.Right().Right(updIdx).SetBackgroundColor(i, "#FF99CC", ExcelFillMode.AllRows);
                                curr.Right().Right(updIdx).Style.Font.Strike = true;
                            }
                            break;

                        case CompareRowKbn.更新後:
                            range.SetBackgroundColor(i, "#CCFFCC", ExcelFillMode.AllRows);
                            foreach (var updIdx in item.UpdateIdx[row.key])
                            {
                                curr.Right().Right(updIdx).SetBackgroundColor(i, "#FF99CC", ExcelFillMode.AllRows);
                                curr.Right().Right(updIdx).Style.Font.Color.SetColor(Color.Red);
                            }
                            break;
                    }

                    _progress.Report(_reportCount++);

                    curr = curr.Bottom();

                    i++;
                }

                SetStyleValues(item.CompareRows.Count, fieldCount + 1);
            }
            else
            {
                curr = curr.Right();

                curr.Value = item.OmitConformity ? "(変更なし)" : "(データが見つかりませんでした)";
                curr.Style.Font.SetFromFont(new Font("ＭＳ Ｐゴシック", 10));
            }

            SetBorder(item.CompareRows.Count, fieldCount + 1);

            top.Value = item.Query;
            SetStyleQueryString();
        }

        private void SetBorder(int rowCount, int colCount)
        {
            ExcelRange range;
            for (int i = 0; i < colCount; i++)
            {
                range = _main.Cells["B2"].Right(i).MultipleRange(2 + rowCount, 1);
                range.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            }

            range = _main.Cells["B3"].MultipleRange(1, colCount);
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            if (rowCount > 0)
            {
                range = _main.Cells["B2"].MultipleRange(2 + rowCount, colCount);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            else
            {
                range = _main.Cells["B2"].MultipleRange(3, colCount);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }

        private void SetStyleQueryString()
        {
            var top = _main.Cells["A1"];
            _main.Row(top.Start.Row).Height = 20;
            _main.Row(top.Start.Row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            top.Style.Font.Italic = true;
            top.Style.Font.UnderLine = true;
        }

        private void SetStyleColumnName(int colCount)
        {
            var top = _main.Cells["B2"];
            var range = top.MultipleRange(1, colCount);
            range.Style.Font.SetFromFont(new Font("ＭＳ Ｐゴシック", 10));
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor("#92D050");
            range.Style.Font.Bold = true;

            for (int i = 0; i < colCount - 1; i++)
                top.Right().Right(i).AutoFitIfGreater();
        }

        private void SetStyleLogicalName(int colCount)
        {
            var top = _main.Cells["B3"];
            var range = top.MultipleRange(1, colCount);
            range.Style.Font.SetFromFont(new Font("ＭＳ Ｐゴシック", 8));
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor("#92D050");

            for (int i = 0; i < colCount - 1; i++)
                top.Right().Right(i).AutoFitIfGreater();
        }

        private void SetStyleValues(int rowCount, int colCount)
        {
            var top = _main.Cells["B4"];
            var range = top.MultipleRange(rowCount, colCount);
            range.Style.Font.Size = 10;

            for (int i = 0; i < colCount - 1; i++)
                top.Right().Right(i).AutoFitIfGreater(30);
        }

        public byte[] GetOutputBinary()
        {
            using (var mStream = new MemoryStream())
            {
                package.Workbook.Properties.Author = Setting.XlsxAuthor;
                package.SaveAs(mStream);
                return mStream.ToArray();
            }
        }
    }
}
