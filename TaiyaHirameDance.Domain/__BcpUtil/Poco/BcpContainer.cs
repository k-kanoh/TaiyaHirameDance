using TaiyaHirameDance.ToolBox;
using YamlDotNet.Serialization;

namespace TaiyaHirameDance.Domain.BcpUtility
{
    public class BcpContainer
    {
        public string Hash { get; set; }
        public DateTime? RegistedAt { get; set; }
        public string Description { get; set; }
        public List<Table> Tables { get; set; }

        [YamlIgnore]
        public string HashPrefix => Hash.Substring(0, 6);

        [YamlIgnore]
        public string ComboBoxDispName => $"({HashPrefix}) {RegistedAt:yyyy/MM/dd HH:mm:ss} {Description}";

        public void Save(string dbName)
        {
            YamlUtil.YamlSave(Util.Roaming.SubDirectory("bcp", dbName), Hash, this);
        }

        public class Table
        {
            public string TableName { get; set; }
            public int Count { get; set; }
        }
    }
}
