using OfficeOpenXml;
using OfficeOpenXml.Style;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ExcelExporter
{
    public static class ExcelExtension
    {
        public static IEnumerable<T[]> GetRows<T>(this T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                yield return array.GetColumns(i).ToArray();
        }

        public static IEnumerable<T> GetColumns<T>(this T[,] array, int row)
        {
            for (int i = 0; i < array.GetLength(1); i++)
                yield return array[row, i];
        }

        public static void SetValue(this ExcelRange cell, object value)
        {
            if (value == null)
            {
                cell.Value = "« NULL »";
                cell.Style.Font.Color.SetColor("#C0C0C0");
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "@";
            }
            else
            {
                switch (value)
                {
                    case string _:
                        cell.Style.Numberformat.Format = "@";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Value = value;
                        break;

                    case bool:
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Value = value;
                        break;

                    case DateTime dt:
                        cell.Style.Numberformat.Format = "@";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Value = dt.ToString("yyyy/MM/dd HH:mm:ss.fff");
                        break;

                    case decimal dec when dec >= 1000000000:
                        cell.Style.Numberformat.Format = "@";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Value = Convert.ToString(value);
                        break;

                    default:
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Value = value;
                        break;
                }
            }
        }

        public static void SetWidthIfGreater(this ExcelRangeBase cell, double width)
        {
            var column = cell.Worksheet.Column(cell.Start.Column);

            if (column.Width < width)
                column.Width = width;
        }

        public static void AutoFitIfGreater(this ExcelRangeBase cell, double? maximum = null)
        {
            try
            {
                var column = cell.Worksheet.Column(cell.Start.Column);

                if (maximum.HasValue)
                {
                    column.AutoFit(column.Width, maximum.Value);
                }
                else
                {
                    column.AutoFit(column.Width);
                }

                column.Width += 1.3;
            }
            catch
            {
                throw new InvalidOperationException("セル操作中にエラー発生。出力しようとしたテーブルに暗号化されたカラムを含んでいませんか?");
            }
        }
    }
}
