using OfficeOpenXml;

namespace TaiyaHirameDance.ToolBox
{
    /// <summary>
    /// Excelファイルのインポート情報を設定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class XlsxImportOptionAttribute(string pos) : Attribute
    {
        public string Pos { get; } = pos;
        public int RowNo { get; } = new ExcelCellAddress(pos).Row;
        public int ColumnNo { get; } = new ExcelCellAddress(pos).Column;
    }
}
