#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace HotLogger
{
  ///<summary>
  /// Logger class for HotBabe.NET
  ///</summary>
  public static class Logger
  {
    #region Events

    ///<summary>
    /// Fired when new entries were added to the log
    ///</summary>
    public static event EventHandler LoggerUpdated;

    #endregion

    #region Member data

    private static List<LoggerLine> sLines;

    #endregion

    #region Public Methods

    ///<summary>
    /// Add an information message to the log
    ///</summary>
    ///<param name="message"></param>
    public static void Info(string message)
    {
      Write(message, InfoSeverity);
    }

    /// <summary>
    /// Add a debug message to the log
    /// </summary>
    /// <param name="message"></param>
    public static void Debug(string message)
    {
      Write(message, DebugSeverity);
    }

    /// <summary>
    /// Add a warning message to the log
    /// </summary>
    /// <param name="message"></param>
    public static void Warn(string message)
    {
      Write(message, WarnSeverity);
    }

    ///<summary>
    ///Add an error message to the log
    ///</summary>
    ///<param name="message"></param>
    public static void Error(string message)
    {
      Write(message, ErrorSeverity);
    }

    ///<summary>
    /// Add a message to the log with custom severity
    ///</summary>
    ///<param name="message"></param>
    ///<param name="severity"></param>
    public static void Write(string message, int severity)
    {
      Write(message, severity, null);
    }

    ///<summary>
    /// Add a message to the log, with custom severity and an associated object
    ///</summary>
    ///<param name="message"></param>
    ///<param name="severity"></param>
    ///<param name="tag"></param>
    public static void Write(string message, int severity, object tag)
    {
      Write(message, severity, DateTime.Now, tag);
    }

    ///<summary>
    /// Add a message to the log with custom severity for a specific time and with an associated object
    ///</summary>
    ///<param name="message"></param>
    ///<param name="severity"></param>
    ///<param name="time"></param>
    ///<param name="tag"></param>
    public static void Write(string message, int severity, DateTime time, object tag)
    {
      LoggerLine line = new LoggerLine
                          {
                              Message = message,
                              Severity = severity,
                              Tag = tag,
                              Time = time
                          };
      Lines.Add(line);
      if (Lines.Count > 10000)
      {
        Lines.RemoveRange(0, 1000);
      }
      if (severity < InfoSeverity)
      {
        System.Diagnostics.Debug.WriteLine(message);
      }
      if (LoggerUpdated != null)
      {
        LoggerUpdated(line, EventArgs.Empty);
      }
    }

    #endregion

    #region Properties

    ///<summary>
    /// Lines of log (there is a maximum limit of 10000 lines)
    ///</summary>
    public static List<LoggerLine> Lines
    {
      get
      {
        if (sLines == null)
        {
          sLines = new List<LoggerLine>();
        }
        return sLines;
      }
    }

    ///<summary>
    /// Constant property returning the debug severity
    ///</summary>
    public static int DebugSeverity
    {
      get
      {
        return 0;
      }
    }

    /// <summary>
    /// Constant property returning the info severity
    /// </summary>
    public static int InfoSeverity
    {
      get
      {
        return 25;
      }
    }

    /// <summary>
    /// Constant property returning the warn severity
    /// </summary>
    public static int WarnSeverity
    {
      get
      {
        return 50;
      }
    }

    /// <summary>
    /// Constant property returning the error severity
    /// </summary>
    public static int ErrorSeverity
    {
      get
      {
        return 75;
      }
    }

    #endregion
  }
}