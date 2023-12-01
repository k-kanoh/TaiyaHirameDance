using System.ComponentModel;
using System.Reflection;

namespace TaiyaHirameDance.ToolBox
{
    public class BaseForm : Form
    {
        private Task _titleMsgEraseTask;
        private bool _titleMsgCancelRequest;

        private string __title;

        [Category("表示")]
        [Description("画面のタイトルを設定します。")]
        public string Title
        {
            get => __title;
            set
            {
                __title = value;
                Text = __title;
            }
        }

        protected bool IsModalWindow { get; set; }

        /// <summary>
        /// エラープロバイダ
        /// </summary>
        protected ErrorProvider ErrorProvider { get; set; } = new ErrorProvider() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        /// <summary>
        /// ツールチップ
        /// </summary>
        protected ToolTip ToolTip { get; set; } = new ToolTip() { ShowAlways = true };

        [Browsable(false)]
        public override string Text { get => base.Text; set => base.Text = value; }

        public BaseForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            ErrorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };
        }

        protected override void OnLoad(EventArgs eventArgs)
        {
            foreach (var textBox in this.ManyControls<TextBox>())
            {
                textBox.ErrorProvider = ErrorProvider;
                ErrorProvider.SetIconPadding(textBox, -20);
            }

            base.OnLoad(eventArgs);
        }

        protected override void OnShown(EventArgs e)
        {
            foreach (var textBox in this.ManyControls<TextBox>())
            {
                textBox.SelectionStart = textBox.TextLength;
                textBox.SelectionLength = 0;
            }

            base.OnShown(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            if (IsModalWindow)
                Close();

            base.OnDeactivate(e);
        }

        /// <summary>
        /// ショートカットキーの実装
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            var key = Util.KeyParse(keyData, out var ctrl, out var alt, out var shift);

            if (ctrl && !alt && !shift)
            {
                switch (key)
                {
                    case Keys.S:
                        Save();

                        if (SaveMethodIsOverrided())
                            _ = DispMessageInTitleBarAsync("保存しました");

                        return true;

                    case Keys.R:    // テキストボックス上で不可?
                        Reload();

                        if (ReloadMethodIsOverrided())
                            _ = DispMessageInTitleBarAsync("リロードしました");

                        return true;

                    case Keys.W:
                        Close();
                        return true;
                }
            }

            if (!ctrl && !alt && !shift)
            {
                switch (key)
                {
                    case Keys.F5:
                        Reload();

                        if (ReloadMethodIsOverrided())
                            _ = DispMessageInTitleBarAsync("リロードしました");

                        return true;
                }
            }

            return base.ProcessDialogKey(keyData);
        }

        protected virtual void Save()
        {
            foreach (var textBox in this.ManyControls<TextBox>())
                textBox.SetSaved();
        }

        protected virtual void Reload()
        {
            foreach (var textBox in this.ManyControls<TextBox>())
                textBox.SetDefaultText(textBox.Text);
        }

        /// <summary>
        /// 子画面をモーダレスで出します。
        /// </summary>
        public void ShowModeless<T>() where T : BaseForm, new()
        {
            ShowModeless<T>(null, null, null);
        }

        /// <summary>
        /// 子画面をモーダレスで出します。
        /// </summary>
        public void ShowModeless<T>(object data) where T : BaseForm, new()
        {
            ShowModeless<T>(data, null, null);
        }

        /// <summary>
        /// 子画面をモーダレスで出します。
        /// </summary>
        public void ShowModeless<T>(object data, Action<T> onCreate, Action subseq) where T : BaseForm, new()
        {
            var newForm = new T();
            newForm.DataContext = data;
            newForm.FormClosed += (s, e) => subseq?.Invoke();

            onCreate?.Invoke(newForm);

            newForm.Show();
        }

        /// <summary>
        /// 子画面をモーダルで出します。
        /// </summary>
        public void ShowDialog<T>() where T : BaseForm, new()
        {
            ShowDialog<T>(null, null, null);
        }

        /// <summary>
        /// 子画面をモーダルで出します。
        /// </summary>
        public void ShowDialog<T>(object data) where T : BaseForm, new()
        {
            ShowDialog<T>(data, null, null);
        }

        /// <summary>
        /// 子画面をモーダルで出します。
        /// </summary>
        public void ShowDialog<T>(object data, Action<T> onCreate, Action subseq) where T : BaseForm, new()
        {
            var newForm = new T();
            newForm.DataContext = data;
            newForm.FormClosed += (s, e) => subseq?.Invoke();

            onCreate?.Invoke(newForm);

            newForm.ShowDialog();
        }

        public void ActionInvoke(Action action)
        {
            Invoke((System.Windows.Forms.MethodInvoker)(() => action.Invoke()));
        }

        private bool ReloadMethodIsOverrided()
        {
            return GetType().GetMethod("Reload", BindingFlags.Instance | BindingFlags.NonPublic).IsOverrided();
        }

        private bool SaveMethodIsOverrided()
        {
            return GetType().GetMethod("Save", BindingFlags.Instance | BindingFlags.NonPublic).IsOverrided();
        }

        protected async Task DispMessageInTitleBarAsync(string msg, int milliseconds = 3000)
        {
            _titleMsgCancelRequest = true;

            if (_titleMsgEraseTask != null)
                await _titleMsgEraseTask;

            _titleMsgCancelRequest = false;

            if (msg != null)
            {
                _titleMsgEraseTask = Task.Run(async () =>
                {
                    ActionInvoke(() => Text = $"{Title}  ({msg})");

                    var eraseTime = DateTime.Now.AddMilliseconds(milliseconds);
                    while (eraseTime > DateTime.Now && !_titleMsgCancelRequest)
                        await Task.Delay(100);

                    ActionInvoke(() => Text = Title);
                });
            }
        }

        /// <summary>
        /// DataContextとコントロールの入力内容をマージした新規のオブジェクトを作成します。
        /// </summary>
        protected T GetMergedNewDataContext<T>() where T : new()
        {
            var instance = new T();

            if (DataContext != null)
                Util.ShallowCopy(DataContext, instance);

            foreach (var (control, prop) in GetPairedControlAndProperty(typeof(T).GetProperties()))
            {
                switch (control)
                {
                    case TextBox textBox:
                        prop.SetValue(instance, textBox.Text);
                        break;

                    case CheckBox checkBox:
                        prop.SetValue(instance, checkBox.Checked);
                        break;
                }
            }

            return instance;
        }

        /// <summary>
        /// DataContextの内容をコントロールに同期します。
        /// </summary>
        protected void SetControlValueFromDataContext()
        {
            if (DataContext == null)
                return;

            foreach (var (control, prop) in GetPairedControlAndProperty(DataContext.GetType().GetProperties()))
            {
                switch (control)
                {
                    case TextBox textBox:
                        textBox.Text = Convert.ToString(prop.GetValue(DataContext));
                        break;

                    case CheckBox checkBox:
                        checkBox.Checked = Convert.ToBoolean(prop.GetValue(DataContext));
                        break;
                }
            }
        }

        private IEnumerable<(Control control, PropertyInfo prop)> GetPairedControlAndProperty(PropertyInfo[] properties)
        {
            var controls = from control in this.ManyControls()
                           where control.Name.IsMatch(@"^[a-z]{3}\w+$")
                           join prop in properties
                           on control.Name.Substring(3) equals prop.Name
                           select (control, prop);

            var checkBoxes = from control in this.ManyControls<CheckBox>().Cast<Control>()
                             where control.Name.IsMatch(@"^[a-z]{3}\w+$")
                             join prop in properties.Where(x => x.Name.IsMatch("^Is[A-Z]"))
                             on control.Name.Substring(3) equals prop.Name.Substring(2)
                             select (control, prop);

            return controls.Concat(checkBoxes);
        }

        /// <summary>
        /// 複数選択可能なファイル選択ダイアログを表示します。
        /// </summary>
        public bool PickMultiFileDialog(out string[] files)
        {
            files = [];
            using (var dlg = new OpenFileDialog())
            {
                dlg.Multiselect = true;
                dlg.Filter = "全てのファイル(*.*)|*.*";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return false;

                files = dlg.FileNames;
                return true;
            }
        }

        /// <summary>
        /// フォルダ選択ダイアログを表示します。
        /// </summary>
        public bool PickFolderDialog(ref string dir)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.InitialDirectory = dir;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return false;

                dir = dlg.SelectedPath;
                return true;
            }
        }

        /// <summary>
        /// 「OK」だけのメッセージダイアログを表示します。
        /// </summary>
        public void InformationMessageBox(string msg)
        {
            var option = new TaskDialogPage()
            {
                Caption = "確認",
                Icon = TaskDialogIcon.Information,
                Buttons = { TaskDialogButton.OK },
                Text = msg
            };

            TaskDialog.ShowDialog(this, option);
        }

        /// <summary>
        /// 「OK」だけのエラーメッセージボックスを表示します。
        /// </summary>
        public void ErrorMessageBox(string msg)
        {
            var option = new TaskDialogPage()
            {
                Caption = "確認",
                Icon = TaskDialogIcon.Error,
                Buttons = { TaskDialogButton.OK },
                Text = msg
            };

            TaskDialog.ShowDialog(this, option);
        }

        /// <summary>
        /// 「OK」「キャンセル」のメッセージボックスを表示します。
        /// </summary>
        public bool InfoMessageBoxOKCancel(string msg)
        {
            var option = new TaskDialogPage()
            {
                Caption = "確認",
                Icon = TaskDialogIcon.Information,
                Buttons = { TaskDialogButton.OK, TaskDialogButton.Cancel },
                Text = msg
            };

            return TaskDialog.ShowDialog(this, option) == TaskDialogButton.OK;
        }

        /// <summary>
        /// 「はい」「いいえ」「キャンセル」のメッセージボックスを表示します。
        /// </summary>
        public bool InfoMessageBoxYesNoCancel(string msg, out bool isYes)
        {
            isYes = false;

            var option = new TaskDialogPage()
            {
                Caption = "確認",
                Icon = TaskDialogIcon.Information,
                Buttons = { TaskDialogButton.Yes, TaskDialogButton.No, TaskDialogButton.Cancel },
                Text = msg
            };

            var res = TaskDialog.ShowDialog(this, option);

            switch (res.Text)
            {
                case "Yes":
                    isYes = true;
                    return true;

                case "No":
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// マウスカーソルを処理中を示す待機カーソルにします。
        /// </summary>
        public TemporarilyStatusChange Executing()
        {
            return new TemporarilyStatusChange(() => Cursor = Cursors.WaitCursor, () => Cursor = Cursors.Default);
        }

        /// <summary>
        /// モーダルダイアログを表示する時に TopMost を設定された自画面を非表示にします。
        /// </summary>
        public TemporarilyStatusChange ModalShowing()
        {
            return new TemporarilyStatusChange(() => { if (TopMost) Hide(); }, () => Show());
        }

        /// <summary>
        /// タスクをキャンセル可能なダイアログを表示します。
        /// </summary>
        public void ShowAbortableDialog(Action<CancellationTokenSource, IProgress<int>> work, int progressMax)
        {
            var prg = new Progress<int>();
            using (var ts = new CancellationTokenSource())
            using (var dlg = new AbortableDialog(ts, prg))
            {
                try
                {
                    dlg.Work = work;
                    dlg.ProgressMax = progressMax;

                    using (ModalShowing())
                        dlg.ShowDialog();
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }
        }
    }
}
