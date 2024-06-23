using System.Collections;
using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class ComboBoxItems(List<ComboBoxItem> _items) : IReadOnlyList<ComboBoxItem>, IListSource
    {
        #region Interface Members

        public ComboBoxItem this[int index] => _items[index];

        public int Count => _items.Count;

        public IEnumerator<ComboBoxItem> GetEnumerator()
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

        public void PrependEmptyItem(string dispName = null)
        {
            if (!_items.Any(x => !x.Key.Val()))
                _items.Insert(0, new ComboBoxItem() { DispName = dispName });
        }
    }
}
