using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.TableCriteria
{
    public class TableSelection : TableInfoEntity, ICheckableGrid
    {
        public bool IsChecked { get; set; }
        public int? QueryRowCount { get; set; }
        public string WhereClause { get; set; }
        public int? Seq { get; set; }
        public bool OmitConformity { get; set; }
        public string QueryErrorMsg { get; set; }
        public TableExpansion Expansion { get; set; }

        public int RealRowCount => QueryRowCount ?? TableRowCount;
        public string TableAndSchema => SchemaName.Val() ? $"{SchemaName}.{TableName}" : null;
        public bool IsEvidenceTableCandidate => (IsChecked || WhereClause.TrimSafety().Val() || Seq.HasValue || OmitConformity) && !QueryErrorMsg.Val();

        public TableSelection()
        { }

        public TableSelection(TableSelection source)
        {
            Util.ShallowCopy(source, this);
        }
    }
}
