using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Net.Mail;

namespace ChurchManagementPortal
{
    class Utility
    {

        public bool checkNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value.Trim()) ? true : false;
        }

        public bool checkEquality(string value1, string value2)
        {
            return value1 == value2;
        }

        public bool isValidPhoneNo(string phoneNo)
        {
            return Regex.IsMatch(phoneNo, @"([\+\d]{2,3}|[0]){1}[\d]{10}$");
        }

        public bool isValidEmail(string email)
        {
            try
            {
                email = new MailAddress(email).Address;
                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
        }
        
        public string createAcronym(string name)
        {
            string[] badWords = { "of", "and", "the" };
            name = Regex.Replace(name, "\\b"+string.Join("\\b|\\b",badWords)+"\\b", "", RegexOptions.IgnoreCase);
            string[] split = name.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string codename = "";
            if (split.Length > 1)
            {
                foreach (string str in split)
                {
                    codename += str.Substring(0, 1);
                }
                return codename;
            }
            return name;
        }

        public string uploadFile(string title, string filter)
        {
            OpenFileDialog ofd = new()
            {
                CheckFileExists = true,
                Filter = filter,
                Title = title
            };

            return (ofd.ShowDialog() == true) ? ofd.FileName : "";
        }

        /// <summary>
        /// Resizes image but maintains aspect ratio
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="size">Size to set the image to</param>
        /// <param name="imageFilename">Filename to save the resized image with</param>
        public void resizeImage(string imagePath, string imageFilename, Size size)
        {
            Image imgToResize = Image.FromFile(imagePath);
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercentW = size.Width / (float)sourceWidth;
            float nPercentH = size.Height / (float)sourceHeight;
            float nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            b.Save(imageFilename);
        }

        /// <summary>
        /// Resizes image to specified size with specified quality and saves an image
        /// </summary>
        /// <param name="imagePath">path to the image</param>
        /// <param name="imageFilename">Filename to save the image with</param>
        /// <param name="dotsPerInch">dpi of the new image</param>
        /// <param name="widthInInch">New width of the image</param>
        /// <param name="heightInInch">New height of the image</param>
        /// <param name="quality">Quality of the new image (between 0 and 100 inclusive)</param>
        /// <param name="mimeType">Mimetype of the new image</param>
        public void resizeImage(string imagePath, string imageFilename, int dotsPerInch = 600, double widthInInch = 10, double heightInInch = 8, int quality = 50, string mimeType = "image/jpeg")
        {
            using (Bitmap bitmap = new(Image.FromFile(imagePath), (int)(widthInInch * dotsPerInch), (int)(heightInInch * dotsPerInch)))
            {
                bitmap.SetResolution(dotsPerInch, dotsPerInch);
                saveWithMimetype(bitmap, imageFilename, quality, mimeType);
            }
        }

        /// <summary>
        /// Saves image file with a specific mime type and quality
        /// </summary>
        /// <param name="img">Image file</param>
        /// <param name="imageFilename">File name to use in saving the image</param>
        /// <param name="quality">Quality of the image (between 0 and 100 inclusive)</param>
        /// <param name="mimeType">Mime type of the image</param>
        public void saveWithMimetype(Image img, string imageFilename, int quality = 50, string mimeType = "image/jpeg")
        {
            if (quality < 0 || quality > 100)
            {
                throw new ArgumentOutOfRangeException("Quality must be between  0 and 100");
            }
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new(System.Drawing.Imaging.Encoder.Quality, quality);
            ImageCodecInfo imageCodecInfo = GetEncoderInfo(mimeType);
            EncoderParameters encoderParams = new(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(imageFilename, imageCodecInfo, encoderParams);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // get image codec for all image formats
            ImageCodecInfo[] codecInfos = ImageCodecInfo.GetImageEncoders();
            // find the correct image codec
            foreach (ImageCodecInfo codecInfo in codecInfos)
            {
                if (codecInfo.MimeType == mimeType) { return codecInfo; }
            }
            return null;
        }

    }
}
