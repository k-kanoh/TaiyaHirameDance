using TaiyaHirameDance.Domain;
using TaiyaHirameDance.Domain.A5MK2Csv;
using TaiyaHirameDance.SettingModel;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public static class Setting
    {
        private static MasterSetting Master = new();

        private static MainSetting Main = new();

        public static (string pattern, Color color)[] TableColor
        {
            get
            {
                var list = new List<(string, Color)>();

                if (Main.TableColor.Val())
                {
                    foreach (var word in Main.TableColor.SplitByWhiteSpace())
                    {
                        var (pat, c) = word.Split2(':');
                        var colorObj = c.ToColor();
                        if (colorObj.HasValue)
                            list.Add((pat, colorObj.Value));
                    }
                }

                return list.ToArray();
            }
        }

        public static void Save()
        {
            YamlUtil.YamlSave(Util.Roaming, "Setting", Master);
            YamlUtil.YamlSave(Common.WorkDir, "Setting", Main, true);
            Master = YamlUtil.YamlLoad<MasterSetting>(Util.Roaming, "Setting");
            Main = YamlUtil.YamlLoad<MainSetting>(Common.WorkDir, "Setting");
        }

        public static void Reload()
        {
            Master = YamlUtil.YamlLoadOrNew<MasterSetting>(Util.Roaming, "Setting");
            Main = YamlUtil.YamlLoadOrNew<MainSetting>(Common.WorkDir, "Setting");
            A5MK2.Reload();
        }

        public static void ChangeMainDirectory(string dir)
        {
            Master.MainDirectory = dir;
            YamlUtil.YamlSave(Util.Roaming, "Setting", Master);
            Reload();
        }
    }
}
