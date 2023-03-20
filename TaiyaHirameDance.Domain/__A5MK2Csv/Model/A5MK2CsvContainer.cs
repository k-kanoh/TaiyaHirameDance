using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    internal class A5MK2CsvContainer
    {
        public IDictionary<string, string> Hash { get; set; } = new Dictionary<string, string>();

        public List<A5MK2ColumnsCsv> Columns { get; set; } = [];

        public List<A5MK2ConstraintsCsv> Constraints { get; set; } = [];

        public List<A5MK2ExtendPropertiesCsv> ExtendProperties { get; set; } = [];

        public List<A5MK2ForeignKeysCsv> ForeignKeys { get; set; } = [];

        public List<A5MK2IndexesCsv> Indexes { get; set; } = [];

        public List<A5MK2TablesCsv> Tables { get; set; } = [];

        public List<A5MK2TriggersCsv> Triggers { get; set; } = [];

        public A5MK2CsvContainer()
        {
            foreach (var prop in GetType().GetProperties())
            {
                var type = prop.PropertyType.GenericTypeArguments[0];
                var name = type.FirstCustomAttribute<CsvNameAttribute>()?.Name;

                if (name != null)
                    Hash.Add(name, null);
            }
        }
    }
}
