using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    [CsvName("a5m2_CONSTRAINTS.csv")]
    public class A5MK2ConstraintsCsv
    {
        [CsvField(1)]
        public string TableCatalog { get; set; }

        [CsvField(2)]
        public string TableSchema { get; set; }

        [CsvField(3)]
        public string TableName { get; set; }

        [CsvField(4)]
        public string ConstraintName { get; set; }

        [CsvField(5)]
        public string ConstraintType { get; set; }

        [CsvField(6)]
        public string ConstraintContents { get; set; }
    }
}
