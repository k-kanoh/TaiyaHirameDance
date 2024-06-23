using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    [CsvName("a5m2_TABLES.csv")]
    public class A5MK2TablesCsv
    {
        [CsvField(1)]
        public string TableCatalog { get; set; }

        [CsvField(2)]
        public string TableSchema { get; set; }

        [CsvField(3)]
        public string TableName { get; set; }

        [CsvField(4)]
        public string LogicalName { get; set; }

        [CsvField(5)]
        public string TableType { get; set; }

        [CsvField(6)]
        public string Description { get; set; }

        [CsvField(7)]
        public string Tag { get; set; }
    }
}
