#region Using directives

using System;
using System.Drawing;
using System.Timers;
using HotBabe.Code.Helpers;

#endregion

namespace HotBabe.Code.Transitions
{
  ///<summary>
  /// A smooth 10 step 1 second transition from an image to another
  ///</summary>
  public class SmoothTransition : IImageTransition
  {
    #region Member data

    private readonly object _locker = new object();
    private readonly Timer _timer;
    private Image _endImage;
    private Image _startImage;
    private int _step;
    private Image mImage;

    #endregion

    #region Constructors

    ///<summary>
    /// Defines an internal timer
    ///</summary>
    public SmoothTransition()
    {
      _timer = new Timer
                 {
                     Interval = 100,
                     AutoReset = true
                 };
      _timer.Elapsed += timerElapsed;
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Transition from the current image to the one given as a parameter
    ///</summary>
    ///<param name="image"></param>
    public void TransitionTo(Image image)
    {
      if (image == null)
      {
        return;
      }
      image = image.CloneImage();
      _startImage = Image ?? image;
      _timer.Stop();
      _endImage = image;
      _step = 0;
      _timer.Start();
    }

    #endregion

    #region Private Methods

    private void timerElapsed(object sender, ElapsedEventArgs e)
    {
      _step++;
      if (_step > 10)
      {
        _timer.Stop();
        return;
      }
      lock (_locker)
        Image = ImageHelper.Blend(_startImage, _endImage, _step/10f);
      Update.Fire(this);
    }

    #endregion

    #region IImageTransition Members

    ///<summary>
    /// The resulting image (read it when Update fires)
    ///</summary>
    public Image Image
    {
      get
      {
        lock (_locker)
        {
          return mImage.CloneImage();
        }
      }
      private set
      {
        lock (_locker)
        {
          mImage = value;
        }
      }
    }

    public event EventHandler Update;

    #endregion
  }
}