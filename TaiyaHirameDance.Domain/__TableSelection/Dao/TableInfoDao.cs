using Dapper;
using Microsoft.Data.SqlClient;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.TableSelection
{
    internal class TableInfoDao : IDisposable
    {
        public static List<T> GetTables<T>() where T : TableInfoEntity
        {
            using (var conn = new SqlConnection(Setting.ConnectionString))
            {
                var sql = Util.GetResourceSql(typeof(TableInfoDao)).Single();
                return conn.Query<T>(sql).ToList();
            }
        }

        private readonly SqlConnection _conn = new(Setting.ConnectionString);

        public int GetRowCount(string tableName, string whereClause)
        {
            return _conn.QuerySingle<int>($"SELECT COUNT(*) FROM {tableName} WHERE {whereClause}");
        }

        public void Dispose()
        {
            _conn.Dispose();
        }
    }
}
