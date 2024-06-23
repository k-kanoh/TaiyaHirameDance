using System.Globalization;
using System.Text.RegularExpressions;

namespace TaiyaHirameDance.ToolBox
{
    public static class StringExtension
    {
        /// <summary>
        /// 文字列を "!string.IsNullOrEmpty(文字列)" で判定します。
        /// </summary>
        public static bool Val(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 文字列が null もしくは Empty の場合 null を返します。
        /// それ以外は文字列をそのまま返します。
        /// </summary>
        public static string ValOrNull(this string value)
        {
            return value.Val() ? value : null;
        }

        /// <summary>
        /// 文字列が指定された正規表現にマッチするか判定します。
        /// </summary>
        public static bool IsMatch(this string value, string pattern, bool ignoreCase = false)
        {
            return Regex.IsMatch(value, pattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 文字列を正規表現で置き換えします。
        /// </summary>
        public static string RegexReplace(this string value, string pattern, string replacement, bool ignoreCase = false)
        {
            return Regex.Replace(value, pattern, replacement, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 文字列を正規表現でキャプチャします。 (0起算)
        /// </summary>
        public static string Match(this string value, string pattern, int index = 0)
        {
            return Regex.Match(value, pattern).Groups.Cast<Group>().Select(x => x.Value).Skip(index + 1).FirstOrDefault() ?? "";
        }

        /// <summary>
        /// 文字列を正規表現でキャプチャします。
        /// </summary>
        public static bool Match(this string value, string pattern, out string match1, out (string, string) match2, out (string, string, string) match3)
        {
            var m = Regex.Match(value, pattern);
            var groups = m.Groups.Cast<Group>().Select(x => x.Value).Skip(1).ToList();
            var grp1 = groups.Count > 0 ? groups[0] : "";
            var grp2 = groups.Count > 1 ? groups[1] : "";
            var grp3 = groups.Count > 2 ? groups[2] : "";

            match1 = grp1;
            match2 = (grp1, grp2);
            match3 = (grp1, grp2, grp3);

            return m.Success;
        }

        /// <summary>
        /// 文字列を空白文字列で分割します。
        /// </summary>
        public static string[] SplitByWhiteSpace(this string value)
        {
            return Regex.Split(value, @"\s+");
        }

        /// <summary>
        /// 文字列を分割してキーと値のペアで返します。
        /// </summary>
        public static KeyValuePair<string, string> Split2(this string value, char separator)
        {
            var strings = value.Split(separator);
            return new KeyValuePair<string, string>(strings[0], strings[1]);
        }

        /// <summary>
        /// 文字列を安全に Trim します。
        /// </summary>
        public static string TrimSafety(this string value)
        {
            return value?.Trim();
        }

        /// <summary>
        /// 文字列の前後の「\r」「\n」「(空白)」を Trim します。
        /// </summary>
        public static string TrimNewline(this string value)
        {
            return value?.Trim('\r', '\n').Trim();
        }

        /// <summary>
        /// 文字列をInt型にキャストします。失敗した場合は null を返します。
        /// </summary>
        public static int? ToInt(this string source)
        {
            return source.Val() && int.TryParse(source, out int value) ? value : default(int?);
        }

        /// <summary>
        /// 文字列をColor型にキャストします。失敗した場合は null を返します。
        /// </summary>
        public static Color? ToColor(this string htmlColor)
        {
            try
            {
                return htmlColor.Val() ? ColorTranslator.FromHtml(htmlColor) : default(Color?);
            }
            catch
            {
                return null;
            }
        }

        private static readonly CompareInfo compareInfo = new CultureInfo(0x411).CompareInfo;

        /// <summary>
        /// 文字列が一致する場合は true を返します。 (大文字/小文字、半角カナ/全角カナの違いを無視します)
        /// </summary>
        public static bool WideEquals(this string source, string value)
        {
            return source.Val() && compareInfo.Compare(source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth) == 0;
        }

        /// <summary>
        /// 文字列が指定した文字列を含む場合は true を返します。 (大文字/小文字、半角カナ/全角カナの違いを無視します)
        /// </summary>
        public static bool WideContains(this string source, string value)
        {
            return source.Val() && compareInfo.IndexOf(source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth) >= 0;
        }

        /// <summary>
        /// Boldを設定した新しいフォントオブジェクトを返します。
        /// </summary>
        public static Font WithBold(this Font font, bool bold = true)
        {
            var style = font.Style;

            if (bold)
            {
                style |= FontStyle.Bold;
            }
            else
            {
                style &= ~FontStyle.Bold;
            }

            return new Font(font, style);
        }
    }
}
