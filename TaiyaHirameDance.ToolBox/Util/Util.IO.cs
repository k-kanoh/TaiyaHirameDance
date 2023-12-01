namespace TaiyaHirameDance.ToolBox
{
    internal static class UtilIO
    {
        /// <summary>
        /// 空ファイルを作成します。
        /// </summary>
        public static void CreateFile(FileInfo finfo)
        {
            finfo.Refresh();

            var dinfo = finfo.Directory;

            if (!dinfo.Exists)
            {
                dinfo.Create();
                dinfo.Refresh();

                if (dinfo.Name.StartsWith("."))
                    dinfo.Attributes |= FileAttributes.Hidden;
            }

            if (!finfo.Exists)
                finfo.OpenWrite().Dispose();

            finfo.Refresh();
        }
    }
}
