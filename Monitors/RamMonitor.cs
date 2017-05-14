#region Using directives

using System.Text.RegularExpressions;
using HotLogger;
using Microsoft.VisualBasic.Devices;

#endregion

namespace Monitors
{
  ///<summary>
  /// Hot Babe monitor for the memory usage
  ///</summary>
  [MonitorKey(Key = "Mem")]
  [MonitorParameter(RecommendedValue = "Auto",
      Help = "'Auto' to adjust based on use; empty for absolute memory percentage")]
  public class RamMonitor : BaseMonitor
  {
    #region Member data

    private double _minUsed = double.MaxValue;

    #endregion

    #region Properties

    ///<summary>
    /// If true, the Auto parameter is set
    ///</summary>
    protected bool Auto
    {
      get
      {
        return Regex.IsMatch(Parameter, @"\b?Auto\b?");
      }
    }

    #endregion

    #region Private Methods

    private double getAvailableRam()
    {
      ulong totalRam = new ComputerInfo().TotalPhysicalMemory;
      ulong available = new ComputerInfo().AvailablePhysicalMemory;
      const double mb = 1048576;
      Logger.Info(string.Format("Available memory {0:N0}MB from {1:N0}MB", (available/mb),
                                (totalRam/mb)));
      ulong used = totalRam - available;
      if (Auto)
      {
        if (used < _minUsed)
        {
          _minUsed = used;
        }
        else
        {
          _minUsed += (used - _minUsed)*0.1;
        }
        return 100.0*(used - _minUsed)/(totalRam - _minUsed);
      }
      return 100.0*used/totalRam;
    }

    ///<summary>
    /// Percentage of memory used.
    ///</summary>
    ///<returns></returns>
    protected override double ReadNewValue()
    {
      return getAvailableRam();
    }

    #endregion
  }
}