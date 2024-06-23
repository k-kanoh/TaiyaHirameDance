using System.ComponentModel;

namespace TaiyaHirameDance.ToolBox
{
    public class GroupBox : System.Windows.Forms.GroupBox
    {
        [Browsable(false)]
        public string[] DroppedPath { get; private set; }

        protected override void OnDragEnter(DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            base.OnDragEnter(e);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            DroppedPath = (string[])e.Data.GetData(DataFormats.FileDrop);

            base.OnDragDrop(e);

            DroppedPath = null;
        }
    }
}
