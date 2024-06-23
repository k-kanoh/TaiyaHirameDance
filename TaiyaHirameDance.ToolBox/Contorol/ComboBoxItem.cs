namespace TaiyaHirameDance.ToolBox
{
    public class ComboBoxItem
    {
        public string Key { get; set; }
        public string DispName { get; set; }
        public object DataBoundItem { get; set; }

        internal ComboBoxItem()
        { }

        public ComboBoxItem(string key, string name)
        {
            Key = key;
            DispName = name;
        }

        public ComboBoxItem(string key, string name, object data)
        {
            Key = key;
            DispName = name;
            DataBoundItem = data;
        }
    }
}
