namespace TaiyaHirameDance.ToolBox
{
    public class StatusChangeScope : IDisposable
    {
        private readonly Action _onDispose;

        public StatusChangeScope(Action onCreate, Action onDispose)
        {
            _onDispose = onDispose;
            onCreate();
        }

        public void Dispose()
        {
            _onDispose();
            GC.SuppressFinalize(this);
        }
    }
}
