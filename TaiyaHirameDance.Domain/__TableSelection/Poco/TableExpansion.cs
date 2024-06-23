using YamlDotNet.Serialization;

namespace TaiyaHirameDance.Domain.TableCriteria
{
    public class TableExpansion
    {
        [YamlMember(Alias = "Keys")]
        public int[] OriginalKeys { get; set; } = [1];

        [YamlMember(Alias = "PreQuery")]
        public string OriginalPreQuery { get; set; }

        [YamlMember(Alias = "Query")]
        public string OriginalQuery { get; set; }
    }
}
