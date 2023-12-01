using System.Diagnostics;
using System.Drawing.Imaging;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    public static class EvidenceUtil
    {
        public static string SaveBinary(DirectoryInfo dinfo, byte[] bytes, string extension)
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

        public static string SaveImage(DirectoryInfo dinfo, Image image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Png);
                var bytes = memory.ToArray();
                return SaveBinary(dinfo, bytes, ".png");
            }
        }

        public static string SaveFile(DirectoryInfo dinfo, FileInfo source)
        {
            var hash = source.GetSha1Hash();
            var save = dinfo.File($"{hash}{source.Extension}");

            if (!save.Exists)
            {
                source.CopyTo(save.FullName);
                save.Refresh();
                save.IsReadOnly = true;
            }

            return hash;
        }

        public static string SaveText(DirectoryInfo dinfo, FileInfo orig, long offset = 0)
        {
            var length = orig.LengthSafety() - offset;

            if (length <= 0)
                return null;

            var bytes = new byte[length];

            using (var stream = orig.OpenReadNoLock())
            {
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Read(bytes, 0, bytes.Length);
            }

            return SaveBinary(dinfo, bytes, ".txt");
        }

        public static Image LoadImage(DirectoryInfo dinfo, string hash)
        {
            var load = dinfo.File($"{hash}.png");

            if (load.Exists)
                return Image.FromFile(load.FullName);

            return null;
        }

        public static void OpenOnApplication(DirectoryInfo dinfo, string hash)
        {
            var open = dinfo.GetFiles($"{hash}.*").FirstOrDefault();

            if (open != null && new[] { ".txt", ".xlsx", ".png" }.Contains(open.Extension))
            {
                Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = open.FullName
                });
            }
        }

        public static void DeleteFile(DirectoryInfo dinfo, string hash)
        {
            var delete = dinfo.GetFiles($"{hash}.*").FirstOrDefault();

            if (delete == null)
                return;

            try
            {
                delete.IsReadOnly = false;
                delete.Delete();
            }
            catch
            {
                // 処理なし
            }
        }
    }
}
