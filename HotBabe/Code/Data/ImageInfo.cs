#region Using directives

using HotBabe.Code.Helpers;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Holds information about an image that can be displayed
  ///</summary>
  public class ImageInfo
  {
    #region Member data

    private SerializableDictionary<string, double> _measures;

    #endregion

    #region Properties

    ///<summary>
    /// The image filename relative to the application folder
    ///</summary>
    public string ImageFileName
    {
      get;
      set;
    }

    ///<summary>
    /// List of measures and values that the image will react to
    ///</summary>
    public SerializableDictionary<string, double> Measures
    {
      get
      {
        if (_measures == null)
        {
          _measures = new SerializableDictionary<string, double>();
        }
        return _measures;
      }
      set
      {
        _measures = value;
      }
    }

    #endregion
  }
}