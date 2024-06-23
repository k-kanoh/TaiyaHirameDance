using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class DirtyMonitoringForm : BaseForm
    {
        private bool __isDirty;

        protected bool IsDirty
        {
            get => __isDirty;
            set
            {
                __isDirty = value;
                Text = __isDirty ? $"{Title} *" : Title;
            }
        }

        public DirtyMonitoringForm() : base()
        { }

        protected override void OnLoad(EventArgs eventArgs)
        {
            base.OnLoad(eventArgs);

            foreach (var textBox in this.ManyControls<TextBox>())
                textBox.TextChanged += (s, e) => RefreshDirtyFlg();
        }

        private async void RefreshDirtyFlg()
        {
            await DispMessageInTitleBarAsync(null);
            IsDirty = this.ManyControls<TextBox>().Any(x => x.IsDirty);
        }

        protected override void Save()
        {
            base.Save();
            RefreshDirtyFlg();
        }

        protected override void Reload()
        {
            base.Reload();
            RefreshDirtyFlg();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!IsDirty)
                return;

            if (InfoMessageBoxYesNoCancel("変更を保存しますか?", out var isYes))
            {
                if (isYes)
                    Save();

                return;
            }

            e.Cancel = true;

            base.OnClosing(e);
        }
    }
}
