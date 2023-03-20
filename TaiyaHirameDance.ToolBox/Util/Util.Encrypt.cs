using System.Security.Cryptography;
using System.Text;

namespace TaiyaHirameDance.ToolBox
{
    internal static class UtilEncrypt
    {
        /// <summary>
        /// 文字列を暗号化します。 (DES-ECBによる簡易な暗号化)
        /// </summary>
        public static string Encrypt(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            using (var des = DES.Create())
            {
                des.Key = Util.SJIS.GetBytes("永江衣玖");
                des.Mode = CipherMode.ECB;
                using (var encryptor = des.CreateEncryptor())
                    return Convert.ToBase64String(encryptor.TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }

        /// <summary>
        /// 暗号文字列を復号します。
        /// </summary>
        public static string Decrypt(string source)
        {
            var bytes = Convert.FromBase64String(source);
            using (var des = DES.Create())
            {
                des.Key = Util.SJIS.GetBytes("永江衣玖");
                des.Mode = CipherMode.ECB;
                using (var decryptor = des.CreateDecryptor())
                    return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(bytes, 0, bytes.Length));
            }
        }
    }
}
