namespace TaiyaHirameDance.Domain.MainModels
{
    public class TableSetting
    {
        public string TableName { get; set; }
        public bool Checked { get; set; }
        public string WhereClause { get; set; }
        public int? Seq { get; set; }
        public bool OmitConformity { get; set; }
    }
}
