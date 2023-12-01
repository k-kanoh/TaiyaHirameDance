namespace TaiyaHirameDance.Domain.Base
{
    public abstract class MasterSettingBase
    {
        public abstract string MainDirectory { get; set; }

        public abstract string XlsxAuthor { get; set; }

        public abstract bool OmitCompareRowIsConformity { get; set; }
    }
}
