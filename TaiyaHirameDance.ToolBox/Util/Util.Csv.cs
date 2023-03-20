using Csv;

namespace TaiyaHirameDance.ToolBox
{
    public static class UtilCsv
    {
        public static List<T> CsvReadFromText<T>(string text, bool skipHeaderRow = false) where T : new()
        {
            var props = from p in typeof(T).GetProperties()
                        where p.AnyCustomAttribute<CsvFieldAttribute>()
                        select new { prop = p, p.FirstCustomAttribute<CsvFieldAttribute>().FieldNo };

            var list = new List<T>();

            var options = new CsvOptions()
            {
                SkipRow = (row, idx) => row.Span.IsEmpty,
                TrimData = true,
                AllowNewLineInEnclosedFieldValues = true,
                HeaderMode = skipHeaderRow ? HeaderMode.HeaderAbsent : HeaderMode.HeaderPresent
            };

            foreach (var line in CsvReader.ReadFromText(text, options))
            {
                var instance = new T();

                foreach (var p in props)
                    p.prop.SetValue(instance, line[p.FieldNo]);

                list.Add(instance);
            }

            return list;
        }

        public static string CsvWriteToText<T>(IEnumerable<T> data, bool skipHeaderRow = false)
        {
            var headers = from p in typeof(T).GetProperties()
                          where p.AnyCustomAttribute<CsvFieldAttribute>()
                          orderby p.FirstCustomAttribute<CsvFieldAttribute>().FieldNo
                          select p.Name;

            var contents = data.Select(x => (from p in x.GetType().GetProperties()
                                             where p.AnyCustomAttribute<CsvFieldAttribute>()
                                             orderby p.FirstCustomAttribute<CsvFieldAttribute>().FieldNo
                                             select (string)p.GetValue(x) ?? "").ToArray());

            return CsvWriter.WriteToText(headers.ToArray(), contents, skipHeaderRow: skipHeaderRow);
        }
    }
}
