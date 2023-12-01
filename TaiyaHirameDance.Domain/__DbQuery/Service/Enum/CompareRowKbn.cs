using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    public enum CompareRowKbn
    {
        OK,

        [EnumString("-(削  除)")]
        削除,

        [EnumString("+(追  加)")]
        追加,

        [EnumString("*(更新前)")]
        更新前,

        [EnumString("*(更新後)")]
        更新後,
    }

    public static class CompareRowKbnExtension
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
