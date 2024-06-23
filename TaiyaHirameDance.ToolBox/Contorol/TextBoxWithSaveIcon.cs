using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class TextBoxWithSaveIcon : TextBox
    {
        internal readonly PictureBox _iconPicture;
        private bool _isIconDisplay;

        public TextBoxWithSaveIcon()
        {
            _iconPicture = new PictureBox();
            _iconPicture.Image = Properties.Resources.SaveIcon;
            _iconPicture.Size = new Size(21, 21);
            _iconPicture.SizeMode = PictureBoxSizeMode.CenterImage;
            _iconPicture.TabStop = false;
            _iconPicture.Cursor = Cursors.Hand;
            _iconPicture.Click += PictureBox_Click;

            SetIconLocation();
            DisplayIcon();
            Controls.Add(_iconPicture);
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            SetError(null);
            IconClick?.Invoke(this, e);
        }

        [Category("オリジナル")]
        [Description("コントロールの右側にアイコンを表示します。アイコンのクリックは IconClick で捕捉できます。")]
        [DefaultValue(false)]
        public bool IconDisplay
        {
            get => _isIconDisplay;
            set
            {
                _isIconDisplay = value;
                DisplayIcon();
            }
        }

        [Category("オリジナル")]
        [Description("アイコンにホバーした時に表示するツールチップを設定します。")]
        [DefaultValue(null)]
        public string IconToolTip { get; set; }

        [Category("オリジナル")]
        [Description("アイコンがクリックされたときに発生します。")]
        public event EventHandler IconClick;

        private void DisplayIcon()
        {
            _iconPicture.Visible = IsDirty && IconDisplay && !ReadOnly && Enabled;
        }

        private void SetIconLocation()
        {
            _iconPicture.Location = new Point((TextAlign == HorizontalAlignment.Right) ? 0 : Width - 26, 2);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            DisplayIcon();
        }

        protected override void OnReadOnlyChanged(EventArgs e)
        {
            DisplayIcon();
            base.OnReadOnlyChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            DisplayIcon();
            base.OnEnabledChanged(e);
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            SetIconLocation();
            base.OnTextAlignChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SetIconLocation();
            base.OnSizeChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _iconPicture.Click -= PictureBox_Click;

            base.Dispose(disposing);
        }
    }
}
