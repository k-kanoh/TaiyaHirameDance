using OfficeOpenXml;

namespace TaiyaHirameDance.ToolBox
{
    public class ExcelUtil : IDisposable
    {
        private bool isDisposed;
        protected ExcelPackage package;
        protected ExcelWorkbook Workbook => package.Workbook;

        /// <summary>
        /// コンストラクタ (new不可)
        /// </summary>
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

            try
            {
                using (var stream = open.OpenReadNoLock())
                    return new ExcelUtil() { package = new ExcelPackage(stream) };
            }
            catch
            {
                throw new ArgumentException();
            }
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
            if (isDisposed)
                package.Dispose();

            isDisposed = true;
        }
    }
}
