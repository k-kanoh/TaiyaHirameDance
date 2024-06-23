namespace TaiyaHirameDance.ToolBox
{
    public class PictureBox : System.Windows.Forms.PictureBox
    {
        public void SetImage(Image image)
        {
            Image?.Dispose();
            Image = image;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Image?.Dispose();

            base.Dispose(disposing);
        }
    }
}
