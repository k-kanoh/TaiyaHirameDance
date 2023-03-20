using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    [CsvName("a5m2_COLUMNS.csv")]
    public class A5MK2ColumnsCsv
    {
        [CsvField(1)]
        public string TableCatalog { get; set; }

        [CsvField(2)]
        public string TableSchema { get; set; }

        [CsvField(3)]
        public string TableName { get; set; }

        [CsvField(4)]
        public string ColumnName { get; set; }

        [CsvField(5)]
        public string LogicalName { get; set; }

        [CsvField(6)]
        public string OrdinalPosition { get; set; }

        [CsvField(7)]
        public string ColumnDefault { get; set; }

        [CsvField(8)]
        public string IsNullable { get; set; }

        [CsvField(9)]
        public string DataType { get; set; }

        [CsvField(10)]
        public string KeyPosition { get; set; }

        [CsvField(11)]
        public string Description { get; set; }
    }
}
