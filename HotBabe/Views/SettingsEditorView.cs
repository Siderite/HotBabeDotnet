#region Using directives

using System;
using System.Windows.Forms;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotBabe.Controls;
using HotLogger;

#endregion

namespace HotBabe.Views
{
  ///<summary>
  /// The Advanced Settings form
  ///</summary>
  public partial class SettingsEditorView : Form, ISettingsEditorView
  {
    #region Member data

    private bool _dirty;
    private bool _reloadAll;
    private HotBabeSettings _settings;

    #endregion

    #region Constructors

    ///<summary>
    /// DefaultConstructor
    ///</summary>
    public SettingsEditorView()
    {
      InitializeComponent();
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Load the settings in the editor
    ///</summary>
    ///<param name="settings"></param>
    public void LoadSettings(HotBabeSettings settings)
    {
      _settings = settings;
      Icon =
          ImageHelper.MakeIcon(
              ImageHelper.FromFile(PathHelper.GetRootedPath(settings.DefaultImageName)),
              16, false);
      tbImage.Text = PathHelper.GetRelativePath(settings.DefaultImageName);
      loadImages();
      loadMonitors();
    }

    #endregion

    #region Private Methods

    private void settingsEditorView_DragDrop(object sender, DragEventArgs e)
    {
      string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
      foreach (string file in files)
      {
        if (ImageHelper.IsValidImage(file))
        {
          addImageByFilename(file);
        }
        else
        {
          if (AssemblyHelper.IsValidMonitorAssembly(file))
          {
            addMonitorByFilename(file);
          }
        }
      }
    }

    private void settingsEditorView_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
      {
        string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
        bool valid = true;
        foreach (string file in files)
        {
          if (!ImageHelper.IsValidImage(file) && !AssemblyHelper.IsValidMonitorAssembly(file))
          {
            valid = false;
            break;
          }
        }
        if (valid)
        {
          e.Effect = DragDropEffects.All;
        }
      }
    }

    private void loadMonitors()
    {
      pnlMonitors.Controls.Clear();
      foreach (MonitorInfo info in Settings.MonitorInfos)
      {
        addMonitor(info);
      }
    }

    private Control addMonitor(MonitorInfo info)
    {
      MonitorInfoEditor control = new MonitorInfoEditor
                                    {
                                        Dock = DockStyle.Top
                                    };
      control.LoadEditorItem(info);
      control.Changed += monitorInfoChanged;
      control.Deleted += monitorInfoDeleted;
      pnlMonitors.Controls.Add(control);
      return control;
    }

    private void monitorInfoDeleted(object sender, EventArgs e)
    {
      MonitorInfoEditor editor = (MonitorInfoEditor) sender;
      pnlMonitors.Controls.Remove(editor);
      _dirty = true;
    }

    private void monitorInfoChanged(object sender, EventArgs e)
    {
      _dirty = true;
    }

    private void loadImages()
    {
      pnlImages.Controls.Clear();
      foreach (ImageInfo info in Settings.ImageInfos)
      {
        addImage(info);
      }
    }

    private Control addImage(ImageInfo info)
    {
      ImageInfoEditor control = new ImageInfoEditor
                                  {
                                      Dock = DockStyle.Top
                                  };
      control.LoadEditorInfo(info);
      control.Changed += imageInfoChanged;
      control.Deleted += imageInfoDeleted;
      pnlImages.Controls.Add(control);
      return control;
    }

    private void imageInfoDeleted(object sender, EventArgs e)
    {
      ImageInfoEditor editor = (ImageInfoEditor) sender;
      pnlImages.Controls.Remove(editor);
      _dirty = true;
    }

    private void imageInfoChanged(object sender, EventArgs e)
    {
      _dirty = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (_dirty)
      {
        ValidationResult result;
        HotBabeSettings settings = checkValues(out result);
        if (!result.IsValid)
        {
          return;
        }
        _settings = settings;
      }
      DialogResult = DialogResult.OK;
      Close();
    }

    private void checkValues()
    {
      ValidationResult result;
      checkValues(out result);
    }

