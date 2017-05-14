#region Using directives

using System;
using System.Drawing;
using HotBabe.Code.Helpers;

#endregion

namespace HotBabe.Code.Transitions
{
  ///<summary>
  /// Transition responsible for showing a custom image dropped on the application
  ///</summary>
  public class CustomImageTransition : IImageTransition
  {
    #region Member data

    private readonly Image _image;

    #endregion

    #region Constructors

    ///<summary>
    /// default constructor
    ///</summary>
    ///<param name="image"></param>
    public CustomImageTransition(Image image)
    {
      _image = image;
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Transition from the current image to the one given as a parameter
    ///</summary>
    ///<param name="image"></param>
    public void TransitionTo(Image image)
    {
      //DO NOTHING
      Update.Fire(this);
    }

    #endregion

    #region IImageTransition Members

    ///<summary>
    /// The Image was updated
    ///</summary>
    public event EventHandler Update;

    ///<summary>
    /// The resulting image (read it when Update fires)
    ///</summary>
    public Image Image
    {
      get
      {
        return _image;
      }
    }

    #endregion
  }
}