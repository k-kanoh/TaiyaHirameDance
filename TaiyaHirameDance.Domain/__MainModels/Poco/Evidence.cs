using TaiyaHirameDance.ToolBox;
using YamlDotNet.Serialization;

namespace TaiyaHirameDance.Domain.MainModels
{
    public class Evidence
    {
        public string Hash { get; set; }
        public DateTime RegistedAt { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }

        [YamlIgnore]
        public string Extension => EvidenceUtil.GetHashedFile(Hash)?.Extension;

        [YamlIgnore]
        public string TooltipDispName => $"{Description}{Extension} ({Hash.Substring(0, 6)}) {RegistedAt:M/d HH:mm:ss}";

        /// <summary>
        /// ハッシュ化されたエビデンスファイルを対象ディレクトリに吐き出します。
        /// </summary>
        public void EjectFile(DirectoryInfo dinfo)
        {
            var source = EvidenceUtil.GetHashedFile(Hash) ?? throw new FileNotFoundException();

            var dest = dinfo.File($"{Description}{source.Extension}");

            var i = 1;
            while (dest.Exists)
                dest = dinfo.File($"{Description}~{i++}{source.Extension}");

            source.CopyTo(dest.FullName);
            source.Refresh();
            source.IsReadOnly = false;
        }
    }
}
