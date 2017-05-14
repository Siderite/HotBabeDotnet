#region Using directives

using System;
using System.Windows.Forms;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotBabe.Views;

#endregion

namespace HotBabe.Controllers
{
  ///<summary>
  /// The controller for the settings editor view
  ///</summary>
  public class SettingsEditorController
  {
    #region Events

    ///<summary>
    /// Fired when the settings have been changed (saved)
    ///</summary>
    public event EventHandler<EventArgs<HotBabeSettings>> SettingsChanged;

    #endregion

    #region Member data

    private readonly BaseSettingsManager<HotBabeSettings> _settingsManager;
    private readonly ISettingsEditorView _view;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    ///<param name="view">the view for the controller</param>
    ///<param name="settingsManager">the manager used to load/save settings</param>
    public SettingsEditorController(ISettingsEditorView view,
                                    BaseSettingsManager<HotBabeSettings> settingsManager)
    {
      _view = view;
      _settingsManager = settingsManager;
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Start the controller
    ///</summary>
    public void Start()
    {
      Form frm = _view as Form;
      if (frm != null)
      {
        HotBabeSettings settings = _settingsManager.LoadSettings();
        _view.LoadSettings(settings);
        DialogResult dia = frm.ShowDialog();
        if (dia == DialogResult.OK)
        {
          _settingsManager.SaveSettings(_view.Settings);
          SettingsChanged.Fire(this, new EventArgs<HotBabeSettings>(_view.Settings));
        }
      }
    }

    #endregion
  }
}