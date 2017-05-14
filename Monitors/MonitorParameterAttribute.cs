#region Using directives

using System;

#endregion

namespace Monitors
{
  ///<summary>
  /// Details about the monitor parameter
  ///</summary>
  public class MonitorParameterAttribute : Attribute
  {
    #region Properties

    ///<summary>
    /// Help string for parameter use
    ///</summary>
    public string Help
    {
      get;
      set;
    }

    ///<summary>
    /// Recommended parameter value
    ///</summary>
    public string RecommendedValue
    {
      get;
      set;
    }

    #endregion
  }
}