#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotBabe.Code.Transitions;
using HotBabe.Views;
using HotLogger;
using Monitors;
using Console=HotBabe.Views.Console;
using Timer=System.Windows.Forms.Timer;

#endregion

namespace HotBabe.Controllers
{
  ///<summary>
  /// The controller for the main form (the images and tray icon)
  ///</summary>
  public class MainController
  {
    #region Events

    ///<summary>
    /// Fired when the settings need editing (in order to open a settings editor)
    ///</summary>
    public event EventHandler EditSettings;

    #endregion

    #region Member data

    private string _imageName;
    private IImageTransition _imageTransition;
    private bool _needUpdate;
    private Timer _timer;
    private IMainView _view;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    ///<param name="view">the view for the controller</param>
    ///<param name="settingsManager">the manager used to load/save settings</param>
    public MainController(IMainView view, BaseSettingsManager<HotBabeSettings> settingsManager)
    {
      this.settingsManager = settingsManager;
      setView(view);
      initialize();
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Start the controller
    ///</summary>
    public void Start()
    {
      applySettings();
      Form frm = _view as Form;
      if (frm != null)
      {
        Application.Run(frm);
      }
    }

    ///<summary>
    /// Reload the settings and refresh the view
    ///</summary>
    public void Refresh()
    {
      Refresh(null);
    }

    ///<summary>
    /// Reload the settings and refresh the view
    ///</summary>
    public void Refresh(HotBabeSettings babeSettings)
    {
      settings.PropertyChanged -= settingsPropertyChanged;
      settings = babeSettings??settingsManager.LoadSettings();
      settings.PropertyChanged += settingsPropertyChanged;

      _imageName = getImageName();
      applySettings();
    }

    #endregion

    #region Properties

    private Dictionary<string, double> measures
    {
      get;
      set;
    }

    private HotBabeSettings settings
    {
      get;
      set;
    }

    private ImageCache cachedImages
    {
      get;
      set;
    }

    private BaseSettingsManager<HotBabeSettings> settingsManager
    {
      get;
      set;
    }

    private List<BaseMonitor> monitors
    {
      get;
      set;
    }

    #endregion

    #region Private Methods

    private void initialize()
    {
      cachedImages = new ImageCache();
      measures = new Dictionary<string, double>();
      monitors = new List<BaseMonitor>();
      loadSettings();
      _timer = new Timer();
      _timer.Tick += timerTick;
    }

    private void setView(IMainView view)
    {
      _view = view;
      _view.ExecuteCommand += (sender, e) => executeCommand(e.Value);
    }

    private void timerTick(object sender, EventArgs e)
    {
      if (_needUpdate)
      {
        updateMeasures();
        _needUpdate = false;
      }
    }

    private void updateMeasures()
    {
      string imageName = getImageName();
      if (imageName != _imageName)
      {
        _imageName = imageName;
        applySettings();
      }
    }

    private string getImageName()
    {
      string imageName = settings.DefaultImageName;
      double delta = double.MaxValue;
      for (int i = 0; i < settings.ImageInfos.Count; i++)
      {
        ImageInfo imageInfo = settings.ImageInfos[i];
        bool affected = false;
        double sum = 0;
        List<string> keys = new List<string>(measures.Keys);
        foreach (string key in keys)
        {
          double measure = measures[key];
          double d;
          if (imageInfo.Measures.TryGetValue(key, out d))
          {
            sum += Math.Pow(d - measure, 2);
            affected = true;
          }
        }
        if (affected && sum < delta)
        {
          imageName = imageInfo.ImageFileName;
          delta = sum;
        }
      }
      imageName = PathHelper.GetRootedPath(imageName);
      if (!PathHelper.FileExists(imageName))
      {
        imageName = settings.DefaultImageName;
      }
      return imageName;
    }

    private void settingsPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      saveSettings();
    }

    private void saveSettings()
    {
      measures.Clear();
      settingsManager.SettingsChanged -= mainSettingsChanged;
      settingsManager.SaveSettings(settings);
      settingsManager.SettingsChanged += mainSettingsChanged;
    }

    private void mainSettingsChanged(object sender, SettingsChangedEventArgs e)
    {
      Refresh();
    }

    private void applySettings()
    {
      Process.GetCurrentProcess().PriorityClass = settings.Priority;
      _view.SetTransition(getTransition());
      loadMonitors();
      if (string.IsNullOrEmpty(_imageName))
      {
        updateMeasures();
        return;
      }
      _timer.Stop();
      _timer.Interval = settings.UpdateInterval;
      _timer.Start();
      initAutoRun();
      updateView();
      _view.ApplySettings();
    }

