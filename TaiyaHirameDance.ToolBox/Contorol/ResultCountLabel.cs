using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class ResultCountLabel : Label
    {
        private int __dbResultCount = -1;
        private int __checkedCount;

        [Browsable(false)]
        [DefaultValue(-1)]
        public int DbResultCount
        {
            get => __dbResultCount;
            set
            {
                __dbResultCount = value;
                Refresh();
            }
        }

        [Browsable(false)]
        [DefaultValue(0)]
        public int CheckedCount
        {
            get => __checkedCount;
            set
            {
                __checkedCount = value;
                Refresh();
            }
        }

        [DefaultValue(typeof(ContentAlignment), "BottomRight")]
        public override ContentAlignment TextAlign { get => base.TextAlign; set => base.TextAlign = value; }

        public ResultCountLabel()
        {
            TextAlign = ContentAlignment.BottomRight;
        }

        public override void Refresh()
        {
            if (DbResultCount < 0)
            {
                Text = "";
            }
            else if (DbResultCount == 0)
            {
                Text = "データが見つかりませんでした";
                Font = Font.WithBold(false);
                ForeColor = Color.Red;
            }
            else
            {
                if (CheckedCount > 0)
                {
                    Text = $"{DbResultCount} 件 ({CheckedCount})";
                }
                else
                {
                    Text = $"{DbResultCount} 件";
                }

                Font = Font.WithBold(true);
                ForeColor = SystemColors.ControlText;
            }

            base.Refresh();
        }
    }
}
