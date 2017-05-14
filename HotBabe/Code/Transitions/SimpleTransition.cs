#region Using directives

using System;
using System.Drawing;
using HotBabe.Code.Helpers;

#endregion

namespace HotBabe.Code.Transitions
{
  ///<summary>
  /// Simply returns the target image
  ///</summary>
  public class SimpleTransition : IImageTransition
  {
    #region Public Methods

    ///<summary>
    /// Transition from the current image to the one given as a parameter
    ///</summary>
    ///<param name="image"></param>
    public void TransitionTo(Image image)
    {
      Image = image;
      Update.Fire(this);
    }

    #endregion

    #region IImageTransition Members

    ///<summary>
    /// The resulting image (read it when Update fires)
    ///</summary>
    public Image Image
    {
      get;
      private set;
    }

    ///<summary>
    /// The Image was updated
    ///</summary>
    public event EventHandler Update;

    #endregion
  }
}