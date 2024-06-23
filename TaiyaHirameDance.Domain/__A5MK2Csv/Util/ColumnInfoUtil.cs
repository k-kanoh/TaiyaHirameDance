namespace TaiyaHirameDance.Domain.A5MK2Csv
{
    public class ColumnInfoUtil
    {
        public static ColumnInfos GetColumnInfos()
        {
            return new ColumnInfos(A5MK2.ColumnInfos.ToList());
        }
    }
}
