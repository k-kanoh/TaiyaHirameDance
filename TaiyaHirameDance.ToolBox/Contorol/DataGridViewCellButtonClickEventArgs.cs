namespace TaiyaHirameDance.ToolBox
{
    public class DataGridViewCellButtonClickEventArgs(int columnIndex, int rowIndex, Func<int, DataGridViewRow> rowAccessor) : DataGridViewCellEventArgs(columnIndex, rowIndex)
    {
        public DataGridViewRow Row => rowAccessor(RowIndex);

        public string Value => (string)Row.Cells[ColumnIndex].Value;
    }
}
