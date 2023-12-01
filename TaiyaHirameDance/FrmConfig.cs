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
            using (Executing())
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

                if (new A5MK2CsvLoader().LoadCsv(di))
                    A5MK2.Save();
            }
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsDirty)
                return;

            if (InfoMessageBoxYesNoCancel("変更を保存しますか?", out var isYes))
            {
                if (isYes)
                    Save();

                return;
            }

            e.Cancel = true;
        }
    }
}
