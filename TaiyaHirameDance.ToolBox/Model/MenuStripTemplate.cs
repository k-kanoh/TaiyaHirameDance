using System.Collections;

namespace TaiyaHirameDance.ToolBox
{
    public class MenuStripTemplate : IList<MenuStripTemplate>
    {
        #region Interface Members

        private readonly IList<MenuStripTemplate> _items = new List<MenuStripTemplate>();

        public MenuStripTemplate this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => _items.IsReadOnly;

        public void Add(MenuStripTemplate item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(MenuStripTemplate item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(MenuStripTemplate[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MenuStripTemplate> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(MenuStripTemplate item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, MenuStripTemplate item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(MenuStripTemplate item)
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

        public string Name { get; set; }
        public Action Method { get; set; }
        public Keys Shortcut { get; set; }
        public Image Icon { get; set; }
        public Func<bool> CanExecute { get; set; }
        public Func<bool> CheckMarkValidation { get; set; }

        public MenuStripTemplate()
        { }

        public MenuStripTemplate(string name, Action method = null, Func<bool> canExecute = null, Keys? shortcut = null)
        {
            Name = name;
            Method = method;
            CanExecute = canExecute;

            if (shortcut.HasValue)
                Shortcut = shortcut.Value;
        }

        public MenuStripTemplate(string name, Action method, Func<bool> canExecute, Func<bool> checkMarkValidation, Keys? shortcut = null)
        {
            Name = name;
            Method = method;
            CanExecute = canExecute;
            CheckMarkValidation = checkMarkValidation;

            if (shortcut.HasValue)
                Shortcut = shortcut.Value;
        }

        public void Add(string name, Action method = null, Func<bool> canExecute = null, Keys? shortcut = null)
        {
            _items.Add(new MenuStripTemplate(name, method, canExecute, shortcut));
        }

        public void Add(string name, Action method, Func<bool> canExecute, Func<bool> checkOnAction, Keys? shortcut = null)
        {
            _items.Add(new MenuStripTemplate(name, method, canExecute, checkOnAction, shortcut));
        }

        public ToolStripItem[] ConvertToStripItem()
        {
            var list = new List<ToolStripItem>();

            foreach (var item in this)
            {
                if (item.Name == "-")
                {
                    list.Add(new ToolStripSeparator());
                }
                else if (item.Name.StartsWith("[x]"))
                {
                    var name = item.Name.RegexReplace(@"^\[x]", "");
                    list.Add(new ToolStripMenuItem(name, item.Icon, (s, e) => item.Method?.Invoke(), item.Shortcut) { CheckOnClick = true });
                }
                else if (item.Any())
                {
                    var menu = new ToolStripMenuItem(item.Name, item.Icon, item.ConvertToStripItem());

                    menu.DropDownOpened += (s, e) =>
                    {
                        for (int i = 0; i < menu.DropDownItems.Count; i++)
                        {
                            menu.DropDownItems[i].Enabled = item[i].CanExecute?.Invoke() ?? true;

                            if (menu.DropDownItems[i] is ToolStripMenuItem menuItem && item[i].CheckMarkValidation != null)
                                menuItem.Checked = item[i].CheckMarkValidation.Invoke();
                        }
                    };

                    menu.DropDownClosed += (s, e) =>
                    {
                        for (int i = 0; i < menu.DropDownItems.Count; i++)
                            menu.DropDownItems[i].Enabled = true;
                    };

                    list.Add(menu);
                }
                else
                {
                    list.Add(new ToolStripMenuItem(item.Name, item.Icon, (s, e) => item.Method?.Invoke(), item.Shortcut));
                }
            }

            return list.ToArray();
        }

        public void OnOpenedMethod(ToolStripItemCollection items)
        {
            for (int i = 0; i < Count; i++)
            {
                items[i].Enabled = this[i].CanExecute?.Invoke() ?? true;

                if (items[i] is ToolStripMenuItem menuItem && this[i].CheckMarkValidation != null)
                    menuItem.Checked = this[i].CheckMarkValidation.Invoke();
            }
        }

        public void OnClosedMethod(ToolStripItemCollection items)
        {
            for (int i = 0; i < Count; i++)
                items[i].Enabled = true;
        }
    }
}
