using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class LinkLabel : System.Windows.Forms.LinkLabel
    {
        [Category("色")]
        [Description("マウスを乗せた時の色を設定します。")]
        [DefaultValue(typeof(Color), "Red")]
        public Color HoverColor { get; set; } = Color.Red;

        private Color _RemindColor;

        protected override void OnMouseEnter(EventArgs e)
        {
            _RemindColor = LinkColor;
            LinkColor = HoverColor;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            LinkColor = _RemindColor;
            base.OnMouseLeave(e);
        }
    }
}
