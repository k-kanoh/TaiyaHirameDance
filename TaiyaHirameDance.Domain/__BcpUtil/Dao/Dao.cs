using Dapper;
using System.Data.SqlClient;

namespace TaiyaHirameDance.Domain.BcpUtility
{
    internal class Dao(ProgressReporter _reporter, CancellationToken _cancel) : IDisposable
    {
        private readonly SqlConnection _conn = new(Setting.ConnectionString);

        public async Task TruncateTables(IList<(string, string)> tables)
        {
            foreach (var (schema, table) in tables)
            {
                _cancel.ThrowIfCancellationRequested();

                var stmt = $"TRUNCATE TABLE {schema}.[{table}]";
                _reporter.All($"{schema}.{table}を空にしています...");
                _reporter.Console($"SQL> {stmt}");

                await _conn.ExecuteAsync(stmt);
            }
        }

        public async Task BackupTables(IList<(string, string, string)> tables)
        {
            foreach (var (schema, src, dest) in tables)
            {
                _cancel.ThrowIfCancellationRequested();

                var stmt = $"SELECT * INTO {schema}.[{dest}] FROM {schema}.[{src}]";
                _reporter.All($"{schema}.{src}を退避しています...");
                _reporter.Console($"SQL> {stmt}");

                await _conn.ExecuteAsync(stmt);
            }
        }

        public async Task SelectInsertTables(IList<(string, string, string)> tables)
        {
            foreach (var (schema, src, dest) in tables)
            {
                _reporter.All($"{schema}.{dest}を復元しています...");

                try
                {
                    var stmt = $"INSERT INTO {schema}.[{dest}] SELECT * FROM {schema}.[{src}]";
                    _reporter.Console($"SQL> {stmt}");

                    await _conn.ExecuteAsync(stmt);
                }
                catch (SqlException ex) when (ex.Number == 8101)
                {
                    _reporter.Console("[SQLエラー][8101]をキャッチしたためSelectInsertTableWithIdentityIsertOnで再実行してみます。");
                    await SelectInsertTableWithIdentityIsertOn(schema, src, dest);
                }
            }
        }

        private async Task SelectInsertTableWithIdentityIsertOn(string schema, string src, string dest)
        {
            var getColumnsQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName ORDER BY ORDINAL_POSITION";
            var columns = string.Join(',', (await _conn.QueryAsync<string>(getColumnsQuery, new { Schema = schema, TableName = dest })));

            var stmt = $"SET IDENTITY_INSERT {schema}.[{dest}] ON;\r\nINSERT INTO {schema}.[{dest}] ({columns}) SELECT * FROM {schema}.[{src}];";
            _reporter.Console($"SQL> {stmt}");

            await _conn.ExecuteAsync(stmt);
        }

        public async Task DropTables(IList<(string, string)> tables)
        {
            foreach (var (schema, table) in tables)
            {
                _cancel.ThrowIfCancellationRequested();

                var stmt = $"DROP TABLE {schema}.[{table}]";
                _reporter.All($"{schema}.{table}を削除しています...");
                _reporter.Console($"SQL> {stmt}");

                await _conn.ExecuteAsync(stmt);
            }
        }

        public void Dispose()
        {
            _conn.Dispose();
        }
    }
}
