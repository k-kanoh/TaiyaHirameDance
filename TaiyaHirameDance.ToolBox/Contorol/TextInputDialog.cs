using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class TextInputDialog : BaseForm
    {
        public TextInputDialog()
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
