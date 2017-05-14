namespace HotBabe.Code
{
  ///<summary>
  /// holds some application global constants
  ///</summary>
  internal static class Constants
  {
    #region Properties

    ///<summary>
    /// Embedded image to show if none is configured correctly
    ///</summary>
    public static string StartImageFileName
    {
      get
      {
        return "res:///StartImage.png";
      }
    }

    /// <summary>
    /// Default maximum size of images cached in memory (100MB)
    /// </summary>
    public static long DefaultMaxImageCacheSize
    {
      get { return 100*1024*1024; }
    }

    #endregion
  }
}