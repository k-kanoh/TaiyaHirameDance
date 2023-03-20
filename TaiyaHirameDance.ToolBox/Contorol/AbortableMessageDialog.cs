namespace TaiyaHirameDance.ToolBox
{
    internal partial class AbortableMessageDialog<T> : Form
    {
        public Task<T> AbortableTask { get; set; }
        public T Result { get; private set; }
        public Exception CaughtException { get; private set; }

        private readonly Progress<string> _progress;
        private readonly CancellationTokenSource _cts;

        public AbortableMessageDialog(Progress<string> progress, CancellationTokenSource cts)
        {
            _cts = cts;
            _progress = progress;

            InitializeComponent();

            _progress.ProgressChanged += (s, msg) => Message.Text = msg;
        }

        private async void Dialog_Load(object sender, EventArgs e)
        {
            _ = Task.Run(async () =>
            {
                while (!AbortableTask.IsCompleted)
                {
                    Invoke((MethodInvoker)(() => Text = "処理中..."));
                    await Task.Delay(700);
                    Invoke((MethodInvoker)(() => Text = ""));
                    await Task.Delay(300);
                }
            });

            try
            {
                Result = await AbortableTask;
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
            if (!AbortableTask.IsCompleted)
                e.Cancel = true;
        }
    }
}
