#region Using directives

using System;
using System.Diagnostics;
using HotLogger;

#endregion

namespace Monitors
{
  ///<summary>
  /// Hot Babe monitor class for the CPU.
  ///</summary>
  [MonitorKey(Key = "Cpu")]
  public class CpuMonitor : BaseMonitor
  {
    #region Member data

    private readonly PerformanceCounter _cpuCounter;
    private readonly PerformanceCounter _ownCpuCounter;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public CpuMonitor()
    {
      _cpuCounter = new PerformanceCounter
                      {
                          CategoryName = "Processor",
                          CounterName = "% Processor Time",
                          InstanceName = "_Total"
                      };
      _ownCpuCounter = new PerformanceCounter
                         {
                             CategoryName = "Process",
                             CounterName = "% Processor Time",
                             InstanceName = Process.GetCurrentProcess().ProcessName,
                             ReadOnly = true
                         };
    }

    #endregion

    #region Private Methods

    private double getCurrentCpuUsage()
    {
      float ownValue;
      string ownValueDebug;
      try
      {
        ownValue = _ownCpuCounter.NextValue();
        ownValueDebug = ownValue + "%";
      }
      catch (InvalidOperationException ex)
      {
        ownValue = 0;
        ownValueDebug = string.Format("{0} not found ({1})", Process.GetCurrentProcess().ProcessName,
                                      ex.Message);
      }

      float value = _cpuCounter.NextValue();
      Logger.Debug(string.Format("Total CPU usage: {0}%, own: {1}", value, ownValueDebug));
      value -= ownValue;
      Logger.Info("CPU usage: " + value + "%");
      return value;
    }

    ///<summary>
    /// Returns the CPU usage
    ///</summary>
    ///<returns></returns>
    protected override double ReadNewValue()
    {
      return getCurrentCpuUsage();
    }

    #endregion
  }
}