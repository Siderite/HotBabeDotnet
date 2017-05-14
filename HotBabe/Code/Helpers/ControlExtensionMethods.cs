#region Using directives

using System;
using System.Windows.Forms;
using HotLogger;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Extension methods for Windows Control
  ///</summary>
  public static class ControlExtensionMethods
  {
    #region Public Methods

    ///<summary>
    /// Execute an action on the control in a thread safe manner
    ///</summary>
    ///<param name="control"></param>
    ///<param name="action"></param>
    public static void SafeInvoke(this Control control, Action action)
    {
      if (control.IsDisposed)
      {
        return;
      }
      if (!control.IsHandleCreated)
      {
        try
        {
          action();
        }
        catch (InvalidOperationException ex)
        {
          Logger.Debug("Exception caught and discarded: " + ex.Message +
                       " at ControlExtensionMethods.SafeInvoke");
        }
        return;
      }
      if (control.InvokeRequired)
      {
        control.BeginInvoke(action);
      }
      else
      {
        action();
      }
    }

    ///<summary>
    /// Execute an action on the control in a thread safe manner
    ///</summary>
    ///<param name="control"></param>
    ///<param name="action"></param>
    ///<param name="parameter"></param>
    ///<typeparam name="T"></typeparam>
    public static void SafeInvoke<T>(this Control control, Action<T> action, T parameter)
    {
      if (control.IsDisposed)
      {
        return;
      }
      if (!control.IsHandleCreated)
      {
        try
        {
          action(parameter);
        }
        catch (InvalidOperationException ex)
        {
          Logger.Debug("Exception caught and discarded: " + ex.Message +
                       " at ControlExtensionMethods.SafeInvoke");
        }
        return;
      }
      if (control.InvokeRequired)
      {
        control.BeginInvoke(action, parameter);
      }
      else
      {
        action(parameter);
      }
    }

    #endregion
  }
}