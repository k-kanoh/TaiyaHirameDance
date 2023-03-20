using System.Text;

namespace TaiyaHirameDance.ToolBox
{
    public static class Util
    {
        /// <summary>
        /// Shift_JIS (CP932)
        /// </summary>
        public static readonly Encoding SJIS;

        static Util()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            SJIS = Encoding.GetEncoding(932);
        }

        /// <summary>
        /// デスクトップ
        /// </summary>
        public static DirectoryInfo Desktop
        {
            get => new(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        /// <summary>
        /// (ユーザ毎) AppData\Roaming\Ikusan
        /// </summary>
        public static DirectoryInfo Roaming
        {
            get => new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).SubDirectory("Ikusan");
        }

        /// <summary>
        /// KeyDataを解析します。
        /// </summary>
        /// <returns>KeyCode</returns>
        public static Keys KeyParse(Keys keyData, out bool ctrl, out bool alt, out bool shift)
            => UtilOther.KeyParse(keyData, out ctrl, out alt, out shift);

        /// <summary>
        /// バイト配列のSHA1ハッシュを返します。
        /// </summary>
        public static string GetSha1Hash(byte[] bytes) => UtilOther.GetSha1Hash(bytes);

        /// <summary>
        /// 文字列のSHA1ハッシュを返します。
        /// </summary>
        public static string GetSha1Hash(string text) => UtilOther.GetSha1Hash(text);

        /// <summary>
        /// StringBuilder のインスタンスを返します。
        /// </summary>
        public static StringBuilder CreateStringBuilder() => UtilOther.CreateStringBuilder();

        /// <summary>
        /// 対象のクラスに関連付くSQLリソースの中身を取り出します。
        /// </summary>
        public static IEnumerable<string> GetResourceSql(Type type) => UtilOther.GetResourceSql(type);

        /// <summary>
        /// オブジェクトを比較しプロパティをシャローコピーします。
        /// </summary>
        public static void ShallowCopy(object sourceObj, object destObj) => UtilOther.ShallowCopy(sourceObj, destObj);

        /// <summary>
        /// 文字列を暗号化します。 (DES-ECBによる簡易な暗号化)
        /// </summary>
        public static string Encrypt(string source) => UtilEncrypt.Encrypt(source);

        /// <summary>
        /// 暗号文字列を復号します。
        /// </summary>
        public static string Decrypt(string source) => UtilEncrypt.Decrypt(source);
    }
}
