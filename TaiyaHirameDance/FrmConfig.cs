using TaiyaHirameDance.Domain;
using TaiyaHirameDance.Domain.A5MK2Csv;
using TaiyaHirameDance.SettingModel;
using TaiyaHirameDance.ToolBox;

namespace TaiyaHirameDance
{
    public partial class FrmConfig : DirtyMonitoringForm
    {
        public FrmConfig()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Reload();
        }

        protected override void Save()
        {
            if (IsDirty)
            {
                var save = GetMergedNewDataContext<MainSetting>();
                YamlUtil.YamlSave(Common.WorkDir, "Setting", save, true);
                Setting.Reload();
                base.Save();
            }
        }

        protected override void Reload()
        {
            DataContext = YamlUtil.YamlLoad<MainSetting>(Common.WorkDir, "Setting");
            SetControlValueFromDataContext();
            base.Reload();
        }

        private void A5MK2CsvGroupBox_DragDrop(object sender, DragEventArgs e)
        {
            using (WithWaitCursor())
            {
                var drop = A5MK2CsvGroupBox.DroppedPath[0];

                DirectoryInfo di;

                if (File.GetAttributes(drop).HasFlag(FileAttributes.Directory))
                {
                    di = new DirectoryInfo(drop);
                }
                else
                {
                    di = new FileInfo(drop).Directory;
                }

                if (A5MK2CsvLoader.LoadCsv(di))
                    A5MK2.Save();
            }
        }
    }
}
