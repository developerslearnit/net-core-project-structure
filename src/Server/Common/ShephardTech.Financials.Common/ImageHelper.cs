using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Common
{
    /// <summary>
    /// Provides methods for image processing.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// return array of accepted image format.
        /// </summary>
        public static string[] AcceptImageExtention { get { return AcceptImageExtentions(); } }
        public static string[] AcceptImageandFilExtention { get { return AcceptImageandFilExtentions(); } }

        /// <summary>
        /// Creates a thumbnail to a given image.
        /// </summary>
        /// <param name="file">The image.</param>
        /// <param name="width">The maximum width of the thumbnail.</param>
        /// <param name="height">The maximum height of the thumbnail.</param>
        /// <param name="relative">Determines whether the image is resized relatively.</param>
        /// <returns>The resized image.</returns>
        public static byte[] MakeThumbnail(byte[] file, int width, int height, bool relative)
        {
            System.Drawing.Image image = null;

            using (var input = new MemoryStream(file))
            {
                image = System.Drawing.Image.FromStream(new MemoryStream(file));
            }

            if (relative)
            {
                if (image.Width > image.Height)
                {
                    height = image.Height * width / image.Width;
                    if (width > image.Width)
                    {
                        return file;
                    }
                }
                else
                {
                    width = image.Width * height / image.Height;
                    if (height > image.Height)
                    {
                        return file;
                    }
                }
            }

            var scaledImage = image.GetThumbnailImage(width, height, null, new System.IntPtr());
            using (var result = new MemoryStream())
            {
                scaledImage.Save(result, ImageFormat.Jpeg);
                return result.ToArray();
            }
        }

        private static string[] AcceptImageExtentions()
        {
            return new string[] { ".png", ".jpg", ".jpeg", ".bmp", ".webp", ".bitmap", ".gif" };
        }
        private static string[] AcceptImageandFilExtentions()
        {
            return new string[] { ".png", ".jpg", ".jpeg", ".bmp", ".webp", ".bitmap", ".pdf" };
        }
    }
}
