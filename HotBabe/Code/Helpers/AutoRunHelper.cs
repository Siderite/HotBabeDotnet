#region Using directives

using System;
using System.IO;
using Microsoft.Win32;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Used to set applications to run at Windows startup
  ///</summary>
  public class AutoRunHelper
  {
    #region Public Methods

    ///<summary>
    /// Set application to autorun at Windows startup
    ///</summary>
    ///<exception cref="ArgumentException"></exception>
    ///<exception cref="FileNotFoundException"></exception>
    public void SetToRun()
    {
      if (string.IsNullOrEmpty(ApplicationKey))
      {
        throw new ArgumentException("Application key cannot be empty");
      }
      if (string.IsNullOrEmpty(ApplicationPath))
      {
        throw new ArgumentException("Application path cannot be empty");
      }
      if (!PathHelper.FileExists(ApplicationPath))
      {
        throw new FileNotFoundException("Application path not found");
      }
      Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Run",
                        ApplicationKey,
                        ApplicationPath);
    }

    ///<summary>
    /// Remove autorun for the application
    ///</summary>
    public void UnsetToRun()
    {
      RegistryKey rk =
          Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
      if (rk != null && rk.GetValue(ApplicationKey) != null)
      {
        rk.DeleteValue(ApplicationKey);
      }
    }

    #endregion

    #region Properties

    ///<summary>
    /// Application filename
    ///</summary>
    public string ApplicationPath
    {
      get;
      set;
    }

    ///<summary>
    /// Registry key for the autorun entry
    ///</summary>
    public string ApplicationKey
    {
      get;
      set;
    }

    #endregion
  }
}