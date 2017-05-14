#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;
using HotBabe.Code.Helpers;

#endregion

namespace HotBabe.Controls
{
  ///<summary>
  /// Shows an image near the mouse cursor
  ///</summary>
  public partial class ImageView : Form
  {
    #region Constructors

    ///<summary>
    /// default constructor
    ///</summary>
    public ImageView()
    {
      InitializeComponent();
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Display an image
    ///</summary>
    ///<param name="fileName"></param>
    public void SetImage(string fileName)
    {
      SetImage(fileName, 50);
    }

    ///<summary>
    /// Display an image with a left offset from the mouse cursor
    ///</summary>
    ///<param name="fileName"></param>
    ///<param name="left"></param>
    public void SetImage(string fileName, int left)
    {
      SetImage(fileName, left, 0);
    }

    /// <summary>
    /// Display an image with top and left offset from the mouse cursor
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="left"></param>
    /// <param name="top"></param>
    public void SetImage(string fileName, int left, int top)
    {
      if (!ImageHelper.IsValidImage(fileName))
      {
        return;
      }
      BackgroundImage = null;
      Show();
      Image image = ImageHelper.FromFile(fileName);
      Point location = Cursor.Position;
      location.X += left;
      location.Y = Math.Max(location.Y - image.Height + top, 0);
      Location = location;
      Size = image.Size;
      BackgroundImage = image;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the window will be activated when it is shown.
    /// </summary>
    /// <returns>
    /// True if the window will not be activated when it is shown; otherwise, false. The default is false.
    /// </returns>
    protected override bool ShowWithoutActivation
    {
      get
      {
        return true;
      }
    }

    /// <summary/>
    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams ws = base.CreateParams;

        ws.ExStyle |= UnsafeNativeMethods.WS_EX_Layered;
        ws.ExStyle |= UnsafeNativeMethods.WS_EX_Transparent;

        // do not show in Alt-tab
        ws.ExStyle |= UnsafeNativeMethods.WS_EX_ToolWindow;
        // make image stay on top other applications
        // Make it show on top normal windows (the Form TopMost property is not always working)
        ws.ExStyle |= UnsafeNativeMethods.WS_EX_TopMost;
        // do not make foreground window
        ws.ExStyle |= UnsafeNativeMethods.WS_EX_NoActivate;
        return ws;
      }
    }

    #endregion
  }
}