namespace TaiyaHirameDance.ToolBox
{
    /// <summary>
    /// Enumの値に文字列を設定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumStringAttribute(string value) : Attribute
    {
        public string Value { get; } = value;
    }
}
