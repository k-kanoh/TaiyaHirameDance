using System.Reflection;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    public class A5MK2CsvLoader
    {
        public static bool LoadCsv(DirectoryInfo dinfo)
        {
            var res = false;
            var a5m2 = typeof(A5MK2).GetField("Container", BindingFlags.Static | BindingFlags.NonPublic);
            var staticObj = (A5MK2CsvContainer)a5m2.GetValue(null);

            foreach (var listProp in a5m2.FieldType.GetProperties())
            {
                var type = listProp.PropertyType.GenericTypeArguments[0];
                var csv = dinfo.File(type.FirstCustomAttribute<CsvNameAttribute>()?.Name);

                if (!csv.Exists)
                    continue;

                var text = File.ReadAllText(csv.FullName, Util.SJIS);

                var hash = Util.GetSha1Hash(text);

                if (staticObj.Hash[csv.Name] == hash)
                {
                    continue;
                }
                else
                {
                    staticObj.Hash[csv.Name] = hash;
                    res = true;
                }

                if (type == typeof(A5MK2ColumnsCsv))
                {
                    staticObj.Columns = UtilCsv.CsvReadFromText<A5MK2ColumnsCsv>(text);
                }
                else if (type == typeof(A5MK2ConstraintsCsv))
                {
                    staticObj.Constraints = UtilCsv.CsvReadFromText<A5MK2ConstraintsCsv>(text);
                }
                else if (type == typeof(A5MK2ExtendPropertiesCsv))
                {
                    staticObj.ExtendProperties = UtilCsv.CsvReadFromText<A5MK2ExtendPropertiesCsv>(text);
                }
                else if (type == typeof(A5MK2ForeignKeysCsv))
                {
                    staticObj.ForeignKeys = UtilCsv.CsvReadFromText<A5MK2ForeignKeysCsv>(text);
                }
                else if (type == typeof(A5MK2IndexesCsv))
                {
                    staticObj.Indexes = UtilCsv.CsvReadFromText<A5MK2IndexesCsv>(text);
                }
                else if (type == typeof(A5MK2TablesCsv))
                {
                    staticObj.Tables = UtilCsv.CsvReadFromText<A5MK2TablesCsv>(text);
                }
                else if (type == typeof(A5MK2TriggersCsv))
                {
                    staticObj.Triggers = UtilCsv.CsvReadFromText<A5MK2TriggersCsv>(text);
                }
            }

            return res;
        }
    }
}
