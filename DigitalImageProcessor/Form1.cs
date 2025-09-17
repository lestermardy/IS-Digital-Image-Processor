

namespace DigitalImageProcessor
{
    public partial class Form1 : Form
    {
        Bitmap loadedImage, processedImage;
        public Form1()
        {
            InitializeComponent();
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loadedImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadedImage;
        }

        private void copyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
            ImageProcessingTools.copyImage(loadedImage, processedImage);
            pictureBox2.Image = processedImage;
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
            ImageProcessingTools.grayscale(loadedImage, processedImage);
            pictureBox2.Image = processedImage;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
            ImageProcessingTools.invertColor(loadedImage, processedImage);
            pictureBox2.Image = processedImage;
        }
    }
}
