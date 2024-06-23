using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    public class ColumnInfo : A5MK2ColumnsCsv, IColorBunruiGrid
    {
        public int FieldNo { get; set; }
        public int? PKeyNo { get; set; }
        public long ColorBunrui { get; set; }
        public string TableLogicalName { get; set; }

        public string TableAndSchema => $"{TableSchema}.{TableName}";
        public string DataTypeUpper => DataType.ToUpper().Replace(", ", ",");

        public bool WordMatching(string word)
        {
            return new[] { TableAndSchema, TableLogicalName, ColumnName, LogicalName, DataType, Description }.Any(x => x.WideContains(word));
        }

        public bool RegexMatching(string pattern)
        {
            return new[] { TableAndSchema, TableLogicalName, ColumnName, LogicalName, DataType, Description }.Any(x => x?.IsMatch(pattern) ?? false);
        }

        public ColumnInfo(A5MK2ColumnsCsv csv)
        {
            Util.ShallowCopy(csv, this);
            FieldNo = OrdinalPosition.ToInt() ?? default;
        }
    }
}
