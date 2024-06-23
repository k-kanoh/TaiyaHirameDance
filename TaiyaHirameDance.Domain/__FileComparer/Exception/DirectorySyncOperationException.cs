namespace TaiyaHirameDance.Domain.FileComparer
{
    public class DirectorySyncOperationException(Comparison operation, FileSystemInfo target) : Exception
    {
        public Comparison Operation { get; private set; } = operation;
        public FileSystemInfo Target { get; private set; } = target;

        public override string Message
        {
            get
            {
                switch (Operation)
                {
                    case Comparison.Remove:
                        return $"「{Target.FullName}」の削除に失敗しました。";

                    case Comparison.Modified:
                        return $"「{Target.FullName}」の上書きに失敗しました。";

                    default:
                        return $"「{Target.FullName}」の作成に失敗しました。";
                }
            }
        }
    }
}
