using System.Diagnostics;
using System.Drawing.Imaging;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.MainModels
{
    public static class EvidenceUtil
    {
        /// <summary>
        /// Xlsxファイルをハッシュ化して保存します。
        /// </summary>
        internal static string CreateHashedXlsx(byte[] bytes)
        {
            return UtilIO.CreateHashedFile(Common.WorkDir, bytes, ".xlsx");
        }

        /// <summary>
        /// 画像をハッシュ化して保存します。
        /// </summary>
        internal static string CreateHashedImage(Image image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Png);
                var bytes = memory.ToArray();
                return UtilIO.CreateHashedFile(Common.WorkDir, bytes, ".png");
            }
        }

        /// <summary>
        /// テキストをハッシュ化して保存します。
        /// </summary>
        internal static string CreateHashedTextIncrease(FileInfo source, long offset)
        {
            var length = source.LengthOrZero() - offset;

            if (length < 0) length = 0;

            var bytes = new byte[length];

            if (length > 0)
            {
                using (var stream = source.OpenReadNoLock())
                {
                    stream.Seek(offset, SeekOrigin.Begin);
                    stream.Read(bytes, 0, bytes.Length);
                }
            }

            return UtilIO.CreateHashedFile(Common.WorkDir, bytes, source.Extension);
        }

        /// <summary>
        /// ファイルのコピーをハッシュ化して保存します。
        /// </summary>
        internal static string CreateHashedCopy(FileInfo source)
        {
            var hash = source.GetSha1Hash();
            var save = Common.WorkDir.File($"{hash}{source.Extension}");

            if (!save.Exists)
            {
                source.CopyTo(save.FullName);
                save.Refresh();
                save.IsReadOnly = true;
            }

            return hash;
        }

        /// <summary>
        /// ハッシュ文字列からファイルを取り出します。
        /// </summary>
        public static FileInfo GetHashedFile(string hash)
        {
            return Common.WorkDir.GetFiles($"{hash}.*").FirstOrDefault();
        }

        public static Image LoadImage(string hash)
        {
            var load = Common.WorkDir.File($"{hash}.png");

            if (load.Exists)
                return Image.FromFile(load.FullName);

            return null;
        }

        public static void OpenOnApplication(string hash)
        {
            var open = GetHashedFile(hash);

            if (open == null) return;

            Process.Start(new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = open.FullName
            });
        }

        public static void DeleteFile(string hash)
        {
            var delete = GetHashedFile(hash);

            if (delete == null) return;

            delete.DeleteQuiet();
        }
    }
}
