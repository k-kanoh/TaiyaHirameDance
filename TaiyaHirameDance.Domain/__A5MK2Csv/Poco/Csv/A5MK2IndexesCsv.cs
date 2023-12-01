using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    [CsvName("a5m2_INDEXES.csv")]
    public class A5MK2IndexesCsv
    {
        [CsvField(1)]
        public string TableCatalog { get; set; }

        [CsvField(2)]
        public string TableSchema { get; set; }

        [CsvField(3)]
        public string TableName { get; set; }

        [CsvField(4)]
        public string IndexName { get; set; }

        [CsvField(5)]
        public string Columns { get; set; }

        [CsvField(6)]
        public string IsPrimaryKey { get; set; }

        [CsvField(7)]
        public string Unique { get; set; }

        [CsvField(8)]
        public string CreateOption { get; set; }
    }
}
