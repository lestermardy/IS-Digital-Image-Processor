using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
