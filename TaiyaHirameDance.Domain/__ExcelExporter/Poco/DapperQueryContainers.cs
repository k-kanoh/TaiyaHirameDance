using System.Collections;
using TaiyaHirameDance.Domain.A5MK2Csv;
using TaiyaHirameDance.Domain.MainModels;
using TaiyaHirameDance.Domain.TableCriteria;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ExcelExporter
{
    public class DapperQueryContainers : IReadOnlyList<DapperQueryContainer>
    {
        private readonly IList<DapperQueryContainer> _items;

        #region Interface Members

        public DapperQueryContainer this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<DapperQueryContainer> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion Interface Members

        private DapperQueryContainers(IList<DapperQueryContainer> items)
        {
            _items = items;
        }

        public static DapperQueryContainers CreateQueryContainers(TableSettings tables)
        {
            var items = new List<DapperQueryContainer>();
            var origQueries = TableInfoUtil.GetOriginalQueries();

            foreach (var table in tables)
            {
                if (table.TableName.IsMatch(@"\.yml$"))
                {
                    var whereClause = (table.WhereClause.Val() ? $" WHERE {table.WhereClause}" : "");

                    var origQuery = origQueries.FirstOrDefault(x => x.TableName == table.TableName);
                    if (origQuery == null) continue;

                    var item = new DapperQueryContainer()
                    {
                        Seq = table.Seq ?? 334,
                        TableLogicalName = origQuery.TableName,
                        KeyFieldIdx = origQuery.Expansion.OriginalKeys.Select(x => x - 1).ToArray(),
                        PreQuery = origQuery.Expansion.OriginalPreQuery,
                        Query = $"WITH ORIGINALQUERY AS (\r\n{origQuery.Expansion.OriginalQuery})\r\nSELECT * FROM ORIGINALQUERY{whereClause}",
                        OmitConformity = table.OmitConformity
                    };

                    items.Add(item);
                }
                else
                {
                    var colInfo = A5MK2.ColumnInfos.Where(x => x.TableAndSchema == table.TableName);
                    var keyInfo = from x in colInfo where x.PKeyNo.HasValue orderby x.PKeyNo select x;
                    var whereClause = table.WhereClause.Val() ? $" WHERE {table.WhereClause}" : "";
                    var orderClause = keyInfo.Any() ? $" ORDER BY {string.Join(", ", keyInfo.Select(x => x.ColumnName))}" : "";
                    var colInfoOne = colInfo.FirstOrDefault() ?? throw new InvalidOperationException("テーブル情報が見つかりません。A5MK2のテーブル定義CSVを最新にしてください。");

                    var item = new DapperQueryContainer()
                    {
                        Seq = table.Seq ?? 334,
                        TableName = table.TableName,
                        TableLogicalName = $"{colInfoOne.TableSchema}.{colInfoOne.TableLogicalName.ValOrNull() ?? colInfoOne.TableName}",
                        ColumnName = colInfo.Select(x => x.ColumnName).ToArray(),
                        LogicalName = colInfo.Select(x => x.LogicalName).ToArray(),
                        KeyFieldIdx = keyInfo.Select(x => x.FieldNo - 1).ToArray(),
                        Query = $"SELECT * FROM {table.TableName}{whereClause}{orderClause}",
                        OmitConformity = table.OmitConformity
                    };

                    if (Setting.TableColor.Any(x => colInfoOne.TableName.IsMatch(x.pattern)))
                        item.SheetColor = Setting.TableColor.First(x => colInfoOne.TableName.IsMatch(x.pattern)).color;

                    items.Add(item);
                }
            }

            return new DapperQueryContainers(items.OrderBy(x => x.Seq).ToList());
        }

        public void FetchByQuery()
        {
            var now = DateTime.Now;
            var dao = new Dao();

            dao.ExecutePreQuery(this.Select(x => x.PreQuery));

            using (var res = dao.MultipleQuery(this.Select(x => x.Query)).GetEnumerator())
            {
                foreach (var item in this)
                {
                    res.MoveNext();
                    var (fields, values) = res.Current;

                    item.ColumnName ??= fields;
                    item.LogicalName ??= new string[fields.Length];
                    item.FetchedAt = now;
                    item.Values = new List<(string, object[])>();

                    foreach (var row in values.GetRows())
                        item.Values.Add((GetKeyHash(row, item.KeyFieldIdx), row));

                    var orderedRows = item.Values.OrderBy(x => x.data[0]);

                    if (item.KeyFieldIdx.Any())
                    {
                        orderedRows = item.Values.OrderBy(x => x.data[item.KeyFieldIdx.First()]);

                        foreach (var idx in item.KeyFieldIdx.Skip(1))
                            orderedRows = orderedRows.ThenBy(x => x.data[idx]);
                    }

                    item.Values = orderedRows.ToList();
                }
            }
        }

        public void FetchByQuery2()
        {
            var now = DateTime.Now;
            var dao = new Dao();

            dao.ExecutePreQuery(this.Select(x => x.PreQuery));

            using (var res = dao.MultipleQuery(this.Select(x => x.Query)).GetEnumerator())
            {
                foreach (var item in this)
                {
                    res.MoveNext();
                    var (_, values) = res.Current;

                    item.FetchedAt2 = now;
                    item.Values2 = new List<(string, object[])>();

                    foreach (var row in values.GetRows())
                        item.Values2.Add((GetKeyHash(row, item.KeyFieldIdx), row));
                }
            }

            foreach (var item in this)
            {
                var insertRows = from x in item.Values2
                                 where !item.Values.Any(y => y.key == x.key)
                                 select (x.key, CompareRowKbn.追加, x.data);

                var deleteRows = from x in item.Values
                                 where !item.Values2.Any(y => y.key == x.key)
                                 select (x.key, CompareRowKbn.削除, x.data);

                item.CompareRows = insertRows.Concat(deleteRows).ToList();

                item.UpdateIdx = new Dictionary<string, int[]>();

                var pairs = from a in item.Values
                            join b in item.Values2 on a.key equals b.key
                            select new { a.key, value1 = a.data, value2 = b.data };

                foreach (var pair in pairs)
                {
                    var discrepancy = pair.value1.Select((value1, i) => new { value1, i }).Where(x => !Equals(x.value1, pair.value2[x.i])).Select(y => y.i);

                    if (discrepancy.Any())
                    {
                        item.UpdateIdx.Add(pair.key, discrepancy.ToArray());
                        item.CompareRows.Add((pair.key, CompareRowKbn.更新前, pair.value1));
                        item.CompareRows.Add((pair.key, CompareRowKbn.更新後, pair.value2));
                    }
                    else
                    {
                        if (!item.OmitConformity)
                            item.CompareRows.Add((pair.key, CompareRowKbn.OK, pair.value1));
                    }
                }

                var orderedRows = item.CompareRows.OrderBy(x => x.data[0]);

                if (item.KeyFieldIdx.Any())
                {
                    orderedRows = item.CompareRows.OrderBy(x => x.data[item.KeyFieldIdx.First()]);

                    foreach (var idx in item.KeyFieldIdx.Skip(1))
                        orderedRows = orderedRows.ThenBy(x => x.data[idx]);
                }

                item.CompareRows = orderedRows.ThenBy(x => (int)x.kbn).ToList();
            }
        }

        private string GetKeyHash(object[] data, int[] keys)
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
