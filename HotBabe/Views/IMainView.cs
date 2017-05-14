#region Using directives

using System;
using System.Drawing;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotBabe.Code.Transitions;

#endregion

namespace HotBabe.Views
{
  ///<summary>
  /// Interface for the main view
  ///</summary>
  public interface IMainView
  {
    // I know, lazy, but I didn't want an event for every single thing while in development

    #region Events

    ///<summary>
    /// Execute a command on the controller 
    ///</summary>
    event EventHandler<EventArgs<CommandItem>> ExecuteCommand;

    #endregion

    #region Public Methods

    ///<summary>
    /// Change the image in the main view (the actual change will be handled by a transition)
    ///</summary>
    ///<param name="image"></param>
    void ChangeImage(Image image);

    ///<summary>
    /// Apply the current settings
    ///</summary>
    void ApplySettings();

    ///<summary>
    /// Set the view image transition object
    ///</summary>
    ///<param name="transition"></param>
    void SetTransition(IImageTransition transition);

    #endregion

    #region Properties

    ///<summary>
    /// If true, the mouse will ignore the application and the
    /// clicks will go through it to the underlying applications
    ///</summary>
    bool ClickThrough
    {
      get;
      set;
    }

    ///<summary>
    /// The filename for the icon of the application and tray icon (can be a normal image)
    ///</summary>
    string IconFileName
    {
      get;
      set;
    }

    ///<summary>
    /// The update interval in milliseconds
    ///</summary>
    int UpdateInterval
    {
      get;
      set;
    }

    ///<summary>
    /// Auto start the application on Windows start
    ///</summary>
    bool AutoRun
    {
      get;
      set;
    }

    ///<summary>
    /// The vertical position of the view
    ///</summary>
    int ViewTop
    {
      get;
      set;
    }

    ///<summary>
    /// The horizontal position of the view
    ///</summary>
    int ViewLeft
    {
      get;
      set;
    }

    ///<summary>
    /// The view should always stay on top other applications
    ///</summary>
    bool AlwaysOnTop
    {
      get;
      set;
    }

    ///<summary>
    /// The opacity of the view
    ///</summary>
    double ViewOpacity
    {
      get;
      set;
    }

    ///<summary>
    /// If set, images will change through a fade-in, fade-out effect
    ///</summary>
    BlendImagesMode BlendImages
    {
      get;
      set;
    }

    /// <summary>
    /// If set, the application will hide if the active application is fullscreen
    /// </summary>
    bool HideOnFullscreen
    {
      get;
      set;
    }

    /// <summary>
    /// Maximum image size cache. If it exceeds this value, 
    /// older images from the cache will be removed.
    /// </summary>
    long MaxImageCacheSize { get; set; }

    Alignment HorizontalAlignment { get; set; }
    Alignment VerticalAlignment { get; set; }

    #endregion
  }
}