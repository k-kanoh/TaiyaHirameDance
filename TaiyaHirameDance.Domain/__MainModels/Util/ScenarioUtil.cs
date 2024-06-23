using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.MainModels
{
    public static class ScenarioUtil
    {
        public static IEnumerable<string> GetScenarioNames(DirectoryInfo dinfo)
        {
            if (!dinfo.Exists)
                return [];

            return from fi in dinfo.GetFiles("*.yml")
                   where fi.Extension == ".yml" && fi.Name != "Setting.yml" && fi.Name != "A5MK2.yml"
                   orderby fi.GetFileNameForSort()
                   select fi.GetFileNameWithoutExtension();
        }

        public static string GetScenarioNoByKeyInput(string currNo, Keys keyData)
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
                        return GetPrevScenarioNo(currNo);

                    case Keys.PageDown:
                        return GetNextScenarioNo(currNo);
                }
            }

            if (ctrl && !alt && !shift)
            {
                switch (key)
                {
                    case Keys.PageUp:
                        return GetPrevScenarioNo(currNo, 10);

                    case Keys.PageDown:
                        return GetNextScenarioNo(currNo, 10);

                    case Keys.Home:
                        return GetPrevScenarioNo(currNo, 1000);

                    case Keys.End:
                        return GetNextScenarioNo(currNo, 1000);
                }
            }

            return null;
        }

        private static string GetNextScenarioNo(string currNo, int skip = 1)
        {
            var ls = GetScenarioNames(Common.WorkDir);
            return ls.SkipWhile(x => x != currNo).Skip(skip).FirstOrDefault() ?? ls.LastOrDefault() ?? "1";
        }

        private static string GetPrevScenarioNo(string currNo, int skip = 1)
        {
            var ls = GetScenarioNames(Common.WorkDir);
            return ls.Reverse().SkipWhile(x => x != currNo).Skip(skip).FirstOrDefault() ?? ls.FirstOrDefault() ?? "1";
        }

        public static string GetNewScenarioNo()
        {
            var ls = GetScenarioNames(Common.WorkDir);
            var last = ls.LastOrDefault() ?? "1";

            string no;
            if (last.Match(@"\d+-(\d+)", out var num1, out _, out _))
            {
                var i = num1.ToInt();
                do no = $"{++i}";
                while (ls.Any(x => x == no));
            }
            else if (last.Match(@"(\d+)", out var num2, out _, out _))
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

        public static DirectoryInfo SaveEvidencesPrepare()
        {
            var names = GetScenarioNames(Common.WorkDir) ?? throw new FileNotFoundException();

            var scenarios = names.Select(x => (x, Scenario.YamlLoadOrNew(x))).ToList();

            var prepareDir = Common.WorkDir.SubDirectory($"{DateTime.Now:yyyyMMddHHmmss}");

            prepareDir.Create();

            foreach (var (name, scenario) in scenarios)
            {
                var dir = prepareDir.SubDirectory(name);

                dir.Create();

                foreach (var file in scenario.Evidences)
                    file.EjectFile(dir);
            }

            return prepareDir;
        }
    }
}
