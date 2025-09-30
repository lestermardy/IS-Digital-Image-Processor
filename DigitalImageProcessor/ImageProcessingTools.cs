using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
            if (originalImage == null) return;
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
            if (originalImage == null) return;
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
            if (originalImage == null) return;
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
            if (originalImage == null) return;
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
            if (originalImage == null) return;
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
            
            Color green = Color.FromArgb(0, 255, 0);
            int greyGreen = (int)((green.R + green.G + green.B) / 3);
            int threshold = 5   ;
            Color pixel, backpixel;
            int width = Math.Min(originalImage.Width, backgroundImage.Width);
            int height = Math.Min(originalImage.Height, backgroundImage.Height);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
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
        public static void Smooth(ref Bitmap originalImage, ref Bitmap resultImage)

        {
            if (originalImage == null) return;

            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();
            float[,] kernelData = new float[,]
            {
                {  1, 1,  1 },
                { 1,  1, 1 },
                {  1, 1,  1 }
            };
            kernelData = NormalizeData(kernelData, 9);

            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        public static void GaussianBlur(ref Bitmap originalImage, ref Bitmap resultImage)

        {
            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();
            float[,] kernelData = new float[,]
            {
                {  1, 2,  1 },
                { 2,  4, 2 },
                {  1, 2,  1 }
            };
            kernelData = NormalizeData(kernelData, 16);

            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel);
            resultImage = BitmapConverter.ToBitmap(result);
        }



        public static void Sharpen(ref Bitmap originalImage, ref Bitmap resultImage)

        {
            if (originalImage == null) return;
            
            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();
            float[,] kernelData = new float[,]
            {
                {  0, -2,  0 },
                { -2,  11, -2 },
                {  0, -2,  0 }
            };
            kernelData = NormalizeData(kernelData, 3);
            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        public static void MeanRemoval(ref Bitmap originalImage, ref Bitmap resultImage)

        {
            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();
            float[,] kernelData = new float[,]
            {
                {  -1, -1,  -1 },
                { -1,  9, -1 },
                {  -1, -1,  -1 }
            };

            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel);
            resultImage = BitmapConverter.ToBitmap(result);
        }


        public static void EmbossLaplacian(ref Bitmap originalImage, ref Bitmap resultImage)

        {
            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();


            float[,] kernelData = new float[,]
            {
                { -1, 0, -1 },
                { 0,  4, 0 },
                {  -1,  0, -1 }
            };
            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel, delta: 127);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        public static void EmbossHorzVerz(ref Bitmap originalImage, ref Bitmap resultImage)

        {

            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();


            float[,] kernelData = new float[,]
            {
            { 0, -1, 0 },
            { -1,  4, -1 },
            {  0,  -1, 0 }
            };
            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel, delta: 127);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        public static void EmbossAll(ref Bitmap originalImage, ref Bitmap resultImage)
        {
            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();

            float[,] kernelData = new float[,]
            {
                { -1, -1, -1 },
                { -1,  8, -1 },
                { -1, -1, -1 }
            };
            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel, delta: 127);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        public static void EmbossLossy(ref Bitmap originalImage, ref Bitmap resultImage)
        {
            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();

            float[,] kernelData = new float[,]
            {
                {  1, -2,  1 },
                { -2,  4, -2 },
                { -2,  1, -2 }
            };
            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel, delta: 127);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        public static void EmbossHorizontal(ref Bitmap originalImage, ref Bitmap resultImage)
        {
            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();

            float[,] kernelData = new float[,]
            {
                { 0,  0,  0 },
                { -1, 2, -1 },
                { 0,  0,  0 }
            };
            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel, delta: 127);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        public static void EmbossVertical(ref Bitmap originalImage, ref Bitmap resultImage)
        {
            if (originalImage == null) return;

            Mat orig = BitmapConverter.ToMat(originalImage);
            Mat result = new Mat();

            float[,] kernelData = new float[,]
            {
                { 0,  -1,  0 },
                { 0, 0, 0 },
                { 0,  1,  0 }
            };
            Mat kernel = Mat.FromArray(kernelData);

            Cv2.Filter2D(orig, result, orig.Depth(), kernel, delta: 127);
            resultImage = BitmapConverter.ToBitmap(result);
        }

        
        private static float[,] NormalizeData(float[,] data, int normalizationFactor)
        {
            if (normalizationFactor == 0) return data;
            for(int i = 0; i < data.GetLength(0); i++)
            {
                for(int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = data[i, j] / normalizationFactor;
                }
            }
            return data;
        }





















        public static bool CSmooth(Bitmap b, int nWeight /* default to 1 */)

        {

            ConvMatrix m = new ConvMatrix();

            m.SetAll(1);

            m.Pixel = nWeight;

            m.Factor = nWeight + 8;

            return BitmapFilter.Conv3x3(b,m);

        }

        public static bool CGaussianBlur(Bitmap b, int nWeight /* default to 1 */)

        {

            ConvMatrix m = new ConvMatrix();

            m.SetAll(1);

            m.TopLeft = 1;
            m.TopMid = 2;
            m.TopRight = 1;

            m.MidLeft = 2;
            m.Pixel = nWeight;
            m.MidRight = 2;

            m.BottomLeft = 1;
            m.BottomMid = 2;
            m.BottomRight = 1;


            m.Factor = nWeight + 12;

            return BitmapFilter.Conv3x3(b, m);

        }

        public static bool CSharpen(Bitmap b, int nWeight /* default to 1 */)

        {

            ConvMatrix m = new ConvMatrix();

            m.SetAll(1);

            m.TopLeft = 0;
            m.TopMid = -2;
            m.TopRight = 0;

            m.MidLeft = -2;
            m.Pixel = nWeight;
            m.MidRight = -2;

            m.BottomLeft = 0;
            m.BottomMid = -2;
            m.BottomRight = 0;


            m.Factor = nWeight - 8;
            if (m.Factor <= 0) m.Factor = 1;

            return BitmapFilter.Conv3x3(b, m);

        }

        public static bool CMeanRemoval(Bitmap b, int nWeight /* default to 1 */)

        {

            ConvMatrix m = new ConvMatrix();

            m.SetAll(-1);

            m.Pixel = nWeight;

            m.Factor = nWeight - 8;
            if (m.Factor <= 0) m.Factor = 1;

            return BitmapFilter.Conv3x3(b, m);

        }

        public static bool CEmbossing(Bitmap b, int nWeight /* default to 1 */)

        {

            ConvMatrix m = new ConvMatrix();

            m.TopLeft = -1;
            m.TopMid = 0;
            m.TopRight = -1;

            m.MidLeft = 0;
            m.Pixel = nWeight;
            m.MidRight = 0;

            m.BottomLeft = -1;
            m.BottomMid = 0;
            m.BottomRight = -1;

            m.Factor = nWeight - 4;
            if (m.Factor <= 0) m.Factor = 1;

            m.Offset = 127;

            return BitmapFilter.Conv3x3(b, m);

        }
    }
}
