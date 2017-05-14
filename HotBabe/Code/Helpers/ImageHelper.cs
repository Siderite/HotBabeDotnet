#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using HotBabe.Controls;
using HotLogger;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Helps with image operations
  ///</summary>
  public static class ImageHelper
  {
    #region Member data

    private static readonly ImageView sImageView = new ImageView();

    #endregion

    #region Public Methods

    /// <summary>
    /// Converts an image into an icon.
    /// </summary>
    /// <param name="img">The image that shall become an icon</param>
    /// <param name="size">The width and height of the icon. Standard
    /// sizes are 16x16, 32x32, 48x48, 64x64.</param>
    /// <param name="keepAspectRatio">Whether the image should be squashed into a
    /// square or whether whitespace should be put around it.</param>
    /// <returns>An icon!!</returns>
    public static Icon MakeIcon(Image img, int size, bool keepAspectRatio)
    {
      try
      {
        Bitmap square = new Bitmap(size, size); // create new bitmap
        Graphics g = Graphics.FromImage(square); // allow drawing to it

        int x,
            y,
            w,
            h; // dimensions for new image

        if (!keepAspectRatio || img.Height == img.Width)
        {
          // just fill the square
          x = y = 0; // set x and y to 0
          w = h = size; // set width and height to size
        }
        else
        {
          // work out the aspect ratio
          float r = img.Width/(float) img.Height;

          // set dimensions accordingly to fit inside size^2 square
          if (r > 1)
          {
            // w is bigger, so divide h by r
            w = size;
            h = (int) (size/r);
            x = 0;
            y = (size - h)/2; // center the image
          }
          else
          {
            // h is bigger, so multiply w by r
            w = (int) (size*r);
            h = size;
            y = 0;
            x = (size - w)/2; // center the image
          }
        }

        // make the image shrink nicely by using HighQualityBicubic mode
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(img, x, y, w, h); // draw image with specified dimensions
        g.Flush(); // make sure all drawing operations complete before we get the icon

        // following line would work directly on any image, but then
        // it wouldn't look as nice.
        return Icon.FromHandle(square.GetHicon());
      }
      catch (Exception ex)
      {
        Logger.Debug("Exception caught and discarded: " + ex.Message + " at ImageHelper.MakeIcon");
        return null;
      }
    }

    ///<summary>
    /// Blend image2 into image1 based on percent.
    ///</summary>
    ///<param name="image1"></param>
    ///<param name="image2"></param>
    ///<param name="percent">Blending quotient (0 means image1, 1 means image2)</param>
    ///<returns></returns>
    public static Image Blend(Image image1, Image image2, float percent)
    {
      return Blend(image1, image2, (1 - percent), percent);
    }

    ///<summary>
    /// Blend image2 into image1 based on percent.
    ///</summary>
    ///<param name="image1"></param>
    ///<param name="image2"></param>
    ///<param name="opacityPercent1">Opacity for image1</param>
    ///<param name="opacityPercent2">Opacity for image2</param>
    ///<returns></returns>
    public static Image Blend(Image image1, Image image2
                              , float opacityPercent1, float opacityPercent2)
    {
      return Blend(image1, image2, opacityPercent1, opacityPercent2, opacityPercent1,
                   opacityPercent2);
    }

    ///<summary>
    /// Blend image2 into image1 based on percent.
    ///</summary>
    ///<param name="image1"></param>
    ///<param name="image2"></param>
    ///<param name="opacityPercent1">Opacity for image1</param>
    ///<param name="opacityPercent2">Opacity for image2</param>
    ///<param name="sizePercent1">percent of image1 size</param>
    ///<param name="sizePercent2">percent of image2 size</param>
    ///<returns></returns>
    public static Image Blend(Image image1, Image image2
                              , float opacityPercent1, float opacityPercent2
                              , float sizePercent1, float sizePercent2)
    {
      if (image1 == null)
      {
        return image2;
      }
      if (image2 == null)
      {
        return image1;
      }

      if (opacityPercent1 > 1)
      {
        opacityPercent1 = 1;
      }
      if (opacityPercent2 > 1)
      {
        opacityPercent2 = 1;
      }

      Size size1 = image1.Size;
      Size size2 = image2.Size;

      int w = (int) (size1.Width*sizePercent1 + size2.Width*sizePercent2);
      if (w <= 0)
      {
        w = 1;
      }
      int h = (int) (size1.Height*sizePercent1 + size2.Height*sizePercent2);
      if (h <= 0)
      {
        h = 1;
      }

      Bitmap square = new Bitmap(w, h); // create new bitmap
      using (Graphics g = Graphics.FromImage(square))
      {
        // allow drawing to it

        // make the image shrink nicely by using HighQualityBicubic mode
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        ColorMatrix cmxPic = new ColorMatrix();
        ImageAttributes iaPic = new ImageAttributes();

        if (opacityPercent1 > 0)
        {
          cmxPic.Matrix33 = opacityPercent1;
          iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
          g.DrawImage(image1, new Rectangle(0, 0, w, h), 0, 0, size1.Width, size1.Height,
                      GraphicsUnit.Pixel, iaPic);
        }

        if (opacityPercent2 > 0)
        {
          cmxPic.Matrix33 = opacityPercent2;
          iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
          g.DrawImage(image2, new Rectangle(0, 0, w, h), 0, 0, size2.Width, size2.Height,
                      GraphicsUnit.Pixel, iaPic);
        }
        g.Flush();
      }
      return square;
    }

    ///<summary>
    /// Clone an image
    ///</summary>
    ///<param name="image"></param>
    ///<returns></returns>
    public static Image CloneImage(this Image image)
    {
      return image == null
                 ? null
                 : (Image) (image.Clone());
    }

    ///<summary>
    /// Get an image from a path, be it file, ftp, http, zip, res, etc.
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    public static Image FromFile(string path)
    {
      Image image;
      using (Stream stream = PathHelper.GetReadStream(path))
      {
        // fixing the strange Out of memory exception from System.Drawing
        using (Image image2 = Image.FromStream(stream))
        {
          image = image2.GetThumbnailImage(image2.Width, image2.Height, null, IntPtr.Zero);
        }
      }
      return image;
    }

    ///<summary>
    /// Return true if an image can be loaded from the filename
    ///</summary>
    ///<param name="fileName"></param>
    ///<returns></returns>
    public static bool IsValidImage(string fileName)
    {
      try
      {
        FromFile(fileName);
        return true;
      }
      catch (Exception ex)
      {
        Logger.Debug(string.Format("Image {0} is not valid ({1})", fileName, ex.Message));
        return false;
      }
    }


    ///<summary>
    /// show a hovering image next to the mouse cursor
    ///</summary>
    ///<param name="filename"></param>
    internal static void HoverImage(string filename)
    {
      sImageView.SetImage(filename);
    }

    /// <summary>
    /// hide a hovering image next to the cursor
    /// </summary>
    internal static void HideHoverImage()
    {
      sImageView.Hide();
    }

    internal static int GetMemorySize(this Image img)
    {

      PixelFormat fmt = img.PixelFormat;
      Int32 BitDepth = 1;

      switch (fmt)
      {
        case PixelFormat.Format1bppIndexed:
          BitDepth = 1;
          break;
        case PixelFormat.Format16bppRgb565:
        case PixelFormat.Format16bppRgb555:
        case PixelFormat.Format16bppGrayScale:
        case PixelFormat.Format16bppArgb1555:
          BitDepth = 16;
          break;
        case PixelFormat.Format24bppRgb:
          BitDepth = 24;
          break;
        case PixelFormat.Format32bppRgb:
        case PixelFormat.Format32bppPArgb:
        case PixelFormat.Format32bppArgb:
          BitDepth = 32;
          break;
        case PixelFormat.Format48bppRgb:
          BitDepth = 48;
          break;
        case PixelFormat.Format4bppIndexed:
          BitDepth = 4;
          break;
        case PixelFormat.Format64bppPArgb:
        case PixelFormat.Format64bppArgb:
          BitDepth = 64;
          break;
        case PixelFormat.Format8bppIndexed:
          BitDepth = 8;
          break;
      }


      return img.Width*img.Height*BitDepth;

    }


    #endregion

    public static Image Resize(Image image, int width, int height)
    {
      try
      {
        Bitmap square = new Bitmap(width, height); // create new bitmap
        using (Graphics g = Graphics.FromImage(square))
        {
          // allow drawing to it

          // make the image shrink nicely by using HighQualityBicubic mode
          g.InterpolationMode = InterpolationMode.HighQualityBicubic;
          g.DrawImage(image, 0, 0, width, height); // draw image with specified dimensions
          g.Flush(); // make sure all drawing operations complete before we get the icon
        }
        return square;
      }
      catch (Exception ex)
      {
        Logger.Debug("Exception caught and discarded: " + ex.Message + " at ImageHelper.MakeIcon");
        return null;
      }

    }
  }
}