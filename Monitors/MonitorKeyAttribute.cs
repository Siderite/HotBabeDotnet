#region Using directives

using System;

#endregion

namespace Monitors
{
  ///<summary>
  /// Default monitor key
  ///</summary>
  public class MonitorKeyAttribute : Attribute
  {
    #region Properties

    ///<summary>
    /// The default monitor key
    ///</summary>
    public string Key
    {
      get;
      set;
    }

    #endregion
  }
}