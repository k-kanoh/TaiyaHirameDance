namespace TaiyaHirameDance.Domain.Base
{
    public abstract class MainSettingBase
    {
        public abstract string ConnectionString { get; set; }

        public abstract string IgnoreSchema { get; set; }

        public abstract string IgnoreTable { get; set; }

        public abstract string TableColor { get; set; }
    }
}
