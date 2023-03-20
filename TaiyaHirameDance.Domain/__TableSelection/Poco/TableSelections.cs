using System.Collections;
using System.ComponentModel;
using System.Data;
using TaiyaHirameDance.Domain.MainModels;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.TableCriteria
{
    public class TableSelections : IReadOnlyList<TableSelection>, IListSource
    {
        private List<TableSelection> _items;

        #region Interface Members

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

        public bool ContainsListCollection => true;

        public IList GetList()
        {
            return (IList)_items;
        }

        #endregion Interface Members

        internal TableSelections(List<TableSelection> items)
        {
            _items = items;
        }

        /// <summary>
        /// 現在のコレクションから無視するスキーマ・テーブルを除外します。
        /// </summary>
        public void ExcludeIgnoreSchemaAndTable()
        {
            var items = this.AsEnumerable();

            foreach (var pattern in Setting.IgnoreSchema)
                items = items.Where(x => !x.SchemaName.IsMatch(pattern));

            foreach (var pattern in Setting.IgnoreTable)
                items = items.Where(x => !x.TableName.IsMatch(pattern));

            _items = items.ToList();
        }

        /// <summary>
        /// 現在のコレクションをTableSettingsコレクションに変換します。
        /// </summary>
        public TableSettings CovertToTableSettings()
        {
            var tables = from item in this
                         where item.IsEvidenceTableCandidate
                         select new TableSetting
                         {
                             TableName = item.TableAndSchema ?? item.TableName,
                             Checked = item.IsChecked,
                             WhereClause = item.WhereClause.TrimSafety(),
                             Seq = item.Seq,
                             OmitConformity = item.OmitConformity
                         };

            return new TableSettings(tables.ToList());
        }

        /// <summary>
        /// 現在のコレクションにTableSettingsの内容をマージしたコレクションを返します。
        /// </summary>
        public TableSelections MergeTableSettings(TableSettings tables)
        {
            var merged = from a in this
                         join b in tables
                         on a.TableAndSchema ?? a.TableName equals b.TableName into outer
                         from ab in outer.DefaultIfEmpty()
                         select new TableSelection(a)
                         {
                             IsChecked = ab?.Checked ?? false,
                             WhereClause = ab?.WhereClause,
                             Seq = ab?.Seq,
                             OmitConformity = ab?.OmitConformity ?? false
                         };

            return new TableSelections(merged.ToList());
        }

        /// <summary>
        /// Where句の設定されたテーブルのレコード件数を更新します。
        /// </summary>
        public void UpdateRowCountFromQuery()
        {
            foreach (var item in this)
            {
                item.QueryRowCount = null;
                item.QueryErrorMsg = null;
            }

            using (var dao = new TableInfoDao())
            {
                foreach (var item in this)
                {
                    try
                    {
                        if (item.Expansion != null)
                        {
                            if (item.WhereClause.Val())
                            {
                                item.QueryRowCount = dao.GetQueryCount(item.Expansion.OriginalQuery, item.WhereClause);
                            }
                            else
                            {
                                item.TableRowCount = dao.GetQueryCount(item.Expansion.OriginalQuery);
                            }
                        }
                        else
                        {
                            if (item.WhereClause.Val())
                            {
                                item.QueryRowCount = dao.GetRowCount(item.TableAndSchema, item.WhereClause);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        item.QueryErrorMsg = ex.Message;
                    }
                }
            }
        }

        /// <summary>
        /// SQLディレクトリに配置されたオリジナルクエリで現在のコレクションを拡張します。
        /// </summary>
        public void Expansion()
        {
            _items.AddRange(TableInfoUtil.GetOriginalQueries());
        }
    }
}
