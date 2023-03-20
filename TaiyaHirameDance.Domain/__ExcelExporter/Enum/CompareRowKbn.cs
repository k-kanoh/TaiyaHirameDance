using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ExcelExporter
{
    public enum CompareRowKbn
    {
        OK,

        [EnumString("-(削 除)")]
        削除,

        [EnumString("+(追 加)")]
        追加,

        [EnumString("*(更新前)")]
        更新前,

        [EnumString("*(更新後)")]
        更新後,
    }
}
