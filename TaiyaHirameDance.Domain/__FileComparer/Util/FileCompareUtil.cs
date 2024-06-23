using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.FileComparer
{
    public static class FileCompareUtil
    {
        public static List<(Comparison compare, DirectoryInfo master, DirectoryInfo target)> CompareDirectories(DirectoryInfo master, DirectoryInfo target, params string[] ignoreNames)
        {
            var masterDirs = master.GetDirectories().Where(x => !ignoreNames.Contains(x.Name));
            var targetDirs = target.GetDirectories().Where(x => !ignoreNames.Contains(x.Name));

            var remove = from x in targetDirs
                         where !masterDirs.Any(y => y.Name == x.Name)
                         select (Comparison.Remove, master.SubDirectory(x.Name), x);

            var push = from x in masterDirs
                       where !targetDirs.Any(y => y.Name == x.Name)
                       select (Comparison.Push, x, target.SubDirectory(x.Name));

            var both = from a in masterDirs
                       join b in targetDirs on a.Name equals b.Name
                       select (Comparison.Both, a, b);

            return both.Concat(push).Concat(remove).ToList();
        }

        public static List<(Comparison compare, FileInfo master, FileInfo target)> CompareFiles(DirectoryInfo master, DirectoryInfo target, params string[] ignoreNames)
        {
            var masterFiles = master.GetFiles().Where(x => !ignoreNames.Contains(x.Name));
            var targetFiles = target.GetFiles().Where(x => !ignoreNames.Contains(x.Name) && !x.Name.StartsWith("~$"));

            var remove = from x in targetFiles
                         where !masterFiles.Any(y => y.Name == x.Name)
                         select (Comparison.Remove, master.File(x.Name), x);

            var push = from x in masterFiles
                       where !targetFiles.Any(y => y.Name == x.Name)
                       select (Comparison.Push, x, target.File(x.Name));

            var both = from a in masterFiles
                       join b in targetFiles on a.Name equals b.Name
                       let sa = a.GetSha1Hash() != b.GetSha1Hash()
                       select (sa ? Comparison.Modified : Comparison.Both, a, b);

            return both.Concat(push).Concat(remove).ToList();
        }

        public static IEnumerable<Action> DirectorySyncInteractive(DirectoryInfo master, DirectoryInfo target, params string[] ignoreNames)
        {
            var dComparisons = CompareDirectories(master, target, ignoreNames);

            foreach (var dComparison in dComparisons.Where(x => x.compare == Comparison.Remove))
            {
                yield return () =>
                {
                    try
                    {
                        dComparison.target.Delete(true);
                    }
                    catch (IOException)
                    {
                        throw new DirectorySyncOperationException(Comparison.Remove, dComparison.target);
                    }
                };
            }

            foreach (var dComparison in dComparisons.Where(x => x.compare == Comparison.Both))
            {
                var fComparisons = CompareFiles(dComparison.master, dComparison.target);

                foreach (var fComparison in fComparisons.Where(x => x.compare == Comparison.Remove))
                {
                    yield return () =>
                    {
                        try
                        {
                            fComparison.target.Delete();
                        }
                        catch (IOException)
                        {
                            throw new DirectorySyncOperationException(Comparison.Remove, fComparison.target);
                        }
                    };
                }

                foreach (var fComparison in fComparisons.Where(x => x.compare == Comparison.Modified))
                {
                    yield return () =>
                    {
                        try
                        {
                            fComparison.master.CopyTo(fComparison.target.FullName, true);
                        }
                        catch (IOException)
                        {
                            throw new DirectorySyncOperationException(Comparison.Modified, fComparison.target);
                        }
                    };
                }
            }

            foreach (var dComparison in dComparisons.Where(x => x.compare == Comparison.Push))
            {
                yield return () =>
                {
                    try
                    {
                        dComparison.master.MoveTo(dComparison.target.FullName);
                    }
                    catch (IOException)
                    {
                        throw new DirectorySyncOperationException(Comparison.Push, dComparison.target);
                    }
                };
            }

            foreach (var dComparison in dComparisons.Where(x => x.compare == Comparison.Both))
            {
                var fComparisons = CompareFiles(dComparison.master, dComparison.target);

                foreach (var fComparison in fComparisons.Where(x => x.compare == Comparison.Push))
                {
                    yield return () =>
                    {
                        try
                        {
                            fComparison.master.CopyTo(fComparison.target.FullName, true);
                        }
                        catch (IOException)
                        {
                            throw new DirectorySyncOperationException(Comparison.Push, fComparison.target);
                        }
                    };
                }
            }
        }
    }
}
