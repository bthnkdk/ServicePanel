using MadsKristensen.ImageOptimizer;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Web.UI.Helper
{
    public class GdiHelper
    {
        public static byte[] CreateImage(string key)
        {
            Random rnd = new Random();
            Bitmap bmp = new Bitmap(200, 80);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(System.Drawing.Color.White);
            Font f = new Font("Arial", 34, FontStyle.Bold);
            Font fa = new Font("Arial", 18, FontStyle.Bold);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(125, 255, 0, 0)), -10, -10, 80, 80);
            g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(85, 85, 119, 100)), 60, -10, 60, 60);
            g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(161, 173, 185, 9)), 90, 0, 80, 80);

            var colorArray = new System.Drawing.Color[] { System.Drawing.Color.Red, System.Drawing.Color.Green, System.Drawing.Color.Blue, System.Drawing.Color.Purple, System.Drawing.Color.Orange };
            int left = 10;

            for (int i = 0; i < colorArray.Length; i++)
            {
                var color = colorArray[rnd.Next(5)];
                var harf = key[i].ToString();

                if (i % 2 == 0)
                {
                    g.RotateTransform(5);
                    g.DrawString(harf, f, new SolidBrush(color), left, 10);
                }
                else
                {
                    g.RotateTransform(-3);
                    g.DrawString(harf, fa, new SolidBrush(color), left, 10);
                }
                left += 30;
            }
            MemoryStream mem = new MemoryStream();
            bmp.Save(mem, ImageFormat.Png);
            return mem.ToArray();
        }

        public static byte[] CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, width, height);
            }
        }

        public static byte[] CropImage(Stream content, int x, int y, int width, int height)
        {
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                double sourceWidth = Convert.ToDouble(sourceBitmap.Size.Width);
                double sourceHeight = Convert.ToDouble(sourceBitmap.Size.Height);
                Rectangle cropRect = new Rectangle(x, y, width, height);

                using (Bitmap newBitMap = new Bitmap(cropRect.Width, cropRect.Height))
                {
                    using (Graphics g = Graphics.FromImage(newBitMap))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(sourceBitmap, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);

                        return GetBitmapBytes(newBitMap);
                    }
                }
            }
        }

        public static byte[] GetBitmapBytes(Bitmap source)
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[4];
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            using (MemoryStream tmpStream = new MemoryStream())
            {
                source.Save(tmpStream, codec, parameters);

                byte[] result = new byte[tmpStream.Length];
                tmpStream.Seek(0, SeekOrigin.Begin);
                tmpStream.Read(result, 0, (int)tmpStream.Length);

                return result;
            }
        }

        public static bool CompressImage(string source, bool loosy = false)
        {
            var result = false;
            try
            {
                if (File.Exists(source))
                {
                    string[] exts = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    FileInfo sourceFile = new FileInfo(source);
                    if (Array.IndexOf(exts, sourceFile.Extension) != -1)
                    {
                        Compressor compressor = new Compressor(HttpContext.Current.Server.MapPath("~/bin/Tools"));

                        CompressionResult compressorResult = compressor.CompressFile(sourceFile.FullName, loosy);

                        if (compressorResult.Processed && compressorResult.Percent > 0)
                        {
                            File.Copy(compressorResult.ResultFileName, compressorResult.OriginalFileName, true);
                            File.Delete(compressorResult.ResultFileName);
                            result = true;
                        }
                    }
                }
            }
            catch
            {
                ;
            }
            return result;
        }
    }
}