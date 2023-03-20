namespace TaiyaHirameDance.Domain.ExcelExporter
{
    public class DapperQueryContainer
    {
        public int Seq { get; set; }
        public string TableName { get; set; }
        public string TableLogicalName { get; set; }
        public string Query { get; set; }
        public string PreQuery { get; set; }
        public bool OmitConformity { get; set; }
        public Color? SheetColor { get; set; }
        public string[] ColumnName { get; set; }
        public string[] LogicalName { get; set; }
        public int[] KeyFieldIdx { get; set; }
        public DateTime? FetchedAt { get; set; }
        public DateTime? FetchedAt2 { get; set; }
        public IList<(string key, object[] data)> Values { get; set; }
        public IList<(string key, object[] data)> Values2 { get; set; }
        public IDictionary<string, int[]> UpdateIdx { get; set; }
        public IList<(string key, CompareRowKbn kbn, object[] data)> CompareRows { get; set; }
    }
}
