using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    public class A5MK2Util
    {
        internal static List<A5MK2ColumnInfo> CreateColumnInfo(A5MK2CsvContainer a5mk2)
        {
            var columns = (from c in a5mk2.Columns
                           join t in a5mk2.Tables on new { c.TableSchema, c.TableName } equals new { t.TableSchema, t.TableName }
                           select new A5MK2ColumnInfo(c) { TableLogicalName = t.LogicalName }).ToList();

            var tables = columns.GroupBy(x => new { x.TableSchema, x.TableName });

            foreach (var t in tables)
            {
                var pKeys = a5mk2.Constraints.SingleOrDefault(x => x.TableSchema == t.Key.TableSchema && x.TableName == t.Key.TableName &&
                                    x.ConstraintType == "PRIMARY KEY")?.ConstraintContents.Split(',').Select((name, idx) => new { name, keyNo = idx + 1 });

                if (pKeys != null)
                    foreach (var pKey in pKeys)
                        t.Single(x => x.ColumnName == pKey.name).PKeyNo = pKey.keyNo;
            }

            return columns;
        }

        public static List<A5MK2ColumnInfo> ExcludeIgnoreSchemaAndTable(List<A5MK2ColumnInfo> source)
        {
            var columns = source.AsEnumerable();

            foreach (var pattern in Setting.IgnoreSchema)
                columns = columns.Where(x => !x.TableSchema.IsMatch(pattern));

            foreach (var pattern in Setting.IgnoreTable)
                columns = columns.Where(x => !x.TableName.IsMatch(pattern));

            SetBunruiPartiteByTableName(columns);

            return columns.ToList();
        }

        private static void SetBunruiPartiteByTableName(IEnumerable<A5MK2ColumnInfo> columns)
        {
            var i = 0;
            foreach (var column in columns)
            {
                if (column.FieldNo == 1)
                    i++;

                column.ColorBunrui = i;
            }
        }
    }
}
