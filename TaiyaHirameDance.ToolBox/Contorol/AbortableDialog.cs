namespace TaiyaHirameDance.ToolBox
{
    internal partial class AbortableDialog : Form
    {
        public Action Work { get; set; }
        public Exception CaughtException { get; private set; }

        public int ProgressMax
        {
            get => ProgressBar.Maximum;
            set => ProgressBar.Maximum = value;
        }

        private Task _task;
        private readonly Progress<int> _progress;
        private readonly CancellationTokenSource _cts;

        public AbortableDialog(Progress<int> progress, CancellationTokenSource cts)
        {
            _cts = cts;
            _progress = progress;

            InitializeComponent();

            _progress.ProgressChanged += (s, value) => ProgressBar.Value = value;
        }

        private async void Dialog_Load(object sender, EventArgs e)
        {
            _ = Task.Run(async () =>
            {
                while (!_task.IsCompleted)
                {
                    Invoke((MethodInvoker)(() => Text = "処理中..."));
                    await Task.Delay(700);
                    Invoke((MethodInvoker)(() => Text = ""));
                    await Task.Delay(300);
                }
            });

            _task = Task.Run(() => Work());

            try
            {
                await _task;
            }
            catch (Exception ex)
            {
                CaughtException = ex;
            }
            finally
            {
                Close();
            }
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
        }

        private void Dialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_task.IsCompleted)
                e.Cancel = true;
        }
    }
}
