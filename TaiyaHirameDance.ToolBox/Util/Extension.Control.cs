namespace TaiyaHirameDance.ToolBox
{
    public static class ControlExtension
    {
        /// <summary>
        /// このコントロールが保持する全てのコントロールを再帰的に返します。
        /// </summary>
        public static IEnumerable<Control> ManyControls(this Control iam, bool containChild = true)
        {
            var control1 = iam.Controls.Cast<Control>();

            if (!containChild)
                return control1;

            var control2 = iam.Controls.Cast<Control>().Where(x => x.GetType().BaseType != typeof(UserControl)).SelectMany(x => x.ManyControls());

            return control1.Concat(control2);
        }

        /// <summary>
        /// このコントロールが保持する指定されたコントロールを再帰的に返します。
        /// </summary>
        public static IEnumerable<T> ManyControls<T>(this Control iam)
        {
            return iam.ManyControls().OfType<T>();
        }

        /// <summary>
        /// DataGridViewRowにバインドされたデータを取得します。
        /// </summary>
        public static T GetBoundItem<T>(this DataGridViewRow row)
        {
            return (T)row.DataBoundItem;
        }

        public static void Reset(this DataGridViewCellStyle style)
        {
            style.Font = null;
            style.ForeColor = Color.Empty;
            style.SelectionForeColor = Color.Empty;
        }

        /// <summary>
        /// コレクションをComboBoxItemsに変換します。
        /// </summary>
        public static ComboBoxItems ToComboBoxItems(this IEnumerable<ComboBoxItem> items)
        {
            return new ComboBoxItems(items.ToList());
        }

        /// <summary>
        /// テキストボックスのテキストに改行コード付きの文字列を追加します。
        /// </summary>
        public static void AppendLine(this TextBoxBase textBox, string text)
        {
            textBox.AppendText(text + Environment.NewLine);
        }
    }
}
