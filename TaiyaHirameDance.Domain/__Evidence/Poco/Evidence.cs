using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    public class Evidence
    {
        public bool IsLogMonitoring { get; set; }
        public string LogPath { get; set; }
        public long LogIncrease { get; set; }
        public bool IsTableCompare { get; set; }
        public List<File> Files { get; set; } = [];
        public List<Table> CompareTables { get; set; } = [];
        public List<Table> SnapshotTables { get; set; } = [];

        public class File
        {
            public string Hash { get; set; }
            public DateTime? RegistAt { get; set; }
            public string Description { get; set; }
            public bool Deleted { get; set; }

            public File()
            { }

            public File(string hash, string desc)
            {
                Hash = hash;
                RegistAt = Util.Now;
                Description = desc;
            }
        }

        public class Table
        {
            public string TableName { get; set; }
            public bool Checked { get; set; }
            public string WhereClause { get; set; }
            public int? Seq { get; set; }
        }

        public void FileAdd(string hash, string desc = null)
        {
            if (hash == null)
                return;

            if (Files.Any(x => x.Hash == hash))
                return;

            Files.Add(new File(hash, desc));
        }
    }
}
