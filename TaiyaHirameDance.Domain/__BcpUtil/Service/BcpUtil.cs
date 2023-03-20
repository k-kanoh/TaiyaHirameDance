using Cysharp.Diagnostics;
using System.Data.SqlClient;
using System.IO.Compression;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.BcpUtility
{
    public class BcpUtil
    {
        public event EventHandler<string> ConsoleReported;

        private readonly string dbUser;
        private readonly string dbPass;
        private readonly string dbName;
        private readonly string dbServer;
        public string DbName => dbName;

        public BcpUtil()
        {
            var connSetting = new SqlConnectionStringBuilder(Setting.ConnectionString);

            dbUser = connSetting.UserID;
            dbPass = connSetting.Password;
            dbName = connSetting.InitialCatalog;
            dbServer = connSetting.DataSource;
        }

        private void Console(string msg)
        {
            ConsoleReported?.Invoke(this, msg);
        }

        public async Task<bool> CheckBcpExists()
        {
            try
            {
                await foreach (var text in ProcessX.StartAsync("bcp /v"))
                    Console(text);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<BcpContainer> BcpBackup(BcpTableSelections tables, IProgress<string> progress, CancellationToken cancel)
        {
            var reporter = new ProgressReporter(Console, progress);

            reporter.BreakLine();

            var container = new BcpContainer()
            {
                RegistedAt = DateTime.Now,
                Tables = tables.Select(x => new BcpContainer.Table() { TableName = x.TableAndSchema, Count = x.TableRowCount }).ToList()
            };

            using (var mStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(mStream, ZipArchiveMode.Create))
                {
                    foreach (var table in tables)
                    {
                        cancel.ThrowIfCancellationRequested();

                        if (table.TableRowCount == 0) continue;

                        var tempExp = Path.GetTempFileName();

                        var (cmd, cmdDisp) = ExportCmd(table.TableAndSchema, tempExp);

                        reporter.All($"{table.TableAndSchema}をエクスポートしています...");
                        reporter.Console(cmdDisp);

                        await foreach (var text in ProcessX.StartAsync(cmd).WithCancellation(cancel))
                            reporter.Console(text);

                        await Task.Run(() => archive.CreateEntryFromFile(tempExp, table.TableAndSchema));

                        File.Delete(tempExp);
                    }
                }

                container.Hash = UtilIO.CreateHashedFile(Util.Roaming.SubDirectory("bcp", dbName), mStream.ToArray(), ".zip");
            }

            container.Save(dbName);

            reporter.End();

            return container;
        }

        public Task BcpRestore(BcpContainer container, BcpTableSelection table, DateTime? backupTimeStamp, IProgress<string> progress, CancellationToken cancel)
        {
            var tables = new List<BcpTableSelection>() { table }.ToBcpTableSelections();

            return BcpRestore(container, tables, backupTimeStamp, progress, cancel);
        }

        public async Task BcpRestore(BcpContainer container, BcpTableSelections tables, DateTime? backupTimeStamp, IProgress<string> progress, CancellationToken cancel)
        {
            var reporter = new ProgressReporter(Console, progress);

            reporter.BreakLine();

            var restores = tables.FilteredChecked();

            var zip = Util.Roaming.File("bcp", dbName, $"{container.Hash}.zip");

            reporter.Progress("ファイルを解凍しています...");

            var tempFiles = new List<(string, string)>();
            using (var archive = ZipFile.OpenRead(zip.FullName))
            {
                foreach (var table in restores)
                {
                    cancel.ThrowIfCancellationRequested();

                    var entry = archive.GetEntry(table.TableAndSchema);

                    if (entry == null) continue;

                    var tempImp = Path.GetTempFileName();

                    await Task.Run(() => entry.ExtractToFile(tempImp, true));

                    tempFiles.Add((table.TableAndSchema, tempImp));
                }
            }

            var dao = new Dao(reporter, cancel);

            if (backupTimeStamp.HasValue)
            {
                var tuples = restores.Select(x => (x.SchemaName, x.TableName, $"{x.TableName}.{backupTimeStamp:yyyyMMddHHmmss}.{container.HashPrefix}")).ToList();
                await dao.BackupTables(tuples);
            }

            await dao.TruncateTables(restores.ToSchemaAndTableNameTuples());

            foreach (var (tableAndSchema, tempImp) in tempFiles)
            {
                cancel.ThrowIfCancellationRequested();

                var (cmd, cmdDisp) = ImportCmd(tableAndSchema, tempImp);

                reporter.All($"{tableAndSchema}をインポートしています...");
                reporter.Console(cmdDisp);

                await foreach (var text in ProcessX.StartAsync(cmd).WithCancellation(cancel))
                    reporter.Console(text);

                File.Delete(tempImp);
            }

            reporter.End();
        }

        private (string cmd, string disp) ExportCmd(string tableAndSchema, string tempExp)
        {
            var cmd = $@"bcp ""{dbName}.{tableAndSchema}"" out ""{tempExp}"" -S ""{dbServer}"" -U ""{dbUser}"" -P ""{dbPass}"" -N";

            var cmdDisp = $@"CMD> bcp ""{dbName}.{tableAndSchema}"" out ""(省略)"" -S ""{dbServer}"" -U ""{dbUser}"" -P ""(省略)"" -N";

            return (cmd, cmdDisp);
        }

        private (string cmd, string disp) ImportCmd(string tableAndSchema, string tempImp)
        {
            var cmd = $@"bcp ""{dbName}.{tableAndSchema}"" in ""{tempImp}"" -S ""{dbServer}"" -U ""{dbUser}"" -P ""{dbPass}"" -N";

            var cmdDisp = $@"CMD> bcp ""{dbName}.{tableAndSchema}"" in ""(省略)"" -S ""{dbServer}"" -U ""{dbUser}"" -P ""(省略)"" -N";

            return (cmd, cmdDisp);
        }

        public Task PopStashTable(BcpTableSelection table, IProgress<string> progress, CancellationToken cancel)
        {
            var tables = new List<BcpTableSelection>() { table }.ToBcpTableSelections();

            return PopStashTable(tables, progress, cancel);
        }

        public async Task PopStashTable(BcpTableSelections tables, IProgress<string> progress, CancellationToken cancel)
        {
            var reporter = new ProgressReporter(Console, progress);

            reporter.BreakLine();

            var dao = new Dao(reporter, cancel);

            var trunc = tables.Select(x => (x.SchemaName, x.StashTableNameParts.orig)).ToList();
            await dao.TruncateTables(trunc);

            var copy = tables.Select(x => (x.SchemaName, x.TableName, x.StashTableNameParts.orig)).ToList();
            await dao.SelectInsertTables(copy);

            await dao.DropTables(tables.ToSchemaAndTableNameTuples());

            reporter.End();
        }

        public async Task CleanStashTables(BcpTableSelections tables)
        {
            var reporter = new ProgressReporter(Console, null);

            reporter.BreakLine();

            var dao = new Dao(reporter, default);

            await dao.DropTables(tables.ToSchemaAndTableNameTuples());

            reporter.End();
        }
    }
}
