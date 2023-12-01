namespace TaiyaHirameDance.Domain.TableSelection
{
    public class TableInfoQuery
    {
        public static TableSelections GetTableSelections()
        {
            return new TableSelections(TableInfoDao.GetTables<TableSelection>());
        }
    }
}
