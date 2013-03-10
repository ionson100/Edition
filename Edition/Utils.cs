using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Edition
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utils
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fileUpload"></param>
        ///// <returns></returns>
        //public static Byte[] HttpPostedFileBaseToBytes(HttpPostedFileBase fileUpload)
        //{
            
        //    if (fileUpload == null) return null;
        //    var str = fileUpload.InputStream;
        //    var strLen = Convert.ToInt32(str.Length);
        //    var strArr = new byte[strLen];
        //    str.Read(strArr, 0, strLen);
        //    return  strArr;
        //}

        /// <summary>
        /// Get bytes picture from HttpPostedFileBase
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        public static Byte[] HttpPostedFileToBytes(HttpPostedFileBase fileUpload)
        {

            if (fileUpload == null) return null;
            var str = fileUpload.InputStream;
            var strLen = Convert.ToInt32(str.Length);
            var strArr = new byte[strLen];
            str.Read(strArr, 0, strLen);
            return strArr;
        }
        /// <summary>
        /// Image To Bytes
        /// </summary>
        /// <param name="img">Image</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        /// Image From Bytes
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>

        public static Image ImageFromBytes(Byte[] img)
        {
            if (img == null) return null;
            var converter = new ImageConverter();
            return (Image)converter.ConvertFrom(img);
        }

        /// <summary>
        /// Resize Image
        /// </summary>
        /// <param name="image">Bytes picture</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] image, int width, int height)
        {

            var ms = new MemoryStream(image.ToArray());
            var i = ImageFromBytes(image);
            if (i.Width > i.Height)
            {
                decimal ch;
                var ww = i.Width;
                var hh = i.Height;
                var dd = ww;
                while (true)
                {
                    ww = ww - 1;
                    var k = ww / (decimal)dd;
                    ch = hh * k;
                    if (ww <= width && ch <= height)
                    {
                        break;
                    }
                }
                var b = new Bitmap(i, ww, (int)ch);

                try
                {
                    return ImageToBytes(b);
                }
                finally
                {
                    i.Dispose();
                    b.Dispose();
                    ms.Dispose();
                }
            }

            if (i.Height > i.Width)
            {
                decimal cw;
                var ww = i.Width;
                var hh = i.Height;
                var dd = hh;
                while (true)
                {
                    hh = hh - 1;
                    var k = hh / (decimal)dd;
                    cw = (ww * k);
                    if (hh <= height && cw <= width)
                    {
                        break;
                    }
                }
                var b = new Bitmap(i, (int)cw, hh);
                try
                {
                    return ImageToBytes(b);
                }
                finally
                {
                    i.Dispose();
                    b.Dispose();
                    ms.Dispose();
                }
            }
            if (i.Width == i.Height)
            {
                Bitmap b = null;
                if (width > height)
                    b = new Bitmap(i, height, height);
                if (width < height)
                    b = new Bitmap(i, width, width);
                if (width == height)
                    b = new Bitmap(i, width, height);
                try
                {
                    return ImageToBytes(b);
                }
                finally
                {
                    i.Dispose();
                    if (b != null) b.Dispose();
                    ms.Dispose();
                }
            }
            return null;
        }

      

        /// <summary>
        /// Render View To String
        /// </summary>
        /// <param name="viewPath"></param>
        /// <param name="model"></param>
        /// <param name="controllerContext"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string RenderViewToString<T>(string viewPath, T model,ControllerContext controllerContext)
        {
            using (var writer = new StringWriter())
            {
                var view = new WebFormView(controllerContext,viewPath);
                var vdd = new ViewDataDictionary<T>(model);
                var viewCxt = new ViewContext(controllerContext, view, vdd, new TempDataDictionary(), writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }

    }
}
