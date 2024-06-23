using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public partial class ComboBox : System.Windows.Forms.ComboBox
    {
        /// <summary>
        /// 現在選択されている項目を取得します。
        /// </summary>
        [Browsable(false)]
        public new ComboBoxItem SelectedItem
        {
            get => (ComboBoxItem)base.SelectedItem;
        }

        /// <summary>
        /// 未選択の場合 true を返します。
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty => !SelectedKey.Val();

        /// <summary>
        /// 現在選択されている項目のKeyを設定/取得します。
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public string SelectedKey
        {
            get => SelectedItem?.Key;
            set
            {
                if (ComboBoxItems != null)
                    base.SelectedItem = ComboBoxItems.FirstOrDefault(x => x.Key == value) ?? ComboBoxItems[0];
            }
        }

        /// <summary>
        /// 現在選択されている項目のNameを設定/取得します。
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public string SelectedName
        {
            get => SelectedItem?.DispName;
            set
            {
                if (ComboBoxItems != null)
                    base.SelectedItem = ComboBoxItems.FirstOrDefault(x => x.DispName == value) ?? ComboBoxItems[0];
            }
        }

        /// <summary>
        /// データソースを設定/取得します。
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public ComboBoxItems ComboBoxItems
        {
            get => (ComboBoxItems)DataSource;
            set => DataSource = value;
        }

        /// <summary>
        /// 現在選択されている項目にバインドされたデータを取得します。
        /// </summary>
        public T GetBoundItem<T>()
        {
            return (T)SelectedItem.DataBoundItem;
        }

        [Category("オリジナル")]
        [Description("空のアイテムが選択されたときに発生します。")]
        public event EventHandler EmptySelected;

        [Category("オリジナル")]
        [Description("有効なアイテムが選択されたときに発生します。")]
        public event EventHandler ItemSelected;

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (IsEmpty)
            {
                EmptySelected?.Invoke(this, e);
            }
            else
            {
                ItemSelected?.Invoke(this, e);
            }
        }

        public ComboBox()
        {
            ValueMember = "Key";
            DisplayMember = "DispName";
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            IntegralHeight = false;
            MaxDropDownItems = 13;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var text = ((ComboBoxItem)Items[e.Index]).DispName;
            var brush = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Brushes.White : Brushes.Black;

            e.DrawBackground();
            e.Graphics.DrawString(text, e.Font, brush, e.Bounds, StringFormat.GenericDefault);
        }
    }
}
