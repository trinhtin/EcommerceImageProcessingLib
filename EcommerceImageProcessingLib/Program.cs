//todo: Generate images for Facebook new feed
//Input: Image 900x900 px
//Output: Image with text and logo, icon has been rendered
//KPI: 
//Assigned: Nam

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace EcommerceImageProcessingLib
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "seiko2.jpg";
            string output = @"D:\\output.png";
            string logo = "logo.png";

            var heading1Font = new Font("Arial Black", 30, FontStyle.Bold);
            var heading2Font = new Font("Arial", 30, FontStyle.Regular);
            var bodyFont = new Font("Arial", 20, FontStyle.Regular);

            //Create new bitmap 1000x1000
            Bitmap finalImage = new Bitmap(1000, 1000);
            Graphics g = Graphics.FromImage(finalImage);

            //Set background color
            g.Clear(System.Drawing.Color.White);

            //Load product image, scale to 900x900
            //Draw product image at 100,0
            System.Drawing.Bitmap inputImage = new System.Drawing.Bitmap(input);
            g.DrawImage(inputImage, new System.Drawing.Rectangle(-100, 100, inputImage.Width, inputImage.Height));


            //Load logo
            //Draw logo at end
            System.Drawing.Bitmap logoImage = new System.Drawing.Bitmap(logo);
            logoImage = ResizeImage(logoImage, 291, 61);
            g.DrawImage(logoImage, new System.Drawing.Rectangle(finalImage.Width - logoImage.Width - 50, finalImage.Height - logoImage.Height - 50, logoImage.Width, logoImage.Height));

            //Load logo
            //Draw logo at end
            System.Drawing.Bitmap hotImage = new System.Drawing.Bitmap("hot.png");
            logoImage = ResizeImage(hotImage, 200, 200);
            g.DrawImage(logoImage, new System.Drawing.Rectangle(800, 50, logoImage.Width, logoImage.Height));

            //Draw product title
            string text = "ANNE KLEIN AK/3480RGTN";
            var textSize = g.MeasureString(text, heading1Font);
            //g.RotateTransform(-30);
            g.FillRectangle(new SolidBrush(Color.Red), 100, 0, textSize.Width, textSize.Height);
            g.DrawString(text, heading1Font, new SolidBrush(Color.White), new Point(100, 0));

            //Draw product price
            text = "Trọn gói chỉ 1.746.000";
            textSize = g.MeasureString(text, heading2Font);
            g.FillRectangle(new SolidBrush(Color.Red), 100, 50, textSize.Width, textSize.Height);
            g.DrawString(text, heading2Font, new SolidBrush(Color.White), new Point(100, 50));

            //Draw functional text
            //g.RotateTransform(0);
            g.DrawString("Đường kính: 30mm", bodyFont, new SolidBrush(Color.Black), new Point(finalImage.Width - 300, finalImage.Height-300));
            g.DrawString("Dày: 9mm", bodyFont, new SolidBrush(Color.Black), new Point(finalImage.Width - 300, finalImage.Height - 260));
            g.DrawString("Mặt kính: Chống trầy", bodyFont, new SolidBrush(Color.Black), new Point(finalImage.Width - 300, finalImage.Height - 220));
            g.DrawString("Máy: Quartz", bodyFont, new SolidBrush(Color.Black), new Point(finalImage.Width - 300, finalImage.Height - 180));

            //Save the new image to the response output stream.
            finalImage.Save(output, ImageFormat.Png);

            //Clean house.
            g.Dispose();
            finalImage.Dispose();
            inputImage.Dispose();
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static System.Drawing.Bitmap CombineBitmap(string[] files)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
    }
}
