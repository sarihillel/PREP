using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PREP.Functions
{
    public class Files
    {
        /// <summary>
        /// get base64 string and convert to image and upload
        /// </summary>
        /// <param name="base64string"></param>
        /// <returns></returns>
        public static String UploadImage(string base64string, string ImageName, bool cropImg)
        {
            string fullOutputPath;
            string SrcImage;
            Image image;
            byte[] bytes = Convert.FromBase64String(base64string);


            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = new Bitmap(Image.FromStream(ms));
                SrcImage = VirtualPathUtility.ToAbsolute("~/Content/Images/ScoreImages") + "/" + ImageName + ".png";
                fullOutputPath = System.Web.HttpContext.Current.Server.MapPath(SrcImage);
                SrcImage = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + SrcImage;

                if (cropImg)
                {
                    Bitmap source = new Bitmap(image);

                    Rectangle section = new Rectangle(new Point(0, 130), new Size(image.Width, image.Height - 130));
                    //image.Dispose();
                    //image = null;
                    Bitmap CroppedImage = CropImage(source, section);
                    try
                    {
                        CroppedImage.Save(fullOutputPath, ImageFormat.Png);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        source.Dispose();
                        CroppedImage.Dispose();
                    }
                }
                else
                    image.Save(fullOutputPath);
                image.Dispose();
            }

            return fullOutputPath;
        }

        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        public static void DeleteFile(string FilePath)
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

    }
}


