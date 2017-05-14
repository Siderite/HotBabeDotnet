namespace HotBabe.Code
{
  ///<summary>
  /// Holds a information about a validation error message
  ///</summary>
  public class ValidationError
  {
    #region Properties

    ///<summary>
    /// Error message
    ///</summary>
    public string Message
    {
      get;
      set;
    }

    ///<summary>
    /// The item that caused the error (optional, used to find the
    /// subobject that caused errors in parent classes or
    /// collections)
    ///</summary>
    public object Item
    {
      get;
      set;
    }

    #endregion
  }
}