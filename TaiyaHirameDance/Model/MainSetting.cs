using TaiyaHirameDance.Domain.Base;
using TaiyaHirameDance.ToolBox;
using YamlDotNet.Serialization;

namespace TaiyaHirameDance.SettingModel
{
    public sealed class MainSetting : MainSettingBase
    {
        [YamlIgnore]
        public override string ConnectionString { get; set; }

        [YamlMember(Alias = "ConnectionString")]
        public string EncryptedConnectionString
        {
            get => ConnectionString.Val() ? Util.Encrypt(ConnectionString) : null;
            set => ConnectionString = value.Val() ? Util.Decrypt(value) : null;
        }

        public override string IgnoreSchema { get; set; }

        public override string IgnoreTable { get; set; }

        public override string TableColor { get; set; }
    }
}
