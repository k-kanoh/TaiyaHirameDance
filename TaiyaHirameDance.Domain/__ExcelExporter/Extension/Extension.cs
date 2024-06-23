using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ExcelExporter
{
    public static class Extension
    {
        /// <summary>
        /// EnumStringAttributeで設定された文字列を返します。
        /// </summary>
        public static string GetEnumString(this CompareRowKbn value)
        {
            return typeof(CompareRowKbn).GetField(value.ToString()).FirstCustomAttribute<EnumStringAttribute>()?.Value;
        }
    }
}
