namespace TaiyaHirameDance.ToolBox
{
    public class TemporarilyStatusChange : IDisposable
    {
        private bool isDisposed;
        private readonly Action OnDisposeAction;

        public TemporarilyStatusChange(Action onCreateAction, Action onDisposeAction)
        {
            OnDisposeAction = onDisposeAction;
            onCreateAction.Invoke();
        }

        public void Dispose()
        {
            if (!isDisposed)
                OnDisposeAction.Invoke();

            isDisposed = true;
        }
    }
}
