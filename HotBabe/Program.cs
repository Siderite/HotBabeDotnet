#region Using directives

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotBabe.Controllers;
using HotBabe.Views;

#endregion

namespace HotBabe
{
  ///<summary>
  /// Startup class
  ///</summary>
  internal static class Program
  {
    #region Member data

    private static MainController sMainController;
    private static BaseSettingsManager<HotBabeSettings> sSettingsManager;

    #endregion

    #region Private Methods

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
// ReSharper disable InconsistentNaming
    private static void Main(string[] args)
// ReSharper restore InconsistentNaming
    {
      Arguments commandLine = new Arguments(args);

      bool owned;
      Mutex mutex = new Mutex(true, "HotBabe_" + Assembly.GetExecutingAssembly().FullName, out owned);
      if (!owned)
      {
        return;
      }
      GC.KeepAlive(mutex);

      ZipHelper.Register();
      ResourceHelper.Register();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      sSettingsManager = string.IsNullOrEmpty(commandLine.ConfigFilePath)
                             ? new XmlSettingsManager()
                             : new XmlSettingsManager(commandLine.ConfigFilePath);
      MainView view = new MainView
                        {
                            DebugEnabled = commandLine.Debug
                        };
      sMainController = new MainController(view, sSettingsManager);
      sMainController.EditSettings += controllerEditSettings;
      sMainController.Start();
    }

    private static void controllerEditSettings(object sender, EventArgs e)
    {
      SettingsEditorView view = new SettingsEditorView();
      SettingsEditorController controller = new SettingsEditorController(view, sSettingsManager);
      controller.SettingsChanged += controllerSettingsChanged;
      controller.Start();
    }

    private static void controllerSettingsChanged(object sender, EventArgs<HotBabeSettings> e)
    {
      sMainController.Refresh(e.Value);
    }

    #endregion
  }
}