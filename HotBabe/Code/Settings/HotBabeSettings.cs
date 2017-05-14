#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using HotLogger;

/*using System.Web.Script.Serialization;
using System.Xml.Serialization;*/

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Holds the application settings
  ///</summary>
  public class HotBabeSettings : INotifyPropertyChanged
  {
    #region Member data

    private bool _alwaysOnTop;
    private bool _autoRun;
    private BlendImagesMode _blendImages;
    private bool _clickThrough;
    private string _customImagePath;
    private string _defaultImageName;
    private bool _hideOnFullscreen;
    private List<ImageInfo> _imageInfos;
    private int _left;
    private List<MonitorInfo> _monitorInfos;
    private float _opacity;
    private int _top;
    private int _updateInterval;
    private ProcessPriorityClass _priority;
    private long _maxImageCacheSize;
    private Alignment _horizontalAlignment;
    private Alignment _verticalAlignment;

    #endregion

    #region Constructors

    ///<summary>
    /// Defines default values
    ///</summary>
    public HotBabeSettings()
    {
      _autoRun = false;
      _left = 0;
      _top = 0;
      _opacity = 0.5f;
      _clickThrough = false;
      _updateInterval = 1000;
      _alwaysOnTop = true;
      _defaultImageName = "HotBabe.png";
      _blendImages = BlendImagesMode.None;
      _hideOnFullscreen = true;
      _priority=ProcessPriorityClass.Normal;
      _maxImageCacheSize = Constants.DefaultMaxImageCacheSize;
      _horizontalAlignment=Alignment.None;
      _verticalAlignment=Alignment.None;
    }

    #endregion

    #region Properties

    ///<summary>
    /// If true, the application will start when Windows starts
    ///</summary>
    public bool AutoRun
    {
      get
      {
        return _autoRun;
      }
      set
      {
        change("AutoRun", value, ref _autoRun);
      }
    }

    ///<summary>
    /// If set to true, the images change gradually
    ///</summary>
    public BlendImagesMode BlendImages
    {
      get
      {
        return _blendImages;
      }
      set
      {
        change("BlendImages", value, ref _blendImages);
      }
    }

    ///<summary>
    /// Default image if none configured as well as the tray icon
    ///</summary>
    public string DefaultImageName
    {
      get
      {
        return _defaultImageName;
      }
      set
      {
        change("DefaultImageName", value, ref _defaultImageName);
      }
    }

    ///<summary>
    /// The horizontal position of the image
    ///</summary>
    public int Left
    {
      get
      {
        return _left;
      }
      set
      {
        change("Left", value, ref _left);
      }
    }

    ///<summary>
    /// The vertical position of the image
    ///</summary>
    public int Top
    {
      get
      {
        return _top;
      }
      set
      {
        change("Top", value, ref _top);
      }
    }

    ///<summary>
    /// Opacity of the image
    ///</summary>
    public float Opacity
    {
      get
      {
        return _opacity;
      }
      set
      {
        change("Opacity", value, ref _opacity);
      }
    }

    ///<summary>
    /// If true, the mouse ignores the image and passes through the background application
    ///</summary>
    public bool ClickThrough
    {
      get
      {
        return _clickThrough;
      }
      set
      {
        change("ClickThrough", value, ref _clickThrough);
      }
    }

    ///<summary>
    /// Interval in milliseconds when the image is refreshed
    ///</summary>
    public int UpdateInterval
    {
      get
      {
        return _updateInterval;
      }
      set
      {
        change("UpdateInterval", value, ref _updateInterval);
      }
    }

    ///<summary>
    /// The image will stay on top of other applications if this is true
    ///</summary>
    public bool AlwaysOnTop
    {
      get
      {
        return _alwaysOnTop;
      }
      set
      {
        change("AlwaysOnTop", value, ref _alwaysOnTop);
      }
    }

    ///<summary>
    /// Hide if the active application if full screen
    ///</summary>
    public bool HideOnFullscreen
    {
      get
      {
        return _hideOnFullscreen;
      }
      set
      {
        change("HideOnFullscreen", value, ref _hideOnFullscreen);
      }
    }

    ///<summary>
    /// Information about the images that appear on different measurements
    ///</summary>
    public List<ImageInfo> ImageInfos
    {
      get
      {
        if (_imageInfos == null)
        {
          _imageInfos = new List<ImageInfo>();
        }
        return _imageInfos;
      }
    }

    ///<summary>
    /// Information about the types of monitors that take measurements
    ///</summary>
    public List<MonitorInfo> MonitorInfos
    {
      get
      {
        if (_monitorInfos == null)
        {
          _monitorInfos = new List<MonitorInfo>();
        }
        return _monitorInfos;
      }
    }

    ///<summary>
    /// Custom image dropped on the application
    ///</summary>
    public string CustomImagePath
    {
      get
      {
        return _customImagePath;
      }
      set
      {
        change("CustomImagePath", value, ref _customImagePath);
      }
    }

    /// <summary>
    /// Priority of the current process
    /// </summary>
    public ProcessPriorityClass Priority
    {
      get { return _priority; }
      set { change("Priority", value, ref _priority); }
    }

    /// <summary>
    /// Maximum size in bytes for the memory image cache
    /// </summary>
    public long MaxImageCacheSize
    {
      get { return _maxImageCacheSize; }
      set { change("MaxImageCacheSize", value, ref _maxImageCacheSize); }
    }

    public Alignment HorizontalAlignment
    {
      get { return _horizontalAlignment; }
      set { change("HorizontalAlignment", value, ref _horizontalAlignment); }
    }

    public Alignment VerticalAlignment
    {
      get { return _verticalAlignment; }
      set { change("VerticalAlignment", value, ref _verticalAlignment); }
    }

    #endregion


 #region Private Methods

    private void change<T>(string propertyName, T value, ref T field)
    {
      bool changed = !Equals(value, field);
      field = value;
      if (changed)
      {
        // weird deserialize error
        try
        {
          PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        catch (Exception ex)
        {
          Logger.Debug("Exception caught and discarded: " + ex.Message +
                       " at HotBabeSettings.change");
        }
      }
    }

    #endregion

    #region INotifyPropertyChanged Members

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}