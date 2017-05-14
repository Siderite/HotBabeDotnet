#region Using directives

using System.Collections.Generic;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Holds information about the result of a validation operation
  ///</summary>
  public class ValidationResult : Dictionary<string, ValidationErrorCollection>
  {
    #region Public Methods

    ///<summary>
    /// Add an error message for a certain property
    ///</summary>
    ///<param name="propertyName"></param>
    ///<param name="message"></param>
    public void Add(string propertyName, string message)
    {
      Add(propertyName, message, null);
    }

    ///<summary>
    /// Add an error message for a certain property, specifying the source object
    ///</summary>
    ///<param name="propertyName"></param>
    ///<param name="message"></param>
    ///<param name="item"></param>
    public void Add(string propertyName, string message, object item)
    {
      ValidationErrorCollection list;
      if (!TryGetValue(propertyName, out list))
      {
        list = new ValidationErrorCollection();
        this[propertyName] = list;
      }
      list.Add(new ValidationError
                 {
                     Message = message,
                     Item = item
                 });
    }

    #endregion

    #region Properties

    ///<summary>
    /// Returns false if the class holds any ValidationErrors
    ///</summary>
    public bool IsValid
    {
      get
      {
        return Count == 0;
      }
    }

    #endregion
  }
}