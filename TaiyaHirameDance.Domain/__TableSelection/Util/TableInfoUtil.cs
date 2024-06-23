using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.TableCriteria
{
    public class TableInfoUtil
    {
        public static TableSelections GetTableSelections()
        {
            return new TableSelections(TableInfoDao.GetTables<TableSelection>().ToList());
        }

        /// <summary>
        /// SQLディレクトリに配置されたオリジナルクエリをロードします。
        /// </summary>
        public static List<TableSelection> GetOriginalQueries()
        {
            var dinfo = Common.WorkDir.SubDirectory("SQL");

            if (!dinfo.Exists)
                return [];

            var expansions = from fi in dinfo.GetFiles("*.yml")
                             where fi.Extension == ".yml" && fi.Name != "Sample.yml"
                             select new TableSelection()
                             {
                                 Expansion = YamlUtil.YamlLoad<TableExpansion>(dinfo, fi.GetFileNameWithoutExtension()),
                                 TableName = fi.Name
                             };

            return expansions.ToList();
        }

        public static void PutOriginalQuerySample()
        {
            var sample = new TableExpansion()
            {
                OriginalKeys = [1, 2, 3],
                OriginalPreQuery = @"OPEN SYMMETRIC KEY symmetric_key_sample DECRYPTION BY PASSWORD = 'P@ssw0rd'",
                OriginalQuery = @"SELECT
    pkey1
  , pkey2
  , pkey3
  , CONVERT(NVARCHAR(MAX), DecryptByKey(last_name))  AS last_name
  , CONVERT(NVARCHAR(MAX), DecryptByKey(first_name)) AS first_name
FROM
  SAMPLE"
            };

            var dinfo = Common.WorkDir.SubDirectory("SQL");

            if (!dinfo.Exists)
                return;

            if (!dinfo.File("Sample.yml").Exists)
            {
                try
                {
                    YamlUtil.YamlSave(dinfo, "Sample", sample);
                    File.WriteAllText(dinfo.File(".gitignore").FullName, "Sample.yml");
                }
                catch
                {
                    // 処理なし
                }
            }
        }
    }
}
