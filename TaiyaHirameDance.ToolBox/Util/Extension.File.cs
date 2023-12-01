using System.Security.Cryptography;

namespace TaiyaHirameDance.ToolBox
{
    public static class FileExtension
    {
        public static string Combine(this DirectoryInfo dinfo, params string[] name)
        {
            return Path.Combine(new[] { dinfo.FullName }.Concat(name.Select(x => x ?? "")).ToArray());
        }

        public static FileInfo File(this DirectoryInfo dinfo, params string[] name)
        {
            return new FileInfo(Combine(dinfo, name));
        }

        public static string GetFileNameWithoutExtension(this FileInfo finfo)
        {
            return Path.GetFileNameWithoutExtension(finfo.Name);
        }

        public static string GetFileNameForSort(this FileInfo finfo)
        {
            var name = Path.GetFileNameWithoutExtension(finfo.Name);
            return name.RegexReplace(@"(\d+)", "000$1").RegexReplace(@"0*(\d{4})", "$1");
        }

        public static DirectoryInfo SubDirectory(this DirectoryInfo dinfo, params string[] name)
        {
            return new DirectoryInfo(Combine(dinfo, name));
        }

        /// <summary>
        /// ファイルをロックせずに読み取りモードで開きます。
        /// </summary>
        public static FileStream OpenReadNoLock(this FileInfo finfo)
        {
            return new FileStream(finfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        /// <summary>
        /// 空ファイルを作成します。
        /// </summary>
        public static void CreateFile(this FileInfo finfo)
        {
            UtilIO.CreateFile(finfo);
        }

        /// <summary>
        /// ファイルのSHA1ハッシュを返します。
        /// </summary>
        public static string GetSha1Hash(this FileInfo finfo)
        {
            return string.Join("", SHA1.HashData(finfo.OpenReadNoLock()).Select(x => x.ToString("x2")));
        }

        public static long LengthSafety(this FileInfo finfo)
        {
            if (finfo == null)
                return 0;

            finfo.Refresh();

            if (!finfo.Exists)
                return 0;

            return finfo.Length;
        }
    }
}
