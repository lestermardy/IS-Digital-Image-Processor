using WebCamLib;
using OpenCvSharp;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenCvSharp.Extensions;

namespace DigitalImageProcessor
{
    public partial class Form1 : Form
    {
        Bitmap loadedImage, backgroundImage, processedImage;
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
            else if(loadedImage != null)
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
    }
}
