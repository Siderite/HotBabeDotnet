#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using HotBabe.Code.Helpers;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Base abstract class that manages the persisting of general settings classes
  ///</summary>
  ///<typeparam name="T"></typeparam>
  public abstract class BaseSettingsManager<T> where T : class, new()
  {
    #region Events

    ///<summary>
    /// Fired when any of the settings is changed. By default fired by <see cref="VerifyChanged"/>.
    ///</summary>
    public event EventHandler<SettingsChangedEventArgs> SettingsChanged;

    #endregion

    #region Member data

    private SettingsManagerPropertyCollection _objectProperties;
    private T _previousSettings;

    #endregion

    #region Public Methods

    ///<summary>
    /// Enable/disable the monitoring on the settings repository
    ///</summary>
    ///<param name="enable"></param>
    public abstract void Monitor(bool enable);

    ///<summary>
    /// Save the settings into the repository
    ///</summary>
    ///<param name="settings"></param>
    public abstract void SaveSettings(T settings);

    ///<summary>
    /// Load the settings from the repository
    ///</summary>
    ///<returns></returns>
    public abstract T LoadSettings();

    /// <summary>
    /// Create new instance of the same type of settings manager
    /// </summary>
    /// <param name="configurationFile"></param>
    /// <returns></returns>
    public abstract BaseSettingsManager<T> GetNewInstance(string configurationFile);

    #endregion

    #region Properties

    ///<summary>
    /// Holds the reflected Read/Write properties of the settings class
    /// that are either string, numeric or bool
    ///</summary>
    public SettingsManagerPropertyCollection ObjectProperties
    {
      get
      {
        if (_objectProperties == null)
        {
          _objectProperties = GetObjectProperties();
        }
        return _objectProperties;
      }
    }

    #endregion

    #region Private Methods

    ///<summary>
    /// Checks if there are differences from the previous settings
    /// and if so, fires the <see cref="SettingsChanged"/> event
    ///</summary>
    ///<param name="newSettings"></param>
    protected virtual bool VerifyChanged(T newSettings)
    {
      List<string> list = new List<string>();
      foreach (SettingsManagerProperty property in ObjectProperties)
      {
        if (_previousSettings == null ||
            !Equals(property.GetValue(_previousSettings), property.GetValue(newSettings)))
        {
          list.Add(property.Name);
        }
      }
      if (list.Count > 0)
      {
        FireSettingsChanged(newSettings, list);
        _previousSettings = newSettings;
        return true;
      }
      return false;
    }

    ///<summary>
    /// Fires the <see cref="SettingsChanged"/> event with the names of changeProperties
    ///</summary>
    ///<param name="settings"></param>
    ///<param name="changedProperties"></param>
    protected void FireSettingsChanged(T settings, params string[] changedProperties)
    {
      FireSettingsChanged(settings, new List<string>(changedProperties));
    }

    ///<summary>
    /// Fires the <see cref="SettingsChanged"/> event with the names of changeProperties
    ///</summary>
    ///<param name="settings"></param>
    ///<param name="changedProperties"></param>
    protected void FireSettingsChanged(T settings, List<string> changedProperties)
    {
      SettingsChanged.Fire(settings, new SettingsChangedEventArgs
                                       {
                                           ChangedProperties = changedProperties
                                       });
    }

    ///<summary>
    /// Get the properties for the settings object that are
    /// read/write, public and string, bool or numeric
    ///</summary>
    ///<returns></returns>
    protected virtual SettingsManagerPropertyCollection GetObjectProperties()
    {
      Type type = typeof (T);
      SettingsManagerPropertyCollection list = new SettingsManagerPropertyCollection();
      PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      foreach (PropertyInfo info in properties)
      {
        if (info.CanRead && info.CanWrite)
        {
          if (info.PropertyType == typeof (string)
              || info.PropertyType == typeof (int)
              || info.PropertyType == typeof (byte)
              || info.PropertyType == typeof (long)
              || info.PropertyType == typeof (float)
              || info.PropertyType == typeof (double)
              || info.PropertyType == typeof (decimal)
              || info.PropertyType == typeof (bool)
              || info.PropertyType.IsEnum)
          {
            SettingsManagerProperty prop = new SettingsManagerProperty
                                             {
                                                 Name = info.Name,
                                                 Type = info.PropertyType
                                             };
            list.Add(prop);
          }
        }
      }
      return list;
    }

    #endregion
  }
}