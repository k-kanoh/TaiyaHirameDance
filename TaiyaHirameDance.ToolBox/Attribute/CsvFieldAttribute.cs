namespace TaiyaHirameDance.ToolBox
{
    /// <summary>
    /// Csv出力の列番号を設定します。 (1起算)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvFieldAttribute(int no) : Attribute
    {
        public int FieldNo { get; } = no - 1;
    }
}
