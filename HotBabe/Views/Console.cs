#region Using directives

using System;
using System.Text;
using System.Windows.Forms;
using HotLogger;

#endregion

namespace HotBabe.Views
{
  ///<summary>
  /// Simple log console class
  ///</summary>
  public partial class Console : Form
  {
    #region Member data

    private DateTime _lastTime;

    #endregion

    #region Constructors

    ///<summary>
    /// default constructor
    ///</summary>
    public Console()
    {
      _lastTime = DateTime.Now;
      InitializeComponent();
      Logger.LoggerUpdated += loggerLoggerUpdated;
    }

    #endregion

    #region Private Methods

    private void loggerLoggerUpdated(object sender, EventArgs e)
    {
      if (cbPaused.Checked)
      {
        return;
      }
      TimeSpan updateInterval = DateTime.Now - _lastTime;
      if (updateInterval.TotalSeconds > 5)
      {
        refreshConsole();
        _lastTime = DateTime.Now;
      }
    }

    private void console_Load(object sender, EventArgs e)
    {
      refreshConsole();
    }

    private void refreshConsole()
    {
      if (Logger.Lines.Count == 0)
      {
        return;
      }
      StringBuilder sb = new StringBuilder();
      int i = Logger.Lines.Count - 1;
      int k = 0;
      int minSeverity;
      if (!int.TryParse(tbSeverity.Text, out minSeverity))
      {
        minSeverity = 0;
      }
      while (i >= 0 && k < 500)
      {
        LoggerLine line = Logger.Lines[i];
        if (line.Severity >= minSeverity)
        {
          sb.AppendFormat("{0:dd/MM/yyyy HH:mm:ss}\t{1}\t{2}\r\n", line.Time, line.Severity,
                          line.Message);
          k++;
        }
        i--;
      }
      try
      {
        BeginInvoke(new Action<string>(setText), sb.ToString());
      }
      catch (InvalidOperationException)
      {
        try
        {
          setText(sb.ToString());
        }
        catch (InvalidOperationException)
        {
        }
      }
    }

    private void setText(string text)
    {
      tbInfo.Text = text;
    }

    private void tbSeverity_TextChanged(object sender, EventArgs e)
    {
      refreshConsole();
    }

    #endregion
  }
}