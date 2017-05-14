#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using HotBabe.Code;
using HotBabe.Code.Helpers;
using HotBabe.Code.Transitions;

#endregion

namespace HotBabe.Views
{
  ///<summary>
  /// Main form of the application resposibile for displaying the
  /// images and the traybar icon and menu.
  ///</summary>
  public class MainView : Form, IMainView
  {
    #region Member data

    private bool _alwaysOnTop;
    private bool _clickThrough;
    private NotifyIcon _icon;
    private Image _image;
    private IImageTransition _imageTransition;
    private bool _requiresUpdateStyles;
    private Timer _timer;

    #endregion

    #region Constructors

    ///<summary>
    /// Default constructor
    ///</summary>
    public MainView()
    {
      initStateTimer();
      showBabe();
    }

    #endregion

    #region Public Methods

    ///<summary>
    /// Change the image in the main view (the actual change will be handled by a transition)
    ///</summary>
    ///<param name="image"></param>
    public void ChangeImage(Image image)
    {
      if (_image == null)
      {
        _image = image;
      }
      _imageTransition.TransitionTo(image);
    }

    ///<summary>
    /// Apply the current settings
    ///</summary>
    public void ApplySettings()
    {
      this.SafeInvoke(applySettings);
    }

    ///<summary>
    /// Set the view image transition object
    ///</summary>
    ///<param name="transition"></param>
    public void SetTransition(IImageTransition transition)
    {
      if (_imageTransition != transition)
      {
        if (_imageTransition != null)
        {
          _imageTransition.Update -= imageTransitionUpdate;
        }
        _imageTransition = transition;
        _imageTransition.Update += imageTransitionUpdate;
      }
    }

    #endregion

    #region Properties

    /// <summary/>
    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams ws = base.CreateParams;

        if (ClickThrough)
        {
          ws.ExStyle |= UnsafeNativeMethods.WS_EX_Layered;
          ws.ExStyle |= UnsafeNativeMethods.WS_EX_Transparent;
        }
        // do not show in Alt-tab
        ws.ExStyle |= UnsafeNativeMethods.WS_EX_ToolWindow;
        // make image stay on top other applications
        if (AlwaysOnTop)
        {
          // Make it show on top normal windows (the Form TopMost property is not always working)
          ws.ExStyle |= UnsafeNativeMethods.WS_EX_TopMost;
        }
        // do not make foreground window
        ws.ExStyle |= UnsafeNativeMethods.WS_EX_NoActivate;
        return ws;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the window will be activated when it is shown.
    /// </summary>
    /// <returns>
    /// True if the window will not be activated when it is shown; otherwise, false. The default is false.
    /// </returns>
    protected override bool ShowWithoutActivation
    {
      get
      {
        return true;
      }
    }

    ///<summary>
    /// Set to true for debug mode.
    /// Currently it only enables the console in the traybar menu
    ///</summary>
    public bool DebugEnabled
    {
      get;
      set;
    }

    ///<summary>
    /// True if it is showing a custom image
    ///</summary>
    public bool ShowCustomImage
    {
      get;
      set;
    }


    /// <summary>
    /// Maximum image size cache. If it exceeds this value, 
    /// older images from the cache will be removed.
    /// </summary>
    public long MaxImageCacheSize { get; set; }

    public Alignment HorizontalAlignment { get; set; }
    public Alignment VerticalAlignment { get; set; }




    #endregion

    #region Private Methods

    private static void mainDragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
      {
        string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
        if (files.Length == 1)
        {
          if (ImageHelper.IsValidImage(files[0]))
          {
            e.Effect = DragDropEffects.All;
          }
        }
      }
    }

