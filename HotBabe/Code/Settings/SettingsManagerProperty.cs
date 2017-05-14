#region Using directives

using System;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Holds information about reflected properties in the settings class
  ///</summary>
  public class SettingsManagerProperty
  {
    #region Public Methods

    ///<summary>
    /// Get the property value on an object
    ///</summary>
    ///<param name="target"></param>
    ///<returns></returns>
    public object GetValue(object target)
    {
      object value = target.GetType().GetProperty(Name).GetValue(target, new object[] {});
      return value;
    }

    ///<summary>
    /// Set the property value on an object
    ///</summary>
    ///<param name="target"></param>
    ///<param name="value"></param>
    public void SetValue(object target, object value)
    {
      target.GetType().GetProperty(Name).SetValue(target, value, new object[] {});
    }

    #endregion

    #region Properties

    ///<summary>
    /// Return Type of the property
    ///</summary>
    public Type Type
    {
      get;
      set;
    }

    ///<summary>
    /// Property name
    ///</summary>
    public string Name
    {
      get;
      set;
    }

    #endregion
  }
}