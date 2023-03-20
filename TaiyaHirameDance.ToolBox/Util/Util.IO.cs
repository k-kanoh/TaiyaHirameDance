namespace TaiyaHirameDance.ToolBox
{
    public static class UtilIO
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
                finfo.Create().Dispose();

            finfo.Refresh();
        }

        /// <summary>
        /// ファイルを削除します。
        /// </summary>
        public static bool DeleteQuiet(FileInfo finfo)
        {
            finfo.Refresh();

            if (finfo.Exists)
            {
                try
                {
                    finfo.IsReadOnly = false;
                    finfo.Delete();
                    return true;
                }
                catch
                {
                    // 処理なし
                }
            }

            return false;
        }

        /// <summary>
        /// ディレクトリを削除します。
        /// </summary>
        public static bool DeleteQuiet(DirectoryInfo dinfo)
        {
            dinfo.Refresh();

            if (dinfo.Exists)
            {
                try
                {
                    dinfo.Delete(true);
                    return true;
                }
                catch
                {
                    // 処理なし
                }
            }

            return false;
        }

        /// <summary>
        /// ファイルをハッシュ化して保存します。
        /// </summary>
        public static string CreateHashedFile(DirectoryInfo dinfo, byte[] bytes, string extension)
        {
            var hash = Util.GetSha1Hash(bytes);
            var save = dinfo.File($"{hash}{extension}");

            if (!save.Exists)
            {
                save.CreateFile();
                File.WriteAllBytes(save.FullName, bytes);
                save.IsReadOnly = true;
            }

            return hash;
        }
    }
}
