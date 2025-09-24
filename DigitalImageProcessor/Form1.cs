using System.Windows.Forms;
using WebCamLib;

namespace DigitalImageProcessor
{
    public partial class Form1 : Form
    {
        Bitmap loadedImage, backgroundImage, processedImage;
        Device[] devices = DeviceManager.GetAllDevices();
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

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
            ImageProcessingTools.sepiaEffect(loadedImage, processedImage);
            pictureBox2.Image = processedImage;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
            ImageProcessingTools.histogram(loadedImage, processedImage);
            pictureBox2.Image = processedImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            backgroundImage = new Bitmap(openFileDialog2.FileName);
            pictureBox3.Image = backgroundImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
            ImageProcessingTools.subtractImage(loadedImage, backgroundImage, processedImage);
            pictureBox2.Image = processedImage;
        }

        private void loadDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            devices = DeviceManager.GetAllDevices();
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap b = new Bitmap(bmap);

            processedImage = b;
            pictureBox2.Image = processedImage;
        }
    }
}
