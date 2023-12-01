namespace TaiyaHirameDance.Domain
{
    public class DapperQueryContainer
    {
        public int Seq { get; set; }
        public string TableName { get; set; }
        public string TableLogicalName { get; set; }
        public string Query { get; set; }
        public Color? SheetColor { get; set; }
        public string[] ColumnName { get; set; }
        public string[] LogicalName { get; set; }
        public int[] KeyFieldIdx { get; set; }
        public DateTime? FetchAt { get; set; }
        public DateTime? FetchAt2 { get; set; }
        public IDictionary<string, object[]> Values { get; set; }
        public IDictionary<string, object[]> Values2 { get; set; }
        public IDictionary<string, int[]> UpdateIdx { get; set; }
        public IList<KeyValuePair<string, object[]>> CompareRows { get; set; }
    }
}
