namespace TaiyaHirameDance.ToolBox
{
    /// <summary>
    /// クラスに対応するCsvのファイル名を設定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CsvNameAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
