using System.Reflection;
using TaiyaHirameDance.Domain.Base;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    internal static class Setting
    {
        private static MasterSettingBase Master => (MasterSettingBase)Assembly.GetEntryAssembly().GetTypes()
                .SelectMany(x => x.GetFields(BindingFlags.Static | BindingFlags.NonPublic)).SingleOrDefault(x => x.FieldType.BaseType == typeof(MasterSettingBase))?.GetValue(null);

        private static MainSettingBase Main => (MainSettingBase)Assembly.GetEntryAssembly().GetTypes()
                .SelectMany(x => x.GetFields(BindingFlags.Static | BindingFlags.NonPublic)).SingleOrDefault(x => x.FieldType.BaseType == typeof(MainSettingBase))?.GetValue(null);

        public static string MainDirectory => Master.MainDirectory;

        /// <summary>
        /// Xlsxファイルの作成者に表示される名前
        /// </summary>
        public static string XlsxAuthor => Master.XlsxAuthor.TrimSafety().ValOrNull();

        public static string ConnectionString => Main?.ConnectionString;

        public static string[] IgnoreSchema
        {
            get
            {
                if (Main != null && Main.IgnoreSchema.Val())
                {
                    return Main.IgnoreSchema.SplitByWhiteSpace().ToArray();
                }
                else
                {
                    return [];
                }
            }
        }

        public static string[] IgnoreTable
        {
            get
            {
                if (Main != null && Main.IgnoreTable.Val())
                {
                    return Main.IgnoreTable.SplitByWhiteSpace().ToArray();
                }
                else
                {
                    return [];
                }
            }
        }

        public static (string pattern, Color color)[] TableColor
        {
            get
            {
                var list = new List<(string, Color)>();

                if (Main != null && Main.TableColor.Val())
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
    }
}
