using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WebCamLib;

namespace DigitalImageProcessor
{
    public partial class Form1 : Form
    {
        Bitmap loadedImage, backgroundImage, processedImage;
        Mat loaded, processed;
        private VideoCapture capture;
        private Thread cameraThread;
        String mode = "";


        private volatile bool cameraOn = false;

        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = false;
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;
                ImageFormat format = ImageFormat.Png;
                string ext = Path.GetExtension(saveFileDialog1.FileName).ToLower();

                if (string.IsNullOrWhiteSpace(ext))
                {
                    ext = ".png";
                    filePath += ext;
                }

                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case ".png":
                    default:
                        format = ImageFormat.Png;
                        break;
                }

                try
                {
                    if (pictureBox2.Image != null)
                    {
                        pictureBox2.Image.Save(filePath, format);
                        MessageBox.Show("Image saved successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No image found in PictureBox.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save image:\n" + ex.Message);
                }
            }
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
            if (cameraOn)
            {
                mode = "copy";
            }
            else if (loadedImage != null)
            {
                try
                {
                    processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
                    ImageProcessingTools.copyImage(loadedImage, processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }

            }


        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "grayscale";
            }
            else if (loadedImage != null)
            {
                try
                {
                    processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
                    ImageProcessingTools.grayscale(loadedImage, processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }

            }
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "invert";
            }
            else if (loadedImage != null)
            {
                try
                {
                    processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
                    ImageProcessingTools.invertColor(loadedImage, processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "sepia";
            }
            else if (loadedImage != null)
            {
                try
                {
                    processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
                    ImageProcessingTools.sepiaEffect(loadedImage, processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }

            }
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "histogram";
            }
            else if (loadedImage != null)
            {
                try
                {
                    processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
                    ImageProcessingTools.histogram(loadedImage, processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
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
            if (cameraOn)
            {
                mode = "subtract";
            }
            else if (loadedImage != null)
            {
                try
                {
                    processedImage = new Bitmap(loadedImage.Width, loadedImage.Height);
                    ImageProcessingTools.subtractImage(loadedImage, backgroundImage, processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }



        private void CaptureCamera()
        {
            while (cameraOn)
            {
                using (Mat frame = capture.RetrieveMat())
                {
                    if (!frame.Empty())
                    {
                        Bitmap bitmap = BitmapConverter.ToBitmap(frame);


                        lock (this)
                        {
                            loadedImage?.Dispose();
                            loadedImage = (Bitmap)bitmap.Clone();
                        }


                        pictureBox1.Invoke((MethodInvoker)(() =>
                        {
                            pictureBox1.Image?.Dispose();
                            pictureBox1.Image = (Bitmap)bitmap.Clone();
                        }));
                    }
                }
            }
        }


        private void ApplyProcess()
        {
            if (loadedImage == null)
                return;

            Bitmap originalImage;

            lock (this)
            {
                originalImage = (Bitmap)loadedImage.Clone();
            }


            switch (mode)
            {
                case "copy":
                    processedImage = new Bitmap(originalImage.Width, originalImage.Height);
                    ImageProcessingTools.copyImage(originalImage, processedImage);
                    break;

                case "grayscale":
                    processedImage = new Bitmap(originalImage.Width, originalImage.Height);
                    ImageProcessingTools.grayscale(originalImage, processedImage);
                    break;
                case "invert":
                    processedImage = new Bitmap(originalImage.Width, originalImage.Height);
                    ImageProcessingTools.invertColor(originalImage, processedImage);
                    break;
                case "sepia":
                    processedImage = new Bitmap(originalImage.Width, originalImage.Height);
                    ImageProcessingTools.sepiaEffect(originalImage, processedImage);
                    break;
                case "histogram":
                    processedImage = new Bitmap(originalImage.Width, originalImage.Height);
                    ImageProcessingTools.histogram(originalImage, processedImage);
                    break;
                case "subtract":
                    processedImage = new Bitmap(originalImage.Width, originalImage.Height);
                    ImageProcessingTools.subtractImage(originalImage, backgroundImage, processedImage);
                    break;
                case "smooth":
                    ImageProcessingTools.Smooth(ref originalImage, ref processedImage);
                    break;
                case "gaussian":
                    ImageProcessingTools.GaussianBlur(ref originalImage, ref  processedImage);
                    break;
                case "sharpen":
                    ImageProcessingTools.Sharpen(ref originalImage, ref processedImage);
                    break;
                case "mean removal":
                    ImageProcessingTools.MeanRemoval(ref originalImage, ref processedImage);
                    break;
                case "emboss laplacian":
                    ImageProcessingTools.EmbossLaplacian(ref originalImage, ref processedImage);
                    break;
                case "emboss horzverz":
                    ImageProcessingTools.EmbossHorzVerz(ref originalImage, ref processedImage);
                    break;
                case "emboss all":
                    ImageProcessingTools.EmbossAll(ref originalImage, ref processedImage);
                    break;
                case "emboss lossy":
                    ImageProcessingTools.EmbossLossy(ref originalImage, ref processedImage);
                    break;
                case "emboss horizontal":
                    ImageProcessingTools.EmbossHorizontal(ref originalImage, ref processedImage);
                    break;
                case "emboss vertical":
                    ImageProcessingTools.EmbossVertical(ref originalImage, ref processedImage);
                    break;
                default:
                    break;
            }

            pictureBox2.Image = processedImage;


        }

        private void cameraToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!cameraOn)
            {
                capture = new VideoCapture(0);
                cameraOn = true;
                cameraThread = new Thread(CaptureCamera);
                cameraThread.IsBackground = true;
                cameraThread.Start();
                loadedImage = null;
                timer1.Enabled = true;
            }
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                cameraOn = false;
                timer1.Enabled = false;
                if (cameraThread != null && cameraThread.IsAlive)
                {
                    cameraThread.Join(10);
                }
                capture?.Release();
                capture?.Dispose();
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ApplyProcess();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void smoothToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (cameraOn)
            {
                mode = "smooth";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.Smooth(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }

        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "gaussian";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.GaussianBlur(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }

        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "sharpen";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.Sharpen(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "mean removal";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.MeanRemoval(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void laplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "emboss laplacian";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.EmbossLaplacian(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void horizontalVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "emboss horzverz";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.EmbossHorzVerz(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void allDirectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "emboss all";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.EmbossAll(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void lossyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "emboss lossy";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.EmbossLossy(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void horizontalOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "emboss horizontal";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.EmbossHorizontal(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }

        private void verticalOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                mode = "emboss vertical";
            }
            else if (loadedImage != null)
            {
                try
                {
                    ImageProcessingTools.EmbossVertical(ref loadedImage, ref processedImage);
                    pictureBox2.Image = processedImage;
                }
                catch
                {

                }
            }
        }
    }
}
