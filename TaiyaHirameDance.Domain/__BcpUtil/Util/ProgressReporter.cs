namespace TaiyaHirameDance.Domain.BcpUtility
{
    internal class ProgressReporter(Action<string> console, IProgress<string> progress)
    {
        public void Console(string msg)
        {
            console?.Invoke(msg);
        }

        public void Progress(string msg)
        {
            progress?.Report(msg);
        }

        public void All(string msg)
        {
            Console(msg);
            Progress(msg);
        }

        public void End()
        {
            Console("処理を完了しました。");
            Console(null);
        }

        public void BreakLine()
        {
            Console($"{new string('-', 150)}");
        }
    }
}
