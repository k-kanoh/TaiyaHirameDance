using Dapper;
using System.Data.SqlClient;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ExcelExporter
{
    internal class Dao : IDisposable
    {
        private readonly SqlConnection _conn = new(Setting.ConnectionString);

        public Dao()
        {
            _conn.Open();
        }

        public void ExecutePreQuery(IEnumerable<string> preQueries)
        {
            foreach (var preQuery in preQueries.Where(x => x.Val()))
                _conn.Execute(preQuery);
        }

        public IEnumerable<(string[] fields, object[,] values)> MultipleQuery(IEnumerable<string> queries)
        {
            var concatQuery = string.Join(";", queries);

            using (var grid = _conn.QueryMultiple(concatQuery))
            {
                foreach (var __ in queries)
                {
                    var res = grid.Read().Select(x => (IDictionary<string, object>)x);

                    var fields = res.FirstOrDefault()?.Keys.ToArray() ?? ["", "", ""];

                    if (res.Count() >= 100000)
                        throw new ArgumentOutOfRangeException();

                    var values = new object[res.Count(), fields?.Count() ?? 0];
                    foreach (var data in res.Select((x, i) => new { x = x.Values, i }))
                        foreach (var val in data.x.Select((y, j) => new { y, j }))
                            values[data.i, val.j] = val.y;

                    yield return (fields, values);
                }
            }
        }

        public void Dispose()
        {
            _conn.Dispose();
        }
    }
}
