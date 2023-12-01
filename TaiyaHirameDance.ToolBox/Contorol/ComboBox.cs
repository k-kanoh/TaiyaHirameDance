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
            get => (ComboBoxItem)base.SelectedItem ?? new ComboBoxItem();
        }

        /// <summary>
        /// 未選択の場合 true を返します。
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty => (SelectedCode == 0);

        /// <summary>
        /// 現在選択されている項目のCodeを設定/取得します。
        /// </summary>
        [Browsable(false)]
        [DefaultValue(0)]
        public int SelectedCode
        {
            get => SelectedItem.Code;
            set
            {
                var dataSource = (IList<ComboBoxItem>)DataSource;

                if (dataSource != null)
                {
                    base.SelectedItem =
                        dataSource.FirstOrDefault(x => x.Code == value) ?? dataSource.First(x => x.Code == 0);
                }
            }
        }

        /// <summary>
        /// 現在選択されている項目のNameを設定/取得します。
        /// </summary>
        [Browsable(false)]
        [DefaultValue("")]
        public string SelectedName
        {
            get => SelectedItem.Name;
            set
            {
                var dataSource = (IList<ComboBoxItem>)DataSource;

                if (dataSource != null)
                {
                    base.SelectedItem =
                        dataSource.FirstOrDefault(x => x.Name == value) ?? dataSource.First(x => x.Code == 0);
                }
            }
        }

        /// <summary>
        /// 現在選択されている項目のObjectを取得します。
        /// </summary>
        [Browsable(false)]
        public object SelectedObject => SelectedItem.Object;

        public ComboBox()
        {
            ValueMember = "Code";
            DisplayMember = "Name";
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            IntegralHeight = false;
            MaxDropDownItems = 13;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var text = ((ComboBoxItem)Items[e.Index]).Name;
            var brush = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Brushes.White : Brushes.Black;

            e.DrawBackground();
            e.Graphics.DrawString(text, e.Font, brush, e.Bounds, StringFormat.GenericDefault);
        }
    }

    public class ComboBoxItem
    {
        public int Code { get; set; }
        public string Name { get; set; } = "";
        public object Object { get; set; }
    }
}
