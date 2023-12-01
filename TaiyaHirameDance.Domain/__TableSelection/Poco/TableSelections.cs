using System.Collections;
using System.ComponentModel;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.TableSelection
{
    public class TableSelections : IReadOnlyList<TableSelection>, IListSource
    {
        #region Interface Members

        private List<TableSelection> _items;

        public TableSelections(List<TableSelection> items)
        {
            _items = items;
        }

        public TableSelection this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<TableSelection> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool ContainsListCollection => _items is IList;

        public IList GetList()
        {
            return _items;
        }

        #endregion Interface Members

        /// <summary>
        /// 無視するスキーマ・テーブルを適用します。
        /// </summary>
        public void ExcludeIgnoreSchemaAndTable()
        {
            var items = _items.AsEnumerable();

            foreach (var pattern in Setting.IgnoreSchema)
                items = items.Where(x => !x.SchemaName.IsMatch(pattern));

            foreach (var pattern in Setting.IgnoreTable)
                items = items.Where(x => !x.TableName.IsMatch(pattern));

            _items = items.ToList();
        }

        /// <summary>
        /// Evidence.Tableのリストに変換します。
        /// </summary>
        public List<Evidence.Table> CovertToTableEvidence()
        {
            var tables = from item in _items
                         where (item.IsChecked || item.WhereClause.TrimSafety().Val()) && !item.IsQueryError
                         select new Evidence.Table()
                         {
                             TableName = item.TableAndSchema,
                             Checked = item.IsChecked,
                             WhereClause = item.WhereClause.TrimSafety(),
                             Seq = item.Seq
                         };

            return tables.ToList();
        }

        /// <summary>
        /// Evidence.Tableのリストを取り込んでマージした結果を返します。
        /// </summary>
        public TableSelections MergeTableEvidence(List<Evidence.Table> eviTables)
        {
            var merged = from a in _items
                         join b in eviTables
                         on a.TableAndSchema equals b.TableName into outer
                         from ab in outer.DefaultIfEmpty()
                         select new TableSelection(a)
                         {
                             IsChecked = ab?.Checked ?? false,
                             WhereClause = ab?.WhereClause,
                             Seq = ab?.Seq
                         };

            return new TableSelections(merged.ToList());
        }

        /// <summary>
        /// Where句の設定されたテーブルのレコード件数を更新します。
        /// </summary>
        public void UpdateRowCountFromQuery()
        {
            foreach (var item in _items)
            {
                item.QueryRowCount = null;
                item.IsQueryError = false;
            }

            var targets = _items.Where(x => x.WhereClause.TrimSafety().Val());

            using (var dao = new TableInfoDao())
            {
                foreach (var item in targets)
                {
                    try
                    {
                        item.QueryRowCount = dao.GetRowCount(item.TableAndSchema, item.WhereClause);
                    }
                    catch
                    {
                        item.IsQueryError = true;
                    }
                }
            }
        }
    }
}
