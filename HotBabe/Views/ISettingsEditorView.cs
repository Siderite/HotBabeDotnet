#region Using directives

using HotBabe.Code;

#endregion

namespace HotBabe.Views
{
  ///<summary>
  /// The interface for the settings editor view
  ///</summary>
  public interface ISettingsEditorView
  {
    #region Public Methods

    ///<summary>
    /// Load the settings in the editor
    ///</summary>
    ///<param name="settings"></param>
    void LoadSettings(HotBabeSettings settings);

    #endregion

    #region Properties

    ///<summary>
    /// Get the edited version of the settings
    ///</summary>
    HotBabeSettings Settings
    {
      get;
    }

    #endregion
  }
}