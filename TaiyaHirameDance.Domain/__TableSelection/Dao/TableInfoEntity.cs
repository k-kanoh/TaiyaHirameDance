namespace TaiyaHirameDance.Domain.TableCriteria
{
    public class TableInfoEntity
    {
        public int SchemaId { get; set; }
        public int TableId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string TableComment { get; set; }
        public int TableRowCount { get; set; }
    }
}
