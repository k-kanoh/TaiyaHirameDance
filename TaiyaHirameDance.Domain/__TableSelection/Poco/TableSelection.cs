using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.TableSelection
{
    public class TableSelection : TableInfoEntity, ICheckableGrid
    {
        public bool IsChecked { get; set; }
        public bool IsQueryError { get; set; }
        public int? QueryRowCount { get; set; }
        public string WhereClause { get; set; }
        public int? Seq { get; set; }

        public int RealRowCount => QueryRowCount ?? TableRow;
        public string TableAndSchema => $"{SchemaName}.{TableName}";

        public TableSelection()
        { }

        public TableSelection(TableSelection source)
        {
            Util.ShallowCopy(source, this);
        }
    }
}
