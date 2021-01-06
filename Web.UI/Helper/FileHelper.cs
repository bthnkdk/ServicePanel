using MadsKristensen.ImageOptimizer;
using System;
using System.IO;
using System.Web;

namespace Web.UI
{
    public class FileHelper
    {
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
            }
            return result;
        }
    }
}