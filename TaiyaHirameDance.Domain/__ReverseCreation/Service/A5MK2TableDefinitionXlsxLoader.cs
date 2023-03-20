using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ReverseCreation
{
    public class A5MK2TableDefinitionXlsxLoader
    {
        public List<A5MK2TableDefinitionXlsx> CreateTableDefinitions(string xlsxPath)
        {
            var tableDefinitions = new List<A5MK2TableDefinitionXlsx>();

            using (var xlsxBook = ExcelUtil.Open(xlsxPath))
            {
                foreach (var sheet in xlsxBook.GetSheets().Skip(1))
                {
                    var tableDef = new A5MK2TableDefinitionXlsx();
                    var tableDefProps = from prop in typeof(A5MK2TableDefinitionXlsx).GetProperties()
                                        where prop.AnyCustomAttribute<XlsxImportOptionAttribute>()
                                        select new { prop, impAttr = prop.FirstCustomAttribute<XlsxImportOptionAttribute>() };

                    foreach (var pair in tableDefProps)
                        pair.prop.SetValue(tableDef, sheet.Cells[pair.impAttr.Pos].TrimmedText());

                    var columnDefProps = from prop in typeof(A5MK2TableDefinitionXlsx.ColumnDefinition).GetProperties()
                                         where prop.AnyCustomAttribute<XlsxImportOptionAttribute>()
                                         select new { prop, impAttr = prop.FirstCustomAttribute<XlsxImportOptionAttribute>() };

                    var currRow = columnDefProps.First().impAttr.RowNo;

                    while (true)
                    {
                        var columnDef = new A5MK2TableDefinitionXlsx.ColumnDefinition();

                        foreach (var pair in columnDefProps)
                            pair.prop.SetValue(columnDef, sheet.Cells[currRow, pair.impAttr.ColumnNo].TrimmedText());

                        if (!columnDef.ColumnName.Val())
                            break;

                        tableDef.Columns.Add(columnDef);

                        currRow++;
                    }

                    currRow += 3;

                    var keyDefProps = from prop in typeof(A5MK2TableDefinitionXlsx.KeyDefinition).GetProperties()
                                      where prop.AnyCustomAttribute<XlsxImportOptionAttribute>()
                                      select new { prop, impAttr = prop.FirstCustomAttribute<XlsxImportOptionAttribute>() };

                    while (true)
                    {
                        var keyDef = new A5MK2TableDefinitionXlsx.KeyDefinition();

                        foreach (var pair in keyDefProps)
                            pair.prop.SetValue(keyDef, sheet.Cells[currRow, pair.impAttr.ColumnNo].TrimmedText());

                        if (!keyDef.KeyName.Val())
                            break;

                        tableDef.Keys.Add(keyDef);

                        currRow++;
                    }

                    tableDefinitions.Add(tableDef);
                }
            }

            return tableDefinitions;
        }
    }
}
