using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.MainModels
{
    public class Scenario
    {
        public bool IsLogMonitoring { get; set; }
        public string LogPath { get; set; }
        public long LogIncrease { get; set; }
        public bool IsTableCompare { get; set; }

        public List<Evidence> Evidences { get; set; } = [];
        public TableSettings CompareTables { get; set; } = [];
        public TableSettings SnapshotTables { get; set; } = [];

        public void AddEvidenceCompareXlsx(byte[] bytes)
        {
            var hash = EvidenceUtil.CreateHashedXlsx(bytes);
            AddEvidence(hash, "DB比較");
        }

        public void AddEvidenceSnapshotXlsx(byte[] bytes)
        {
            var hash = EvidenceUtil.CreateHashedXlsx(bytes);
            AddEvidence(hash, "DBスナップショット");
        }

        public void AddEvidenceScreenShotImage(Image image)
        {
            var hash = EvidenceUtil.CreateHashedImage(image);
            AddEvidence(hash, "スクリーンショット");
        }

        public void AddEvidenceTextIncrease(FileInfo source, long offset)
        {
            var hash = EvidenceUtil.CreateHashedTextIncrease(source, offset);
            AddEvidence(hash, source.GetFileNameWithoutExtension());
        }

        public void AddEvidenceFileCopy(FileInfo source)
        {
            var hash = EvidenceUtil.CreateHashedCopy(source);
            AddEvidence(hash, source.GetFileNameWithoutExtension());
        }

        private void AddEvidence(string hash, string description = null)
        {
            if (hash == null || Evidences.Any(x => x.Hash == hash))
                return;

            Evidences.Add(new Evidence()
            {
                Hash = hash,
                Description = description,
                RegistedAt = DateTime.Now
            });
        }

        public void YamlSave(string no)
        {
            YamlUtil.YamlSave(Common.WorkDir, no, this);
        }

        public static Scenario YamlLoadOrNew(string no)
        {
            return YamlUtil.YamlLoadOrNew<Scenario>(Common.WorkDir, no);
        }
    }
}
