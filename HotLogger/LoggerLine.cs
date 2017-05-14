using System;

namespace HotLogger
{
  ///<summary>
  /// A class representing a line in the log
  ///</summary>
  [Serializable]
  public class LoggerLine
  {
    #region Properties

    ///<summary>
    /// Line message
    ///</summary>
    public string Message
    {
      get;
      set;
    }

    /// <summary>
    /// Line severity
    /// </summary>
    public int Severity
    {
      get;
      set;
    }

    /// <summary>
    /// Line associated object (if any)
    /// </summary>
    public object Tag
    {
      get;
      set;
    }

    /// <summary>
    /// Line date and time
    /// </summary>
    public DateTime Time
    {
      get;
      set;
    }

    #endregion
  }
}