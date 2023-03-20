using System.Collections;
using System.ComponentModel;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    public class ColumnInfos : IReadOnlyList<ColumnInfo>, IListSource
    {
        private IList<ColumnInfo> _items;

        #region Interface Members

        public ColumnInfo this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<ColumnInfo> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool ContainsListCollection => true;

        public IList GetList()
        {
            return (IList)_items;
        }

        #endregion Interface Members

        internal ColumnInfos(IList<ColumnInfo> items)
        {
            _items = items;
        }

        internal static ColumnInfos CreateColumnInfo(A5MK2CsvContainer csv)
        {
            var columns = (from c in csv.Columns
                           join t in csv.Tables on new { c.TableSchema, c.TableName } equals new { t.TableSchema, t.TableName }
                           select new ColumnInfo(c) { TableLogicalName = t.LogicalName }).ToList();

            var tables = columns.GroupBy(x => new { x.TableSchema, x.TableName });

            foreach (var t in tables)
            {
                var pKeys = csv.Constraints.SingleOrDefault(x => x.TableSchema == t.Key.TableSchema && x.TableName == t.Key.TableName &&
                                    x.ConstraintType == "PRIMARY KEY")?.ConstraintContents.Split(',').Select((name, idx) => new { name, keyNo = idx + 1 });

                if (pKeys != null)
                    foreach (var pKey in pKeys)
                        t.Single(x => x.ColumnName == pKey.name).PKeyNo = pKey.keyNo;
            }

            return new ColumnInfos(columns.ToList());
        }

        public void ExcludeIgnoreSchemaAndTable()
        {
            var columns = this.AsEnumerable();

            foreach (var pattern in Setting.IgnoreSchema)
                columns = columns.Where(x => !x.TableSchema.IsMatch(pattern));

            foreach (var pattern in Setting.IgnoreTable)
                columns = columns.Where(x => !x.TableName.IsMatch(pattern));

            _items = columns.ToList();

            SetBunruiPartiteByTableName();
        }

        private void SetBunruiPartiteByTableName()
        {
            var i = 0;
            foreach (var column in this)
            {
                if (column.FieldNo == 1)
                    i++;

                column.ColorBunrui = i;
            }
        }

        public void FilterByPhraseWord(string phraseWord)
        {
            var columns = this.AsEnumerable();

            foreach (var word in phraseWord.SplitByWhiteSpace())
                columns = columns.Where(x => x.WordMatching(word));

            _items = columns.ToList();
        }

        public void FilterByRegex(string pattern)
        {
            _items = this.Where(x => x.RegexMatching(pattern)).ToList();
        }
    }
}
