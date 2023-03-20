using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class SimpleTextEditDialog : BaseForm
    {
        public SimpleTextEditDialog()
        {
            InitializeComponent();
        }

        public new string Text
        {
            get => TextBox.Text.Trim();
            set => TextBox.SetDefaultText(value);
        }

        public void SetTextBoxError(string msg)
        {
            TextBox.SetError(msg);
        }
    }
}
