using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace TaiyaHirameDance.ToolBox
{
    internal static class UtilOther
    {
        /// <summary>
        /// ミリ秒なしの Datetime.Now を返します
        /// </summary>
        public static DateTime Now
        {
            get
            {
                var now = DateTime.Now;
                return new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerSecond));
            }
        }

        /// <summary>
        /// KeyDataを解析します。
        /// </summary>
        /// <returns>KeyCode</returns>
        public static Keys KeyParse(Keys keyData, out bool ctrl, out bool alt, out bool shift)
        {
            alt = (keyData & Keys.Modifiers & Keys.Alt) == Keys.Alt;
            ctrl = (keyData & Keys.Modifiers & Keys.Control) == Keys.Control;
            shift = (keyData & Keys.Modifiers & Keys.Shift) == Keys.Shift;
            return (keyData & Keys.KeyCode);
        }

        /// <summary>
        /// バイト配列のSHA1ハッシュを返します。
        /// </summary>
        public static string GetSha1Hash(byte[] bytes)
        {
            return string.Join("", SHA1.HashData(bytes).Select(x => x.ToString("x2")));
        }

        /// <summary>
        /// 文字列のSHA1ハッシュを返します。
        /// </summary>
        public static string GetSha1Hash(string text)
        {
            return GetSha1Hash(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// ストリームのSHA1ハッシュを返します。
        /// </summary>
        public static string GetSha1Hash(Stream stream)
        {
            var hash = string.Join("", SHA1.HashData(stream).Select(x => x.ToString("x2")));
            stream.Seek(0, SeekOrigin.Begin);
            return hash;
        }

        /// <summary>
        /// StringBuilder のインスタンスを返します。
        /// </summary>
        public static StringBuilder CreateStringBuilder()
        {
            return new StringBuilder();
        }

        /// <summary>
        /// 対象のクラスに関連付くSQLリソースの中身を取り出します。
        /// </summary>
        public static IEnumerable<string> GetResourceSql(Type type)
        {
            var assembly = Assembly.GetAssembly(type);

            var typePureName = type.Name.RegexReplace("dao$", "", true);
            var resources = assembly.GetManifestResourceNames().Where(x => x.IsMatch($@"\.{typePureName}[0-9]?\.sql$", true));

            foreach (var resource in resources.OrderBy(x => x))
            {
                using (var stream = assembly.GetManifestResourceStream(resource))
                using (var reader = new StreamReader(stream))
                {
                    yield return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// オブジェクトを比較しプロパティをシャローコピーします。
        /// </summary>
        public static void ShallowCopy(object sourceObj, object destObj)
        {
            var joinedProps = from source in sourceObj.GetType().GetProperties()
                              join dest in destObj.GetType().GetProperties()
                              on source.Name equals dest.Name
                              select new { source, dest };

            foreach (var pair in joinedProps)
                if (pair.dest.CanWrite && pair.source.CanRead)
                    pair.dest.SetValue(destObj, pair.source.GetValue(sourceObj));
        }
    }
}
