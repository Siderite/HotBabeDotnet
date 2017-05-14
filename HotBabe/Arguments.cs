using HotBabe.Code.Helpers;

namespace HotBabe
{
  ///<summary>
  /// Parser for command line arguments
  ///</summary>
  internal class Arguments
  {
    #region Constructors

    ///<summary>
    /// default constructor
    ///</summary>
    ///<param name="args">command line arguments</param>
    public Arguments(string[] args)
    {
      for (int i = 0; i < args.Length; i++)
      {
        string arg = args[i];
        if (!string.IsNullOrEmpty(arg))
        {
          switch (arg.ToLower())
          {
            case "-d":
            case "-debug":
              Debug = true;
              break;
            case "-config":
            case "-c":
            case "-cfg":
              if (i < args.Length - 1)
              {
                i++;
                ConfigFilePath = args[i];
              }
              break;
            default:
              if (PathHelper.FileExists(arg))
              {
                ConfigFilePath = arg;
              }
              break;
          }
        }
      }
    }

    #endregion

    #region Properties

    ///<summary>
    /// Config filename
    ///</summary>
    public string ConfigFilePath
    {
      get;
      private set;
    }

    ///<summary>
    /// Debug mode
    ///</summary>
    internal bool Debug
    {
      get;
      private set;
    }

    #endregion
  }
}