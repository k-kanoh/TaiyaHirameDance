using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    [CsvName("a5m2_TRIGGERS.csv")]
    public class A5MK2TriggersCsv
    {
        [CsvField(1)]
        public string TriggerCatalog { get; set; }

        [CsvField(2)]
        public string TriggerSchema { get; set; }

        [CsvField(3)]
        public string TriggerName { get; set; }

        [CsvField(4)]
        public string EventManipulation { get; set; }

        [CsvField(5)]
        public string EventObjectCatalog { get; set; }

        [CsvField(6)]
        public string EventObjectSchema { get; set; }

        [CsvField(7)]
        public string EventObjectTable { get; set; }

        [CsvField(8)]
        public string ActionOrder { get; set; }

        [CsvField(9)]
        public string ActionCondition { get; set; }

        [CsvField(10)]
        public string ActionOrientation { get; set; }

        [CsvField(11)]
        public string ActionTiming { get; set; }

        [CsvField(12)]
        public string ActionReferenceOldTable { get; set; }

        [CsvField(13)]
        public string ActionReferenceNewTable { get; set; }

        [CsvField(14)]
        public string ActionReferenceOldRow { get; set; }

        [CsvField(15)]
        public string ActionReferenceNewRow { get; set; }

        [CsvField(16)]
        public string Created { get; set; }

        [CsvField(17)]
        public string UpdateColumnNames { get; set; }
    }
}
