using System.Windows.Forms;
using HotBabe.Views;

namespace HotBabe.Views
{
  partial class SettingsEditorView 
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.gbMonitors = new System.Windows.Forms.GroupBox();
      this.btnReloadMonitors = new System.Windows.Forms.Button();
      this.btnAddMonitor = new System.Windows.Forms.Button();
      this.pnlMonitors = new System.Windows.Forms.Panel();
      this.gbImages = new System.Windows.Forms.GroupBox();
      this.btnAddImageEditor = new System.Windows.Forms.Button();
      this.btnBrowseImage = new System.Windows.Forms.Button();
      this.tbImage = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnAddImage = new System.Windows.Forms.Button();
      this.pnlImages = new System.Windows.Forms.Panel();
      this.btnSave = new System.Windows.Forms.Button();
      this.gbButtons = new System.Windows.Forms.GroupBox();
      this.btnConsole = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.gbMonitors.SuspendLayout();
      this.gbImages.SuspendLayout();
      this.gbButtons.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // gbMonitors
      // 
      this.gbMonitors.Controls.Add(this.btnReloadMonitors);
      this.gbMonitors.Controls.Add(this.btnAddMonitor);
      this.gbMonitors.Controls.Add(this.pnlMonitors);
      this.gbMonitors.Dock = System.Windows.Forms.DockStyle.Right;
      this.gbMonitors.Location = new System.Drawing.Point(381, 0);
      this.gbMonitors.Name = "gbMonitors";
      this.gbMonitors.Size = new System.Drawing.Size(386, 557);
      this.gbMonitors.TabIndex = 0;
      this.gbMonitors.TabStop = false;
      this.gbMonitors.Text = "Monitors";
      // 
      // btnReloadMonitors
      // 
      this.btnReloadMonitors.AutoSize = true;
      this.btnReloadMonitors.Location = new System.Drawing.Point(6, 530);
      this.btnReloadMonitors.Name = "btnReloadMonitors";
      this.btnReloadMonitors.Size = new System.Drawing.Size(106, 23);
      this.btnReloadMonitors.TabIndex = 3;
      this.btnReloadMonitors.Text = "Reload all monitors";
      this.btnReloadMonitors.UseVisualStyleBackColor = true;
      this.btnReloadMonitors.Click += new System.EventHandler(this.btnReloadMonitors_Click);
      // 
      // btnAddMonitor
      // 
      this.btnAddMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddMonitor.Location = new System.Drawing.Point(289, 528);
      this.btnAddMonitor.Name = "btnAddMonitor";
      this.btnAddMonitor.Size = new System.Drawing.Size(91, 23);
      this.btnAddMonitor.TabIndex = 2;
      this.btnAddMonitor.Text = "Add Monitor(s)";
      this.btnAddMonitor.UseVisualStyleBackColor = true;
      this.btnAddMonitor.Click += new System.EventHandler(this.btnAddMonitor_Click);
      // 
      // pnlMonitors
      // 
      this.pnlMonitors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlMonitors.AutoScroll = true;
      this.pnlMonitors.Location = new System.Drawing.Point(6, 12);
      this.pnlMonitors.Name = "pnlMonitors";
      this.pnlMonitors.Size = new System.Drawing.Size(374, 512);
      this.pnlMonitors.TabIndex = 0;
      // 
      // gbImages
      // 
      this.gbImages.Controls.Add(this.btnAddImageEditor);
      this.gbImages.Controls.Add(this.btnBrowseImage);
      this.gbImages.Controls.Add(this.tbImage);
      this.gbImages.Controls.Add(this.label1);
      this.gbImages.Controls.Add(this.btnAddImage);
      this.gbImages.Controls.Add(this.pnlImages);
      this.gbImages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gbImages.Location = new System.Drawing.Point(0, 0);
      this.gbImages.Name = "gbImages";
      this.gbImages.Size = new System.Drawing.Size(381, 557);
      this.gbImages.TabIndex = 1;
      this.gbImages.TabStop = false;
      this.gbImages.Text = "Images";
      // 
      // btnAddImageEditor
      // 
      this.btnAddImageEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddImageEditor.Location = new System.Drawing.Point(187, 528);
      this.btnAddImageEditor.Name = "btnAddImageEditor";
      this.btnAddImageEditor.Size = new System.Drawing.Size(91, 23);
      this.btnAddImageEditor.TabIndex = 6;
      this.btnAddImageEditor.Text = "Add Editor";
      this.toolTip1.SetToolTip(this.btnAddImageEditor, "Add empty editor");
      this.btnAddImageEditor.UseVisualStyleBackColor = true;
      this.btnAddImageEditor.Click += new System.EventHandler(this.btnAddImageEditor_Click);
      // 
      // btnBrowseImage
      // 
      this.btnBrowseImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBrowseImage.Location = new System.Drawing.Point(351, 19);
      this.btnBrowseImage.Name = "btnBrowseImage";
      this.btnBrowseImage.Size = new System.Drawing.Size(24, 23);
      this.btnBrowseImage.TabIndex = 5;
      this.btnBrowseImage.Text = "...";
      this.btnBrowseImage.UseVisualStyleBackColor = true;
      this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
      // 
      // tbImage
      // 
      this.tbImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tbImage.Location = new System.Drawing.Point(94, 19);
      this.tbImage.Name = "tbImage";
      this.tbImage.Size = new System.Drawing.Size(251, 20);
      this.tbImage.TabIndex = 4;
      this.tbImage.TextChanged += new System.EventHandler(this.tbImage_TextChanged);
      this.tbImage.MouseLeave += new System.EventHandler(this.tbImage_MouseLeave);
      this.tbImage.MouseEnter += new System.EventHandler(this.tbImage_MouseEnter);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(76, 23);
      this.label1.TabIndex = 3;
      this.label1.Text = "Main Image:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTip1.SetToolTip(this.label1, "Program icon / default image when nothing is configured");
      // 
      // btnAddImage
      // 
      this.btnAddImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddImage.Location = new System.Drawing.Point(284, 528);
      this.btnAddImage.Name = "btnAddImage";
      this.btnAddImage.Size = new System.Drawing.Size(91, 23);
      this.btnAddImage.TabIndex = 1;
      this.btnAddImage.Text = "Add Image(s)";
      this.toolTip1.SetToolTip(this.btnAddImage, "Add an editor for each selected image");
      this.btnAddImage.UseVisualStyleBackColor = true;
      this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
      // 
      // pnlImages
      // 
      this.pnlImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlImages.AutoScroll = true;
      this.pnlImages.Location = new System.Drawing.Point(3, 48);
      this.pnlImages.Name = "pnlImages";
      this.pnlImages.Size = new System.Drawing.Size(372, 474);
      this.pnlImages.TabIndex = 0;
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(603, 12);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(158, 22);
      this.btnSave.TabIndex = 2;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // gbButtons
      // 
      this.gbButtons.Controls.Add(this.btnConsole);
      this.gbButtons.Controls.Add(this.btnCancel);
      this.gbButtons.Controls.Add(this.btnSave);
      this.gbButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.gbButtons.Location = new System.Drawing.Point(0, 557);
      this.gbButtons.Name = "gbButtons";
      this.gbButtons.Size = new System.Drawing.Size(767, 40);
      this.gbButtons.TabIndex = 3;
      this.gbButtons.TabStop = false;
      // 
      // btnConsole
      // 
      this.btnConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnConsole.Font = new System.Drawing.Font("Lucida Console", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnConsole.Location = new System.Drawing.Point(322, 12);
      this.btnConsole.Name = "btnConsole";
      this.btnConsole.Size = new System.Drawing.Size(103, 22);
      this.btnConsole.TabIndex = 4;
      this.btnConsole.Text = "Debug console";
      this.btnConsole.UseVisualStyleBackColor = true;
      this.btnConsole.Click += new System.EventHandler(this.btnConsole_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(6, 12);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(158, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // SettingsEditorView
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(767, 597);
      this.Controls.Add(this.gbImages);
      this.Controls.Add(this.gbMonitors);
      this.Controls.Add(this.gbButtons);
      this.Name = "SettingsEditorView";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SettingsEditorView";
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.settingsEditorView_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.settingsEditorView_DragEnter);
      this.gbMonitors.ResumeLayout(false);
      this.gbMonitors.PerformLayout();
      this.gbImages.ResumeLayout(false);
      this.gbImages.PerformLayout();
      this.gbButtons.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox gbMonitors;
    private System.Windows.Forms.GroupBox gbImages;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Panel pnlMonitors;
    private System.Windows.Forms.Panel pnlImages;
    private System.Windows.Forms.Button btnAddImage;
    private System.Windows.Forms.GroupBox gbButtons;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnAddMonitor;
    private Button btnBrowseImage;
    private TextBox tbImage;
    private Label label1;
    private OpenFileDialog openFileDialog1;
    private ErrorProvider errorProvider1;
    private ToolTip toolTip1;
    private Button btnReloadMonitors;
    private Button btnConsole;
    private Button btnAddImageEditor;
  }
}