    /// <summary/>
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == UnsafeNativeMethods.WM_MOVING)
      {
        UnsafeNativeMethods.ReDrawWindow(m);
      }

      base.WndProc(ref m);
    }

    private void initStateTimer()
    {
      _timer = new Timer
                 {
                     Interval = 1000
                 };
      _timer.Tick += timerTick;
      _timer.Start();
    }

    private void timerTick(object sender, EventArgs e)
    {
      this.SafeInvoke(checkState);
    }

    private void checkState()
    {
      if (WindowState == FormWindowState.Minimized)
      {
        WindowState = FormWindowState.Normal;
      }
      if (Visible != getExpectedVisibility())
      {
        Visible = !Visible;
      }
    }

    private bool getExpectedVisibility()
    {
      bool result = true;
      if (HideOnFullscreen && UnsafeNativeMethods.IsActiveWindowFullscreen())
      {
        result = false;
      }
      if (ViewOpacity == 0)
      {
        result = false;
      }
      return result;
    }

    private void imageTransitionUpdate(object sender, EventArgs e)
    {
      this.SafeInvoke(setImagePositionAndSize, _imageTransition.Image);
    }

    private void showBabe()
    {
      MouseDown += mainMouseDown;
      LocationChanged += mainLocationChanged;
      DragEnter += mainDragEnter;
      DragDrop += mainDragDrop;

      SuspendLayout();
      BackColor = Color.White;
      TransparencyKey = BackColor;
      AllowTransparency = true;
      FormBorderStyle = FormBorderStyle.None;
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.Manual;
      TopMost = true;

      _icon = new NotifyIcon(new Container())
                {
                    Visible = true
                };
      _icon.MouseClick += iconMouseClick;

      AllowDrop = true;
      DoubleBuffered = true;
      SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
               ControlStyles.OptimizedDoubleBuffer, true);
      ResumeLayout(false);
    }

    private void mainDragDrop(object sender, DragEventArgs e)
    {
      string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
      if (files.Length > 0)
      {
        requestExecuteCommand(new CommandItem(HotBabeCommand.DropFile, files[0]));
      }
    }

    private void iconMouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        string iconText = requestIconText();

        if (!string.IsNullOrEmpty(iconText))
        {
          _icon.BalloonTipIcon = ToolTipIcon.None;
          _icon.BalloonTipTitle = "HotBabe.NET";
          _icon.BalloonTipText = iconText;
          _icon.ShowBalloonTip(1000);
        }
      }
    }

    private string requestIconText()
    {
      EventArgs<CommandItem> args =
          new EventArgs<CommandItem>(new CommandItem(HotBabeCommand.RequestIconText));
      ExecuteCommand.Fire(this, args);
      return (string) args.Value.Parameter;
    }

    private ContextMenu getContextMenu()
    {
      ContextMenu menu = new ContextMenu();

      MenuItem item = new MenuItem("Start with Windows")
                        {
                            Tag = new CommandItem(HotBabeCommand.AutoRun),
                            Checked = AutoRun
                        };
      item.Click += itemClick;
      menu.MenuItems.Add(item);

      item = new MenuItem("Click through image")
               {
                   Tag = new CommandItem(HotBabeCommand.ClickThrough),
                   Checked = ClickThrough
               };
      item.Click += itemClick;
      menu.MenuItems.Add(item);

      item = new MenuItem("Always stay on top")
               {
                   Tag = new CommandItem(HotBabeCommand.AlwaysOnTop),
                   Checked = AlwaysOnTop
               };
      item.Click += itemClick;
      menu.MenuItems.Add(item);

      item = new MenuItem("Hide on fullscreen applications")
               {
                   Tag = new CommandItem(HotBabeCommand.HideOnFullscreen),
                   Checked = HideOnFullscreen
               };
      item.Click += itemClick;
      menu.MenuItems.Add(item);


      item = new MenuItem("Blend images");
      MenuItem item1 = new MenuItem("None")
                         {
                             Tag = new CommandItem(HotBabeCommand.BlendImages, BlendImagesMode.None),
                             Checked = BlendImages == BlendImagesMode.None
                         };
      item1.Click += itemClick;
      item.MenuItems.Add(item1);
      item1 = new MenuItem("In and Out")
                {
                    Tag = new CommandItem(HotBabeCommand.BlendImages, BlendImagesMode.InAndOut),
                    Checked = BlendImages == BlendImagesMode.InAndOut
                };
      item1.Click += itemClick;
      item.MenuItems.Add(item1);
      item1 = new MenuItem("Out then In")
                {
                    Tag = new CommandItem(HotBabeCommand.BlendImages, BlendImagesMode.OutThenIn),
                    Checked = BlendImages == BlendImagesMode.OutThenIn
                };
      item1.Click += itemClick;
      item.MenuItems.Add(item1);
      item1 = new MenuItem("Custom Image")
                {
                    Tag = new CommandItem(HotBabeCommand.BlendImages, BlendImagesMode.CustomImage),
                    Checked = BlendImages == BlendImagesMode.CustomImage,
                    Visible = BlendImages == BlendImagesMode.CustomImage
                };
      item1.Click += itemClick;
      item.MenuItems.Add(item1);

      menu.MenuItems.Add(item);


      item = new MenuItem("Update interval");
      for (int i = 1; i <= 100; i *= 2)
      {
        item1 = new MenuItem(i + " seconds")
                  {
                      Tag = new CommandItem(HotBabeCommand.UpdateInterval, i),
                      Checked = UpdateInterval == i*1000
                  };
        item1.Click += itemClick;
        item.MenuItems.Add(item1);
      }
      menu.MenuItems.Add(item);


      item = new MenuItem("Opacity");
      for (int i = 0; i <= 100; i += 10)
      {
        string text = i == 0
                          ? "Hide"
                          : (i + " %");
        item1 = new MenuItem(text)
                  {
                      Tag = new CommandItem(HotBabeCommand.Opacity, i),
                      Checked = Math.Abs(ViewOpacity*100 - i) <= 1
                  };
        item1.Click += itemClick;
        item.MenuItems.Add(item1);
      }
      menu.MenuItems.Add(item);

      item = new MenuItem("Alignment");

      item1 = new MenuItem("Horizontal");

      var dict3 = new Dictionary<string, Alignment>
                 {
                   {"None",Alignment.None},
                   {"Left",Alignment.LeftOrTop},
                   {"Center",Alignment.Center},
                   {"Right",Alignment.RightOrBottom},
                   {"Stretch",Alignment.Stretch},
                 };
      foreach (var pair in dict3)
      {
        MenuItem item2 = new MenuItem(pair.Key)
                         {
                           Tag = new CommandItem(HotBabeCommand.HorizontalAlignment, pair.Value),
                           Checked = HorizontalAlignment == pair.Value
                         };
        item2.Click += itemClick;
        item1.MenuItems.Add(item2);
      }

      item.MenuItems.Add(item1);

      item1 = new MenuItem("Vertical");
      dict3 = new Dictionary<string, Alignment>
                 {
                   {"None",Alignment.None},
                   {"Top",Alignment.LeftOrTop},
                   {"Center",Alignment.Center},
                   {"Bottom",Alignment.RightOrBottom},
                   {"Stretch",Alignment.Stretch},
                 };
      foreach (var pair in dict3)
      {
        MenuItem item2 = new MenuItem(pair.Key)
        {
          Tag = new CommandItem(HotBabeCommand.VerticalAlignment, pair.Value),
          Checked = VerticalAlignment == pair.Value
        };
        item2.Click += itemClick;
        item1.MenuItems.Add(item2);
      }

      item.MenuItems.Add(item1);

      item1 = new MenuItem("Clear");
      item1.Click += (sender,args)=>
                       {
                         requestExecuteCommand(new CommandItem(HotBabeCommand.HorizontalAlignment, Alignment.None));
                         requestExecuteCommand(new CommandItem(HotBabeCommand.VerticalAlignment, Alignment.None));
                       };
      item.MenuItems.Add(item1);

      menu.MenuItems.Add(item);

      menu.MenuItems.Add(new MenuItem("-"));

      item = new MenuItem("Advanced");

      item1 = new MenuItem("Priority");
      var dict = new Dictionary<string, ProcessPriorityClass>
                 {
                   {"Idle",ProcessPriorityClass.Idle},
                   {"Below Normal",ProcessPriorityClass.BelowNormal},
                   {"Normal",ProcessPriorityClass.Normal},
                   {"Above Normal",ProcessPriorityClass.AboveNormal},
                   {"High",ProcessPriorityClass.High}
                 };
      foreach (var pair in dict)
      {
        MenuItem item2 = new MenuItem(pair.Key)
                         {
                           Tag = new CommandItem(HotBabeCommand.Priority, pair.Value), Checked = Process.GetCurrentProcess().PriorityClass == pair.Value
                         };
        item2.Click += itemClick;
        item1.MenuItems.Add(item2);
      }
      item.MenuItems.Add(item1);

      item1 = new MenuItem("Max Cache Size");
      var dict2 = new Dictionary<string, long>
                    {
                      {"50MB", 50*1024*1024},
                      {"100MB", 100*1024*1024},
                      {"150MB", 150*1024*1024},
                      {"200MB", 200*1024*1024},
                      {"Unlimited", long.MaxValue},
                 };
      foreach (var pair in dict2)
      {
        MenuItem item2 = new MenuItem(pair.Key)
        {
          Tag = new CommandItem(HotBabeCommand.MaxImageCacheSize, pair.Value),
          Checked = MaxImageCacheSize==pair.Value
        };
        item2.Click += itemClick;
        item1.MenuItems.Add(item2);
      }
      item.MenuItems.Add(item1);

      item1 = new MenuItem("Settings")
                {
                    Tag = new CommandItem(HotBabeCommand.AdvancedSettings)
                };
      item1.Click += itemClick;
      item.MenuItems.Add(item1);

      item1 = new MenuItem("Load Pack")
                {
                    Tag = new CommandItem(HotBabeCommand.LoadPack)
                };
      item1.Click += itemClick;
      item.MenuItems.Add(item1);

      menu.MenuItems.Add(item);

      menu.MenuItems.Add(new MenuItem("-"));

      if (DebugEnabled)
      {
        item = new MenuItem("Console")
                 {
                     Tag = new CommandItem(HotBabeCommand.Console)
                 };
        item.Click += itemClick;
        menu.MenuItems.Add(item);
        menu.MenuItems.Add(new MenuItem("-"));
      }

      item = new MenuItem("Exit")
               {
                   Tag = new CommandItem(HotBabeCommand.Exit)
               };
      item.Click += itemClick;
      menu.MenuItems.Add(item);

      return menu;
    }

    private void itemClick(object sender, EventArgs e)
    {
      MenuItem item = (MenuItem) sender;
      CommandItem commandItem = (CommandItem) item.Tag;
      switch (commandItem.Command)
      {
        case HotBabeCommand.Exit:
          {
            DialogResult exitConfirmation = MessageBox.Show("Are you sure?", "Exit HotBabe.NET",
                                                            MessageBoxButtons.YesNoCancel,
                                                            MessageBoxIcon.Question,
                                                            MessageBoxDefaultButton.Button2,
                                                            MessageBoxOptions.ServiceNotification);
            switch (exitConfirmation)
            {
              case DialogResult.Cancel:
                break;
              case DialogResult.Yes:
                _icon.Visible = false;
                Close();
                break;
              case DialogResult.No:
                requestExecuteCommand(new CommandItem(HotBabeCommand.Opacity, 0));
                break;
            }
          }
          break;
        case HotBabeCommand.LoadPack:
          {
            if (
                MessageBox.Show(
                    "Loading a pack will overwrite the current settings.\r\nAre you sure?",
                    "Load pack", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
              OpenFileDialog dialog = new OpenFileDialog
                                        {
                                            FileName = string.Empty,
                                            CheckFileExists = true,
                                            Filter = "HotBabe packs| *.zip|All files|*.*",
                                            InitialDirectory = PathHelper.StartUpFolder,
                                            Multiselect = false
                                        };
              DialogResult dia = dialog.ShowDialog();
              if (dia == DialogResult.OK)
              {
                string fileName = dialog.FileName;
                requestExecuteCommand(new CommandItem(HotBabeCommand.LoadPack, fileName));
              }
            }
          }
          break;
        default:
          requestExecuteCommand(commandItem);
          break;
      }
    }

    private void requestExecuteCommand(CommandItem commandItem)
    {
      EventArgs<CommandItem> args = new EventArgs<CommandItem>(commandItem);
      ExecuteCommand.Fire(this, args);
      applySettings();
    }

    private void applySettings()
    {
      string iconFileName = IconFileName;
      if (!PathHelper.FileExists(iconFileName))
      {
        iconFileName = Constants.StartImageFileName;
      }
      _icon.Icon = ImageHelper.MakeIcon(ImageHelper.FromFile(iconFileName), 16, true);

      ContextMenu contextMenu = getContextMenu();

      _icon.ContextMenu = contextMenu;
      ContextMenu = contextMenu;

      setImagePositionAndSize(_image);
      if (Opacity != ViewOpacity)
      {
        Opacity = ViewOpacity;
      }
      if (TopMost != AlwaysOnTop)
      {
        TopMost = AlwaysOnTop;
        if (AlwaysOnTop)
        {
          BringToFront();
        }
      }
      if (_requiresUpdateStyles)
      {
        UpdateStyles();
        _requiresUpdateStyles = false;
      }
      PerformLayout();
    }

    private void setImagePositionAndSize(Image image)
    {
      _image = image;
      if (_image!=null&&(HorizontalAlignment!=Alignment.None|| VerticalAlignment!=Alignment.None))
      {
        var bounds = Screen.GetWorkingArea(this);
        var width = _image.Width;
        var height = _image.Height;
        if (HorizontalAlignment == Alignment.Stretch)
        {
          width = bounds.Width;
        }
        if (VerticalAlignment==Alignment.Stretch)
        {
          height = bounds.Height;
        }
        if (width!=_image.Width||height!=_image.Height)
        {
          _image = ImageHelper.Resize(_image, width, height);
        }
        switch(HorizontalAlignment)
        {
          case Alignment.None:
            break;
          case Alignment.LeftOrTop:
            ViewLeft = bounds.Left;
            break;
          case Alignment.RightOrBottom:
            ViewLeft = bounds.Right-width;
            break;
          case Alignment.Center:
            ViewLeft = bounds.Left + (bounds.Width - width)/2;
            break;
          case Alignment.Stretch:
            ViewLeft = bounds.Left;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        switch (VerticalAlignment)
        {
          case Alignment.None:
            break;
          case Alignment.LeftOrTop:
            ViewTop = bounds.Top;
            break;
          case Alignment.RightOrBottom:
            ViewTop = bounds.Bottom - height;
            break;
          case Alignment.Center:
            ViewTop = bounds.Top + (bounds.Height - height) / 2;
            break;
          case Alignment.Stretch:
            ViewTop = bounds.Top;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      setImage(_image);
      if (Location.X != ViewLeft || Location.Y != ViewTop)
      {
        Location = new Point(ViewLeft, ViewTop);
      }
    }

    private void setImage(Image image)
    {
      _image = image;
      if (BackgroundImage != image)
      {
        BackgroundImage = image;
      }
      if (ClientSize != image.Size)
      {
        ClientSize = image.Size;
        Refresh();
      }
      Opacity = ViewOpacity;
    }

    private void mainLocationChanged(object sender, EventArgs e)
    {
      requestLocationChange();
    }

    private void requestLocationChange()
    {
      EventArgs<CommandItem> args =
          new EventArgs<CommandItem>(new CommandItem(HotBabeCommand.LocationChanged, Location));
      ExecuteCommand.Fire(this, args);
    }

    private void mainMouseDown(object sender, MouseEventArgs e)
    {
      // Draggable from anywhere
      if (e.Button == MouseButtons.Left)
      {
        UnsafeNativeMethods.ReleaseCapture();
        UnsafeNativeMethods.SendMessage(Handle, UnsafeNativeMethods.WM_NCLBUTTONDOWN,
                                        UnsafeNativeMethods.HT_CAPTION, 0);
      }
    }

    #endregion

    #region IMainView Members

    ///<summary>
    /// If set, images will change through a fade-in, fade-out effect
    ///</summary>
    public BlendImagesMode BlendImages
    {
      get;
      set;
    }

    ///<summary>
    /// Execute a command on the controller 
    ///</summary>
    public event EventHandler<EventArgs<CommandItem>> ExecuteCommand;

    ///<summary>
    /// If true, the mouse will ignore the application and the
    /// clicks will go through it to the underlying applications
    ///</summary>
    public bool ClickThrough
    {
      get
      {
        return _clickThrough;
      }
      set
      {
        _clickThrough = value;
        _requiresUpdateStyles = true;
      }
    }

    ///<summary>
    /// The filename for the icon of the application and tray icon (can be a normal image)
    ///</summary>
    public string IconFileName
    {
      get;
      set;
    }

    ///<summary>
    /// The update interval in milliseconds
    ///</summary>
    public int UpdateInterval
    {
      get;
      set;
    }

    ///<summary>
    /// Auto start the application on Windows start
    ///</summary>
    public bool AutoRun
    {
      get;
      set;
    }

    ///<summary>
    /// The vertical position of the view
    ///</summary>
    public int ViewTop
    {
      get;
      set;
    }

    ///<summary>
    /// The horizontal position of the view
    ///</summary>
    public int ViewLeft
    {
      get;
      set;
    }

    ///<summary>
    /// The view should always stay on top other applications
    ///</summary>
    public bool AlwaysOnTop
    {
      get
      {
        return _alwaysOnTop;
      }
      set
      {
        _alwaysOnTop = value;
        _requiresUpdateStyles = true;
      }
    }

    ///<summary>
    /// The opacity of the view
    ///</summary>
    public double ViewOpacity
    {
      get;
      set;
    }

    /// <summary>
    /// If set, the application will hide if the active application is fullscreen
    /// </summary>
    public bool HideOnFullscreen
    {
      get;
      set;
    }

    #endregion
  }
}