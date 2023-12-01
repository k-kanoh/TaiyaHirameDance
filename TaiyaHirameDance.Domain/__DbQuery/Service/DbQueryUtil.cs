using TaiyaHirameDance.Domain.A5MK2Csv;
using TaiyaHirameDance.Domain.Repository;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    public class DbQueryUtil
    {
        public static List<DapperQueryContainer> CreateQueryContainer(List<Evidence.Table> tables)
        {
            var items = new List<DapperQueryContainer>();

            foreach (var table in tables)
            {
                var colInfo = A5MK2.Columns.Where(x => x.TableAndSchema == table.TableName);
                var keyInfo = from x in colInfo where x.PKeyNo.HasValue orderby x.PKeyNo select x;
                var whereClause = (table.WhereClause.Val() ? $" WHERE {table.WhereClause}" : "");
                var orderClause = (keyInfo.Any() ? $" ORDER BY {string.Join(", ", keyInfo.Select(x => x.ColumnName))}" : "");
                var colInfoOne = colInfo.First();

                var item = new DapperQueryContainer()
                {
                    Seq = table.Seq ?? 334,
                    TableName = table.TableName,
                    TableLogicalName = $"{colInfoOne.TableSchema}.{colInfoOne.TableLogicalName.ValOrNull() ?? colInfoOne.TableName}",
                    ColumnName = colInfo.Select(x => x.ColumnName).ToArray(),
                    LogicalName = colInfo.Select(x => x.LogicalName).ToArray(),
                    KeyFieldIdx = keyInfo.Select(x => x.FieldNo - 1).ToArray(),
                    Query = $"SELECT * FROM {table.TableName}{whereClause}{orderClause}"
                };

                if (Setting.TableColor.Any(x => colInfoOne.TableName.IsMatch(x.pattern)))
                    item.SheetColor = Setting.TableColor.First(x => colInfoOne.TableName.IsMatch(x.pattern)).color;

                items.Add(item);
            }

            return items.OrderBy(x => x.Seq).ToList();
        }

        public static void FetchByQuery(List<DapperQueryContainer> containers)
        {
            var now = Util.Now;
            using (var res = QueryDao.MultipleQuery(containers.Select(x => x.Query)).GetEnumerator())
            {
                foreach (var item in containers)
                {
                    res.MoveNext();
                    var values = res.Current;

                    item.FetchAt = now;
                    item.Values = new Dictionary<string, object[]>();
                    foreach (var data in values.GetRows())
                        item.Values.Add(GetKeyHash(data, item.KeyFieldIdx), data);
                }
            }
        }

        public static void FetchByQuery2(List<DapperQueryContainer> containers)
        {
            var now = Util.Now;
            using (var res = QueryDao.MultipleQuery(containers.Select(x => x.Query)).GetEnumerator())
            {
                foreach (var item in containers)
                {
                    res.MoveNext();
                    var values = res.Current;

                    item.FetchAt2 = now;
                    item.Values2 = new Dictionary<string, object[]>();
                    foreach (var data in values.GetRows())
                        item.Values2.Add(GetKeyHash(data, item.KeyFieldIdx), data);
                }
            }

            foreach (var item in containers)
            {
                var insertRows = from x in item.Values2
                                 where !item.Values.ContainsKey(x.Key)
                                 select new KeyValuePair<string, object[]>(x.Key, SetCompareRowKbn(x.Value, CompareRowKbn.追加));

                var deleteRows = from x in item.Values
                                 where !item.Values2.ContainsKey(x.Key)
                                 select new KeyValuePair<string, object[]>(x.Key, SetCompareRowKbn(x.Value, CompareRowKbn.削除));

                item.CompareRows = insertRows.Concat(deleteRows).ToList();

                item.UpdateIdx = new Dictionary<string, int[]>();

                var pairs = from a in item.Values
                            join b in item.Values2 on a.Key equals b.Key
                            select new { key = a.Key, value1 = a.Value, value2 = b.Value };

                foreach (var pair in pairs)
                {
                    var discrepancy = pair.value1.Select((value1, i) => new { value1, i }).Where(x => !Equals(x.value1, pair.value2[x.i])).Select(y => y.i);

                    if (discrepancy.Any())
                    {
                        item.UpdateIdx.Add(pair.key, discrepancy.ToArray());
                        item.CompareRows.Add(pair.key, SetCompareRowKbn(pair.value1, CompareRowKbn.更新前));
                        item.CompareRows.Add(pair.key, SetCompareRowKbn(pair.value2, CompareRowKbn.更新後));
                    }
                    else
                    {
                        if (!Setting.OmitCompareRowIsConformity)
                            item.CompareRows.Add(pair.key, SetCompareRowKbn(pair.value1, CompareRowKbn.OK));
                    }
                }
            }
        }

        private static object[] SetCompareRowKbn(object[] data, CompareRowKbn compareKbn)
        {
            return new object[] { compareKbn }.Concat(data).ToArray();
        }

        private static string GetKeyHash(object[] data, int[] keys)
        {
            var values = new List<object>();

            if (keys.Any())
            {
                foreach (var i in keys)
                    values.Add(data[i]);
            }
            else
            {
                values = data.ToList();
            }

            return Util.GetSha1Hash(string.Join("\t", values.Select(x => (x is DateTime dt) ? dt.Ticks : x)));
        }
    }
}