    private IImageTransition getTransition()
    {
      switch (settings.BlendImages)
      {
        case BlendImagesMode.CustomImage:
          {
            Image image;
            try
            {
              image = ImageHelper.FromFile(settings.CustomImagePath);
            }
            catch (Exception ex)
            {
              image = ImageHelper.FromFile(settings.DefaultImageName);
              Logger.Debug(string.Format("Cannot open custom image {0} : {1}",
                                         settings.CustomImagePath, ex.Message));
            }
            CustomImageTransition transition = _imageTransition as CustomImageTransition;
            if (transition == null)
            {
              _imageTransition = new CustomImageTransition(image);
            }
          }
          break;
        case BlendImagesMode.InAndOut:
          {
            SmoothTransition smooth = _imageTransition as SmoothTransition;
            if (smooth == null)
            {
              _imageTransition = new SmoothTransition();
            }
          }
          break;
        case BlendImagesMode.OutThenIn:
          {
            OutThenInTransition outThenIn = _imageTransition as OutThenInTransition;
            if (outThenIn == null)
            {
              _imageTransition = new OutThenInTransition();
            }
          }
          break;
        case BlendImagesMode.None:
          {
            SimpleTransition simple = _imageTransition as SimpleTransition;
            if (simple == null)
            {
              _imageTransition = new SimpleTransition();
            }
          }
          break;
      }
      return _imageTransition;
    }

    private void updateView()
    {
      var frm = _view as Form;
      //_view.ChangeImage(getImage());
      frm.SafeInvoke(_view.ChangeImage, getImage());
      _view.IconFileName = PathHelper.GetRootedPath(settings.DefaultImageName);
      _view.ClickThrough = settings.ClickThrough;
      _view.AutoRun = settings.AutoRun;
      _view.AlwaysOnTop = settings.AlwaysOnTop;
      _view.UpdateInterval = settings.UpdateInterval;
      _view.ViewOpacity = settings.Opacity;
      _view.ViewTop = settings.Top;
      _view.ViewLeft = settings.Left;
      _view.BlendImages = settings.BlendImages;
      _view.HideOnFullscreen = settings.HideOnFullscreen;
      _view.MaxImageCacheSize = settings.MaxImageCacheSize;
      _view.HorizontalAlignment = settings.HorizontalAlignment;
      _view.VerticalAlignment = settings.VerticalAlignment;
    }

    private void initAutoRun()
    {
      AutoRunHelper arh = new AutoRunHelper
                            {
                                ApplicationPath = PathHelper.ApplicationPath,
                                ApplicationKey = "HotBabe.NET"
                            };
      if (settings.AutoRun)
      {
        arh.SetToRun();
      }
      else
      {
        arh.UnsetToRun();
      }
    }

    private Image getImage()
    {
      Image image = cachedImages.Get(_imageName,
                                     name =>
                                     PathHelper.FileExists(_imageName)
                                       ? image = ImageHelper.FromFile(_imageName)
                                       : ImageHelper.FromFile(Constants.StartImageFileName));
      return image;
    }

    private void monitorUpdated(object sender, EventArgs e)
    {
      BaseMonitor monitor = (BaseMonitor) sender;
      measures[monitor.Key] = monitor.Value;
      _needUpdate = true;
    }

    private void loadMonitors()
    {
      //unloadMonitors();
      List<BaseMonitor> newList = new List<BaseMonitor>();
      foreach (MonitorInfo info in settings.MonitorInfos)
      {
        Type type = null;
        if (string.IsNullOrEmpty(info.AssemblyFileName))
        {
          type = Type.GetType(info.TypeName);
        }
        else
        {
          Assembly assembly;
          if (AssemblyHelper.TryLoadAssembly(info.AssemblyFileName, out assembly))
          {
            type = assembly.GetType(info.TypeName);
          }
        }
        if (type != null)
        {
          BaseMonitor monitor = monitors.Find(mon => mon.GetType() == type);
          if (monitor != null)
          {
            monitor.Stop();
            monitors.Remove(monitor);
          }
          if (monitor == null || info.Reload)
          {
            Logger.Debug(string.Format("Loading monitor {0} ({1})", type, info.Reload?"reload":"new"));
            monitor = (BaseMonitor) Activator.CreateInstance(type);
            monitor.Updated += monitorUpdated;
            info.Reload = false;
          }
          monitor.MinValue = info.MinValue;
          monitor.Smooth = info.Smooth;
          monitor.UpdateInterval = info.UpdateInterval;
          monitor.Key = info.Key;
          monitor.Parameter = info.Parameter;
          monitor.Start();
          newList.Add(monitor);
        }
      }
      foreach (BaseMonitor monitor in monitors)
      {
        monitor.Stop();
        monitor.Updated -= monitorUpdated;
      }
      monitors = newList;
    }

    private void loadSettings()
    {
      settings = settingsManager.LoadSettings();
      settings.PropertyChanged += settingsPropertyChanged;
      settingsManager.SettingsChanged += mainSettingsChanged;
      settingsManager.Monitor(true);
    }

    private string getIconText()
    {
      StringBuilder sb = new StringBuilder();
      foreach (KeyValuePair<string, double> pair in measures)
      {
        if (sb.Length > 0)
        {
          sb.Append("\r\n");
        }
        sb.AppendFormat("{0}: {1:N1}", pair.Key, pair.Value);
      }
      return sb.ToString();
    }

