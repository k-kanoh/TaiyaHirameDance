using System.Text;
using TaiyaHirameDance.Domain.ReverseCreation;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class FrmReverseCreation : BaseForm
    {
        public FrmReverseCreation()
        {
            InitializeComponent();

            DragEnter += (s, e) => e.Effect = DragDropEffects.Move;
            DragDrop += (s, e) =>
            {
                using (WithWaitCursor())
                {
                    var drop = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                    var loader = new A5MK2TableDefinitionXlsxLoader();
                    var tableDefinitions = loader.CreateTableDefinitions(drop);

                    foreach (var tableDef in tableDefinitions)
                    {
                        var save = Util.Desktop.File("CreateSQL", $"{tableDef.SchemaName}.{tableDef.TableName}.txt");
                        save.CreateFile();

                        var dropDdl = Util.CreateStringBuilder();

                        if (AssignDropSql.Checked)
                        {
                            dropDdl.AppendLine($"DROP TABLE {tableDef.SchemaName}.{tableDef.TableName};");
                            dropDdl.AppendLine();
                        }

                        File.WriteAllText(save.FullName, dropDdl.ToString() + tableDef.GetCreateSql(), Encoding.UTF8);
                    }
                }
            };
        }
    }
}
