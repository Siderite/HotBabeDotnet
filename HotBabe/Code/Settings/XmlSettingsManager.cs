#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using HotBabe.Code.Helpers;
using HotLogger;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// XML file Hot Babe settings manager
  ///</summary>
  public class XmlSettingsManager : BaseSettingsManager<HotBabeSettings>
  {
    #region Member data

    private string _prevSettingsString;
    private bool _saving;
    private FileSystemWatcher _watcher;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public XmlSettingsManager() : this("HotBabe.Xml")
    {
    }

    ///<summary>
    /// Allows to save the configuration in a specific file
    ///</summary>
    ///<param name="configFile"></param>
    public XmlSettingsManager(string configFile)
    {
      ConfigFile = PathHelper.GetRootedPath(configFile);
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Get a new instance of the same type, using a specific configuration file
    ///</summary>
    ///<param name="configurationFile"></param>
    ///<returns></returns>
    public override BaseSettingsManager<HotBabeSettings> GetNewInstance(string configurationFile)
    {
      return new XmlSettingsManager(configurationFile);
    }

    ///<summary>
    /// Enable/disable the monitoring on the settings repository
    ///</summary>
    ///<param name="enable"></param>
    public override void Monitor(bool enable)
    {
      if (Watcher != null)
      {
        Watcher.EnableRaisingEvents = enable;
      }
    }

    ///<summary>
    /// Save the settings into the repository
    ///</summary>
    ///<param name="settings"></param>
    public override void SaveSettings(HotBabeSettings settings)
    {
      string localPath = PathHelper.GetLocalPath(ConfigFile);
      if (string.IsNullOrEmpty(localPath))
      {
        Logger.Info("Cannot save settings except on local files (" + ConfigFile + ")");
        return;
      }
      XmlSerializer xs = new XmlSerializer(typeof (HotBabeSettings));
      _saving = true;
      bool monitor = true;
      if (Watcher != null)
      {
        monitor = Watcher.EnableRaisingEvents;
        Monitor(false);
      }
      using (
          Stream stream = PathHelper.GetWriteStream(localPath)
          )
      {
        xs.Serialize(stream, settings);
      }
      if (Watcher != null)
      {
        Monitor(monitor);
      }
      _saving = false;
    }

    ///<summary>
    /// Load the settings from a stream
    ///</summary>
    ///<returns></returns>
    public HotBabeSettings LoadSettings(Stream stream)
    {
      while (_saving)
      {
        Thread.Sleep(100);
      }
      if (!PathHelper.FileExists(ConfigFile))
      {
        return new HotBabeSettings();
      }
      XmlSerializer xs = new XmlSerializer(typeof (HotBabeSettings));
      HotBabeSettings settings;
      try
      {
        settings = (HotBabeSettings) xs.Deserialize(stream);
      }
      catch (InvalidOperationException ex)
      {
        Logger.Debug("Exception caught and discarded: " + ex.Message +
                     " at XmlSettingsManager.LoadSettings");
        backupSettings();
        settings = new HotBabeSettings();
      }
      return settings;
    }

    ///<summary>
    /// Load the settings from the repository
    ///</summary>
    ///<returns></returns>
    public override HotBabeSettings LoadSettings()
    {
      HotBabeSettings settings;
      if (PathHelper.FileExists(ConfigFile))
      {
        using (
            Stream stream = PathHelper.GetReadStream(ConfigFile)
            )
        {
          settings = LoadSettings(stream);
        }
      }
      else
      {
        settings = new HotBabeSettings();
      }
      return settings;
    }

    #endregion

    #region Properties

    ///<summary>
    /// Filename of the configuration file
    ///</summary>
    public string ConfigFile
    {
      get;
      set;
    }

    ///<summary>
    /// FileWatcher for the configuration file
    ///</summary>
    public FileSystemWatcher Watcher
    {
      get
      {
        if (_watcher == null)
        {
          string localPath = PathHelper.GetLocalPath(ConfigFile);
          if (localPath != null)
          {
            string dir = Path.GetDirectoryName(localPath);
            string file = Path.GetFileName(localPath);
            _watcher = new FileSystemWatcher(dir, file);
            _watcher.Changed += watcherChanged;
          }
          else
          {
            Logger.Info("Cannot watch " + ConfigFile + ". Only local files are supported.");
          }
        }
        return _watcher;
      }
    }

    #endregion

    #region Private Methods

    private void backupSettings()
    {
      string localPath = PathHelper.GetLocalPath(ConfigFile);
      if (string.IsNullOrEmpty(localPath))
      {
        Logger.Info("Cannot backup settings except on local files (" + ConfigFile + ")");
        return;
      }
      using (
          Stream stream = PathHelper.GetReadStream(localPath)
          )
      {
        using (
            Stream stream2 = PathHelper.GetWriteStream(localPath + ".bak")
            )
        {
          using (StreamReader sr = new StreamReader(stream))
          {
            using (StreamWriter sw = new StreamWriter(stream2))
            {
              sw.Write(sr.ReadToEnd());
            }
          }
        }
      }
    }

    ///<summary>
    /// Checks if there are differences from the previous settings
    /// and if so, fires the <see cref="BaseSettingsManager{T}.SettingsChanged"/> event
    ///</summary>
    ///<param name="newSettings"></param>
    protected override bool VerifyChanged(HotBabeSettings newSettings)
    {
      bool result = base.VerifyChanged(newSettings);
      if (!result)
      {
        XmlSerializer xs = new XmlSerializer(typeof (HotBabeSettings));
        string newSettingsString;
        using (StringWriter sw = new StringWriter())
        {
          xs.Serialize(sw, newSettings);
          newSettingsString = sw.ToString();
        }
        if (newSettingsString != _prevSettingsString)
        {
          FireSettingsChanged(newSettings);
          _prevSettingsString = newSettingsString;
          result = true;
        }
      }
      return result;
    }

    private void watcherChanged(object sender, FileSystemEventArgs e)
    {
      Thread.Sleep(300);
      HotBabeSettings newSettings = LoadSettings();
      VerifyChanged(newSettings);
    }

    #endregion
  }
}