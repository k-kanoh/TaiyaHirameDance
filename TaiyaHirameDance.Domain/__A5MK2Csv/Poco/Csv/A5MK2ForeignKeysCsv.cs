using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    [CsvName("a5m2_FOREIGN_KEYS.csv")]
    public class A5MK2ForeignKeysCsv
    {
        [CsvField(1)]
        public string ConstraintCatalog { get; set; }

        [CsvField(2)]
        public string ConstraintSchema { get; set; }

        [CsvField(3)]
        public string ConstraintName { get; set; }

        [CsvField(4)]
        public string TableSchema { get; set; }

        [CsvField(5)]
        public string TableName { get; set; }

        [CsvField(6)]
        public string Columns { get; set; }

        [CsvField(7)]
        public string Cardinarity { get; set; }

        [CsvField(8)]
        public string ReferencedTableSchema { get; set; }

        [CsvField(9)]
        public string ReferencedTableName { get; set; }

        [CsvField(10)]
        public string ReferencedColumns { get; set; }

        [CsvField(11)]
        public string ReferencedCardinarity { get; set; }

        [CsvField(12)]
        public string Caption { get; set; }
    }
}
