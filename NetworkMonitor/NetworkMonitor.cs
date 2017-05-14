#region Using directives

using System.Net.NetworkInformation;
using HotLogger;
using Monitors;

#endregion

namespace NetworkMonitor
{
  ///<summary>
  /// Monitors network use
  ///</summary>
  [MonitorKey(Key = "Net")]
  public class NetworkMonitor : BaseMonitor
  {
    #region Member data

    private double _maxBytes;
    private double _prevBytes;

    #endregion

    #region Private Methods

    ///<summary>
    /// The method employed to get a new measurement (a value between 0 and 100)
    ///</summary>
    ///<returns></returns>
    protected override double ReadNewValue()
    {
      double bytes = getNetworkTraffic();
      if (bytes > _maxBytes)
      {
        _maxBytes = bytes;
      }
      if (_maxBytes == 0)
      {
        return 0;
      }
      return 100.0*bytes/_maxBytes;
    }

    private double getNetworkTraffic()
    {
      NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
      double bytes = 0;
      foreach (NetworkInterface adapter in adapters)
      {
        if (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
        {
          IPv4InterfaceStatistics stats = adapter.GetIPv4Statistics();
          Logger.Info("Network adapter " + adapter.Name + ". Bytes received: " + stats.BytesReceived +
                      ", Bytes sent: " + stats.BytesSent);
          bytes += stats.BytesReceived + stats.BytesSent;
        }
      }
      if (_prevBytes == 0)
      {
        _prevBytes = bytes;
      }
      double result = bytes - _prevBytes;
      Logger.Info(string.Format("Total network traffic: {0} (+{1})", bytes, result));
      _prevBytes = bytes;
      return result;
    }

    #endregion
  }
}