#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// <see cref="EventArgs"/> for the <c>BaseSettingsManager</c> SettingsChanged event
  ///</summary>
  public class SettingsChangedEventArgs : EventArgs
  {
    #region Properties

    ///<summary>
    /// List of changed properties
    ///</summary>
    public List<string> ChangedProperties
    {
      get;
      set;
    }

    #endregion
  }
}