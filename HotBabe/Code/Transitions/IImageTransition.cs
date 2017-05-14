#region Using directives

using System;
using System.Drawing;

#endregion

namespace HotBabe.Code.Transitions
{
  ///<summary>
  /// Interface for the transition of images from one to the other 
  ///</summary>
  public interface IImageTransition
  {
    #region Events

    ///<summary>
    /// The Image was updated
    ///</summary>
    event EventHandler Update;

    #endregion

    #region Public Methods

    ///<summary>
    /// Transition from the current image to the one given as a parameter
    ///</summary>
    ///<param name="image"></param>
    void TransitionTo(Image image);

    #endregion

    #region Properties

    ///<summary>
    /// The resulting image (read it when Update fires)
    ///</summary>
    Image Image
    {
      get;
    }

    #endregion
  }
}