using System.Collections;

namespace TaiyaHirameDance.Domain.MainModels
{
    public class TableSettings : IList<TableSetting>
    {
        private readonly IList<TableSetting> _items;

        public TableSettings()
        {
            _items = [];
        }

        public TableSettings(IList<TableSetting> items)
        {
            _items = items;
        }

        #region Interface Members

        public TableSetting this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => _items.IsReadOnly;

        public void Add(TableSetting item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(TableSetting item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(TableSetting[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TableSetting> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(TableSetting item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, TableSetting item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(TableSetting item)
        {
            return _items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion Interface Members

        /// <summary>
        /// グリッドで☑のものだけを抽出します。
        /// </summary>
        public TableSettings FilteredChecked()
        {
            return new TableSettings(this.Where(x => x.Checked).ToList());
        }
    }
}