    private HotBabeSettings checkValues(out ValidationResult result)
    {
      HotBabeSettings settings = new HotBabeSettings
                                   {
                                       AlwaysOnTop = Settings.AlwaysOnTop,
                                       AutoRun = Settings.AutoRun,
                                       ClickThrough = Settings.ClickThrough,
                                       DefaultImageName = tbImage.Text,
                                       Left = Settings.Left,
                                       Opacity = Settings.Opacity,
                                       Top = Settings.Top,
                                       UpdateInterval = Settings.UpdateInterval,
                                       BlendImages = Settings.BlendImages,
                                       HideOnFullscreen = Settings.HideOnFullscreen
                                   };

      foreach (ImageInfoEditor editor  in pnlImages.Controls)
      {
        settings.ImageInfos.Add(editor.GetEditorItem());
      }
      foreach (MonitorInfoEditor editor in pnlMonitors.Controls)
      {
        settings.MonitorInfos.Add(editor.GetEditorItem());
      }
      result = SettingsValidationHelper.Validate(settings);
      if (result.ContainsKey("DefaultImageName"))
      {
        errorProvider1.SetError(tbImage, result["DefaultImageName"].ToString());
      }
      return settings;
    }

    private void btnAddImage_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = string.Empty;
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.Filter = "Images | *.png;*.gif;*.jpg;*.jpeg;*.bmp;*.tiff|All files|*.*";
      openFileDialog1.InitialDirectory = PathHelper.GetDirectoryName(tbImage.Text) ??
                                         PathHelper.StartUpFolder;
      openFileDialog1.Multiselect = true;
      DialogResult dia = openFileDialog1.ShowDialog();
      if (dia == DialogResult.OK)
      {
        foreach (string fileName in openFileDialog1.FileNames)
        {
          addImageByFilename(fileName);
        }
        checkValues();
      }
    }

    private void addImageByFilename(string fileName)
    {
      _dirty = true;
      ImageInfo item = new ImageInfo
                         {
                             ImageFileName = PathHelper.GetRelativePath(fileName)
                         };
      Control control = addImage(item);
      control.BringToFront();
      pnlImages.ScrollControlIntoView(control);
    }

    private void btnAddMonitor_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = string.Empty;
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.Filter = "Assemblies | *.dll;*.exe|All files|*.*";
      openFileDialog1.InitialDirectory = PathHelper.StartUpFolder;
      openFileDialog1.Multiselect = true;
      DialogResult dia = openFileDialog1.ShowDialog();
      if (dia == DialogResult.OK)
      {
        foreach (string fileName in openFileDialog1.FileNames)
        {
          addMonitorByFilename(fileName);
        }
        checkValues();
      }
    }

    private void addMonitorByFilename(string fileName)
    {
      _dirty = true;
      MonitorInfo item = new MonitorInfo
                           {
                               AssemblyFileName = PathHelper.GetRelativePath(fileName)
                           };
      Control control = addMonitor(item);
      control.BringToFront();
      pnlMonitors.ScrollControlIntoView(control);
    }

    private void btnBrowseImage_Click(object sender, EventArgs e)
    {
      openFileDialog1.FileName = string.Empty;
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.Filter = "Images | *.png;*.gif;*.jpg;*.jpeg;*.bmp;*.tiff|All files|*.*";
      openFileDialog1.InitialDirectory = PathHelper.GetDirectoryName(tbImage.Text) ??
                                         PathHelper.StartUpFolder;
      openFileDialog1.Multiselect = false;
      DialogResult dia = openFileDialog1.ShowDialog();
      if (dia == DialogResult.OK)
      {
        string fileName = openFileDialog1.FileName;
        tbImage.Text = PathHelper.GetRelativePath(fileName);
      }
    }

    private void tbImage_TextChanged(object sender, EventArgs e)
    {
      _dirty = true;
      checkValues();
    }

    private void btnReloadMonitors_Click(object sender, EventArgs e)
    {
      _reloadAll = !_reloadAll;
      foreach (MonitorInfoEditor editor in pnlMonitors.Controls)
      {
        editor.Reload = _reloadAll;
      }
    }

    private void btnConsole_Click(object sender, EventArgs e)
    {
      Logger.Debug("Started debug console.");
      new Console().Show();
    }

    private void btnAddImageEditor_Click(object sender, EventArgs e)
    {
      addImageByFilename(null);
    }


    private void tbImage_MouseEnter(object sender, EventArgs e)
    {
      ImageHelper.HoverImage(tbImage.Text);
    }

    private void tbImage_MouseLeave(object sender, EventArgs e)
    {
      ImageHelper.HideHoverImage();
    }

    #endregion

    #region ISettingsEditorView Members

    ///<summary>
    /// Application current settings
    ///</summary>
    public HotBabeSettings Settings
    {
      get
      {
        return _settings;
      }
    }

    #endregion
  }
}