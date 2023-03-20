using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace TaiyaHirameDance.ToolBox
{
    public static class ExcelExtension
    {
        public static string TrimmedText(this ExcelRange cell)
        {
            return cell.Text.TrimNewline().ValOrNull();
        }

        public static void SetColor(this ExcelColor xlColor, string htmlColor)
        {
            xlColor.SetColor(ColorTranslator.FromHtml(htmlColor));
        }

        public static ExcelRange Right(this ExcelRange cell, int i = 1)
        {
            return (i == 0) ? cell : cell.Worksheet.Cells[cell.Start.Row, cell.Start.Column + i];
        }

        public static ExcelRange Bottom(this ExcelRange cell, int i = 1)
        {
            return (i == 0) ? cell : cell.Worksheet.Cells[cell.Start.Row + i, cell.Start.Column];
        }

        public static ExcelRange MultipleRange(this ExcelRange cell, int bottom, int right)
        {
            var top = cell.Start;
            return cell.Worksheet.Cells[top.Row, top.Column, top.Row + bottom - 1, top.Column + right - 1];
        }

        public static void SetBackgroundColor(this ExcelRange range, int rowNumber, string htmlColor, ExcelFillMode mode)
        {
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;

            if (mode == ExcelFillMode.AlternateRows && rowNumber % 2 == 1)
            {
                range.Style.Fill.BackgroundColor.SetColor("#FFFFFF");
            }
            else
            {
                range.Style.Fill.BackgroundColor.SetColor(htmlColor);
            }
        }
    }
}
