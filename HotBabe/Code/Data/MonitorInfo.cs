#region Using directives

using System.Xml.Serialization;

#endregion

namespace HotBabe.Code
{
  ///<summary>
  /// Holds information about a monitor class that is loaded in the application
  ///</summary>
  public class MonitorInfo
  {
    #region Constructors

    ///<summary>
    /// Sets the default values
    ///</summary>
    public MonitorInfo()
    {
      UpdateInterval = 1000;
    }

    #endregion

    #region Properties

    ///<summary>
    /// The assembly filename relative to the application folder
    ///</summary>
    public string AssemblyFileName
    {
      get;
      set;
    }

    ///<summary>
    /// The full name of the type of the monitor class
    ///</summary>
    public string TypeName
    {
      get;
      set;
    }

    ///<summary>
    /// Value between 0 and 99 representing the minimum value from
    /// which the measurement becomes valid. Ex: set it to 50 and
    /// then a measurement of 75 will actually come as 50 (halfway
    /// between 50 and 100)
    ///</summary>
    public double MinValue
    {
      get;
      set;
    }

    ///<summary>
    /// A value between 0 and 1 determining the smoothing of the
    /// measurement. Set it to something like 0.7-0.9 if you want
    /// to enable smoothing.
    ///</summary>
    public double Smooth
    {
      get;
      set;
    }

    ///<summary>
    /// The interval in milliseconds at which a measurement is taken.
    ///</summary>
    public int UpdateInterval
    {
      get;
      set;
    }

    ///<summary>
    /// The string value that determines the name of the
    /// measurement. It must match the key of an <see cref="ImageInfo"/>
    /// dictionary item. 
    ///</summary>
    public string Key
    {
      get;
      set;
    }

    /// <summary>
    /// An optional string value for parameters
    /// </summary>
    public string Parameter
    {
      get;
      set;
    }

    /// <summary>
    /// Set it to true in the settings editor to reload the monitor
    /// </summary>
    [XmlIgnore]
    public bool Reload
    {
      get;
      set;
    }

    #endregion
  }
}