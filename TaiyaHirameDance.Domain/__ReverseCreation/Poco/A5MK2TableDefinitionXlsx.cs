using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance.Domain.ReverseCreation
{
    public class A5MK2TableDefinitionXlsx
    {
        /// <summary>
        /// スキーマ名
        /// </summary>
        [XlsxImportOption("C4")]
        public string SchemaName { get; set; }

        /// <summary>
        /// 論理テーブル名
        /// </summary>
        [XlsxImportOption("C5")]
        public string LogicalTableName { get; set; }

        /// <summary>
        /// 物理テーブル名
        /// </summary>
        [XlsxImportOption("C6")]
        public string TableName { get; set; }

        /// <summary>
        /// カラム情報
        /// </summary>
        public List<ColumnDefinition> Columns { get; set; } = [];

        /// <summary>
        /// キー情報
        /// </summary>
        public List<KeyDefinition> Keys { get; set; } = [];

        public class ColumnDefinition
        {
            /// <summary>
            /// 論理名
            /// </summary>
            [XlsxImportOption("B14")]
            public string LogicalColumnName { get; set; }

            /// <summary>
            /// 物理名
            /// </summary>
            [XlsxImportOption("C14")]
            public string ColumnName { get; set; }

            /// <summary>
            /// データ型
            /// </summary>
            [XlsxImportOption("D14")]
            public string DataType { get; set; }

            /// <summary>
            /// NOT NULL制約
            /// </summary>
            [XlsxImportOption("E14")]
            public string NotNull { get; set; }

            /// <summary>
            /// デフォルト
            /// </summary>
            [XlsxImportOption("F14")]
            public string Default { get; set; }

            /// <summary>
            /// 備考
            /// </summary>
            [XlsxImportOption("G14")]
            public string Description { get; set; }

            /// <summary>
            /// Create文のカラム部を返します。
            /// </summary>
            public string GetCreateSqlColumnParts()
            {
                var sql = $"{ColumnName} {DataType.ToUpper()} " + (NotNull.Val() ? "NOT NULL " : "") + (Default.Val() ? $"DEFAULT {Default}" : "");
                return sql.Trim();
            }
        }

        public class KeyDefinition
        {
            /// <summary>
            /// キー名
            /// </summary>
            [XlsxImportOption("B")]
            public string KeyName { get; set; }

            /// <summary>
            /// キー対象
            /// </summary>
            [XlsxImportOption("C")]
            public string Columns { get; set; }

            /// <summary>
            /// 主キーか否か
            /// </summary>
            [XlsxImportOption("E")]
            public string IsPKey { get; set; }

            /// <summary>
            /// ユニークキーか否か
            /// </summary>
            [XlsxImportOption("F")]
            public string IsUnique { get; set; }
        }

        /// <summary>
        /// Create文を生成します。
        /// </summary>
        public string GetCreateSql()
        {
            var createDdl = Util.CreateStringBuilder();

            createDdl.AppendLine($"CREATE TABLE {SchemaName}.{TableName}( ");
            createDdl.AppendLine($"  {Columns.First().GetCreateSqlColumnParts()}");

            foreach (var column in Columns.Skip(1))
                createDdl.AppendLine($"  , {column.GetCreateSqlColumnParts()}");

            foreach (var key in Keys)
            {
                if (key.IsPKey.Val())
                {
                    createDdl.AppendLine($"  , PRIMARY KEY({key.Columns})");
                }
                else if (key.IsUnique.Val())
                {
                    createDdl.AppendLine($"  , UNIQUE({key.Columns})");
                }
            }

            createDdl.AppendLine(");");

            if (LogicalTableName.Val())
            {
                createDdl.AppendLine();
                createDdl.AppendLine($"EXEC sys.sp_addextendedproperty");
                createDdl.AppendLine($"  @name       = N'MS_Description'");
                createDdl.AppendLine($", @value      = N'{LogicalTableName}'");
                createDdl.AppendLine($", @level0type = N'SCHEMA'");
                createDdl.AppendLine($", @level0name = N'{SchemaName}'");
                createDdl.AppendLine($", @level1type = N'TABLE'");
                createDdl.AppendLine($", @level1name = N'{TableName}';");
            }

            foreach (var column in Columns)
            {
                var items = new List<string>();

                if (column.LogicalColumnName.Val())
                    items.Add(column.LogicalColumnName);

                if (column.Description.Val())
                    items.Add(column.Description);

                if (items.Any())
                {
                    createDdl.AppendLine();
                    createDdl.AppendLine($"EXEC sys.sp_addextendedproperty");
                    createDdl.AppendLine($"  @name       = N'MS_Description'");
                    createDdl.AppendLine($", @value      = N'{string.Join("\r\n", items)}'");
                    createDdl.AppendLine($", @level0type = N'SCHEMA'");
                    createDdl.AppendLine($", @level0name = N'{SchemaName}'");
                    createDdl.AppendLine($", @level1type = N'TABLE'");
                    createDdl.AppendLine($", @level1name = N'{TableName}'");
                    createDdl.AppendLine($", @level2type = N'COLUMN'");
                    createDdl.AppendLine($", @level2name = N'{column.ColumnName}';");
                }
            }

            return createDdl.ToString();
        }
    }
}
