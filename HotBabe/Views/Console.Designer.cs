namespace HotBabe.Views
{
  partial class Console
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
      this.tbInfo = new System.Windows.Forms.TextBox();
      this.cbPaused = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.tbSeverity = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // tbInfo
      // 
      this.tbInfo.Location = new System.Drawing.Point(0, 35);
      this.tbInfo.Multiline = true;
      this.tbInfo.Name = "tbInfo";
      this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.tbInfo.Size = new System.Drawing.Size(1075, 549);
      this.tbInfo.TabIndex = 0;
      // 
      // cbPaused
      // 
      this.cbPaused.AutoSize = true;
      this.cbPaused.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.cbPaused.Location = new System.Drawing.Point(12, 12);
      this.cbPaused.Name = "cbPaused";
      this.cbPaused.Size = new System.Drawing.Size(62, 17);
      this.cbPaused.TabIndex = 1;
      this.cbPaused.Text = "Paused";
      this.cbPaused.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(113, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(87, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Minimum severity";
      // 
      // tbSeverity
      // 
      this.tbSeverity.Location = new System.Drawing.Point(206, 8);
      this.tbSeverity.Name = "tbSeverity";
      this.tbSeverity.Size = new System.Drawing.Size(153, 20);
      this.tbSeverity.TabIndex = 3;
      this.tbSeverity.Text = "0";
      this.tbSeverity.TextChanged += new System.EventHandler(this.tbSeverity_TextChanged);
      // 
      // Console
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1074, 584);
      this.Controls.Add(this.tbSeverity);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbPaused);
      this.Controls.Add(this.tbInfo);
      this.Name = "Console";
      this.Text = "Console";
      this.Load += new System.EventHandler(this.console_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox tbInfo;
    private System.Windows.Forms.CheckBox cbPaused;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox tbSeverity;
  }
}