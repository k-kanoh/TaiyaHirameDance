using OfficeOpenXml;

namespace TaiyaHirameDance.ToolBox
{
    public class ExcelUtil : IDisposable
    {
        protected ExcelPackage package;
        protected ExcelWorkbook Workbook => package.Workbook;

        protected ExcelUtil()
        { }

        /// <summary>
        /// ファイルを開きます。
        /// </summary>
        public static ExcelUtil Open(string xlsxPath)
        {
            var open = new FileInfo(xlsxPath);

            if (!open.Exists)
            {
                var ex = new FileNotFoundException($"ファイル '{open.FullName}' が見つかりませんでした。");
                throw ex;
            }

            using (var stream = open.OpenReadNoLock())
                return new ExcelUtil() { package = new ExcelPackage(stream) };
        }

        /// <summary>
        /// ブックの持つ全てのシートを取り出します。
        /// </summary>
        public IEnumerable<ExcelWorksheet> GetSheets()
        {
            foreach (var sheet in Workbook.Worksheets)
                yield return sheet;
        }

        public void Dispose()
        {
            package.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
