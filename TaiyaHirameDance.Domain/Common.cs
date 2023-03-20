using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    public static class Common
    {
        /// <summary>
        /// メインフォルダ
        /// </summary>
        public static DirectoryInfo MainDir => new(Setting.MainDirectory);

        /// <summary>
        /// 作業フォルダ
        /// </summary>
        public static DirectoryInfo WorkDir => MainDir.SubDirectory(".nai");
    }
}
