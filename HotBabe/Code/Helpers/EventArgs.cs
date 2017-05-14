#region Using directives

using System;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Generic EventArgs with a Value property of T
  ///</summary>
  ///<typeparam name="T"></typeparam>
  public class EventArgs<T> : EventArgs
  {
    #region Constructors

    ///<summary>
    /// Instantiate with the Value
    ///</summary>
    ///<param name="value"></param>
    public EventArgs(T value) : this()
    {
      Value = value;
    }

    ///<summary>
    /// Default constructor
    ///</summary>
    public EventArgs()
    {
    }

    #endregion

    #region Properties

    ///<summary>
    /// Default value
    ///</summary>
    public T Value
    {
      get;
      set;
    }

    #endregion
  }
}