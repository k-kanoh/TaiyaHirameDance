using TaiyaHirameDance.Domain.TableCriteria;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.BcpUtility
{
    public class BcpTableSelection : TableSelection
    {
        public bool IsStash { get; private set; }

        public int? BackupedCount { get; set; }

        public bool IsOwnedStash { get; set; }

        public (string orig, string datetime, string container) StashTableNameParts { get; private set; }

        public BcpTableSelection(TableSelection source)
        {
            Util.ShallowCopy(source, this);

            IsStash = TableName.Match(@"^(.*)\.([0-9]{14})\.([0-9a-f]{6})$", out _, out _, out var tuple);
            StashTableNameParts = tuple;
        }
    }
}
