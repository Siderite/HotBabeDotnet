#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using HotBabe.Code.Helpers;
using Monitors;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Helps with validating Hot Babe settings
  ///</summary>
  public static class SettingsValidationHelper
  {
    #region Public Methods

    ///<summary>
    /// Validate an entire settings file
    ///</summary>
    ///<param name="settings"></param>
    ///<returns></returns>
    public static ValidationResult Validate(HotBabeSettings settings)
    {
      ValidationResult result = new ValidationResult();
      if (settings.Opacity < 0 || settings.Opacity > 100)
      {
        result.Add("Opacity", "Opacity must be a value between 0 and 100");
      }
      if (!PathHelper.FileExists(settings.DefaultImageName))
      {
        result.Add("DefaultImageName", "Default image not found (" + settings.DefaultImageName + ")");
      }
      if (settings.UpdateInterval < 100)
      {
        result.Add("UpdateInterval", "UpdateInterval cannot be smaller than 100 milliseconds");
      }
      foreach (ImageInfo info in settings.ImageInfos)
      {
        Validate(info, result);
      }
      foreach (MonitorInfo info in settings.MonitorInfos)
      {
        Validate(info, result);
      }
      return result;
    }

    ///<summary>
    /// Validate only a <see cref="MonitorInfo"/> object.
    ///</summary>
    ///<param name="info"></param>
    ///<returns></returns>
    public static ValidationResult Validate(MonitorInfo info)
    {
      return Validate(info, null);
    }

    ///<summary>
    /// Validate a <see cref="MonitorInfo"/> object from a starting <see cref="ValidationResult"/> 
    ///</summary>
    ///<param name="info"></param>
    ///<param name="result"></param>
    ///<returns></returns>
    public static ValidationResult Validate(MonitorInfo info, ValidationResult result)
    {
      if (result == null)
      {
        result = new ValidationResult();
      }
      Assembly assembly;
      if (!AssemblyHelper.TryLoadAssembly(info.AssemblyFileName, out assembly))
      {
        result.Add("MonitorInfo.AssemblyFileName", "Assembly not valid or not found.", info);
      }
      else
      {
        if (!AssemblyHelper.TypeExists(assembly, info.TypeName))
        {
          result.Add("MonitorInfo.TypeName", "Monitor type not valid or not found.", info);
        }
        else
        {
          Type type = assembly.GetType(info.TypeName);
          try
          {
            BaseMonitor monitor =
                (BaseMonitor) Activator.CreateInstance(type);
            monitor.Parameter = info.Parameter;
            string paramMessage = monitor.ValidateParameter();
            if (!string.IsNullOrEmpty(paramMessage))
            {
              result.Add("MonitorInfo.Parameter",
                         "Monitor parameter validation failed: " + paramMessage);
            }
          }
          catch (Exception ex)
          {
            result.Add("MonitorInfo.TypeName", "Monitor instantiation failed: " + ex.Message);
          }
        }
      }
      if (info.MinValue < 0 || info.MinValue > 99)
      {
        result.Add("MonitorInfo.MinValue", "Minimum value must be a value between 0 and 99", info);
      }
      if (string.IsNullOrEmpty(info.Key))
      {
        result.Add("MonitorInfo.Key", "The measurement key cannot be empty.", info);
      }
      if (info.Smooth < 0 || info.Smooth > 1)
      {
        result.Add("MonitorInfo.Smooth", "The Smooth value must be a value between 0 and 1", info);
      }
      if (info.UpdateInterval <= 0)
      {
        result.Add("MonitorInfo.UpdateInterval", "Monitor update interval cannot be negative or 0");
      }
      return result;
    }

    ///<summary>
    /// Validate only an <see cref="ImageInfo"/> object
    ///</summary>
    ///<param name="info"></param>
    ///<returns></returns>
    public static ValidationResult Validate(ImageInfo info)
    {
      return Validate(info, null);
    }

    ///<summary>
    /// Validate only an <see cref="ImageInfo"/> object starting from a <see cref="ValidationResult"/>
    ///</summary>
    ///<param name="info"></param>
    ///<param name="result"></param>
    ///<returns></returns>
    public static ValidationResult Validate(ImageInfo info, ValidationResult result)
    {
      if (result == null)
      {
        result = new ValidationResult();
      }
      if (string.IsNullOrEmpty(info.ImageFileName))
      {
        result.Add("ImageInfo.ImageFileName", "Image file cannot be empty", info);
      }
      else
      {
        if (!PathHelper.FileExists(info.ImageFileName))
        {
          result.Add("ImageInfo.ImageFileName",
                     string.Format("Image file not found ({0})", info.ImageFileName), info);
        }
      }
      if (info.Measures.Count == 0)
      {
        result.Add("ImageInfo.Measures", "The image has no measures");
      }
      foreach (KeyValuePair<string, double> measure in info.Measures)
      {
        if (string.IsNullOrEmpty(measure.Key))
        {
          result.Add("ImageInfo.Measures.Key", "Measure key cannot be empty",
                     new object[] {info, measure});
        }
        if (measure.Value < 0 || measure.Value > 100)
        {
          result.Add("ImageInfo.Measures.Value",
                     string.Format("Measure must be a value between 0 and 100 ({0})", measure.Key),
                     new object[] {info, measure});
        }
      }
      return result;
    }

    #endregion
  }
}