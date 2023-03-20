using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class DataGridView : System.Windows.Forms.DataGridView
    {
        private readonly CheckBox _allCheckBox;
        private bool _isEditingTextBoxInitialized;

        [Category("オリジナル")]
        [Description("このプロパティが true のとき、結果にセットされた ColorBunrui の値に基づいて色分けされます。")]
        [DefaultValue(false)]
        public bool ColorBunrui { get; set; }

        [Category("オリジナル")]
        [Description("結果件数を表示するラベルコントロール")]
        [DefaultValue(null)]
        public ResultCountLabel ResultCountLabel { get; set; }

        /// <summary>
        /// DefaultCellStyleのフォント
        /// </summary>
        [Browsable(false)]
        public new Font DefaultFont => RowTemplate.DefaultCellStyle.Font;

        public event EventHandler<DataGridViewCellButtonClickEventArgs> ButtonClick;

        public DataGridView()
        {
            TabStop = false;
            RowHeadersVisible = false;
            ShowCellToolTips = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            RowTemplate.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9.75F);
            RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 192, 255);
            RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;

            DataMember = null;
            DoubleBuffered = true;
            AutoGenerateColumns = false;

            _allCheckBox = new CheckBox();
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);

            if (Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex == -1)
            {
                using (var bmp = new Bitmap(13, 18))
                {
                    _allCheckBox.BackColor = SystemColors.ControlLightLight;
                    _allCheckBox.DrawToBitmap(bmp, new Rectangle(0, 0, 13, 18));

                    e.Paint(e.ClipBounds, e.PaintParts);
                    e.Graphics.DrawImage(bmp, new Rectangle(10, 6, 13, 13), new Rectangle(0, 5, 13, 13), GraphicsUnit.Pixel);
                    e.Handled = true;
                }
            }
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            if (Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex == -1)
            {
                if (ReadOnly || Columns[0].ReadOnly)
                    return;

                _allCheckBox.Checked = !_allCheckBox.Checked;
            }

            base.OnCellClick(e);
        }

        private void AllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentCell != null && CurrentCell.ColumnIndex == 0)
                CurrentCell = CurrentRow.Cells[1];

            foreach (var row in Rows.Cast<DataGridViewRow>())
                if (row.DataBoundItem is ICheckableGrid item && item != null)
                    item.IsChecked = _allCheckBox.Checked;

            Refresh();
            OnCellValueChanged(new DataGridViewCellEventArgs(0, 0));
        }

        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            if (Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn || e.RowIndex == -1)
                return;

            base.OnCellDoubleClick(e);
        }

        protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
        {
            if (CurrentCell.OwningColumn is DataGridViewCheckBoxColumn && IsCurrentCellDirty)
                CommitEdit(DataGridViewDataErrorContexts.Commit);

            base.OnCurrentCellDirtyStateChanged(e);
            Refresh();
        }

        protected override void OnCellContentClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (CurrentCell is DataGridViewButtonCell)
                ButtonClick?.Invoke(this, new DataGridViewCellButtonClickEventArgs(e.ColumnIndex, e.RowIndex, index => Rows[index]));

            base.OnCellContentClick(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Columns[0] is DataGridViewCheckBoxColumn && e.KeyCode == Keys.Space)
            {
                if (ReadOnly || Columns[0].ReadOnly)
                    return;

                var i = 0;
                var val = true;
                foreach (var row in SelectedRows.Cast<DataGridViewRow>())
                {
                    if (row.DataBoundItem is ICheckableGrid item && item != null)
                    {
                        if (i++ == 0)
                            val = !item.IsChecked;

                        item.IsChecked = val;
                    }
                }

                Refresh();
                OnCellValueChanged(new DataGridViewCellEventArgs(0, 0));
            }

            base.OnKeyDown(e);
        }

        public override void Refresh()
        {
            if (ResultCountLabel != null)
            {
                if (DataSource is IListSource source)
                {
                    ResultCountLabel.CheckedCount = source.GetList().OfType<ICheckableGrid>().Count(x => x.IsChecked);
                    ResultCountLabel.DbResultCount = (DataSource == null) ? -1 : source.GetList().Count;
                }
            }

            base.Refresh();
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);

            Refresh();
            SetAllCheckBoxUnchecked();
            SetAlternatingRowColorsByBunrui();
        }

        private void SetAllCheckBoxUnchecked()
        {
            _allCheckBox.CheckedChanged -= AllCheckBox_CheckedChanged;

            try
            {
                _allCheckBox.Checked = false;
            }
            finally
            {
                _allCheckBox.CheckedChanged += AllCheckBox_CheckedChanged;
            }
        }

        private void SetAlternatingRowColorsByBunrui()
        {
            long lag = 1, i = 0;
            foreach (var row in Rows.Cast<DataGridViewRow>())
            {
                if (ColorBunrui && row.DataBoundItem is IColorBunruiGrid item && item != null)
                {
                    if (lag != item.ColorBunrui)
                        i++;

                    row.DefaultCellStyle.BackColor = (i % 2 == 1 ? Color.Empty : Color.LightYellow);

                    lag = item.ColorBunrui;
                }
                else
                {
                    break;
                }
            }
        }

        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            if (Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;

            base.OnCellBeginEdit(e);
        }

        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            if (Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;

            base.OnCellEndEdit(e);
        }

        protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            base.OnCellValidating(e);

            if (Columns[e.ColumnIndex].ReadOnly)
                return;

            var name = Columns[e.ColumnIndex].DataPropertyName;

            if (DataSource is IListSource source)
            {
                if (source.GetList().GetType().GenericTypeArguments[0].GetProperty(name)?.GetPrimitiveType() == typeof(int))
                {
                    var input = (string)e.FormattedValue;
                    if (input.Val() && !int.TryParse(input, out _))
                        e.Cancel = true;
                }
            }
        }

        protected override void OnCellParsing(DataGridViewCellParsingEventArgs e)
        {
            if (e.Value is string text)
            {
                e.Value = text.TrimSafety();
                e.ParsingApplied = true;
            }

            base.OnCellParsing(e);
        }

        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs eventArgs)
        {
            if (!_isEditingTextBoxInitialized && eventArgs.Control is System.Windows.Forms.TextBox textBox)
            {
                textBox.KeyPress += (s, e) =>
                {
                    e.Handled = (char.IsControl(e.KeyChar) && e.KeyChar != '\b');
                    base.OnKeyPress(e);
                };

                var menuTemplate = new MenuStripTemplate()
                {
                    new MenuStripTemplate("全て選択", () => textBox.SelectAll(), () => (textBox.TextLength > 0), (Keys.Control | Keys.A)),
                    new MenuStripTemplate("-"),
                    new MenuStripTemplate("切り取り", () => textBox.Cut(), () => (textBox.SelectionLength > 0), (Keys.Control | Keys.X)),
                    new MenuStripTemplate("コピー", () => textBox.Copy(), () => (textBox.SelectionLength > 0), (Keys.Control | Keys.C)),
                    new MenuStripTemplate("貼り付け", () => textBox.Paste(), () => Clipboard.GetText().Val(), (Keys.Control | Keys.V)),
                };

                var menu = new ContextMenuStrip();
                menu.Items.AddRange(menuTemplate.ConvertToStripItem());
                menu.Opened += (s, e) => menuTemplate.OnOpenedMethod(menu.Items);
                menu.Closed += (s, e) => menuTemplate.OnClosedMethod(menu.Items);
                textBox.ContextMenuStrip = menu;

                _isEditingTextBoxInitialized = true;
            }

            base.OnEditingControlShowing(eventArgs);
        }

        public void SetCellStyleRows<T>(string dataPropertyName, Action<T, DataGridViewCellStyle> action)
        {
            foreach (var row in Rows.Cast<DataGridViewRow>())
            {
                var item = row.GetBoundItem<T>();
                var style = row.Cells.Cast<DataGridViewCell>().First(x => x.OwningColumn.DataPropertyName == dataPropertyName).Style;
                action.Invoke(item, style);
            }
        }

        public void SetCellStyleAllRow<T>(Action<T, DataGridViewCellStyle> action)
        {
            foreach (var row in Rows.Cast<DataGridViewRow>())
            {
                var item = row.GetBoundItem<T>();
                foreach (var cell in row.Cells.Cast<DataGridViewCell>())
                    action.Invoke(item, cell.Style);
            }
        }

        public override DataObject GetClipboardContent()
        {
            var newObject = new DataObject();
            newObject.SetData(DataFormats.Text, base.GetClipboardContent().GetData(DataFormats.Text));

            return newObject;
        }

        public void SelectionBlink(Color color)
        {
            Task.Run(async () =>
            {
                foreach (var cell in SelectedCells.Cast<DataGridViewCell>())
                    cell.Style.SelectionBackColor = color;

                await Task.Delay(100);

                foreach (var cell in SelectedCells.Cast<DataGridViewCell>())
                    cell.Style.SelectionBackColor = RowTemplate.DefaultCellStyle.SelectionBackColor;
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _allCheckBox.Dispose();

            base.Dispose(disposing);
        }
    }
}
