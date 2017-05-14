#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotLogger;
using Monitors;

#endregion

namespace HotBabe.Controls
{
  ///<summary>
  /// Editor control for the <see cref="MonitorInfo"/> class
  ///</summary>
  public partial class MonitorInfoEditor : UserControl
  {
    #region Events

    ///<summary>
    /// Fired when any of the values change
    ///</summary>
    public event EventHandler Changed;

    ///<summary>
    /// Fires when the delete button is clicked
    ///</summary>
    public event EventHandler Deleted;

    #endregion

    #region Member data

    private List<Type> _monitorTypes;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public MonitorInfoEditor()
    {
      InitializeComponent();
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Checks the validity of the control values
    ///</summary>
    ///<returns></returns>
    public bool CheckValues()
    {
      bool result = true;
      errorProvider1.Clear();

      if (string.IsNullOrEmpty(tbMinValue.Text))
      {
        errorProvider1.SetError(tbMinValue, "The minimum value must be a number between 0 and 99.");
        result = false;
      }
      if (string.IsNullOrEmpty(tbSmooth.Text))
      {
        errorProvider1.SetError(tbSmooth, "The smooth value must be a number between 0 and 1.");
        result = false;
      }
      try
      {
        MonitorInfo item = GetEditorItem();
        ValidationResult validationResult = SettingsValidationHelper.Validate(item);
        if (!validationResult.IsValid)
        {
          result = false;
          foreach (KeyValuePair<string, ValidationErrorCollection> pair in validationResult)
          {
            switch (pair.Key)
            {
              case "MonitorInfo.AssemblyFileName":
                {
                  errorProvider1.SetError(tbAssembly, pair.Value.ToString());
                }
                break;
              case "MonitorInfo.TypeName":
                {
                  errorProvider1.SetError(cboxMonitor, pair.Value.ToString());
                }
                break;
              case "MonitorInfo.MinValue":
                {
                  errorProvider1.SetError(tbMinValue, pair.Value.ToString());
                }
                break;
              case "MonitorInfo.Key":
                {
                  errorProvider1.SetError(tbKey, pair.Value.ToString());
                }
                break;
              case "MonitorInfo.Smooth":
                {
                  errorProvider1.SetError(tbSmooth, pair.Value.ToString());
                }
                break;
              case "MonitorInfo.UpdateInterval":
                {
                  errorProvider1.SetError(numUpdateInterval, pair.Value.ToString());
                }
                break;
              case "MonitorInfo.Parameter":
                {
                  errorProvider1.SetError(tbParameter, pair.Value.ToString());
                }
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Debug("Exception caught and discarded: " + ex.Message +
                     " at MonitorInfoEditor.CheckValues");
        errorProvider1.SetError(tbAssembly,
                                string.Format("General conversion error ({0}).", ex.Message));
        result = false;
      }
      return result;
    }

    ///<summary>
    /// Get an <see cref="MonitorInfo"/> item from the values of the control
    ///</summary>
    ///<returns></returns>
    public MonitorInfo GetEditorItem()
    {
      double minValue;
      double.TryParse(tbMinValue.Text, out minValue);
      double smooth;
      double.TryParse(tbSmooth.Text, out smooth);
      return new MonitorInfo
               {
                   AssemblyFileName = tbAssembly.Text,
                   Key = tbKey.Text,
                   MinValue = minValue,
                   Smooth = smooth,
                   TypeName = cboxMonitor.Text,
                   UpdateInterval = ((int) numUpdateInterval.Value),
                   Parameter = tbParameter.Text,
                   Reload = cbReload.Checked
               };
    }

    ///<summary>
    /// Loads a <see cref="MonitorInfo"/> item in the editor
    ///</summary>
    ///<param name="value"></param>
    public void LoadEditorItem(MonitorInfo value)
    {
      tbAssembly.Text = value.AssemblyFileName;
      bool assemblyLoaded = loadMonitorAssembly(value.AssemblyFileName);
      if (assemblyLoaded && string.IsNullOrEmpty(value.TypeName))
      {
        if (cboxMonitor.Items.Count == 1)
        {
          cboxMonitor.SelectedIndex = 0;
        }
      }
      else
      {
        cboxMonitor.Text = value.TypeName;
      }

      if (!string.IsNullOrEmpty(value.Key)) tbKey.Text = value.Key;
      tbMinValue.Text = value.MinValue.ToString();
      tbSmooth.Text = value.Smooth.ToString();
      numUpdateInterval.Value = value.UpdateInterval;
      if (!string.IsNullOrEmpty(value.Parameter)) tbParameter.Text = value.Parameter;

      
      CheckValues();
    }

    #endregion

    #region Properties

    ///<summary>
    /// Set to true to reload the monitor when saving
    ///</summary>
    public bool Reload
    {
      set
      {
        cbReload.Checked = value;
      }
    }

    #endregion

    #region Private Methods

    private void tbAssembly_TextChanged(object sender, EventArgs e)
    {
      loadMonitorAssembly(tbAssembly.Text);
      raiseChanged();
    }

    private void raiseChanged()
    {
      CheckValues();
      Changed.Fire(this);
    }

    private void tbKey_TextChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    private void tbMinValue_TextChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    private void tbSmooth_TextChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    private void numUpdateInterval_ValueChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    private void btnBrowseAssembly_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = string.Empty;
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.Filter = "Assemblies | *.dll;*.exe|All files|*.*";
      openFileDialog1.InitialDirectory = PathHelper.StartUpFolder;
      openFileDialog1.Multiselect = false;
      DialogResult dia = openFileDialog1.ShowDialog();
      if (dia == DialogResult.OK)
      {
        string fileName = openFileDialog1.FileName;
        if (loadMonitorAssembly(fileName))
        {
          tbAssembly.Text = PathHelper.GetRelativePath(fileName);
        }
      }
    }

    private bool loadMonitorAssembly(string fileName)
    {
      cboxMonitor.Enabled = false;
      Assembly assembly;
      if (!AssemblyHelper.TryLoadAssembly(fileName, out assembly))
      {
        return false;
      }
      _monitorTypes = AssemblyHelper.GetAssemblyTypes<BaseMonitor>(assembly);
      if (_monitorTypes.Count == 0)
      {
        MessageBox.Show("This assembly contains no monitor classes");
        return false;
      }
      cboxMonitor.Items.Clear();
      foreach (Type type in _monitorTypes)
      {
        cboxMonitor.Items.Add(type.FullName);
      }
      cboxMonitor.Enabled = true;
      return true;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      Deleted.Fire(this);
    }


    private void cboxMonitor_TextChanged(object sender, EventArgs e)
    {
      setAutoKey();
      raiseChanged();
    }

    private void setAutoKey()
    {
      if (string.IsNullOrEmpty(cboxMonitor.Text))
      {
        return;
      }
      Assembly assembly;
      if (AssemblyHelper.TryLoadAssembly(tbAssembly.Text, out assembly))
      {
        Type type = assembly.GetType(cboxMonitor.Text);
        if (type == null)
        {
          return;
        }
        object[] keys = type.GetCustomAttributes(typeof (MonitorKeyAttribute), true);
        if (keys.Length > 0)
        {
          tbKey.Text = ((MonitorKeyAttribute) keys[0]).Key;
        }
        keys = type.GetCustomAttributes(typeof (MonitorParameterAttribute), true);
        if (keys.Length > 0)
        {
          MonitorParameterAttribute attr = ((MonitorParameterAttribute) keys[0]);
          tbParameter.Text = attr.RecommendedValue;
          toolTip1.SetToolTip(tbParameter, attr.Help);
        }
      }
    }

    private void tbParameter_TextChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    private void cbReload_CheckedChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    #endregion
  }
}