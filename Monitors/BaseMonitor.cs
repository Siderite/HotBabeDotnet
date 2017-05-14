#region Using directives

using System;
using System.Timers;
using HotLogger;
using Monitors.Code.Helpers;

#endregion

namespace Monitors
{
  ///<summary>
  /// Base Hot Babe monitor class
  ///</summary>
  public abstract class BaseMonitor
  {
    #region Events

    ///<summary>
    /// Fired when the value of the monitor has been updated
    ///</summary>
    public virtual event EventHandler Updated;

    #endregion

    #region Member data

    private readonly Timer _timer;
    private int _updateInterval;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    protected BaseMonitor()
    {
      _timer = new Timer
                 {
                     AutoReset = true
                 };
      _timer.Elapsed += timerTick;
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Start the monitor
    ///</summary>
    public virtual void Start()
    {
      _timer.Stop();
      _timer.Interval = UpdateInterval;
      _timer.Start();
    }

    ///<summary>
    /// Stop the monitor
    ///</summary>
    public virtual void Stop()
    {
      _timer.Stop();
    }

    /// <summary>
    /// Use it to validate the monitor parameter
    /// </summary>
    /// <returns>An error message or null</returns>
    public virtual string ValidateParameter()
    {
      return null;
    }

    #endregion

    #region Properties

    ///<summary>
    /// The interval in milliseconds at which measurements are updated
    ///</summary>
    public virtual int UpdateInterval
    {
      get
      {
        return _updateInterval;
      }
      set
      {
        bool started = _timer.Enabled;
        _updateInterval = value;
        if (started)
        {
          Start();
        }
      }
    }

    ///<summary>
    /// The monitor measure key (it needs to be set and be the same
    /// as a measure name in an <c>ImageInfo</c> class)
    ///</summary>
    public virtual string Key
    {
      get;
      set;
    }

    ///<summary>
    /// The monitor optional parameter
    ///</summary>
    public virtual string Parameter
    {
      get;
      set;
    }

    ///<summary>
    /// Value between 0 and 1. Anything about 0 smooths it.
    /// Suggested values 0.7-0.9
    ///</summary>
    public virtual double Smooth
    {
      get;
      set;
    }

    ///<summary>
    /// Minimum value at which a measurement is taken into account.
    /// Ex: using a value of 90 will make a measurement of 95 appear as 50 (halfway between 90 and 100)
    ///</summary>
    public virtual double MinValue
    {
      get;
      set;
    }

    ///<summary>
    /// The last monitor value (between 0 and 100)
    ///</summary>
    public virtual double Value
    {
      get;
      protected set;
    }

    #endregion

    #region Private Methods

    private void timerTick(object sender, EventArgs e)
    {
      double val = ReadNewValue();
      Logger.Info(string.Format("Monitor {0} got value {1:N4}", Key, val));
      if (MinValue > 0)
      {
        val = val - MinValue;
        if (val < 0)
        {
          val = 0;
        }
        val = val/(100 - MinValue)*100;
      }
      Value += (val - Value)*(1 - Smooth);
      Logger.Info(string.Format("Monitor {0} reported value {1:N4}", Key, Value));
      Updated.Fire(this);
    }

    ///<summary>
    /// The method employed to get a new measurement (a value between 0 and 100)
    ///</summary>
    ///<returns></returns>
    protected abstract double ReadNewValue();

    #endregion
  }
}