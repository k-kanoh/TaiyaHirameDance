using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    public class A5MK2
    {
        private A5MK2()
        { }

        private static A5MK2CsvContainer Container;

        public static List<A5MK2ConstraintsCsv> Constraints
        {
            get => Container.Constraints;
            set => Container.Constraints = value;
        }

        public static List<A5MK2ExtendPropertiesCsv> ExtendProperties
        {
            get => Container.ExtendProperties;
            set => Container.ExtendProperties = value;
        }

        public static List<A5MK2ForeignKeysCsv> ForeignKeys
        {
            get => Container.ForeignKeys;
            set => Container.ForeignKeys = value;
        }

        public static List<A5MK2IndexesCsv> Indexes
        {
            get => Container.Indexes;
            set => Container.Indexes = value;
        }

        public static List<A5MK2TablesCsv> Tables
        {
            get => Container.Tables;
            set => Container.Tables = value;
        }

        public static List<A5MK2TriggersCsv> Triggers
        {
            get => Container.Triggers;
            set => Container.Triggers = value;
        }

        private static string YamlHash;

        internal static ColumnInfos ColumnInfos { get; private set; }

        public static void Save()
        {
            YamlUtil.YamlSave(Common.WorkDir, "A5MK2", Container, true);
            Reload();
        }

        public static void Reload()
        {
            if (YamlUtil.YamlLoadOrNew<A5MK2CsvContainer>(Common.WorkDir, "A5MK2", out var instance, ref YamlHash))
                Container = instance;

            ColumnInfos = ColumnInfos.CreateColumnInfo(Container);
        }
    }
}
