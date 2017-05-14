namespace HotBabe.Code
{
  ///<summary>
  /// Used by menu items to store command information
  ///</summary>
  public class CommandItem
  {
    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    ///<param name="command"></param>
    public CommandItem(HotBabeCommand command) : this(command, null)
    {
    }

    ///<summary>
    /// Default constructor
    ///</summary>
    ///<param name="command"></param>
    ///<param name="parameter"></param>
    public CommandItem(HotBabeCommand command, object parameter)
    {
      Command = command;
      Parameter = parameter;
    }

    #endregion

    #region Properties

    ///<summary>
    /// Command name
    ///</summary>
    public HotBabeCommand Command
    {
      get;
      set;
    }

    ///<summary>
    /// Command parameter if any
    ///</summary>
    public object Parameter
    {
      get;
      set;
    }

    #endregion
  }
}