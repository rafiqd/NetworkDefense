/**********************************************
 ****************Blaise Jarrett*****************
 **********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web.Script.Serialization;

namespace Game
{
    /// <summary>
    /// Decodes image metadata for easier integration of animated images.  Metadata is included as encoded pixels
    /// on the leftmost column of pixels on the image.
    /// </summary>
    static class ImageDecoder
    {
        /// <summary>
        /// Gets the meta data from the first column of pixels
        /// </summary>
        /// <param name="s">The name of the image to load</param>
        /// <returns>The metadata describing the animation</returns>
        public static ImageMetaData GetMetaData(string s)
        {
            Bitmap bitmap = (Bitmap)Image.FromFile(s);

            StringBuilder msg = new StringBuilder();
            for (int i = 0; i < bitmap.Height; i++)
            {
                Color pixelValue = bitmap.GetPixel(0, i);

                string value = getValue(pixelValue.R);
                if (value == null)
                    break;
                msg.Append(value);

                value = getValue(pixelValue.G);
                if (value == null)
                    break;
                msg.Append(value);

                value = getValue(pixelValue.B);
                if (value == null)
                    break;
                msg.Append(value);

                value = getValue(pixelValue.A);
                if (value == null)
                    break;
                msg.Append(value);
            }

            return new JavaScriptSerializer().Deserialize<ImageMetaData>(msg.ToString());
        }

        /// <summary>
        /// Gets the character value of an integer character code
        /// </summary>
        /// <param name="r">The integer value to decode</param>
        /// <returns>The character value</returns>
        static string getValue(int r)
        {
            if (r != 0)
            {
                string s = Convert.ToChar(r).ToString();
                return s;
            }

            return null;
        }
    }

    /// <summary>
    /// ImageMetaData contains values that describe an image animation.
    /// </summary>
    public class ImageMetaData
    {
        /// <summary>
        /// The width of a single frame of animation
        /// </summary>
        public int width;

        /// <summary>
        /// The height of a single frame of animation
        /// </summary>
        public int height;

        /// <summary>
        /// The intended frame rate of the image animation
        /// </summary>
        public int fps;

        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        public int frames;

        /// <summary>
        /// The horizontal center of the image
        /// </summary>
        public int center_x;

        /// <summary>
        /// The vertical center of the image.
        /// </summary>
        public int center_y;
    }
}
