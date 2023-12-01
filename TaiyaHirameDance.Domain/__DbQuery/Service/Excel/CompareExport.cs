using OfficeOpenXml;
using OfficeOpenXml.Style;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    public class CompareExport : ExcelUtil
    {
        private ExcelWorksheet _main;
        private IProgress<int> _progress;
        private CancellationToken _cancel;
        private int _reportCount = 0;

        public static CompareExport Create()
        {
            return new CompareExport() { package = new ExcelPackage() };
        }

        public void Export(IList<DapperQueryContainer> containers, CancellationToken cancel, IProgress<int> prg)
        {
            _progress = prg;
            _cancel = cancel;

            foreach (var item in containers)
            {
                AddSheet(item.TableLogicalName, item.SheetColor);
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

            int colorBunrui = 0;
            curr = top.Right().Bottom(3);
            if (item.CompareRows.Any())
            {
                foreach (var row in item.CompareRows)
                {
                    _cancel.ThrowIfCancellationRequested();

                    var kbn = (CompareRowKbn)row.Value.First();
                    var data = row.Value.Skip(1).ToArray();

                    var next = curr;
                    if (kbn != CompareRowKbn.OK)
                    {
                        next.SetValue(kbn.GetEnumString());
                        next.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    next = next.Right();
                    foreach (var val in data)
                    {
                        next.SetValue(val);
                        next = next.Right();
                    }

                    var range = curr.MultipleRange(1, fieldCount + 1);

                    switch (kbn)
                    {
                        case CompareRowKbn.追加:
                            SetColorIfCounterIsOddNumberElseWhite(range, colorBunrui++, "#FFCCFF");
                            break;

                        case CompareRowKbn.削除:
                            SetColorIfCounterIsOddNumberElseWhite(range, colorBunrui++, "#D9D9D9");
                            curr.Right().MultipleRange(1, fieldCount).Style.Font.Strike = true;
                            break;

                        case CompareRowKbn.更新前:
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor("#D9D9D9");
                            curr.Right().MultipleRange(1, fieldCount).Style.Font.Strike = true;
                            break;

                        case CompareRowKbn.更新後:
                            SetColorIfCounterIsOddNumberElseWhite(range, colorBunrui++, "#FFF2CC");
                            foreach (var updIdx in item.UpdateIdx[row.Key])
                                curr.Right().Right(updIdx).Style.Font.Color.SetColor(Color.Red);
                            break;

                        default:
                            SetColorIfCounterIsOddNumberElseWhite(range, colorBunrui++, "#FFF2CC");
                            break;
                    }

                    _progress.Report(_reportCount++);

                    curr = curr.Bottom();
                }

                SetStyleValues(item.CompareRows.Count, fieldCount + 1);
            }
            else
            {
                curr = curr.Right();

                curr.Value = Setting.OmitCompareRowIsConformity ? "(変更なし)" : "(データが見つかりませんでした)";
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

        private void SetColorIfCounterIsOddNumberElseWhite(ExcelRange range, int counter, string htmlColor)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            var bgColor = (counter % 2 == 0) ? htmlColor : "#FFFFFF";
            range.Style.Fill.BackgroundColor.SetColor(bgColor);
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
