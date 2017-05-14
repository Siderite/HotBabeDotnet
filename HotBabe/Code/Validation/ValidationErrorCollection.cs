#region Using directives

using System.Collections.Generic;
using System.Text;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// A collection of validation error messages
  ///</summary>
  public class ValidationErrorCollection : List<ValidationError>
  {
    #region Public Methods

    ///<summary>
    /// Returns a new line separated join of all the error messages
    /// in the child <see cref="ValidationError"/> objects
    ///</summary>
    ///<returns></returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      foreach (ValidationError error in this)
      {
        if (sb.Length > 0)
        {
          sb.AppendLine();
        }
        sb.Append(error.Message);
      }
      return sb.ToString();
    }

    #endregion
  }
}