using System.Collections;
using System.ComponentModel;
using TaiyaHirameDance.Domain.TableCriteria;

namespace TaiyaHirameDance.Domain.BcpUtility
{
    public class BcpTableSelections : IReadOnlyList<BcpTableSelection>, IListSource
    {
        private List<BcpTableSelection> _items;

        #region Interface Members

        public BcpTableSelection this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<BcpTableSelection> GetEnumerator()
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
            return _items;
        }

        #endregion Interface Members

        private readonly TableSelections _tableSelections;

        internal BcpTableSelections(List<BcpTableSelection> items)
        {
            _items = items;
        }

        private BcpTableSelections(TableSelections tableSelections)
        {
            _tableSelections = tableSelections;
            ResetItems();
        }

        public static BcpTableSelections CreateBcpTableSelections(TableSelections tableSelections)
        {
            return new BcpTableSelections(tableSelections);
        }

        private void ResetItems()
        {
            _items = _tableSelections.Select(x => new BcpTableSelection(x)).ToList();
        }

        /// <summary>
        /// 現在のコレクションにコンテナの情報をマージします。
        /// </summary>
        public void MergeBcpContainer(BcpContainer container)
        {
            if (container != null)
            {
                var merged = from a in _tableSelections
                             join b in container.Tables
                             on a.TableAndSchema equals b.TableName into outer
                             from ab in outer.DefaultIfEmpty()
                             select new BcpTableSelection(a)
                             {
                                 IsChecked = ab != null,
                                 BackupedCount = ab?.Count,
                             };

                _items = merged.ToList();

                foreach (var item in _items)
                    item.IsOwnedStash = item.StashTableNameParts.container == container.HashPrefix;
            }
            else
            {
                ResetItems();
            }
        }

        /// <summary>
        /// 現在のコレクションからスタッシュテーブルを除外します。
        /// </summary>
        public void ExcludeStash(bool keepOwnedStash = false)
        {
            if (keepOwnedStash)
            {
                _items = this.Where(x => !x.IsStash || x.IsOwnedStash).ToList();
            }
            else
            {
                _items = this.Where(x => !x.IsStash).ToList();
            }
        }

        /// <summary>
        /// グリッドで☑のものだけを抽出します。
        /// </summary>
        public BcpTableSelections FilteredChecked()
        {
            return this.Where(x => x.IsChecked).ToBcpTableSelections();
        }

        /// <summary>
        /// コンテナが所有しているスタッシュテーブルだけを抽出します。
        /// </summary>
        public BcpTableSelections FilteredOwnedStash()
        {
            return this.Where(x => x.IsOwnedStash).ToBcpTableSelections();
        }

        /// <summary>
        /// スキーマ名とテーブル名のタプルのリストを返します。
        /// </summary>
        /// <returns></returns>
        public List<(string, string)> ToSchemaAndTableNameTuples()
        {
            return this.Select(x => (x.SchemaName, x.TableName)).ToList();
        }
    }
}
