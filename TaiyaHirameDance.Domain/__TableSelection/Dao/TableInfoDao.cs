using Dapper;
using System.Data.SqlClient;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.TableCriteria
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

        public int GetQueryCount(string query, string whereClause = null)
        {
            if (whereClause.Val())
            {
                return _conn.QuerySingle<int>($"SELECT COUNT(*) FROM ({query}) ORIGINALQUERY WHERE {whereClause}");
            }
            else
            {
                return _conn.QuerySingle<int>($"SELECT COUNT(*) FROM ({query}) ORIGINALQUERY");
            }
        }

        public void Dispose()
        {
            _conn.Dispose();
        }
    }
}
