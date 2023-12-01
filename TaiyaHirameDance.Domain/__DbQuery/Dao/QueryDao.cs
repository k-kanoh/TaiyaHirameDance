using Dapper;
using Microsoft.Data.SqlClient;

namespace TaiyaHirameDance.Domain.Repository
{
    internal class QueryDao
    {
        public static IEnumerable<object[,]> MultipleQuery(IEnumerable<string> query)
        {
            using (var conn = new SqlConnection(Setting.ConnectionString))
            {
                var concatQuery = string.Join(Environment.NewLine, query);

                using (var grid = conn.QueryMultiple(concatQuery))
                {
                    foreach (var __ in query)
                    {
                        var res = grid.Read().Select(x => ((IDictionary<string, object>)x).Values);

                        if (res.Count() >= 100000)
                            throw new ArgumentOutOfRangeException();

                        var values = new object[res.Count(), res.FirstOrDefault()?.Count ?? 0];
                        foreach (var data in res.Select((x, i) => new { x, i }))
                            foreach (var val in data.x.Select((y, j) => new { y, j }))
                                values[data.i, val.j] = val.y;

                        yield return values;
                    }
                }
            }
        }
    }
}
