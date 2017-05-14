#region Using directives

using System;

#endregion

namespace Monitors.Code.Helpers
{
  ///<summary>
  /// Extension methods for events
  ///</summary>
  public static class EventExtensionMethods
  {
    #region Public Methods

    ///<summary>
    /// Fire an event
    ///</summary>
    ///<param name="handler"></param>
    ///<param name="sender"></param>
    public static void Fire(this EventHandler handler, object sender)
    {
      Fire(handler, sender, EventArgs.Empty);
    }

    ///<summary>
    /// Fire an event specifying the <see cref="EventArgs"/>
    ///</summary>
    ///<param name="handler"></param>
    ///<param name="sender"></param>
    ///<param name="args"></param>
    public static void Fire(this EventHandler handler, object sender, EventArgs args)
    {
      if (handler != null)
      {
        handler(sender, args);
      }
    }

    ///<summary>
    /// Fire an event with arguments of type T
    ///</summary>
    ///<param name="handler"></param>
    ///<param name="sender"></param>
    ///<param name="args"></param>
    ///<typeparam name="T"></typeparam>
    public static void Fire<T>(this EventHandler<T> handler, object sender, T args)
        where T : EventArgs
    {
      if (handler != null)
      {
        handler(sender, args);
      }
    }

    #endregion
  }
}