using System.ComponentModel;
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

        /// <summary>
        /// DB比較の出力時、変更なし行の出力を省略する
        /// </summary>
        [DefaultValue(true)]
        [YamlMember(Order = 2, DefaultValuesHandling = DefaultValuesHandling.Preserve)]
        public override bool OmitCompareRowIsConformity { get; set; }

        public MasterSetting()
        {
            MainDirectory = Util.Desktop.Combine("Ikusan");
            XlsxAuthor = "User";
            OmitCompareRowIsConformity = true;
        }
    }
}
