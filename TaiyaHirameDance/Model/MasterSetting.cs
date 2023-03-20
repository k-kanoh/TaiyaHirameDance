using TaiyaHirameDance.Domain.Base;
using TaiyaHirameDance.ToolBox;
using YamlDotNet.Serialization;

namespace TaiyaHirameDance.SettingModel
{
    public sealed class MasterSetting : MasterSettingBase
    {
        [YamlMember(Order = 3, DefaultValuesHandling = DefaultValuesHandling.Preserve)]
        public override string MainDirectory { get; set; }

        /// <summary>
        /// Xlsxファイルの作成者に表示される名前
        /// </summary>
        [YamlMember(Order = 1, DefaultValuesHandling = DefaultValuesHandling.Preserve)]
        public override string XlsxAuthor { get; set; }

        public MasterSetting()
        {
            MainDirectory = Util.Desktop.Combine("Ikusan");
            XlsxAuthor = "User";
        }
    }
}
