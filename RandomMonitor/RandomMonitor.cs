#region Using directives

using System;
using Monitors;

#endregion

namespace RandomMonitor
{
  ///<summary>
  /// Sample Hot Babe monitor produces a random number between 0 and 100
  ///</summary>
  [MonitorKey(Key = "Rnd")]
  public class RandomMonitor : BaseMonitor
  {
    #region Member data

    private readonly Random _rnd;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public RandomMonitor()
    {
      _rnd = new Random();
    }

    #endregion

    #region Private Methods

    ///<summary>
    /// returns a random integer value between 0 and 100
    ///</summary>
    ///<returns></returns>
    protected override double ReadNewValue()
    {
      return _rnd.Next(0, 101);
    }

    #endregion
  }
}