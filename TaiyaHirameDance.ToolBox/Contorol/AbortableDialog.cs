namespace TaiyaHirameDance.ToolBox
{
    internal partial class AbortableDialog : BaseForm
    {
        public Action<CancellationTokenSource, IProgress<int>> Work { get; set; }

        public int ProgressMax
        {
            get => ProgressBar.Maximum;
            set => ProgressBar.Maximum = value;
        }

        private Task _task;
        private readonly Progress<int> _progress;
        private readonly CancellationTokenSource _tokenSource;

        public AbortableDialog(CancellationTokenSource ts, Progress<int> prg)
        {
            _progress = prg;
            _tokenSource = ts;
            InitializeComponent();

            _progress.ProgressChanged += (s, value) => ProgressBar.Value = value;
        }

        private async void Dialog_Load(object sender, EventArgs e)
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    ActionInvoke(() => Text = "処理中...");
                    await Task.Delay(700);
                    ActionInvoke(() => Text = "");
                    await Task.Delay(300);
                }
            });

            _task = Task.Run(() => Work.Invoke(_tokenSource, _progress), _tokenSource.Token);
            await Task.WhenAll(_task);
            Close();
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }

        private void Dialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_task.IsCompleted)
                e.Cancel = true;
        }
    }
}
