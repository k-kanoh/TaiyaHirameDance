using YamlDotNet.Serialization;

namespace TaiyaHirameDance.ToolBox
{
    public static class YamlUtil
    {
        private static readonly ISerializer YamlSerializer =
            new SerializerBuilder().ConfigureDefaultValuesHandling((DefaultValuesHandling)7).Build();

        public static void YamlSave(DirectoryInfo dinfo, string name, object obj, bool deleteEmptyFile = false)
        {
            var ni = YamlSerializer.Serialize(obj);

            var save = dinfo.File($"{name}.yml");

            save.CreateFile();

            if (deleteEmptyFile && ni.StartsWith("{}"))
            {
                save.DeleteQuiet();
            }
            else
            {
                File.WriteAllText(save.FullName, ni);
            }
        }

        private static readonly IDeserializer YamlDeserializer = new DeserializerBuilder().Build();

        public static T YamlLoad<T>(DirectoryInfo dinfo, string name) where T : new()
        {
            var load = dinfo.File($"{name}.yml");

            if (!load.Exists)
                return default;

            return YamlDeserializer.Deserialize<T>(File.ReadAllText(load.FullName));
        }

        public static T YamlLoadOrNew<T>(DirectoryInfo dinfo, string name) where T : new()
        {
            var load = dinfo.File($"{name}.yml");

            if (!load.Exists)
                return new T();

            return YamlDeserializer.Deserialize<T>(File.ReadAllText(load.FullName)) ?? new T();
        }

        public static bool YamlLoadOrNew<T>(DirectoryInfo dinfo, string name, out T instance, ref string oldHash) where T : new()
        {
            instance = default;

            var load = dinfo.File($"{name}.yml");

            if (!load.Exists)
            {
                instance = new T();
                oldHash = null;
                return true;
            }

            var hash = load.GetSha1Hash();

            if (hash == oldHash)
                return false;

            instance = YamlLoadOrNew<T>(dinfo, name);

            oldHash = hash;

            return true;
        }

        public static bool YamlRename(DirectoryInfo dinfo, string name, string dest)
        {
            try
            {
                var source = dinfo.File($"{name}.yml");
                var target = dinfo.File($"{dest}.yml");

                if (target.Exists)
                    return false;

                if (!source.Exists)
                    source.CreateFile();

                source.MoveTo(target.FullName);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
