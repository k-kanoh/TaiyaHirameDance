namespace TaiyaHirameDance.Domain.BcpUtility
{
    public static class Extension
    {
        /// <summary>
        /// リストをBcpTableSelectionsに変換します。
        /// </summary>
        public static BcpTableSelections ToBcpTableSelections(this IEnumerable<BcpTableSelection> items)
        {
            return new BcpTableSelections(items.ToList());
        }

        /// <summary>
        /// リストをBcpTableSelectionsに変換します。
        /// </summary>
        public static BcpTableSelections ToBcpTableSelections(this List<BcpTableSelection> items)
        {
            return new BcpTableSelections(items);
        }
    }
}
