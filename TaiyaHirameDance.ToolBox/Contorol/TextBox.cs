using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class TextBox : System.Windows.Forms.TextBox
    {
        private int undoIndex, savedIndex;
        private bool recording = true;
        private List<(int pos, string text)> UndoBuffer;

        [Browsable(false)]
        [DefaultValue(null)]
        public ErrorProvider ErrorProvider { get; set; }

        [Browsable(false)]
        public new bool CanUndo => !ReadOnly && undoIndex > 0;

        [Browsable(false)]
        public bool CanRedo => !ReadOnly && undoIndex < UndoBuffer.Count - 1;

        [Browsable(false)]
        public bool CanCut => !ReadOnly && SelectionLength > 0;

        [Browsable(false)]
        public bool CanPaste => !ReadOnly && Clipboard.GetText().Val();

        [Browsable(false)]
        public bool IsDirty => undoIndex != savedIndex;

        public TextBox()
        {
            Clear();

            var menuTemplate = new MenuStripTemplate()
            {
                new MenuStripTemplate("全て選択", SelectAll, () => TextLength > 0, Keys.Control | Keys.A),
                new MenuStripTemplate("-"),
                new MenuStripTemplate("切り取り", Cut, () => CanCut, Keys.Control | Keys.X),
                new MenuStripTemplate("コピー", Copy, () => SelectionLength > 0, Keys.Control | Keys.C),
                new MenuStripTemplate("貼り付け",  Paste, () => CanPaste, Keys.Control | Keys.V),
                new MenuStripTemplate("-"),
                new MenuStripTemplate("元に戻す", Undo, () => CanUndo, Keys.Control | Keys.Z),
                new MenuStripTemplate("やり直し", Redo, () => CanRedo, Keys.Control | Keys.Y)
            };

            var menu = new ContextMenuStrip();
            menu.Items.AddRange(menuTemplate.ConvertToStripItem());
            menu.Opened += (s, e) => menuTemplate.OnOpenedMethod(menu.Items);
            menu.Closed += (s, e) => menuTemplate.OnClosedMethod(menu.Items);
            ContextMenuStrip = menu;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (recording)
            {
                while (undoIndex < UndoBuffer.Count - 1)
                    UndoBuffer.RemoveAt(UndoBuffer.Count - 1);

                UndoBuffer.Add((SelectionStart, Text));
                undoIndex = UndoBuffer.Count - 1;
            }

            SetError(null);

            base.OnTextChanged(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            e.Handled = (char.IsControl(e.KeyChar) && e.KeyChar != '\b');
            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // [Ctrl] + [Tab] を潰す
            e.Handled = (e.Control && e.KeyCode == Keys.Tab);
            base.OnKeyDown(e);
        }

        public new void SelectAll()
        {
            Focus();
            base.SelectAll();
        }

        public new void Undo()
        {
            if (!CanUndo)
                return;

            try
            {
                recording = false;
                var (pos, text) = UndoBuffer[--undoIndex];
                Text = text;
                SelectionStart = pos;
            }
            finally
            {
                recording = true;
            }
        }

        public void Redo()
        {
            if (!CanRedo)
                return;

            try
            {
                recording = false;
                var (pos, text) = UndoBuffer[++undoIndex];
                Text = text;
                SelectionStart = pos;
            }
            finally
            {
                recording = true;
            }
        }

        public virtual void SetDefaultText(string text)
        {
            try
            {
                recording = false;
                Text = text;
                UndoBuffer = [(0, text)];
                undoIndex = savedIndex = 0;
            }
            finally
            {
                recording = true;
            }
        }

        public void SetSaved()
        {
            savedIndex = undoIndex;
        }

        public new void Clear()
        {
            SetDefaultText("");
        }

        protected string GetOldText()
        {
            return UndoBuffer[savedIndex].text;
        }

        /// <summary>
        /// エラーメッセージを設定します。
        /// </summary>
        public void SetError(string msg)
        {
            ErrorProvider?.SetError(this, msg);
        }
    }
}
