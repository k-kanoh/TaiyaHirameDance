using System.Collections;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.BcpUtility
{
    public class BcpContainers : IList<BcpContainer>
    {
        private readonly IList<BcpContainer> _items;

        #region Interface Members

        public BcpContainer this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => _items.IsReadOnly;

        public void Add(BcpContainer item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(BcpContainer item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(BcpContainer[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<BcpContainer> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(BcpContainer item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, BcpContainer item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(BcpContainer item)
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

        private BcpContainers(IList<BcpContainer> items)
        {
            _items = items;
        }

        public static BcpContainers LoadBcpContainers(string dbName)
        {
            var dinfo = Util.Roaming.SubDirectory("bcp", dbName);

            if (!dinfo.Exists)
                return new BcpContainers([]);

            var containers = from fi in dinfo.GetFiles("*.yml")
                             let container = YamlUtil.YamlLoad<BcpContainer>(dinfo, fi.GetFileNameWithoutExtension())
                             orderby container.RegistedAt
                             select container;

            return new BcpContainers(containers.ToList());
        }

        public ComboBoxItems GetComboBoxItems()
        {
            var items = this.Select(x => new ComboBoxItem(x.Hash, x.ComboBoxDispName, x)).ToComboBoxItems();
            items.PrependEmptyItem();

            return items;
        }
    }
}
