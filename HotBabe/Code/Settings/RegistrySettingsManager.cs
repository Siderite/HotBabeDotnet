#region Using directives

using System;
using System.Timers;
using Microsoft.Win32;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Hot Babe settings manager using the Windows registry
  ///</summary>
  public class RegistrySettingsManager : BaseSettingsManager<HotBabeSettings>
  {
    #region Member data

    private Timer mTimer;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public RegistrySettingsManager()
        : this("HKEY_LOCAL_MACHINE", "Software\\Siderite\\HotBabe.NET")
    {
    }

    ///<summary>
    /// Constructor that accepts a custom hive and registry subkey
    ///</summary>
    ///<param name="hive"></param>
    ///<param name="subKey"></param>
    public RegistrySettingsManager(string hive, string subKey)
    {
      Hive = hive;
      SubKey = subKey;
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Enable/disable the monitoring on the settings repository
    ///</summary>
    ///<param name="enable"></param>
    public override void Monitor(bool enable)
    {
      if (mTimer == null)
      {
        mTimer = new Timer(1000)
                   {
                       AutoReset = true
                   };
        mTimer.Elapsed += timerElapsed;
      }
      if (enable)
      {
        mTimer.Start();
      }
      else
      {
        mTimer.Stop();
      }
    }

    ///<summary>
    /// Save the settings into the repository
    ///</summary>
    ///<param name="settings"></param>
    /// <exception cref="ArgumentNullException"><c>settings</c> is null.</exception>
    public override void SaveSettings(HotBabeSettings settings)
    {
      verifyKey();
      if (settings == null)
      {
        throw new ArgumentNullException("settings");
      }
      foreach (SettingsManagerProperty property in ObjectProperties)
      {
        object value = property.GetValue(settings);
        Registry.SetValue(Key, property.Name, value);
      }
    }

    ///<summary>
    /// Load the settings from the repository
    ///</summary>
    ///<returns></returns>
    public override HotBabeSettings LoadSettings()
    {
      verifyKey();
      HotBabeSettings settings = new HotBabeSettings();
      foreach (SettingsManagerProperty property in ObjectProperties)
      {
        object prevValue = property.GetValue(settings);
        object value = Registry.GetValue(Key, property.Name, prevValue);
        if (value != null)
        {
          string s = value.ToString();
          if (property.Type == typeof (int))
          {
            int i;
            if (int.TryParse(s, out i))
            {
              value = i;
            }
          }
          if (property.Type == typeof (byte))
          {
            byte i;
            if (byte.TryParse(s, out i))
            {
              value = i;
            }
          }
          if (property.Type == typeof (long))
          {
            long i;
            if (long.TryParse(s, out i))
            {
              value = i;
            }
          }
          if (property.Type == typeof (float))
          {
            float i;
            if (float.TryParse(s, out i))
            {
              value = i;
            }
          }
          if (property.Type == typeof (double))
          {
            double i;
            if (double.TryParse(s, out i))
            {
              value = i;
            }
          }
          if (property.Type == typeof (decimal))
          {
            decimal i;
            if (decimal.TryParse(s, out i))
            {
              value = i;
            }
          }
          if (property.Type == typeof (bool))
          {
            bool i;
            if (bool.TryParse(s, out i))
            {
              value = i;
            }
          }
          if (property.Type.IsEnum)
          {
            try
            {
              value=Enum.Parse(property.Type, s);
            }catch
            {
              // get first enum value
              foreach (var val in Enum.GetValues(property.Type))
              {
                value = val;
                break;
              }
            }
          }
          property.SetValue(settings, value);
        }
      }
      return settings;
    }

    #endregion

    #region Properties

    ///<summary>
    /// The registry hive used to store the settings.
    /// Defaults to HKEY_LOCAL_MACHINE.
    ///</summary>
    public string Hive
    {
      get;
      set;
    }

    ///<summary>
    /// Registry subkey used to store the settings:
    /// Defaults to Software\Siderite\HotBabe.NET
    ///</summary>
    public string SubKey
    {
      get;
      set;
    }

    ///<summary>
    /// Registry key used to store the settings (Hive\SubKey)
    ///</summary>
    public string Key
    {
      get
      {
        return Hive + "\\" + SubKey;
      }
    }

    #endregion

    #region Private Methods

    private void timerElapsed(object sender, ElapsedEventArgs e)
    {
      HotBabeSettings settings = LoadSettings();
      VerifyChanged(settings);
    }

    /// <exception cref="ApplicationException">Registry settings manager requires a valid Hive/Subkey</exception>
    private void verifyKey()
    {
      if (string.IsNullOrEmpty(Hive))
      {
        throw new ApplicationException("Registry settings manager requires a valid hive");
      }
      if (string.IsNullOrEmpty(SubKey))
      {
        throw new ApplicationException("Registry settings manager requires a valid subKey");
      }
    }

    #endregion
  }
}