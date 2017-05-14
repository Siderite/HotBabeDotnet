#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotLogger;

#endregion

namespace HotBabe.Controls
{
  ///<summary>
  /// Editor control for the <see cref="ImageInfo"/> class
  ///</summary>
  public partial class ImageInfoEditor : UserControl
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

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public ImageInfoEditor()
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
      if (!string.IsNullOrEmpty(tbMeasures.Text) && tbMeasures.Text.Split('=').Length < 2)
      {
        errorProvider1.SetError(tbMeasures,
                                "Measures should be a series of key=value parts, separated by spaces");
        result = false;
      }
      else
      {
        try
        {
          ImageInfo info = GetEditorItem();
          ValidationResult validationResult = SettingsValidationHelper.Validate(info);
          if (!validationResult.IsValid)
          {
            result = false;
            foreach (KeyValuePair<string, ValidationErrorCollection> pair in validationResult)
            {
              switch (pair.Key)
              {
                case "ImageInfo.ImageFileName":
                  {
                    errorProvider1.SetError(tbImage, pair.Value.ToString());
                  }
                  break;
                case "ImageInfo.Measures":
                case "ImageInfo.Measures.Value":
                case "ImageInfo.Measures.Key":
                  {
                    errorProvider1.SetError(tbMeasures, pair.Value.ToString());
                  }
                  break;
              }
            }
          }
        }
        catch (Exception ex)
        {
          Logger.Debug("Exception caught and discarded: " + ex.Message +
                       " at ImageInfoEditor.CheckValues");
          errorProvider1.SetError(tbMeasures, "The measures are not valid (" + ex.Message + ").");
          result = false;
        }
      }
      return result;
    }

    ///<summary>
    /// Get an <see cref="ImageInfo"/> item from the values of the control
    ///</summary>
    ///<returns></returns>
    public ImageInfo GetEditorItem()
    {
      ImageInfo item = new ImageInfo
                         {
                             ImageFileName = tbImage.Text
                         };
      string[] measures = tbMeasures.Text.Split(' ');
      foreach (string s in measures)
      {
        string[] splits = s.Split('=');
        if (splits.Length == 2)
        {
          item.Measures.Add(splits[0].Trim(), double.Parse(splits[1]));
        }
      }
      return item;
    }

    ///<summary>
    /// Load an <see cref="ImageInfo"/> object in the editor
    ///</summary>
    ///<param name="value"></param>
    public void LoadEditorInfo(ImageInfo value)
    {
      tbImage.Text = value.ImageFileName;
      tbMeasures.Clear();
      StringBuilder sb = new StringBuilder();
      foreach (KeyValuePair<string, double> measure in value.Measures)
      {
        sb.AppendFormat(" {0}={1}", measure.Key, measure.Value);
      }
      tbMeasures.Text = sb.ToString().Trim();
      CheckValues();
    }

    #endregion

    #region Private Methods

    private void tbImage_TextChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    private void raiseChanged()
    {
      CheckValues();
      Changed.Fire(this);
    }

    private void tbMeasures_TextChanged(object sender, EventArgs e)
    {
      raiseChanged();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      Deleted.Fire(this);
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

    private void tbImage_MouseEnter(object sender, EventArgs e)
    {
      ImageHelper.HoverImage(tbImage.Text);
    }

    private void tbImage_MouseLeave(object sender, EventArgs e)
    {
      ImageHelper.HideHoverImage();
    }

    #endregion
  }
}