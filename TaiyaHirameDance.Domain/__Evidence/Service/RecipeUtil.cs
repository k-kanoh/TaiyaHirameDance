using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain
{
    public static class RecipeUtil
    {
        public static IEnumerable<string> GetRecipeList(DirectoryInfo dinfo)
        {
            if (!dinfo.Exists)
                return [];

            return from fi in dinfo.GetFiles($"*.yml")
                   where !new[] { "Setting", "A5MK2" }.Contains(fi.GetFileNameWithoutExtension())
                   orderby fi.GetFileNameForSort()
                   select fi.GetFileNameWithoutExtension();
        }

        public static string GetRecipeNoByKeyInput(string currNo, Keys keyData)
        {
            var key = Util.KeyParse(keyData, out var ctrl, out var alt, out var shift);

            switch (key)
            {
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Home:
                case Keys.End:
                    break;

                default:
                    return null;
            }

            if (!ctrl && !alt && !shift)
            {
                switch (key)
                {
                    case Keys.PageUp:
                        return GetPrevRecipeNo(currNo);

                    case Keys.PageDown:
                        return GetNextRecipeNo(currNo);
                }
            }

            if (ctrl && !alt && !shift)
            {
                switch (key)
                {
                    case Keys.PageUp:
                        return GetPrevRecipeNo(currNo, 10);

                    case Keys.PageDown:
                        return GetNextRecipeNo(currNo, 10);

                    case Keys.Home:
                        return GetPrevRecipeNo(currNo, 1000);

                    case Keys.End:
                        return GetNextRecipeNo(currNo, 1000);
                }
            }

            return null;
        }

        private static string GetNextRecipeNo(string currNo, int skip = 1)
        {
            var ls = GetRecipeList(Common.WorkDir);
            return ls.SkipWhile(x => x != currNo).Skip(skip).FirstOrDefault() ?? ls.LastOrDefault() ?? "1";
        }

        private static string GetPrevRecipeNo(string currNo, int skip = 1)
        {
            var ls = GetRecipeList(Common.WorkDir);
            return ls.Reverse().SkipWhile(x => x != currNo).Skip(skip).FirstOrDefault() ?? ls.FirstOrDefault() ?? "1";
        }

        public static string GetNewRecipeNo()
        {
            var ls = GetRecipeList(Common.WorkDir);
            var last = ls.LastOrDefault() ?? "1";

            string no;
            if (last.Match(@"\d+-(\d+)", out var num1, out _))
            {
                var i = num1.ToInt();
                do no = $"{++i}";
                while (ls.Any(x => x == no));
            }
            else if (last.Match(@"(\d+)", out var num2, out _))
            {
                var i = num2.ToInt();
                do no = $"{++i}";
                while (ls.Any(x => x == no));
            }
            else
            {
                var i = 1;
                do no = $"新しいファイル~{i++}";
                while (ls.Any(x => x == no));
            }

            return no;
        }
    }
}
