using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalImageProcessor
{
    internal class ImageProcessingTools
    {
        public static void copyImage(Bitmap originalImage, Bitmap resultImage)
        {
            Color pixel;
            for(int x = 0; x < originalImage.Width; x++) 
                for(int y = 0; y < originalImage.Height; y++)
                {
                    pixel = originalImage.GetPixel(x, y);
                    resultImage.SetPixel(x, y, pixel);
                }
        }

        public static void grayscale(Bitmap originalImage, Bitmap resultImage)
        {
            Color pixel;
            for (int x = 0; x < originalImage.Width; x++)
                for (int y = 0; y < originalImage.Height; y++)
                {
                    pixel = originalImage.GetPixel(x, y);
                    int average = (int)((pixel.R + pixel.G + pixel.B) / 3);
                   
                    resultImage.SetPixel(x, y, Color.FromArgb(average, average, average));
                }
        }

        public static void invertColor(Bitmap originalImage, Bitmap resultImage)
        {
            Color pixel;
            for (int x = 0; x < originalImage.Width; x++)
                for (int y = 0; y < originalImage.Height; y++)
                {
                    pixel = originalImage.GetPixel(x, y);
                    resultImage.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
        }

        public static void sepiaEffect(Bitmap originalImage, Bitmap resultImage)
        {
            Color pixel;
            for (int x = 0; x < originalImage.Width; x++)
                for (int y = 0; y < originalImage.Height; y++)
                {
                    pixel = originalImage.GetPixel(x, y);
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    int sepiaR = Math.Min(255, (int)(0.393 * r + 0.769 * g + 0.189 * b));
                    int sepiaG = Math.Min(255, (int)(0.349 * r + 0.686 * g + 0.168 * b));
                    int sepiaB = Math.Min(255, (int)(0.272 * r + 0.534 * g + 0.131 * b));
                    resultImage.SetPixel(x, y, Color.FromArgb(sepiaR, sepiaG, sepiaB));
                }
        }

        public static void histogram(Bitmap originalImage, Bitmap resultImage)
        {
            Color pixel;
            int[] magnitude = new int[256];

            Array.Fill(magnitude, 0);
            for (int x = 0; x < originalImage.Width; x++)
                for (int y = 0; y < originalImage.Height; y++)
                {
                    pixel = originalImage.GetPixel(x, y);
                    int average = (int)((pixel.R + pixel.G + pixel.B) / 3);

                    magnitude[average] += 1;
                }

            Graphics g = Graphics.FromImage(resultImage);
            int max = magnitude.Max();
            int imgHeight = resultImage.Height;
            int width = (int)Math.Floor((float)resultImage.Width / 256);
            for(int i = 0; i < 256; i++)
            {
                int height = (int)((float)magnitude[i] / max * imgHeight);
                g.FillRectangle(Brushes.Gray, width * i, imgHeight - height, width, height);
            }
        }

        public static void subtractImage(Bitmap originalImage, Bitmap backgroundImage, Bitmap resultImage)
        {
            Color green = Color.FromArgb(0, 0, 255);
            int greyGreen = (int)((green.R + green.G + green.B) / 3);
            int threshold = 5   ;
            Color pixel, backpixel;
            for (int x = 0; x < originalImage.Width; x++)
                for (int y = 0; y < originalImage.Height; y++)
                {
                    pixel = originalImage.GetPixel(x, y);
                    
                    int grey = (int)((pixel.R + pixel.G + pixel.B) / 3);
                    int subtractValue = Math.Abs(grey - greyGreen);
                    if(subtractValue > threshold)
                    {
                        
                        resultImage.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        backpixel = backgroundImage.GetPixel(x, y); 
                        resultImage.SetPixel(x, y, backpixel);
                    }
                }
        }
    }
}
