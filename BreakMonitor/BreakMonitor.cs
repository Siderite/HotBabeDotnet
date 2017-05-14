#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using HotLogger;
using Monitors;

#endregion

namespace BreakMonitor
{
  ///<summary>
  /// Monitors idle time and returns 100 when you need to take a break
  ///</summary>
  [MonitorKey(Key = "Break")]
  [MonitorParameter(RecommendedValue = "55,5",
      Help = "Time to keep active followed by the time a break should last, in minutes. Ex: 55,5")]
  public class BreakMonitor : BaseMonitor
  {
    // ReSharper disable InconsistentNaming

    #region Nested type: LASTINPUTINFO

    [StructLayout(LayoutKind.Sequential)]
    private struct LASTINPUTINFO
    {
      public static readonly int SizeOf = Marshal.SizeOf(typeof (LASTINPUTINFO));

      [MarshalAs(UnmanagedType.U4)]
      public int cbSize;

      [MarshalAs(UnmanagedType.U4)]
      public int dwTime;
    }

    #endregion

    // ReSharper restore InconsistentNaming

    #region Member data

    private static readonly Regex sReg = new Regex(@"(?<active>\d+)\s*[^\d]\s*(?<break>\d+)");
    private int _activeTime;
    private int _breakTime;
    private bool _warn;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public BreakMonitor()
    {
      Coarseness = 30000;
    }

    #endregion

    #region Properties

    ///<summary>
    /// Milliseconds in which no action must be taken to be
    /// considered a break.
    /// Default: 60000 (a minute)
    ///</summary>
    public int Coarseness
    {
      get;
      set;
    }

    ///<summary>
    /// Configured active time interval at which 100 is returned
    ///</summary>
    protected int ActiveInterval
    {
      get
      {
        Match m = sReg.Match(Parameter);
        if (m.Success)
        {
          int active;
          if (int.TryParse(m.Groups["active"].Value, out active))
          {
            return active*60000;
          }
        }
        return 55*60000;
      }
    }

    ///<summary>
    /// Configured break time interval that resets the active timer
    /// and makes the application return 0
    ///</summary>
    protected int BreakInterval
    {
      get
      {
        Match m = sReg.Match(Parameter);
        if (m.Success)
        {
          int breakTime;
          if (int.TryParse(m.Groups["break"].Value, out breakTime))
          {
            return breakTime*60000;
          }
        }
        return 5*60000;
      }
    }

    #endregion

    // ReSharper disable InconsistentNaming

    #region Private Methods

    [DllImport("user32.dll")]
    private static extern bool GetLastInputInfo(out LASTINPUTINFO plii);

    // ReSharper restore InconsistentNaming

    private static int getIdleTime()
    {
      int idleTime = 0;
      LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
      lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
      lastInputInfo.dwTime = 0;
      int envTicks = Environment.TickCount;
      if (GetLastInputInfo(out lastInputInfo))
      {
        int lastInputTick = lastInputInfo.dwTime;
        idleTime = envTicks - lastInputTick;
      }
      return idleTime;
    }

    ///<summary>
    /// Return 100 when a break is needed, else 0.
    ///</summary>
    ///<returns></returns>
    protected override double ReadNewValue()
    {
      int idleTime = getIdleTime();
      if (idleTime < Coarseness)
      {
        _activeTime += UpdateInterval;
        _breakTime = 0;
      }
      else
      {
        _breakTime = idleTime;
      }
      Logger.Debug(
          string.Format("Idle milliseconds: {0}, breakTime: {1} of {2}, activeTime: {3} of {4}",
                        idleTime, _breakTime, BreakInterval, _activeTime, ActiveInterval));
      if (_activeTime > ActiveInterval)
      {
        _warn = true;
      }
      if (_breakTime >= BreakInterval)
      {
        _warn = false;
        _activeTime = 0;
      }
      return _warn
                 ? 100
                 : 0;
    }

    #endregion
  }
}