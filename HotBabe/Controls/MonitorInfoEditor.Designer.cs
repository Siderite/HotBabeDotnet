namespace HotBabe.Controls
{
    partial class MonitorInfoEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          this.components = new System.ComponentModel.Container();
          this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
          this.gbMonitor = new System.Windows.Forms.GroupBox();
          this.cbReload = new System.Windows.Forms.CheckBox();
          this.tbParameter = new System.Windows.Forms.TextBox();
          this.label7 = new System.Windows.Forms.Label();
          this.cboxMonitor = new System.Windows.Forms.ComboBox();
          this.numUpdateInterval = new System.Windows.Forms.NumericUpDown();
          this.tbSmooth = new System.Windows.Forms.TextBox();
          this.tbMinValue = new System.Windows.Forms.TextBox();
          this.tbKey = new System.Windows.Forms.TextBox();
          this.btnDelete = new System.Windows.Forms.Button();
          this.btnBrowseAssembly = new System.Windows.Forms.Button();
          this.tbAssembly = new System.Windows.Forms.TextBox();
          this.label4 = new System.Windows.Forms.Label();
          this.label5 = new System.Windows.Forms.Label();
          this.label6 = new System.Windows.Forms.Label();
          this.label3 = new System.Windows.Forms.Label();
          this.label2 = new System.Windows.Forms.Label();
          this.label1 = new System.Windows.Forms.Label();
          this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
          this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
          this.gbMonitor.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.numUpdateInterval)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
          this.SuspendLayout();
          // 
          // openFileDialog1
          // 
          this.openFileDialog1.FileName = "openFileDialog1";
          // 
          // gbMonitor
          // 
          this.gbMonitor.AutoSize = true;
          this.gbMonitor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
          this.gbMonitor.Controls.Add(this.cbReload);
          this.gbMonitor.Controls.Add(this.tbParameter);
          this.gbMonitor.Controls.Add(this.label7);
          this.gbMonitor.Controls.Add(this.cboxMonitor);
          this.gbMonitor.Controls.Add(this.numUpdateInterval);
          this.gbMonitor.Controls.Add(this.tbSmooth);
          this.gbMonitor.Controls.Add(this.tbMinValue);
          this.gbMonitor.Controls.Add(this.tbKey);
          this.gbMonitor.Controls.Add(this.btnDelete);
          this.gbMonitor.Controls.Add(this.btnBrowseAssembly);
          this.gbMonitor.Controls.Add(this.tbAssembly);
          this.gbMonitor.Controls.Add(this.label4);
          this.gbMonitor.Controls.Add(this.label5);
          this.gbMonitor.Controls.Add(this.label6);
          this.gbMonitor.Controls.Add(this.label3);
          this.gbMonitor.Controls.Add(this.label2);
          this.gbMonitor.Controls.Add(this.label1);
          this.gbMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
          this.gbMonitor.Location = new System.Drawing.Point(0, 0);
          this.gbMonitor.Name = "gbMonitor";
          this.gbMonitor.Size = new System.Drawing.Size(370, 184);
          this.gbMonitor.TabIndex = 0;
          this.gbMonitor.TabStop = false;
          // 
          // cbReload
          // 
          this.cbReload.AutoSize = true;
          this.cbReload.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
          this.cbReload.Location = new System.Drawing.Point(318, 70);
          this.cbReload.Name = "cbReload";
          this.cbReload.Size = new System.Drawing.Size(34, 17);
          this.cbReload.TabIndex = 34;
          this.cbReload.Text = "R";
          this.toolTip1.SetToolTip(this.cbReload, "Reload monitor on save");
          this.cbReload.UseVisualStyleBackColor = true;
          this.cbReload.CheckedChanged += new System.EventHandler(this.cbReload_CheckedChanged);
          // 
          // tbParameter
          // 
          this.tbParameter.Location = new System.Drawing.Point(71, 145);
          this.tbParameter.Name = "tbParameter";
          this.tbParameter.Size = new System.Drawing.Size(251, 20);
          this.tbParameter.TabIndex = 33;
          this.tbParameter.TextChanged += new System.EventHandler(this.tbParameter_TextChanged);
          // 
          // label7
          // 
          this.label7.Location = new System.Drawing.Point(5, 143);
          this.label7.Name = "label7";
          this.label7.Size = new System.Drawing.Size(60, 23);
          this.label7.TabIndex = 32;
          this.label7.Text = "Parameter:";
          this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // cboxMonitor
          // 
          this.cboxMonitor.Enabled = false;
          this.cboxMonitor.FormattingEnabled = true;
          this.cboxMonitor.Location = new System.Drawing.Point(71, 44);
          this.cboxMonitor.Name = "cboxMonitor";
          this.cboxMonitor.Size = new System.Drawing.Size(251, 21);
          this.cboxMonitor.TabIndex = 31;
          this.cboxMonitor.TextChanged += new System.EventHandler(this.cboxMonitor_TextChanged);
          // 
          // numUpdateInterval
          // 
          this.numUpdateInterval.Location = new System.Drawing.Point(85, 119);
          this.numUpdateInterval.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
          this.numUpdateInterval.Name = "numUpdateInterval";
          this.numUpdateInterval.Size = new System.Drawing.Size(237, 20);
          this.numUpdateInterval.TabIndex = 30;
          this.numUpdateInterval.ValueChanged += new System.EventHandler(this.numUpdateInterval_ValueChanged);
          // 
          // tbSmooth
          // 
          this.tbSmooth.Location = new System.Drawing.Point(213, 95);
          this.tbSmooth.Name = "tbSmooth";
          this.tbSmooth.Size = new System.Drawing.Size(109, 20);
          this.tbSmooth.TabIndex = 29;
          this.tbSmooth.TextChanged += new System.EventHandler(this.tbSmooth_TextChanged);
          // 
          // tbMinValue
          // 
          this.tbMinValue.Location = new System.Drawing.Point(71, 93);
          this.tbMinValue.Name = "tbMinValue";
          this.tbMinValue.Size = new System.Drawing.Size(89, 20);
          this.tbMinValue.TabIndex = 28;
          this.tbMinValue.TextChanged += new System.EventHandler(this.tbMinValue_TextChanged);
          // 
          // tbKey
          // 
          this.tbKey.Location = new System.Drawing.Point(71, 67);
          this.tbKey.Name = "tbKey";
          this.tbKey.Size = new System.Drawing.Size(241, 20);
          this.tbKey.TabIndex = 27;
          this.tbKey.TextChanged += new System.EventHandler(this.tbKey_TextChanged);
          // 
          // btnDelete
          // 
          this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnDelete.Location = new System.Drawing.Point(340, 142);
          this.btnDelete.Name = "btnDelete";
          this.btnDelete.Size = new System.Drawing.Size(24, 23);
          this.btnDelete.TabIndex = 26;
          this.btnDelete.Text = "X";
          this.toolTip1.SetToolTip(this.btnDelete, "Delete monitor");
          this.btnDelete.UseVisualStyleBackColor = true;
          this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
          // 
          // btnBrowseAssembly
          // 
          this.btnBrowseAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnBrowseAssembly.Location = new System.Drawing.Point(340, 15);
          this.btnBrowseAssembly.Name = "btnBrowseAssembly";
          this.btnBrowseAssembly.Size = new System.Drawing.Size(24, 23);
          this.btnBrowseAssembly.TabIndex = 25;
          this.btnBrowseAssembly.Text = "...";
          this.toolTip1.SetToolTip(this.btnBrowseAssembly, "Browse for monitor assembly");
          this.btnBrowseAssembly.UseVisualStyleBackColor = true;
          this.btnBrowseAssembly.Click += new System.EventHandler(this.btnBrowseAssembly_Click);
          // 
          // tbAssembly
          // 
          this.tbAssembly.Location = new System.Drawing.Point(71, 17);
          this.tbAssembly.Name = "tbAssembly";
          this.tbAssembly.Size = new System.Drawing.Size(251, 20);
          this.tbAssembly.TabIndex = 24;
          this.tbAssembly.TextChanged += new System.EventHandler(this.tbAssembly_TextChanged);
          // 
          // label4
          // 
          this.label4.Location = new System.Drawing.Point(5, 39);
          this.label4.Name = "label4";
          this.label4.Size = new System.Drawing.Size(46, 23);
          this.label4.TabIndex = 23;
          this.label4.Text = "Monitor:";
          this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // label5
          // 
          this.label5.Location = new System.Drawing.Point(5, 62);
          this.label5.Name = "label5";
          this.label5.Size = new System.Drawing.Size(48, 23);
          this.label5.TabIndex = 22;
          this.label5.Text = "Key:";
          this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // label6
          // 
          this.label6.Location = new System.Drawing.Point(7, 116);
          this.label6.Name = "label6";
          this.label6.Size = new System.Drawing.Size(82, 23);
          this.label6.TabIndex = 21;
          this.label6.Text = "UpdateInterval:";
          this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // label3
          // 
          this.label3.Location = new System.Drawing.Point(5, 15);
          this.label3.Name = "label3";
          this.label3.Size = new System.Drawing.Size(60, 23);
          this.label3.TabIndex = 20;
          this.label3.Text = "Assembly:";
          this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // label2
          // 
          this.label2.Location = new System.Drawing.Point(166, 93);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(58, 23);
          this.label2.TabIndex = 19;
          this.label2.Text = "Smooth:";
          this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // label1
          // 
          this.label1.Location = new System.Drawing.Point(7, 90);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(58, 23);
          this.label1.TabIndex = 18;
          this.label1.Text = "MinValue:";
          this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // errorProvider1
          // 
          this.errorProvider1.ContainerControl = this;
          // 
          // MonitorInfoEditor
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.AutoSize = true;
          this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
          this.Controls.Add(this.gbMonitor);
          this.Name = "MonitorInfoEditor";
          this.Size = new System.Drawing.Size(370, 184);
          this.gbMonitor.ResumeLayout(false);
          this.gbMonitor.PerformLayout();
          ((System.ComponentModel.ISupportInitialize)(this.numUpdateInterval)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox gbMonitor;
        private System.Windows.Forms.ComboBox cboxMonitor;
        private System.Windows.Forms.NumericUpDown numUpdateInterval;
        private System.Windows.Forms.TextBox tbSmooth;
        private System.Windows.Forms.TextBox tbMinValue;
        private System.Windows.Forms.TextBox tbKey;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnBrowseAssembly;
        private System.Windows.Forms.TextBox tbAssembly;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox tbParameter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox cbReload;
    }
}