    private void executeCommand(CommandItem commandItem)
    {
      switch (commandItem.Command)
      {
        case HotBabeCommand.UpdateInterval:
          {
            settings.UpdateInterval = (int) commandItem.Parameter*1000;
          }
          break;
        case HotBabeCommand.AutoRun:
          {
            settings.AutoRun = !settings.AutoRun;
          }
          break;
        case HotBabeCommand.AlwaysOnTop:
          {
            settings.AlwaysOnTop = !settings.AlwaysOnTop;
          }
          break;
        case HotBabeCommand.BlendImages:
          {
            settings.BlendImages = (BlendImagesMode) commandItem.Parameter;
          }
          break;
        case HotBabeCommand.HideOnFullscreen:
          {
            settings.HideOnFullscreen = !settings.HideOnFullscreen;
          }
          break;
        case HotBabeCommand.ClickThrough:
          {
            settings.ClickThrough = !settings.ClickThrough;
          }
          break;
        case HotBabeCommand.Opacity:
          {
            float opacity = (int) commandItem.Parameter/100f;
            settings.Opacity = opacity;
          }
          break;
        case HotBabeCommand.RequestIconText:
          {
            commandItem.Parameter = getIconText();
          }
          break;
        case HotBabeCommand.LocationChanged:
          {
            Point location = (Point) commandItem.Parameter;
            settings.Left = location.X;
            settings.Top = location.Y;
          }
          break;
        case HotBabeCommand.AdvancedSettings:
          {
            EditSettings.Fire(this);
          }
          break;
        case HotBabeCommand.DropFile:
          {
            handleDroppedFile((string) commandItem.Parameter);
          }
          break;
        case HotBabeCommand.Console:
          {
            Logger.Debug("Started debug console.");
            new Console().Show();
          }
          break;
        case HotBabeCommand.LoadPack:
          {
            loadPack((string) commandItem.Parameter);
          }
          break;
        case HotBabeCommand.Priority:
          {
            settings.Priority = (ProcessPriorityClass) commandItem.Parameter;
          }
          break;
        case HotBabeCommand.MaxImageCacheSize:
          {
            long cacheSize = (long)commandItem.Parameter;
            settings.MaxImageCacheSize = cacheSize;
            cachedImages.MaxSize = cacheSize;
          }
          break;
        case HotBabeCommand.HorizontalAlignment:
          {
            settings.HorizontalAlignment = (Alignment)commandItem.Parameter;
          }
          break;
        case HotBabeCommand.VerticalAlignment:
          {
            settings.VerticalAlignment = (Alignment)commandItem.Parameter;
          }
          break;
      }
      applySettings();
    }

    private void loadPack(string packFileName)
    {
      string zipUriPrefix = "zip:///" + PathHelper.GetRelativePath(packFileName) + "?";
      BaseSettingsManager<HotBabeSettings> tempManager =
          settingsManager.GetNewInstance(zipUriPrefix + "HotBabe.xml");
      HotBabeSettings tempSettings = tempManager.LoadSettings();

      settings.PropertyChanged -= settingsPropertyChanged;
      settings.AlwaysOnTop = tempSettings.AlwaysOnTop;
      settings.AutoRun = tempSettings.AutoRun;
      settings.ClickThrough = tempSettings.ClickThrough;
      settings.Left = tempSettings.Left;
      settings.Opacity = tempSettings.Opacity;
      settings.Top = tempSettings.Top;
      settings.UpdateInterval = tempSettings.UpdateInterval;
      settings.BlendImages = tempSettings.BlendImages;
      settings.HideOnFullscreen = tempSettings.HideOnFullscreen;

      settings.MonitorInfos.Clear();
      settings.MonitorInfos.AddRange(tempSettings.MonitorInfos);

      foreach (ImageInfo info in tempSettings.ImageInfos)
      {
        if (!PathHelper.IsRooted(info.ImageFileName))
        {
          info.ImageFileName = zipUriPrefix + info.ImageFileName;
        }
      }
      settings.ImageInfos.Clear();
      settings.ImageInfos.AddRange(tempSettings.ImageInfos);

      if (!PathHelper.IsRooted(tempSettings.DefaultImageName))
      {
        tempSettings.DefaultImageName = zipUriPrefix + tempSettings.DefaultImageName;
      }
      settings.DefaultImageName = tempSettings.DefaultImageName;

      settings.PropertyChanged += settingsPropertyChanged;
      saveSettings();
      Refresh(settings);
    }


    private void handleDroppedFile(string filePath)
    {
      try
      {
        ImageHelper.FromFile(filePath);
        settings.BlendImages = BlendImagesMode.CustomImage;
        settings.CustomImagePath = filePath;
        _imageTransition = null;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Not an image", "Display custom image", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        Logger.Debug(string.Format("Cannot open dropped image {0} : {1}", filePath, ex.Message));
      }
    }

    #endregion
  }
}